using GameEnd;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using GameUtilities.PlayerUtility;
using UnityEngine;

public class GameEnding : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables
	private Event e;
	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private float distanceFromPlayer;
	private float angleFromPlayer;

	private bool canEnterTheatre;
	private bool tooltipEnabled;

	private Vector3 tooltipToScreen;
	private EndingType endingType;

	private bool gameEndEnabled;
	private GameManager gameManager;
	private UserInterface userInterface;
	private PlayerInformation playerInformation;
	private PlayerControl playerControl;
	private NPCManager npcManager;

	# endregion Private Variables

	// Public Properties
	public bool GameEndEnabled {
		get { return gameEndEnabled; }
	}
	// --

	public static GameEnding current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		userInterface = UserInterface.current;

		canEnterTheatre = true;
	}

	private void OnEnable() {
		PlayerControl.OnInteract += OnInteract;
	}

	private void OnDisable() {
		PlayerControl.OnInteract -= OnInteract;
	}

	private void Update() {
		if (gameManager.GameState == GameState.MainGame) {
			if ((playerInformation == null || playerControl == null) && gameManager.BasePlayerData != null) {
				playerInformation = gameManager.BasePlayerData.PlayerInformation;
				playerControl = gameManager.BasePlayerData.PlayerControl;
			}

			if (tooltipEnabled || gameEndEnabled) {
				distanceFromPlayer = Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position);
				angleFromPlayer = Vector3.Angle(gameManager.BasePlayerData.PlayerT.forward, (transform.position - gameManager.BasePlayerData.PlayerT.position));

				if (distanceFromPlayer > 3f || angleFromPlayer > playerControl.InteractViewAngle) {
					tooltipEnabled = false;
				}
				else {
					GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
					if (Input.GetButtonDown(PlayerUtility.EnterTheatre)) {
						gameEndEnabled = true;

						if (RunFifthEnding()) { endingType = EndingType.TrueEnding; }
						else if (RunFourthEnding()) { endingType = EndingType.Ending4; }
						else if (RunThirdEnding()) { endingType = EndingType.Ending3; }
						else if (RunSecondEnding()) { endingType = EndingType.Ending2; }
						else if (RunFirstEnding()) { endingType = EndingType.Ending1; }
						else endingType = EndingType.None;

						// When ending type is not none and player has talked to all the npcs
						canEnterTheatre = (endingType != EndingType.None) && (playerInformation.GetNpcCount() == npcManager.MainNpc.Length);
					}

					if (Input.GetButtonDown(PlayerUtility.ExitWindow)) {
						gameEndEnabled = false;
					}

					if (Input.GetButtonDown(PlayerUtility.JoinGameJam)) {
						Debug.Log(endingType.ToString() + "Run Animation Clip");
						userInterface.GameEndUi.SetBaseGameEnd(endingType);
						GameManager.current.SwitchGameState(GameState.GameEnd);
					}

					if (tooltipEnabled && !gameEndEnabled) {
						tooltipToScreen = gameManager.BasePlayerData.PlayerControl.PCamera.WorldToScreenPoint(transform.position);
					}
				}
			}
		}
	}

	private void OnGUI() {
		if (gameManager.GameState == GameState.MainGame) {
			if (tooltipEnabled && userInterface.InactiveUI()) {
				InteractTooltip();
			}

			if (gameEndEnabled) {
				e = Event.current;
				DrawInstructions();
			}
		}
	}

	private void OnInteract(GameObject go) {
		tooltipEnabled = (gameObject == go);
	}

	private void InteractTooltip() {
		Rect interactTooltip = new Rect(tooltipToScreen.x, Screen.height - tooltipToScreen.y, mainRect.width * 0.22f, mainRect.height * 0.04f);
		AnchorPoint.SetAnchor(ref interactTooltip, Anchor.BottomCenter);

		GUI.BeginGroup(interactTooltip);
		Rect tooltipRect = new Rect(interactTooltip.width * 0.5f, interactTooltip.height * 0.5f, interactTooltip.width * 0.95f, interactTooltip.height * 0.9f);
		AnchorPoint.SetAnchor(ref tooltipRect, Anchor.MiddleCenter);
		GUI.Box(tooltipRect, "Enter theatre?", tempSkin.GetStyle("Block"));
		GUI.EndGroup();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.94f, Screen.width * 0.5f, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, string.Empty);

		GUI.BeginGroup(textRect);
		Rect inventoryTextRect = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.35f, textRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref inventoryTextRect, Anchor.MiddleCenter);
		GameGUI.HotkeyBox(inventoryTextRect, 0.48f, "E", "Enter", tempSkin.GetStyle("Text"));
		GUI.EndGroup();
	}

	private void DrawInstructions() {
		GUI.BeginGroup(mainRect);
		Rect instructionBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.6f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionBox, Anchor.MiddleCenter);
		GUI.Box(instructionBox, string.Empty, tempSkin.GetStyle("Block"));

		GUI.BeginGroup(instructionBox);
		Rect titleBox = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.125f, instructionBox.width * 0.9f, instructionBox.height * 0.1f);
		AnchorPoint.SetAnchor(ref titleBox, Anchor.MiddleCenter);
		GUI.Box(titleBox, "Instructions", tempSkin.GetStyle("Block"));

		Rect messageBox = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.475f, instructionBox.width * 0.9f, instructionBox.height * 0.6f);
		AnchorPoint.SetAnchor(ref messageBox, Anchor.MiddleCenter);
		GUI.Box(messageBox, "Instruction goes here!", tempSkin.GetStyle("Block"));

		if (canEnterTheatre) {
			Rect canEnterText = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.82f, instructionBox.width * 0.9f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref canEnterText, Anchor.MiddleCenter);
			GUI.Box(canEnterText, "You may now enter game jam", tempSkin.GetStyle("Block"));

			Rect enterBtn = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.91f, instructionBox.width * 0.2f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref enterBtn, Anchor.MiddleCenter);
			GUI.Box(enterBtn, "Join Game Jam!", tempSkin.GetStyle("Button"));

			if (enterBtn.Contains(e.mousePosition)) {
				if(e.button == 0 && e.type == EventType.mouseDown) {
					Debug.Log(endingType.ToString() + "Run Animation Clip");
					userInterface.GameEndUi.SetBaseGameEnd(endingType);
					GameManager.current.SwitchGameState(GameState.GameEnd);
				}
			}
		}
		else {
			Rect cannotEnterText = new Rect(instructionBox.width * 0.5f, instructionBox.height * 0.87f, instructionBox.width * 0.9f, instructionBox.height * 0.08f);
			AnchorPoint.SetAnchor(ref cannotEnterText, Anchor.MiddleCenter);
			GUI.Box(cannotEnterText, "You need more experience to enter game jam", tempSkin.GetStyle("Block"));
		}

		# region Exit
		Rect exitRect = new Rect(instructionBox.width * 0.935f, instructionBox.height * 0.05f, mainRect.width * 0.03f, mainRect.height * 0.03f);
		AnchorPoint.SetAnchor(ref exitRect, Anchor.MiddleCenter);
		GUI.Box(exitRect, "X", tempSkin.GetStyle("Block"));

		if (exitRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				gameEndEnabled = false;
			}
		}

		# endregion Exit

		GUI.EndGroup();

		GUI.EndGroup();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.96f, Screen.width * 0.9f, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, string.Empty);

		GUI.BeginGroup(textRect);
		if (canEnterTheatre) {
			Rect inventoryTextRect = new Rect(textRect.width * 0.45f, textRect.height * 0.5f, textRect.width * 0.26f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref inventoryTextRect, Anchor.MiddleCenter);
			GameGUI.HotkeyBox(inventoryTextRect, 0.47f, "Esc", "Cancel", tempSkin.GetStyle("Text"));

			Rect playerProfileKeyRect = new Rect(textRect.width * 0.625f, textRect.height * 0.5f, textRect.width * 0.27f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref playerProfileKeyRect, Anchor.MiddleCenter);
			GameGUI.HotkeyBox(playerProfileKeyRect, 0.43f, "Enter", "Join", tempSkin.GetStyle("Text"));
		}
		else {
			Rect inventoryTextRect = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.26f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref inventoryTextRect, Anchor.MiddleCenter);
			GameGUI.HotkeyBox(inventoryTextRect, 0.47f, "Esc", "Cancel", tempSkin.GetStyle("Text"));
		}

		GUI.EndGroup();
	}

	// Condition 1:
	// All NPC's get 90 or more like points
	public bool RunFirstEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.MainNpc.Length; i++) {
			NPCInformation npcInformation = npcManager.MainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation.NpcStatistics.Like >= 90) {
				count++;
			}
		}
		return (count == npcManager.MainNpc.Length);
	}

	// Condition 2:
	// 3 or more NPC's get at 70 or more like points
	public bool RunSecondEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.MainNpc.Length; i++) {
			NPCInformation npcInformation = npcManager.MainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation.NpcStatistics.Like >= 70) {
				count++;
			}
		}
		return (count >= 3);
	}

	// Condition 3:
	// No NPC's get at 70 or more like points
	public bool RunThirdEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.MainNpc.Length; i++) {
			NPCInformation npcInformation = npcManager.MainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation.NpcStatistics.Like <= 70) {
				count++;
			}
		}
		return (count == npcManager.MainNpc.Length);
	}

	// Condition 4:
	// Any NPC get 80 or more dislike points
	public bool RunFourthEnding() {
		int count = 0;
		for (int i = 0; i < npcManager.MainNpc.Length; i++) {
			NPCInformation npcInformation = npcManager.MainNpc[i].GetComponent<NPCInformation>();
			if (npcInformation.NpcStatistics.Dislike >= 80) {
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
		return (gameManager.BasePlayerData.PlayerStatistics.Like == 100);
	}
}