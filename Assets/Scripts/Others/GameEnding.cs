using UnityEngine;
using System.Collections;
using NPC.Database;
using GameUtilities.PlayerUtility;
using GameUtilities.AnchorPoint;

public enum EndingType {
	None,
	Ending1,
	Ending2,
	Ending3,
	Ending4,
	TrueEnding
}

public class GameEnding : BaseInteractedObject {

	# region Public Variables

    [SerializeField]
    private GUISkin theaterSkin;
	[SerializeField]
	private Cutscene ending1;
	[SerializeField]
	private Cutscene ending2;
	[SerializeField]
	private Cutscene ending3;
	[SerializeField]
	private Cutscene ending4;
	[SerializeField]
	private Cutscene ending5;

	# endregion Public Variables

	# region Private Variables

	private Cutscene baseCutscene;
	private bool canEnterTheater;
	private bool showTheaterWindow;

	private string[] keyString = new string[2] { "[ESC] Cancel", "[Enter] Join" };

	private EndingType endingType;
	private GameEndingData baseGameEndingData;
	private GameEndingData[] gameEndingData;

	# endregion Private Variables

	// Public Property
	public GameEndingData BaseGameEndingData {
		get { return baseGameEndingData; }
	}

	public bool ShowTheaterWindow {
		get { return showTheaterWindow; }
	}

	public EndingType EndingType {
		get { return endingType; }
	}

	public Cutscene BaseCutscene {
		get { return baseCutscene; }
	}
	// --

	public static GameEnding current;

	private void Awake() {
		current = this;
	}

	protected override void Start() {
		base.Start();
		PopulateGameEnding();

		canEnterTheater = false;
		showTheaterWindow = false;
		endingType = EndingType.None;
	}

	protected override void Update() {
		base.Update();
		if (gameManager.GameState == GameState.MainGame) {
			if (objectTooltipEnabled) {
				if (UserInterface.InactiveUI()) {
					if (Input.GetButtonDown(PlayerUtility.EnterTheatre)) {
						if (RunFifthEnding()) { endingType = EndingType.TrueEnding; }
						else if (RunFourthEnding()) { endingType = EndingType.Ending4; }
						else if (RunThirdEnding()) { endingType = EndingType.Ending3; }
						else if (RunSecondEnding()) { endingType = EndingType.Ending2; }
						else if (RunFirstEnding()) { endingType = EndingType.Ending1; }
						else endingType = EndingType.None;

						// When ending type is not none and player has talked to all the npcs
						canEnterTheater = (endingType != EndingType.None) && (playerInformation.GetNpcCount() == npcManager.Npc.Length);
						showTheaterWindow = true;
					}
				}

				if (Input.GetButtonDown(PlayerUtility.JoinGameJam)) {
					Debug.Log(endingType.ToString() + "Run Animation Clip");
					showTheaterWindow = false;
					gameManager.SwitchGameState(GameState.EndingResult);
					StartCoroutine(WaitTransition());
				}
			}

			if (Input.GetButtonDown(PlayerUtility.ExitWindow)) {
				showTheaterWindow = false;
			}
		}
	}

	protected override void OnGUI() {
		base.OnGUI();
		if (gameManager.GameState == GameState.MainGame) {
			if (objectTooltipEnabled && UserInterface.InactiveUI()) {
				InteractTooltip();
			}

			if (showTheaterWindow) {
				e = Event.current;
				DrawInstructions();
			}
		}
	}

	public override void ResetObject() {
		base.ResetObject();
	}

	private void InteractTooltip() {
		Rect interactTooltip = new Rect(objectTooltipToScreen.x, Screen.height - objectTooltipToScreen.y, mainRect.width * 0.22f, mainRect.height * 0.04f);
		AnchorPoint.SetAnchor(ref interactTooltip, Anchor.BottomCenter);

		GUI.BeginGroup(interactTooltip);
		Rect tooltipRect = new Rect(interactTooltip.width * 0.5f, interactTooltip.height * 0.5f, interactTooltip.width * 0.95f, interactTooltip.height * 0.9f);
		AnchorPoint.SetAnchor(ref tooltipRect, Anchor.MiddleCenter);
		GUI.Box(tooltipRect, "Enter theatre?", tempSkin.GetStyle("Block"));
		GUI.EndGroup();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, string.Empty);

