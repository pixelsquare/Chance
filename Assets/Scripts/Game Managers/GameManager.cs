using System.Collections.Generic;
using GameEnd.Database;
using GameUtilities;
using Mission;
using Player;
using UnityEngine;

public enum GameState { 
	Intro, 
	MainMenu, 
	CharacterSelection, 
	GameStory, 
	MainGame,
	GameEnd
};

public delegate void StartTransition();
public delegate void MidTransition();
public delegate void EndTransition();

public class GameManager : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private GameState gameState;
	[SerializeField]
	private BaseCamera mainCamera;
	[SerializeField]
	private RoamingCamera roamingCamera;

	[SerializeField]
	private Level baseLevel;

	# endregion Public Variables

	# region Private Variables

	private bool initializeMainGame;
	private PlayerData basePlayerData;

	private NPCManager npcManager;
	private DialogueManager dialogueManager;
	private UserInterface userInterface;
	private ScreenFade screenFade;
	private CharacterSelectionUI characterSelection;
	private MissionManager missionManager;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public GameState GameState {
		get { return gameState; }
		set { gameState = value; }
	}

	public bool EnableMainCamera {
		set { 
			mainCamera.EnableCamera = value;
			roamingCamera.EnableCamera = !value;
		}
	}

	public bool EnableRoamingCamera {
		set {
			roamingCamera.EnableCamera = value;
			mainCamera.EnableCamera = !value;
		}
	}

	public PlayerData BasePlayerData {
		get { return basePlayerData; }
		set { basePlayerData = value; }
	}
	// --

	public static GameManager current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		npcManager = NPCManager.current;
		dialogueManager = DialogueManager.current;
		userInterface = UserInterface.current;
		screenFade = ScreenFade.current;
		characterSelection = CharacterSelectionUI.current;
		missionManager = MissionManager.current;

		GameEndDatabase.Initialize();

		mainCamera.EnableCamera = false;
		roamingCamera.EnableCamera = false;

		SwitchGameState(gameState);
	}

	private void MidInitIntro() { 
		Debug.Log("[GAME STATE] Initialize Intro");
		gameState = global::GameState.Intro;

		EnableMainCamera = true;
	}

	private void MidInitMainMenu() {
		Debug.Log("[GAME STATE] Initialize Main Menu");
		gameState = global::GameState.MainMenu;
		userInterface.CharacterSelectionUi.Reset();

		InitializeGame();
		EnableRoamingCamera = true;
	}

	private void MidInitCharacterSelection() {
		Debug.Log("[GAME STATE] Initialize Character Selection");
		gameState = global::GameState.CharacterSelection;
		userInterface.CharacterSelectionUi.Reset();

		InitializeGame();
		EnableRoamingCamera = true;
	}

	private void MidInitGameStory() {
		Debug.Log("[GAME STATE] Initialize Game Story");
		gameState = global::GameState.GameStory;
		userInterface.CharacterSelectionUi.Reset();
		dialogueManager.Reset();
		dialogueManager.RunGameStory();

		EnableMainCamera = true;
		mainCamera.camera.enabled = false;
	}

	private void MidInitMainGame() {
		Debug.Log("[GAME STATE] Initialize Main Game");
		gameState = global::GameState.MainGame;

		dialogueManager.Reset();
		userInterface.ResetMainGame();

		if (basePlayerData == null) {
			basePlayerData = new PlayerData("No Name", null, Statistics.one * 50);
			basePlayerData.PlayerT = characterSelection.CharacterOwen;		
		}

		basePlayerData.PlayerInformation.Reset();
		basePlayerData.PlayerInformation.PlayerEnabled = true;

		initializeMainGame = false;
		InitializeGame();

		missionManager.RunMission();

		mainCamera.EnableCamera = false;
		mainCamera.camera.enabled = true;
		roamingCamera.EnableCamera = false;
	}

	private void MidInitGameEnd() {
		Debug.Log("[GAME STATE] Initialize Main End");
		gameState = global::GameState.GameEnd;

		basePlayerData.PlayerInformation.PlayerEnabled = false;
		basePlayerData = new PlayerData();

		EnableMainCamera = true;
	}

	public void SwitchGameState(GameState state) {
		if (!screenFade.IsTransitioning) {
			if (state == global::GameState.Intro) {
				screenFade.Run(new FadeTransition(null, MidInitIntro, null));
			}
			else if (state == global::GameState.MainMenu) {
				screenFade.Run(new FadeTransition(null, MidInitMainMenu, null));
			}
			else if (state == global::GameState.CharacterSelection) {
				screenFade.Run(new FadeTransition(null, MidInitCharacterSelection, null));
			}
			else if (state == global::GameState.GameStory) {
				screenFade.Run(new FadeTransition(null, MidInitGameStory, null));
			}
			else if (state == global::GameState.MainGame) {
				screenFade.Run(new FadeTransition(null, MidInitMainGame, null));
			}
			else if (state == global::GameState.GameEnd) {
				screenFade.Run(new FadeTransition(null, MidInitGameEnd, null));
			}
		}
	}

	public void InitializeGame() {
		if (!initializeMainGame) {
			// Spawn Player
			if (basePlayerData != null) {
				basePlayerData.PlayerT.position = baseLevel.PlayerSpawnPoint.position;
				basePlayerData.PlayerT.rotation = baseLevel.PlayerSpawnPoint.rotation;
			}

			// Spawn Main npc's
			List<Transform> npcSpawnPoints = new List<Transform>();
			GameUtility.GetChildrenRecursively(baseLevel.MainNpcSpawnPoint, ref npcSpawnPoints);
			System.Random rng = new System.Random();
			int randIndx = 0;

			if (npcManager.MainNpc != null) {
				for (int i = 0; i < npcManager.MainNpc.Length; i++) {
					if (npcManager.MainNpc[i] != null) {
						randIndx = rng.Next(0, npcSpawnPoints.Count);
						NPCControl tmpNpcControl = npcManager.MainNpc[i].GetComponent<NPCControl>();
						tmpNpcControl.NpcInformation.NPCEnable = false;
						tmpNpcControl.transform.position = npcSpawnPoints[randIndx].position;
						tmpNpcControl.transform.rotation = npcSpawnPoints[randIndx].rotation;
						tmpNpcControl.TargetPos = tmpNpcControl.transform.position;
						tmpNpcControl.NpcInformation.NPCEnable = true;
						npcSpawnPoints.RemoveAt(randIndx);
					}
				}
			}

			npcSpawnPoints = new List<Transform>();
			GameUtility.GetChildrenRecursively(baseLevel.WandererNpcSpawnPoint, ref npcSpawnPoints);
			randIndx = 0;

			if (npcManager.WandererNpc != null) {
				for (int i = 0; i < npcManager.WandererNpc.Length; i++) {
					if (npcManager.WandererNpc[i] != null) {
						randIndx = rng.Next(0, npcSpawnPoints.Count);
						NPCControl tmpNpcControl = npcManager.WandererNpc[i].GetComponent<NPCControl>();
						tmpNpcControl.NpcInformation.NPCEnable = false;
						tmpNpcControl.transform.position = npcSpawnPoints[randIndx].position;
						tmpNpcControl.transform.rotation = npcSpawnPoints[randIndx].rotation;
						tmpNpcControl.TargetPos = tmpNpcControl.transform.position;
						tmpNpcControl.NpcInformation.NPCEnable = true;
						npcSpawnPoints.RemoveAt(randIndx);
					}
				}
			}

			initializeMainGame = true;
		}
	}
}

public struct FadeTransition {
	public FadeTransition(StartTransition start, MidTransition midT, EndTransition endT) {
		startTransition = start;
		midTransition = midT;
		endTransition = endT;
	}

	private StartTransition startTransition;
	public StartTransition StartTransition {
		get { return startTransition; }
	}

	private MidTransition midTransition;
	public MidTransition Midtransition {
		get { return midTransition; }
	}

	private EndTransition endTransition;
	public EndTransition EndTransition {
		get { return endTransition; }
	}
}

[System.Serializable]
public class Level {
	[SerializeField]
	private Transform playerSpawnPoint;
	public Transform PlayerSpawnPoint {
		get { return playerSpawnPoint; }
	}

	[SerializeField]
	private Transform mainNpcSpawnPoint;
	public Transform MainNpcSpawnPoint {
		get { return mainNpcSpawnPoint; }
	}

	[SerializeField]
	private Transform wandererNpcSpawnPoint;
	public Transform WandererNpcSpawnPoint {
		get { return wandererNpcSpawnPoint; }
	}
}