using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using Mission.Database;
using UnityEngine;

public class MissionUi : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin missionSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;
	private Color guiOriginalColor;

	private string missionMainName;
	private string missionMainDesc;
	private int missionIndx;

	private bool missionButtonActive;
	private bool showMissionButton;
	private float missionButtonTime;
	private float missionButtonAlpha;

	private bool hasAcceptedMission;
	private bool showMissionTitle;
	private bool showMissionInfo;
	private float missionTitleAlpha;

	private GUIContent content = new GUIContent();
	private Vector2 descScrollPos;
	private string missionDescription;

	private float origMessageBoxPosY;
	private float targetMessageBoxPosY;
	private float messageBoxVel;
	private float messageBoxDampTime = 0.3f;

	private float missionTime;

	private GameManager gameManager;
	private PlayerInformation playerInformation;
	private MissionManager missionManager;
	private UserInterface userInterface;

	# endregion Private Variables

	public bool ShowMissionInfo {
		get { return showMissionInfo; }
		set { showMissionInfo = value; }
	}

	public bool HasAcceptedMission {
		get { return hasAcceptedMission; }
		set { hasAcceptedMission = value; }
	}

	public float MissionTime {
		get { return missionTime; }
	}

	public static MissionUi current;

	private void Awake() {
		current = this;
		MissionDatabase.Initialize();
	}

	private void Start() {
		gameManager = GameManager.current;
		missionManager = MissionManager.current;
		userInterface = UserInterface.current;
	}

	private void Update() {
		if (playerInformation == null && gameManager.BasePlayerData != null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
		}

		if (gameManager.GameState == GameState.MainGame && missionManager.BaseMissionData != null) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (hasAcceptedMission && missionTime > 0f) {
				missionTime -= Time.deltaTime;

				if (missionTime <= 0f) {
					if (missionManager.BaseMissionData.Mission.MissionFailed != null) {
						missionManager.BaseMissionData.Mission.MissionFailed();
					}

					if (missionManager.BaseMissionData.Mission.MissionResult != null) {
						missionManager.BaseMissionData.Mission.MissionResult();
					}

					hasAcceptedMission = false;
				}
			}

			if (!showMissionButton && missionButtonActive) {
				missionButtonTime += Time.deltaTime;
				missionButtonAlpha = Mathf.Lerp(1f, 0.1f, Mathf.PingPong(missionButtonTime, 0.5f) / 0.5f);
			}
			else {
				if (origMessageBoxPosY != targetMessageBoxPosY) {
					origMessageBoxPosY = Mathf.SmoothDamp(origMessageBoxPosY, targetMessageBoxPosY, ref messageBoxVel, messageBoxDampTime);
				}
			}
		}
	}

	public void MissionGUI(Event e) {
		if (missionManager.BaseMissionData != null && userInterface.InactiveUI()) {
			if (missionButtonActive) {
				if (showMissionButton) {
					MissionBox(e);
				}
				else {
					MissionButton(e);
				}
			}

			if (showMissionTitle) {
				MissionTitle(e);
			}
		}
	}

	public void MissionInformation(Event e) {
		GUI.BeginGroup(mainRect);
		Rect missionInfoBox = new Rect(mainRect.width * 0.85f, mainRect.height * 0.23f, mainRect.width * 0.15f * screenHeightRatio, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref missionInfoBox, Anchor.MiddleCenter);

		GUI.BeginGroup(missionInfoBox);
		Rect missionTitle = new Rect(missionInfoBox.width * 0.5f, missionInfoBox.height * 0.35f, missionInfoBox.width, missionInfoBox.height * 0.4f);
		AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
		GUI.Box(missionTitle, missionManager.BaseMissionData.MissionName, missionSkin.GetStyle("Mission Info Title"));

		Rect missionDivider = new Rect(missionInfoBox.width * 0.5f, missionInfoBox.height * 0.5f, missionInfoBox.width, missionInfoBox.height * 0.1f);
		AnchorPoint.SetAnchor(ref missionDivider, Anchor.MiddleCenter);
		GUI.Box(missionDivider, string.Empty, missionSkin.GetStyle("Mission Divider"));

		Rect missionTimer = new Rect(missionInfoBox.width * 0.5f, missionInfoBox.height * 0.65f, missionInfoBox.width, missionInfoBox.height * 0.4f);
		AnchorPoint.SetAnchor(ref missionTimer, Anchor.MiddleCenter);
		GUI.Box(missionTimer, GameUtility.ToHHMMSS(missionTime), missionSkin.GetStyle("Mission Info Time"));

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void MissionButton(Event e) {
		GUI.BeginGroup(mainRect);

		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, missionButtonAlpha);
		Rect missionButtonEffect = new Rect(mainRect.width * 0.89f, mainRect.height * 0.77f, mainRect.width * 0.11f * screenHeightRatio, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref missionButtonEffect, Anchor.MiddleCenter);
		GUI.Box(missionButtonEffect, string.Empty, tempSkin.GetStyle("Block"));
		GUI.color = guiOriginalColor;

		Rect missionButton = new Rect(mainRect.width * 0.89f, mainRect.height * 0.77f, mainRect.width * 0.1f * screenHeightRatio, mainRect.height * 0.03f);
		AnchorPoint.SetAnchor(ref missionButton, Anchor.MiddleCenter);
		GUI.Box(missionButton, "New Mission", tempSkin.GetStyle("Button"));

		if (missionButton.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showMissionButton = true;
			}
		}

		GUI.EndGroup();
	}

	private void MissionBox(Event e) {
		GUI.BeginGroup(mainRect);
		Rect missionBoxRect = new Rect(mainRect.width * 0.85f, mainRect.height * origMessageBoxPosY, mainRect.width * 0.15f * screenHeightRatio, mainRect.height * 0.25f);
		AnchorPoint.SetAnchor(ref missionBoxRect, Anchor.MiddleCenter);
		GUI.Box(missionBoxRect, string.Empty, tempSkin.GetStyle("Block"));

		GUI.BeginGroup(missionBoxRect);
		Rect missionTitle = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.15f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
		GUI.Box(missionTitle, "Mission", missionSkin.GetStyle("Mission Title"));

		if (missionManager.BaseMissionData != null) {
			Rect missionName = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.3f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref missionName, Anchor.MiddleCenter);
			GUI.Box(missionName, missionManager.BaseMissionData.MissionName, missionSkin.GetStyle("Mission Name"));

			Rect missionDescBg = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.55f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.4f);
			AnchorPoint.SetAnchor(ref missionDescBg, Anchor.MiddleCenter);
			GUI.Box(missionDescBg, string.Empty, tempSkin.GetStyle("Block"));

			Rect missionDesc = new Rect(missionBoxRect.width * 0.45f, missionBoxRect.height * 0.55f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.3f);
			AnchorPoint.SetAnchor(ref missionDesc, Anchor.MiddleCenter);
			GUI.Box(missionDesc, string.Empty, GUIStyle.none);

			Rect descScrollView = new Rect(0f, 0f, missionBoxRect.width * 0.8f, missionBoxRect.height * 0.8f);
			content.text = missionDescription;
			descScrollView.height = missionSkin.GetStyle("Mission Desc").CalcHeight(content, missionBoxRect.width * 0.69f);
			descScrollPos = GUI.BeginScrollView(missionDesc, descScrollPos, descScrollView);

			Rect missionDescText = new Rect(
				missionBoxRect.width * 0.1f,
				0f,
				missionBoxRect.width * 0.75f,
				missionSkin.GetStyle("Mission Desc").CalcHeight(content, missionBoxRect.width * 0.69f)
			);

			GUI.Label(missionDescText, missionDescription, missionSkin.GetStyle("Mission Desc"));
			GUI.EndScrollView();

			Rect missionButton = new Rect(missionBoxRect.width * 0.5f, missionBoxRect.height * 0.825f, missionBoxRect.width * 0.9f, missionBoxRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref missionButton, Anchor.MiddleCenter);
			GUI.Box(missionButton, string.Empty, missionSkin.GetStyle("Mission Button Box"));

			GUI.BeginGroup(missionButton);
			Rect acceptButton = new Rect(missionButton.width * 0.3f, missionButton.height * 0.5f, missionButton.width * 0.35f, missionButton.height * 0.7f);
			AnchorPoint.SetAnchor(ref acceptButton, Anchor.MiddleCenter);
			GUI.Box(acceptButton, "Accept", missionSkin.GetStyle("Mission Accept Button"));

			if (acceptButton.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					missionButtonActive = false;
					hasAcceptedMission = true;

					if (missionManager.BaseMissionData != null) {
						if (missionManager.BaseMissionData.Mission.MissionEnded != null) {
							if (!missionManager.BaseMissionData.Mission.MissionEnded()) {
								RunMissionTitle(
									missionManager.BaseMissionData.MissionName,
									missionManager.BaseMissionData.MissionDesc,
									new FadeTransition(null, null, EndMissionTitle)
								);
							}
						}
					}
				}
			}

			Rect cancelButton = new Rect(missionButton.width * 0.7f, missionButton.height * 0.5f, missionButton.width * 0.35f, missionButton.height * 0.7f);
			AnchorPoint.SetAnchor(ref cancelButton, Anchor.MiddleCenter);
			GUI.Box(cancelButton, "Decline", missionSkin.GetStyle("Mission Cancel Button"));

			if (cancelButton.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					showMissionButton = false;
					origMessageBoxPosY = 0.9f;
					targetMessageBoxPosY = 0.65f;
				}
			}
			GUI.EndGroup();
		}
		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void MissionTitle(Event e) {
		GUI.BeginGroup(mainRect);
		Rect missionTitleBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width, mainRect.height * 0.2f);
		AnchorPoint.SetAnchor(ref missionTitleBox, Anchor.MiddleCenter);
		//GUI.Box(missionTitleBox, string.Empty);

		GUI.BeginGroup(missionTitleBox);
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, missionTitleAlpha);

		Rect missionTitle = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.2f, missionTitleBox.width * 0.9f, missionTitleBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
		GUI.Box(missionTitle, missionMainName, missionSkin.GetStyle("Mission Main Title"));

		Rect missionDivider = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.35f, missionTitleBox.width * 0.8f, missionTitleBox.height * 0.1f);
		AnchorPoint.SetAnchor(ref missionDivider, Anchor.MiddleCenter);
		GUI.Box(missionDivider, string.Empty, missionSkin.GetStyle("Mission Divider"));

		Rect missionName = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.5f, missionTitleBox.width * 0.9f, missionTitleBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref missionName, Anchor.MiddleCenter);
		GUI.Box(missionName, missionMainDesc, missionSkin.GetStyle("Mission Main Name"));
		GUI.color = guiOriginalColor;

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void RunMissionButton(float time) {
		missionButtonActive = false;
		hasAcceptedMission = false;
		showMissionButton = false;
		StartCoroutine(MissionButton(time));
	}

	private IEnumerator MissionButton(float time) {
		yield return new WaitForSeconds(time);
		missionButtonActive = true;
		origMessageBoxPosY = 0.9f;
		targetMessageBoxPosY = 0.65f;
		missionButtonTime = 0f;
		missionButtonAlpha = 1f;
		messageBoxVel = 0f;
		missionDescription = missionManager.BaseMissionData.MissionDesc + "\n\n<size='12'>[DURATION: " + missionManager.BaseMissionData.MissionDuration + "s.]</size>";
		
	}

	public void RunMissionTitle(string name, string desc, FadeTransition fadeT) {
		missionMainName = name;
		missionMainDesc = desc;
		missionTitleAlpha = 0f;

		if (fadeT.StartTransition != null) {
			fadeT.StartTransition();
		}

		StartCoroutine("MissionTitleFade", fadeT);
	}

	private IEnumerator MissionTitleFade(FadeTransition fadeT) {
		showMissionTitle = true;
		float time = 0f;
		while (time < 1f) {
			time += Time.deltaTime;
			missionTitleAlpha = Mathf.Lerp(missionTitleAlpha, 1f, Mathf.PingPong(time, 3f) / 3f);
			yield return null;
		}

		if (fadeT.Midtransition != null) {
			fadeT.Midtransition();
		}

		yield return new WaitForSeconds(1f);

		time = 0f;
		while (time < 1f) {
			time += Time.deltaTime;
			missionTitleAlpha = Mathf.Lerp(missionTitleAlpha, 0f, Mathf.PingPong(time, 3f) / 3f);
			yield return null;
		}

		if (fadeT.EndTransition != null) {
			fadeT.EndTransition();
		}

		showMissionTitle = false;
		StopCoroutine("MissionTitleFade");
	}

	private void EndMissionTitle() {
		showMissionButton = false;
		showMissionTitle = false;

		if (missionManager.BaseMissionData != null && !missionManager.BaseMissionData.Mission.MissionEnded()) {
			showMissionInfo = true;
			missionTime = missionManager.BaseMissionData.MissionDuration;
		}
	}
}