		GUI.BeginGroup(textRect);
		Rect keyText = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.35f, textRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
		GUI.Box(keyText, "[E] Enter", tempSkin.GetStyle("Text"));
		GUI.EndGroup();
	}

	private void DrawInstructions() {
		GUI.BeginGroup(mainRect);
		Rect instructionBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.6f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionBox, Anchor.MiddleCenter);
		GUI.Box(instructionBox, string.Empty, theaterSkin.GetStyle("Theater BG"));

		GUI.BeginGroup(instructionBox);
        //Rect titleBox = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.125f, instructionBox.width * 0.9f, instructionBox.height * 0.1f);
        //AnchorPoint.SetAnchor(ref titleBox, Anchor.MiddleCenter);
        //GUI.Box(titleBox, "Instructions", theaterSkin.GetStyle("Theater Instruction"));

		Rect messageBox = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.425f, instructionBox.width * 0.9f, instructionBox.height * 0.7f);
		AnchorPoint.SetAnchor(ref messageBox, Anchor.MiddleCenter);
        GUI.Box(messageBox, string.Empty, theaterSkin.GetStyle("Theater Instruction"));

		if (canEnterTheater) {
			Rect canEnterText = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.82f, instructionBox.width * 0.9f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref canEnterText, Anchor.MiddleCenter);
            GUI.Box(canEnterText, "You may now enter game jam", theaterSkin.GetStyle("Theater Info"));

			Rect enterBtn = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.91f, instructionBox.width * 0.3f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref enterBtn, Anchor.MiddleCenter);
            GUI.Box(enterBtn, string.Empty, theaterSkin.GetStyle("Theater Button"));

			if (enterBtn.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseDown) {
					Debug.Log(endingType.ToString() + "Run Animation Clip");
					showTheaterWindow = false;
					gameManager.SwitchGameState(GameState.EndingResult);
					StartCoroutine(WaitTransition());
					//baseGameEndingData = GetGameEnding(endingType);
					//GameManager.current.SwitchGameState(GameState.GameEnd);
				}
			}
		}
		else {
			Rect cannotEnterText = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.87f, instructionBox.width * 0.9f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref cannotEnterText, Anchor.MiddleCenter);
            GUI.Box(cannotEnterText, "You need more experience to enter game jam", theaterSkin.GetStyle("Theater Info"));
		}

		# region Exit
		Rect exitRect = new Rect(instructionBox.width * 0.915f, instructionBox.height * 0.07f, mainRect.width * 0.03f, mainRect.height * 0.03f);
		AnchorPoint.SetAnchor(ref exitRect, Anchor.MiddleCenter);
		GUI.Box(exitRect, "X", theaterSkin.GetStyle("Theater Exit Button"));

		if (exitRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showTheaterWindow = false;
			}
		}

		# endregion Exit

		GUI.EndGroup();

				GUI.EndGroup();

		Rect keyText = new Rect();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, string.Empty);

		GUI.BeginGroup(textRect);
		if (canEnterTheater) {
			for (int i = 0; i < keyString.Length; i++) {
				keyText = new Rect((textRect.width * 0.15f) * i + (textRect.width * 0.425f), textRect.height * 0.5f, textRect.width * 0.2f, textRect.height);
				AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
				GUI.Box(keyText, keyString[i], tempSkin.GetStyle("Text"));
			}
		}
		else {
			Rect inventoryTextRect = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.26f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref inventoryTextRect, Anchor.MiddleCenter);
			GUI.Box(inventoryTextRect, "[ESC] Cancel", tempSkin.GetStyle("Text"));
		}
		GUI.EndGroup();
	}

	private void PopulateGameEnding() {
		gameEndingData = new GameEndingData[5];

		gameEndingData[0] = new GameEndingData(
			"The Ending 1",
			EndingType.Ending1,
            "<color=#001829><size=20><b>You got convinced little kid!</b></size> <size=12> \n You met a lot of friends who encouraged you to join the Global Game Jam. Hand in hand, you all entered the doors to opportunity, towards the 3 days and 2 nights journey.</size></color>"
		);

		gameEndingData[1] = new GameEndingData(
			"The Ending 2",
			EndingType.Ending2,
            "<color=#001829><size=20><b>Determined!</b></size> <size=12> \n You met a few who told you about the Global Game Jam. You hesitated, but, remembered their words of what you are able to do on the event.\nWith that, you turned that door knob that will open you to more possibilities and a chance.</size></color>"
		);

		gameEndingData[2] = new GameEndingData(
			"The Ending 3",
			EndingType.Ending3,
            "<color=#001829><size=20><b>Doubts and Fears</b></size> <size=14>\n Hesitations overcame determination. Instead of opening that door, you turned away and left, giving up.</size></color>"
		);

		gameEndingData[3] = new GameEndingData(
			"The Ending 4",
			EndingType.Ending4,
            "<color=#001829><size=20><b>Black and Blue</b></size> <size=14> \n You were either too depressed or all the people you met were angry at you so you decided not to go to the event.</size></color>"
		);

		gameEndingData[4] = new GameEndingData(
			"True Ending",
			EndingType.TrueEnding,
            "<color=#001829><size=20><b>Victory!</b></size> <size=14> \n You joined the game jam and won with your teammates!</size></color>"
		);
	}

	private IEnumerator WaitTransition() {
		yield return new WaitForSeconds(0.5f);
		if (endingType == EndingType.Ending1) {
			baseCutscene = ending1;
			ending1.RunCutscene(ShowGameEnd);
		}
		else if (endingType == EndingType.Ending2) {
			baseCutscene = ending2;
			ending2.RunCutscene(ShowGameEnd);
		}
		else if (endingType == EndingType.Ending3) {
			baseCutscene = ending3;
			ending3.RunCutscene(ShowGameEnd);
		}
		else if (endingType == EndingType.Ending4) {
			baseCutscene = ending4;
			ending4.RunCutscene(ShowGameEnd);
		}
		else if (endingType == EndingType.TrueEnding) {
			baseCutscene = ending5;
			ending5.RunCutscene(ShowGameEnd);
		}
	}

	private void ShowGameEnd() {
		baseGameEndingData = GetGameEnding(endingType);
		gameManager.SwitchGameState(GameState.GameEnd);
	}

	public GameEndingData GetGameEnding(EndingType gameEndType) {
		for (int i = 0; i < gameEndingData.Length; i++) {
			if (gameEndingData[i] != null && gameEndingData[i].EndingType == gameEndType) {
				return gameEndingData[i];
			}
		}
		return null;
	}

		// Condition 1:
	// All NPC's get 90 or more like points
	public bool RunFirstEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.Npc.Length; i++) {
			if (npcManager.Npc != null && NPCDatabase.NPCDataInfoList[i].NpcStatistics.Like >= 90) {
				count++;
			}
		}
		return (count == npcManager.Npc.Length);
	}

	// Condition 2:
	// 3 or more NPC's get at 70 or more like points
	public bool RunSecondEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.Npc.Length; i++) {
			if (npcManager.Npc != null && NPCDatabase.NPCDataInfoList[i].NpcStatistics.Like >= 70) {
				count++;
			}
		}
		return (count >= 3);
	}

	// Condition 3:
	// No NPC's get at 70 or more like points
	public bool RunThirdEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.Npc.Length; i++) {
			if (npcManager.Npc != null && NPCDatabase.NPCDataInfoList[i].NpcStatistics.Like <= 70) {
				count++;
			}
		}
		return (count == npcManager.Npc.Length);
	}

	// Condition 4:
	// Any NPC get 80 or more dislike points
	public bool RunFourthEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.Npc.Length; i++) {
			if (npcManager.Npc != null && NPCDatabase.NPCDataInfoList[i].NpcStatistics.Dislike >= 80) {
				count++;
			}
		}
		return (count > 0);
	}

	// Condition 5:
	// Player gets 100 in like points in all npc
	public bool RunFifthEnding() {
		//int count = 0;
		// If player gets all ending
		return (gameManager.BasePlayerData != null && gameManager.BasePlayerData.PlayerStatistics.Like == 100);
	}
}

public class GameEndingData {
	public GameEndingData() {
		endingTitle = string.Empty;
		endingType = global::EndingType.None;
		endingDesc = string.Empty;
	}

	public GameEndingData(string name, EndingType type, string desc) {
		endingTitle = name;
		endingType = type;
		endingDesc = desc;
	}

	private string endingTitle;
	public string EndingTitle {
		get { return endingTitle; }
	}

	private EndingType endingType;
	public EndingType EndingType {
		get { return endingType; }
	}

	private string endingDesc;
	public string EndingDesc {
		get { return endingDesc; }
	}
}