using System.Collections.Generic;
using GameUtilities;
using GameUtilities.AnchorPoint;
using UnityEngine;
using NPC.Database;

public class HideNSeekUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin tempSkin;
    [SerializeField]
    private GUISkin progressSkin;
    [SerializeField]
    private GUISkin hidenseekSkin;

	# endregion

	# region Private Variables

	// Screen Variables
	private Rect mainRect;
	private float oldScreenHeightRatio;
	private float screenHeightRatio;
	private Color originalColor;

	private bool hideAndSeekEnabled;
	private bool sideMissionActive;
	private bool sideMissionHasStarted;
	private bool showGameInstruction = true;
	private float startCountDown;
    private float instructionTime;

	private float missionTimer;
	private TransitionEnd reward;

	private GameManager gameManager;
	private NPCManager npcManager;
	private MissionUi missionUi;

	# endregion Private Variables

	// Public Properties
	public bool HideAndSeekEnabled {
		get { return hideAndSeekEnabled; }
	}

	public bool ShowingHideNSeekInstruction { get; set; }

	public bool SideMissionActive {
		get { return sideMissionActive; }
	}
	// --

	public static HideNSeekUI current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		missionUi = MissionUi.current;
		startCountDown = 5f;
        instructionTime = 5f;
	}

	private void Update() {
		if (hideAndSeekEnabled) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (sideMissionActive && sideMissionHasStarted) {
				showGameInstruction = false;

				if (missionTimer > 0f) {
					missionTimer -= Time.deltaTime;
				}
				else {
					UserInterface.RunTitle(string.Empty, "MISSION FAILED", string.Empty, new FadeTransition(2f, null, null, GameEndDone));
					sideMissionActive = false;
					hideAndSeekEnabled = false;
				}

				if (HasFinished()) {
					reward();
					gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.HideNSeekSuccess, false));
					UserInterface.RunTitle(string.Empty, "MISSION ACCOMPLISHED", string.Empty, new FadeTransition(2f, null, null, GameEndDone));

					sideMissionActive = false;
					hideAndSeekEnabled = false;
				}
			}
			else {
				if (startCountDown > 0f) {
					startCountDown -= Time.deltaTime;
					startCountDown = Mathf.Clamp(startCountDown, 0f, 3f);
				}
				else {
					if (ShowingHideNSeekInstruction) {
						showGameInstruction = false;
						UserInterface.RunTitle("MISSION", "Hide 'n Seek", "All of the characters are hiding, find them!", new FadeTransition(2f, null, null, GameStart));
						sideMissionActive = true;
						ShowingHideNSeekInstruction = false;
					}
				}
			}
		}
	}

	public void MainGUI(Event e) {
		if (hideAndSeekEnabled) {
			if (sideMissionActive && sideMissionHasStarted && UserInterface.InactiveUI()) {
				DrawInformation();
			}
			else {
				if (ShowingHideNSeekInstruction) {
					if (showGameInstruction) {
						DrawInstructions();
					}
					else {
						DrawCountDown();
					}
				}
			}
		}
	}

	private void DrawInstructions() {
		GUI.BeginGroup(mainRect);
		Rect instructionBoxRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.5f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionBoxRect, Anchor.MiddleCenter);
        GUI.Box(instructionBoxRect, string.Empty, hidenseekSkin.GetStyle("Hide and Seek Instruction Box"));

		GUI.BeginGroup(instructionBoxRect);

        //Rect instructionTitle = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.1f, instructionBoxRect.width * 0.9f, instructionBoxRect.height * 0.15f);
        //AnchorPoint.SetAnchor(ref instructionTitle, Anchor.MiddleCenter);
        //GUI.Box(instructionTitle, "INSTRUCTIONS!", tempSkin.GetStyle("Block"));

		Rect instructionRect = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.5f, instructionBoxRect.width * 0.85f, instructionBoxRect.height * 0.7f);
		AnchorPoint.SetAnchor(ref instructionRect, Anchor.MiddleCenter);
        GUI.Box(instructionRect, string.Empty, hidenseekSkin.GetStyle("Hide and Seek Secondary"));
        //if (Resources.HideNSeekInstruction != null) {
        //    GUI.DrawTexture(instructionRect, Resources.HideNSeekInstruction);
        //}
        //else {
			
        //}

		Rect countdownRect = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.9f, instructionBoxRect.width * 0.8f, instructionBoxRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref countdownRect, Anchor.MiddleCenter);
        //GUI.Box(countdownRect, "STARTING IN \n" + startCountDown.ToString("F1"), hidenseekSkin.GetStyle("Hide and Seek Timer Bar"));

        UserInterface.ProgressBar(
            "Get Ready in " + (int)startCountDown + " ...",
            startCountDown,
            instructionTime,
            countdownRect,
            progressSkin.GetStyle("Progress Bar BG"),
            progressSkin.GetStyle("Progress Bar Overlay")
        );

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void DrawCountDown() {
		GUI.BeginGroup(mainRect);
		Rect countDownBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.2f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref countDownBox, Anchor.MiddleCenter);
		GUI.Box(countDownBox, string.Empty, tempSkin.GetStyle("Block"));

		GUI.BeginGroup(countDownBox);
		Rect titleRect = new Rect(countDownBox.width * 0.5f, countDownBox.height * 0.3f, countDownBox.width * 0.9f, countDownBox.height * 0.45f);
		AnchorPoint.SetAnchor(ref titleRect, Anchor.MiddleCenter);
		GUI.Box(titleRect, "Hide n' Seek", tempSkin.GetStyle("Block"));

		Rect countdownRect = new Rect(countDownBox.width * 0.5f, countDownBox.height * 0.7f, countDownBox.width * 0.9f, countDownBox.height * 0.45f);
		AnchorPoint.SetAnchor(ref countdownRect, Anchor.MiddleCenter);
		GUI.Box(countdownRect, "STARTING IN \n" + startCountDown.ToString("F1"), tempSkin.GetStyle("Block"));

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void DrawInformation() {
		GUI.BeginGroup(mainRect);
		Rect hideAndSeekBox = (missionUi.MissionActive)
			? new Rect(mainRect.width * 0.85f, mainRect.height * 0.28f, mainRect.width * 0.15f * screenHeightRatio, mainRect.height * 0.1f)
			: new Rect(mainRect.width * 0.85f, mainRect.height * 0.2f, mainRect.width * 0.15f * screenHeightRatio, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref hideAndSeekBox, Anchor.MiddleCenter);

		GUI.BeginGroup(hideAndSeekBox);
		Rect hideAndSeekTitle = new Rect(hideAndSeekBox.width * 0.5f, hideAndSeekBox.height * 0.35f, hideAndSeekBox.width, hideAndSeekBox.height * 0.4f);
		AnchorPoint.SetAnchor(ref hideAndSeekTitle, Anchor.MiddleCenter);
		GUI.Box(hideAndSeekTitle, "Character List", tempSkin.GetStyle("Text"));

		Rect hideAndSeekDivider = new Rect(hideAndSeekBox.width * 0.5f, hideAndSeekBox.height * 0.5f, hideAndSeekBox.width, hideAndSeekBox.height * 0.1f);
		AnchorPoint.SetAnchor(ref hideAndSeekDivider, Anchor.MiddleCenter);
		GUI.DrawTexture(hideAndSeekDivider, tempSkin.GetStyle("Divider").normal.background);

		GUI.EndGroup();

		for (int i = 0; i < npcManager.Npc.Length; i++) {
			NPCInformation npcInformation = npcManager.Npc[i].GetComponent<NPCInformation>();

			Rect npcName = (missionUi.MissionActive)
				? new Rect(mainRect.width * 0.85f, (mainRect.height * 0.27f) + ((mainRect.height * 0.03f) * (i + 1)), mainRect.width, mainRect.height * 0.4f)
				: new Rect(mainRect.width * 0.85f, (mainRect.height * 0.19f) + ((mainRect.height * 0.03f) * (i + 1)), mainRect.width, mainRect.height * 0.4f);
			AnchorPoint.SetAnchor(ref npcName, Anchor.MiddleCenter);
			GUI.Box(npcName, npcInformation.BaseNpcData.NpcName, tempSkin.GetStyle("Text"));

			if (npcInformation.BaseNpcData.NpcFound) {
				Rect nameStrikeThrough = (missionUi.MissionActive)
					? new Rect(
						mainRect.width * 0.85f,
						(mainRect.height * 0.27f) + ((mainRect.height * 0.03f) * (i + 1)),
						mainRect.width * 0.3f,
						mainRect.height * 0.01f)
					: new Rect(
						mainRect.width * 0.85f,
						(mainRect.height * 0.19f) + ((mainRect.height * 0.03f) * (i + 1)),
						mainRect.width * 0.2f,
						mainRect.height * 0.01f);
				AnchorPoint.SetAnchor(ref nameStrikeThrough, Anchor.MiddleCenter);
				GUI.Box(nameStrikeThrough, string.Empty, tempSkin.GetStyle("Divider"));
			}
			else {
				Rect npcAvatar = (missionUi.MissionActive)
					? new Rect(mainRect.width * 0.73f, (mainRect.height * 0.27f) + ((mainRect.height * 0.03f) * (i + 1)), mainRect.width * 0.03f, mainRect.height * 0.03f)
					: new Rect(mainRect.width * 0.73f, (mainRect.height * 0.19f) + ((mainRect.height * 0.03f) * (i + 1)), mainRect.width * 0.03f, mainRect.height * 0.03f);
				AnchorPoint.SetAnchor(ref npcAvatar, Anchor.MiddleCenter);
				GUI.DrawTexture(npcAvatar, npcInformation.BaseNpcData.NpcAvatar);
			}
		}

		hideAndSeekDivider = (missionUi.MissionActive)
			? new Rect(mainRect.width * 0.85f, mainRect.height * 0.47f, mainRect.width * 0.25f, mainRect.height * 0.01f)
			: new Rect(mainRect.width * 0.85f, mainRect.height * 0.39f, mainRect.width * 0.25f, mainRect.height * 0.01f);
		AnchorPoint.SetAnchor(ref hideAndSeekDivider, Anchor.MiddleCenter);
		GUI.DrawTexture(hideAndSeekDivider, tempSkin.GetStyle("Divider").normal.background);

		Rect hideAndSeekTimer = (missionUi.MissionActive)
			? new Rect(mainRect.width * 0.85f, mainRect.height * 0.49f, mainRect.width * 0.25f, mainRect.height * 0.05f)
			: new Rect(mainRect.width * 0.85f, mainRect.height * 0.42f, mainRect.width * 0.25f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref hideAndSeekTimer, Anchor.MiddleCenter);
		GUI.Box(hideAndSeekTimer, GameUtility.ToHHMMSS(missionTimer), tempSkin.GetStyle("Text"));

		GUI.EndGroup();
	}

	public void RunHideAndSeek(TransitionEnd hideNSeekReward) {
		List<Transform> npcSpawnPoints = new List<Transform>();
		GameUtility.GetChildrenRecursively(gameManager.BaseLevel.HideAndSeekSpawnPoints, ref npcSpawnPoints);
		System.Random rng = new System.Random();
		int randIndx = 0;

		if (npcManager.Npc != null) {
			for (int i = 0; i < npcManager.Npc.Length; i++) {
				if (npcManager.Npc[i] != null) {
					randIndx = rng.Next(0, npcSpawnPoints.Count);
					NPCControl npcControl = npcManager.Npc[i].GetComponent<NPCControl>();
					npcControl.NpcInformation.ObjectEnabled = false;
					npcControl.InitializeNpcControl(npcSpawnPoints[randIndx].position, npcSpawnPoints[randIndx].rotation, npcManager.FirstFloor, npcManager.SecondFloor);
					npcControl.NpcInformation.ObjectEnabled = true;
					npcSpawnPoints.RemoveAt(randIndx);

					NPCInformation npcInformation = npcManager.Npc[i].GetComponent<NPCInformation>();
					npcInformation.BaseNpcData.NpcFound = false;
				}
			}
		}

		gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.HideNSeekBGM, false));

        instructionTime = 5f;
		startCountDown = 5f;
		missionTimer = 240f;
		reward = hideNSeekReward;
		ShowingHideNSeekInstruction = true;
		hideAndSeekEnabled = true;
	}

	private bool HasFinished() {
		int count = 0;
		for (int i = 0; i < npcManager.Npc.Length; i++) {
			NPCInformation npcInformation = npcManager.Npc[i].GetComponent<NPCInformation>();
			if (npcInformation.BaseNpcData.NpcFound) {
				count++;
			}
		}
		return (count >= npcManager.Npc.Length);
	}

	private void GameStart() {
		sideMissionHasStarted = true;
	}

	private void GameEndDone() {
		gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.MainGameBGM, false));
	}
}