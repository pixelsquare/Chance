using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using Item;
using Mission;
using Mission.Database;
using NPC;
using NPC.Database;
using UnityEngine;

public class MissionUi : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private MissionNameID initialMission;
	[SerializeField]
	private GUISkin missionSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private bool missionEnabled;

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;
	private Color guiOriginalColor;

	private bool showMissionBox;
	private bool showMissionButton;
	private float missionButtonTime;
	private float missionButtonAlpha;

	private GUIContent content = new GUIContent();
	private Vector2 descScrollPos;

	private float origMessageBoxPosY;
	private float targetMessageBoxPosY;
	private float messageBoxVel;
	private float messageBoxDampTime = 0.3f;

	private NPCData data;
	private System.Random rng;
	private int indx;

	private int missionIndx;
	private float missionTime;
	private bool missionActive;
	private MissionData baseMissionData;

	private string rewardText;

	private GameManager gameManager;
	private PlayerInformation playerInformation;

	# endregion Private Variables

	# region Reset Variables

	private MissionNameID _initialMission;

	# endregion Reset Variables

	// Public Properties
	public bool MissionEnabled {
		get { return missionEnabled; }
	}

	public bool MissionActive {
		get { return missionActive; }
	}
	// --

	public static MissionUi current;

	private void Awake() {
		current = this;
		MissionDatabase.Initialize();
	}

	private void Start() {
		gameManager = GameManager.current;

		missionEnabled = gameObject.activeInHierarchy;

		rng = new System.Random();
		_initialMission = initialMission;
		missionIndx = (int)_initialMission;
	}

	private void Update() {
		if (playerInformation == null && gameManager.BasePlayerData != null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
		}

		if (gameManager.GameState == GameState.MainGame) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (baseMissionData != null) {
				if (showMissionButton) {
					missionButtonTime += Time.deltaTime;
					missionButtonAlpha = Mathf.Lerp(1f, 0.2f, Mathf.PingPong(missionButtonTime, 0.5f) / 0.5f);
				}

				if (showMissionBox) {
					if (origMessageBoxPosY != targetMessageBoxPosY) {
						origMessageBoxPosY = Mathf.SmoothDamp(origMessageBoxPosY, targetMessageBoxPosY, ref messageBoxVel, messageBoxDampTime);
					}
				}

				if (missionActive) {
					if (missionTime != -1) {
						if (missionTime > 0f) {
							missionTime -= Time.deltaTime;
						}
						else {
							rewardText = (baseMissionData.MissionReward.Dislike > 0) ? "-" + baseMissionData.MissionReward.Dislike + " to all" : string.Empty;
							UserInterface.RunTitle(baseMissionData.MissionTitle, "MISSION FAILED", rewardText, new FadeTransition(2f, null, null, MissionFailed));
							gameManager.BasePlayerData.MissionsFailed++;

							AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.MissionFailed, true);
							audioSource.gameObject.SetActive(true);
							audioSource.Play();

							missionActive = false;
						}
					}

					if (baseMissionData.MissionCondition != null) {
						if (baseMissionData.MissionCondition()) {
							rewardText = (baseMissionData.MissionReward.Like > 0) ? "REWARD: +" + baseMissionData.MissionReward.Like + " to all" : string.Empty;
							UserInterface.RunTitle(baseMissionData.MissionTitle, "MISSION ACCOMPLISHED", rewardText, new FadeTransition(2f, null, null, MissionSuccess));
							gameManager.BasePlayerData.MissionsCompleted++;
							AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.MissionSuccess, true);
							audioSource.gameObject.SetActive(true);
							audioSource.Play();

							missionActive = false;
						}
					}
				}
			}
		}
	}

	public void MissionGUI(Event e) {
		if (baseMissionData != null && UserInterface.InactiveUI()) {
			if (showMissionButton) {
				MissionButton(e);
			}

			if(showMissionBox) {
				MissionBox(e);
			}

			if (missionActive) {
				MissionInformationGUI(e);
			}
		}
	}

	private void MissionButton(Event e) {
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, missionButtonAlpha);
		Rect missionButtonEffect = new Rect(Screen.width * 0.85f, Screen.height * 0.925f, Screen.width * 0.24f, Screen.height * 0.07f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref missionButtonEffect, Anchor.MiddleCenter);
		GUI.Box(missionButtonEffect, string.Empty, tempSkin.GetStyle("Block"));
		GUI.color = guiOriginalColor;

		Rect missionButton = new Rect(Screen.width * 0.85f, Screen.height * 0.925f, Screen.width * 0.21f, Screen.height * 0.05f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref missionButton, Anchor.MiddleCenter);
		GUI.Box(missionButton, string.Empty, missionSkin.GetStyle("Mission Alert Button"));


		if (missionButton.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showMissionButton = false;
				showMissionBox = true;
			}
		}
	}

	private void MissionBox(Event e) {
		GUI.BeginGroup(mainRect);
		Rect missionBoxRect = new Rect(mainRect.width * 0.85f, mainRect.height * origMessageBoxPosY, mainRect.width * 0.15f * screenHeightRatio, mainRect.height * 0.25f);
		AnchorPoint.SetAnchor(ref missionBoxRect, Anchor.MiddleCenter);
        GUI.Box(missionBoxRect, string.Empty, missionSkin.GetStyle("Mission Box"));

		GUI.BeginGroup(missionBoxRect);
        //Rect missionTitle = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.15f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.15f);
        //AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
        //GUI.Box(missionTitle, "Mission", missionSkin.GetStyle("Mission Title"));

		if (baseMissionData != null) {
			Rect missionName = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.25f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref missionName, Anchor.MiddleCenter);
			GUI.Box(missionName, baseMissionData.MissionName, missionSkin.GetStyle("Mission Name"));

            Rect missionDescBg = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.525f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.4f);
            AnchorPoint.SetAnchor(ref missionDescBg, Anchor.MiddleCenter);
            GUI.Box(missionDescBg, string.Empty, missionSkin.GetStyle("Mission Desc Box"));

			Rect missionDesc = new Rect(missionBoxRect.width * 0.45f, missionBoxRect.height * 0.55f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.3f);
			AnchorPoint.SetAnchor(ref missionDesc, Anchor.MiddleCenter);
			GUI.Box(missionDesc, string.Empty, GUIStyle.none);

			Rect descScrollView = new Rect(0f, 0f, missionBoxRect.width * 0.8f, missionBoxRect.height * 0.8f);
			content.text = baseMissionData.MissionDesc;
			descScrollView.height = missionSkin.GetStyle("Mission Desc").CalcHeight(content, missionBoxRect.width * 0.69f);
			descScrollPos = GUI.BeginScrollView(missionDesc, descScrollPos, descScrollView);

			Rect missionDescText = new Rect(
				missionBoxRect.width * 0.1f,
				0f,
				missionBoxRect.width * 0.75f,
				missionSkin.GetStyle("Mission Desc").CalcHeight(content, missionBoxRect.width * 0.69f)
			);

			GUI.Label(missionDescText, baseMissionData.MissionDesc, missionSkin.GetStyle("Mission Desc"));
			GUI.EndScrollView();
		}

		Rect missionButton = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.86f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref missionButton, Anchor.MiddleCenter);
		GUI.Box(missionButton, string.Empty, missionSkin.GetStyle("Mission Button Box"));

		GUI.BeginGroup(missionButton);
		Rect acceptButton = new Rect(missionButton.width * 0.5f, missionButton.height * 0.3f, missionButton.width * 0.9f, missionButton.height * 0.35f);
		AnchorPoint.SetAnchor(ref acceptButton, Anchor.MiddleCenter);
		GUI.Box(acceptButton, string.Empty, missionSkin.GetStyle("Mission Accept Button"));

		if (acceptButton.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showMissionBox = false;
				showMissionButton = false;
				RunMissionTitle();
			}
		}

		Rect cancelButton = new Rect(missionButton.width * 0.5f, missionButton.height * 0.7f, missionButton.width * 0.9f, missionButton.height * 0.35f);
		AnchorPoint.SetAnchor(ref cancelButton, Anchor.MiddleCenter);
        GUI.Box(cancelButton, string.Empty, missionSkin.GetStyle("Mission Cancel Button"));

		if (cancelButton.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showMissionButton = true;
				showMissionBox = false;
				origMessageBoxPosY = 0.9f;
				targetMessageBoxPosY = 0.65f;
			}
		}

		GUI.EndGroup();

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void MissionInformationGUI(Event e) {
		GUI.BeginGroup(mainRect);
		Rect infoBox = new Rect(mainRect.width * 0.85f, mainRect.height * 0.2f, mainRect.width * 0.25f, mainRect.height * 0.08f);
		AnchorPoint.SetAnchor(ref infoBox, Anchor.MiddleCenter);

		GUI.BeginGroup(infoBox);
		Rect missionTitle = new Rect(infoBox.width * 0.5f, infoBox.height * 0.25f, infoBox.width, infoBox.height * 0.2f);
		AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
		GUI.Box(missionTitle, baseMissionData.MissionName, missionSkin.GetStyle("Mission Info Title"));

		Rect missionDivider = new Rect(infoBox.width * 0.5f, infoBox.height * 0.5f, infoBox.width, infoBox.height * 0.1f);
		AnchorPoint.SetAnchor(ref missionDivider, Anchor.MiddleCenter);
		GUI.Box(missionDivider, string.Empty, missionSkin.GetStyle("Mission Divider"));

		if (missionTime > 0) {
			Rect missionTimer = new Rect(infoBox.width * 0.5f, infoBox.height * 0.75f, infoBox.width, infoBox.height * 0.2f);
			AnchorPoint.SetAnchor(ref missionTimer, Anchor.MiddleCenter);
			GUI.Box(missionTimer, GameUtility.ToHHMMSS(missionTime), missionSkin.GetStyle("Mission Info Time"));
		}

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void RunMissionButton() {
		showMissionBox = false;
		showMissionButton = false;
		StopCoroutine("ActivateMissionButton");
		StartCoroutine("ActivateMissionButton");
	}

	private IEnumerator ActivateMissionButton() {
		yield return new WaitForSeconds(1f);
		showMissionButton = true;
		origMessageBoxPosY = 0.9f;
		targetMessageBoxPosY = 0.65f;
		missionButtonTime = 0f;
		missionButtonAlpha = 1f;
		messageBoxVel = 0f;

		StopCoroutine("ActivateMissionButton");
	}

	public void RunMission() {
		if (missionIndx < MissionDatabase.MissionDataList.Length) {
			MissionNameID nameID = (MissionNameID)missionIndx;
			baseMissionData = MissionDatabase.GetMission(nameID);
			missionTime = baseMissionData.MissionDuration;

			if (nameID == MissionNameID.Mission1) {
				baseMissionData.MissionCondition = Mission1End;
			}
			else if (nameID == MissionNameID.Mission2) {
				baseMissionData.MissionCondition = Mission2End;
			}
			else if (nameID == MissionNameID.Mission3) {
				baseMissionData.MissionCondition = Mission3End;
			}
			else if (nameID == MissionNameID.Mission4) {
				baseMissionData.MissionCondition = Mission4End;
			}
			else if (nameID == MissionNameID.Mission5) {
				baseMissionData.MissionCondition = Mission5End;
			}
			else if (nameID == MissionNameID.Mission6) {
				baseMissionData.MissionCondition = Mission6End;
			}
			else if (nameID == MissionNameID.Mission7) {
				baseMissionData.MissionCondition = Mission7End;
			}
			else if (nameID == MissionNameID.Mission8) {
				baseMissionData.MissionCondition = Mission8End;
			}
			else if (nameID == MissionNameID.Mission9) {
				baseMissionData.MissionCondition = Mission9End;
			}
			else if (nameID == MissionNameID.Mission10) {
				baseMissionData.MissionCondition = Mission10End;
			}
			else if (nameID == MissionNameID.Mission11) {
				baseMissionData.MissionCondition = Mission11End;
			}
			else if (nameID == MissionNameID.Mission12) {
				baseMissionData.MissionCondition = Mission12End;
			}
			else if (nameID == MissionNameID.Mission13) {
				baseMissionData.MissionCondition = Mission13End;
			}

			RunMissionButton();
		}
		else {
			Debug.Log("[MISSION] ENDED!");
		}
	}

	private void RunMissionTitle() {
		if (baseMissionData != null) {
			baseMissionData.RunMissionTitle(new FadeTransition(2f, null, null, ActivateMission));
			if (baseMissionData.MissionNameID == MissionNameID.Mission3) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission3Start, ActivateMission));
			}
			else if (baseMissionData.MissionNameID == MissionNameID.Mission5) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission5Start, ActivateMission));
			}
			else if (baseMissionData.MissionNameID == MissionNameID.Mission7) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission7Start, ActivateMission));
			}
			else if (baseMissionData.MissionNameID == MissionNameID.Mission9) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission9Start, ActivateMission));
			}
			else if (baseMissionData.MissionNameID == MissionNameID.Mission11) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission11Start, ActivateMission));
			}
			else if (baseMissionData.MissionNameID == MissionNameID.Mission13) {
				baseMissionData.RunMissionTitle(new FadeTransition(2f, null, Mission13Start, ActivateMission));
			}

			//missionIndx++;
			_initialMission = (MissionNameID)missionIndx;
		}
	}

	# region Mission 1

	private bool Mission1End() {
		return (playerInformation.GetNpcCount() >= 6);
	}

	# endregion Mission 1

	# region Mission 2

	private bool Mission2End() {
		return (NPCDatabase.GetNpc(NPCNameID.Andy).NpcTalkedCount >= 5);
	}

	# endregion Mission 2

	// Andy
	# region Mission 3

	private void Mission3Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Andy);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission3End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Andy, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 3

	# region Mission 4

	private bool Mission4End() {
		return (NPCDatabase.GetNpc(NPCNameID.Noelle).NpcTalkedCount >= 5);
	}

	# endregion Mission 4

	// Noelle
	# region Mission 5

	private void Mission5Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Noelle);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission5End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Noelle, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 5

	# region Mission 6

	private bool Mission6End() {
		return (NPCDatabase.GetNpc(NPCNameID.Franz).NpcTalkedCount >= 5);
	}

	# endregion Mission 6

	// Franz
	# region Mission 7

	private void Mission7Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Franz);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission7End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Franz, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 7

	# region Mission 8

	private bool Mission8End() {
		return (NPCDatabase.GetNpc(NPCNameID.Bart).NpcTalkedCount >= 5);
	}

	# endregion Mission 8

	// Bart
	# region Mission 9

	private void Mission9Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Bart);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission9End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Bart, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 9

	# region Mission 10

	private bool Mission10End() {
		return (NPCDatabase.GetNpc(NPCNameID.Jenevieve).NpcTalkedCount >= 5);
	}

	# endregion Mission 10

	// Jen
	# region Mission 11

	private void Mission11Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Jenevieve);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission11End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Jenevieve, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 11

	# region Mission 12

	private bool Mission12End() {
		return (NPCDatabase.GetNpc(NPCNameID.Maxine).NpcTalkedCount >= 5);
	}

	# endregion Mission 12

	// Maxine
	# region Mission 13

	private void Mission13Start() {
		rng = new System.Random();
		data = NPCDatabase.GetNpc(NPCNameID.Maxine);
		indx = rng.Next(0, data.NpcItemsNeeded.Length);
	}

	private bool Mission13End() {
		ItemsNeeded itemNeeded = new ItemsNeeded();
		if (data != null) {
			itemNeeded = NPCDatabase.GetNpcItemNeeded(NPCNameID.Maxine, data.NpcItemsNeeded[indx].ItemID);
		}
		return (itemNeeded.ItemID != ItemNameID.None && itemNeeded.ItemRecieved);
	}

	# endregion Mission 13

	private void ActivateMission() {
		missionActive = true;
	}

	private void MissionFailed() {
		baseMissionData = null;
		missionActive = false;
		RunMission();
	}

	private void MissionSuccess() {
		baseMissionData = null;
		missionActive = false;
		missionIndx++;
		RunMission();
	}

	//private void DeactivateMission() {
	//    baseMissionData = null;
	//    missionActive = false;
	//    RunMission();
	//}

	private void Reset() {
		rng = new System.Random();
		_initialMission = initialMission;
		missionIndx = (int)_initialMission;
	}
}