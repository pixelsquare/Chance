using GameUtilities;
using GameUtilities.GameGUI;
using Player;
using UnityEngine;

public class UserInterface : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin userInterfaceSkin;
	[SerializeField]
	private GUISkin playerProfileSkin;
	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private Event e;

	private MainMenuUI mainMenuUi;
	private CharacterSelectionUI characterSelectionUi;
	private MainGameUI mainGameUi;
	private GameEndUI gameEndUi;

	private DialogueManager dialogueManager;
	private GameManager gameManager;
	private AngerGameUi miniGameUi;
	private GameEnding gameEnding;
	private MissionUi missionUi;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public int InventorySlotMax { get; set; }
	public bool ShowUI { get; set; }
	public bool ShowPlayerProfile { get; set; }
	public bool ShowInventory { get; set; }
	public bool ShowSettings { get; set; }
	public bool GivingItem { get; set; }

	public MainMenuUI MainMenuUi {
		get { return mainMenuUi; }
	}

	public CharacterSelectionUI CharacterSelectionUi {
		get { return characterSelectionUi; }
	}

	public MainGameUI MainGameUi {
		get { return mainGameUi; }
	}

	public GameEndUI GameEndUi {
		get { return gameEndUi; }
	}
	// --

	public static UserInterface current;

	private void Awake() {
		current= this;
	}

	private void Start() {
		dialogueManager = DialogueManager.current;
		gameManager = GameManager.current;
		miniGameUi = AngerGameUi.current;
		gameEnding = GameEnding.current;
		
		mainMenuUi = GetComponent<MainMenuUI>();
		characterSelectionUi = GetComponent<CharacterSelectionUI>();
		mainGameUi = GetComponent<MainGameUI>();
		gameEndUi = GetComponent<GameEndUI>();
		missionUi = GetComponent<MissionUi>();
	}

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

		if (dialogueManager.DialogueEnabled) {
			ShowUI = false;
		}
		else {
			ShowUI = true;
		}
	}

	private void OnGUI() {
		e = Event.current;
		GUI.depth = GUIDepth.userInterfaceDepth;

		if (gameManager.GameState == GameState.Intro) {

		}
		else if (gameManager.GameState == GameState.MainMenu) {
			mainMenuUi.MenuWindow(e);
		}
		else if (gameManager.GameState == GameState.CharacterSelection) {
			characterSelectionUi.CharacterSelectionWindow(e);
		}
		else if (gameManager.GameState == GameState.MainGame) {
			if (ShowUI && !miniGameUi.DangerModeEnabled) {
				mainGameUi.MainGUI(e);
				missionUi.MissionGUI(e);
			}

			if (missionUi.ShowMissionInfo) {
				missionUi.MissionInformation(e);
			}

			if (ShowPlayerProfile) {
				mainGameUi.PlayerProfileWindow(e);
			}

			if (ShowSettings) {
				mainGameUi.SettingsWindow(e);
			}

			if (ShowInventory || GivingItem) {
				mainGameUi.InventoryWindow(e);
			}

			mainGameUi.HotKeyText();
		}
		else if (gameManager.GameState == GameState.GameEnd) {
			gameEndUi.GameEndWindow(e);
		}
	}

	public void ResetMainGame() {
		mainGameUi.ResetStatisticWindow();
		mainGameUi.ResetInventoryWindow();
		mainGameUi.ResetSettingsWindow();
		ShowUI = true;
		ShowPlayerProfile = false;
		ShowInventory = false;
		ShowSettings = false;
	}

	public bool InactiveUI() {
		return !dialogueManager.DialogueEnabled && !miniGameUi.DangerModeEnabled && !gameEnding.GameEndEnabled &&
			(!ShowInventory && !ShowSettings && !ShowPlayerProfile && !GivingItem);
	}
}