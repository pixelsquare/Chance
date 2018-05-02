using System.Collections.Generic;
using GameUtilities;
using Player;
using UnityEngine;

public enum GameState { 
	Intro, 
	MainMenu, 
	CharacterSelection, 
	GameStory, 
	MainGame,
	EndingResult,
	GameEnd
};

public delegate void TransitionStart();
public delegate void TransitionMid();
public delegate void TransitionEnd();

public class GameManager : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private GameState gameState;
	[SerializeField]
	private float fadeDuration = 2f;
	[SerializeField]
	private ParticleSystem birdParticle;
	[SerializeField]
	private BaseCamera mainCamera;
	[SerializeField]
	private RoamingCamera roamingCamera;
	[SerializeField]
	private Cutscene gameStory;
	[SerializeField]
	private GameEnding theater;

	[SerializeField]
	private Level baseLevel;

	# endregion Public Variables

	# region Private Variables

	private bool initializeMainGame;
	private PlayerData basePlayerData;

	private AudioSource audioSource;
	private AudioPool audioPool;

	private NPCManager npcManager;
	private DialogueManager dialogueManager;
	private UserInterface userInterface;
	private CharacterSelectionUI characterSelection;
	private MissionUi mission;
    private GameEnding gameEnding;
	private AudioManager audioManager;

	# endregion Private Variables

	private float _fadeDuration;

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public GameState GameState {
		get { return gameState; }
		set { gameState = value; }
	}

	public bool EnableMainCamera {
		set { 
			mainCamera.CameraEnabled = value;
			roamingCamera.CameraEnabled = !value;
		}
	}

	public bool EnableRoamingCamera {
		set {
			roamingCamera.CameraEnabled = value;
			mainCamera.CameraEnabled = !value;
		}
	}

	public PlayerData BasePlayerData {
		get { return basePlayerData; }
		set { basePlayerData = value; }
	}

	public Level BaseLevel {
		get { return baseLevel; }
		set { baseLevel = value; }
	}

    public Cutscene GameStory {
        get { return gameStory; }
    }

	public GameEnding Theater {
		get { return theater; }
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
		characterSelection = CharacterSelectionUI.current;
		mission = MissionUi.current;
        gameEnding = GameEnding.current;
		audioManager = AudioManager.current;

		_fadeDuration = fadeDuration;
		mainCamera.CameraEnabled = false;
		roamingCamera.CameraEnabled = false;

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
		birdParticle.startRotation = 0f;

		if (audioSource != null) {
			audioSource.Stop();
			audioSource.gameObject.SetActive(false);
		}

		audioSource = audioManager.GetAudioSource(AudioNameID.MainMenuBGM, false);
		audioSource.gameObject.SetActive(true);
		audioSource.loop = true;
		audioSource.Play();

		//basePlayerData.PlayerInformation.ResetPlayerInformation();

		InitializeGame();
		EnableRoamingCamera = true;
	}

	private void MidInitCharacterSelection() {
		Debug.Log("[GAME STATE] Initialize Character Selection");
		gameState = global::GameState.CharacterSelection;
		userInterface.CharacterSelectionUi.Reset();
		birdParticle.startRotation = 0f;

		//audioPool = audioManager.GetAudioPool(AudioNameID.MainMenuBGM);
		if (audioSource == null) {
			audioSource = audioManager.GetAudioSource(AudioNameID.MainMenuBGM, false);
			audioSource.gameObject.SetActive(true);
			audioSource.loop = true;
			audioSource.Play();
		}
		//if (audioSource.clip != audioPool.AudioClip && audioPool != null) {
		//    if (audioSource != null) {
		//        audioSource.Stop();
		//        audioSource.gameObject.SetActive(false);
		//    }

		//    audioSource = audioManager.GetAudioSource(AudioNameID.MainMenuBGM, false);
		//    audioSource.gameObject.SetActive(true);
		//    audioSource.loop = true;
		//    audioSource.Play();
		//}

		InitializeGame();
		EnableRoamingCamera = true;
	}

	private void MidInitGameStory() {
		Debug.Log("[GAME STATE] Initialize Game Story");
		gameState = global::GameState.GameStory;
		userInterface.CharacterSelectionUi.Reset();
		birdParticle.startRotation = 0f;
		//dialogueManager.Reset();
		//dialogueManager.RunGameStory();

		if (audioSource != null) {
			audioSource.Stop();
			audioSource.gameObject.SetActive(false);
		}

		audioSource = audioManager.GetAudioSource(AudioNameID.GameStoryBGM, false);
        audioSource.gameObject.SetActive(true);
		audioSource.loop = true;
        audioSource.Play();

		gameStory.RunCutscene(GameStoryEnd);
		InitializeGame();

		mainCamera.GetComponent<Camera>().enabled = false;
		roamingCamera.CameraEnabled = false;
		mainCamera.CameraEnabled = false;
		//EnableMainCamera = true;
	}

	private void MidInitMainGame() {
		Debug.Log("[GAME STATE] Initialize Main Game");
		gameState = global::GameState.MainGame;
		birdParticle.startRotation = 0f;

		dialogueManager.Reset();
		userInterface.ResetMainGame();

		if (audioSource != null) {
			audioSource.Stop();
			audioSource.gameObject.SetActive(false);
		}

		audioSource = audioManager.GetAudioSource(AudioNameID.MainGameBGM, false);
		audioSource.gameObject.SetActive(true);
		audioSource.loop = true;
		audioSource.Play();

		if (basePlayerData == null) {
			basePlayerData = new PlayerData("No Name", null, Statistics.one * 50);
			basePlayerData.PlayerT = characterSelection.CharacterOwen;		
		}

		basePlayerData.PlayerInformation.ResetPlayerInformation();
		basePlayerData.PlayerInformation.PlayerEnabled = true;

		mission.RunMission();

		initializeMainGame = false;
		InitializeGame();

		mainCamera.CameraEnabled = false;
		roamingCamera.CameraEnabled = false;
	}

	private void MidInitEndingResult() {
		Debug.Log("[GAME STATE] Initialize Main End");
		gameState = global::GameState.EndingResult;
		birdParticle.startRotation = 0f;

		if (audioSource != null) {
			audioSource.Stop();
			audioSource.gameObject.SetActive(false);
		}

        if (gameEnding.EndingType == EndingType.Ending3 || gameEnding.EndingType == EndingType.Ending4) {
			audioSource = audioManager.GetAudioSource(AudioNameID.SadEnding, false);
        }
        else {
			audioSource = audioManager.GetAudioSource(AudioNameID.HappyEnding, false);
        }

        audioSource.gameObject.SetActive(true);
		audioSource.loop = true;
        audioSource.Play();

		basePlayerData.PlayerInformation.PlayerEnabled = false;
		basePlayerData = new PlayerData();

		mainCamera.GetComponent<Camera>().enabled = false;
		roamingCamera.CameraEnabled = false;
		mainCamera.CameraEnabled = false;
	}

	private void MidInitGameEnd() {
		Debug.Log("[GAME STATE] Initialize Main End");
		gameState = global::GameState.GameEnd;
		birdParticle.startRotation = 0f;

		//if (audioSource != null) {
		//    audioSource.Stop();
		//    audioSource.gameObject.SetActive(false);
		//}

		//audioSource = audioManager.GetAudioSource(AudioNameID.MainMenuBG);
		//audioSource.gameObject.SetActive(true);
		//audioSource.loop = true;
		//audioSource.Play();

		EnableMainCamera = true;
	}

	public void SwitchGameState(GameState state) {
		if (!userInterface.IsTransitioning) {
			if (state == global::GameState.Intro) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitIntro, null));
			}
			else if (state == global::GameState.MainMenu) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitMainMenu, null));
			}
			else if (state == global::GameState.CharacterSelection) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitCharacterSelection, null));
			}
			else if (state == global::GameState.GameStory) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitGameStory, null));
			}
			else if (state == global::GameState.MainGame) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitMainGame, null));
			}
			else if (state == global::GameState.EndingResult) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitEndingResult, null));
			}
			else if (state == global::GameState.GameEnd) {
				UserInterface.RunTransitionFade(new FadeTransition(_fadeDuration, null, MidInitGameEnd, null));
			}
		}
	}

	public void SetAudioSource(AudioSource source) {
		if (audioSource != null) {
			audioSource.Stop();
			audioSource.gameObject.SetActive(false);
		}

		audioSource = source;
		audioSource.gameObject.SetActive(true);
		audioSource.loop = true;
		audioSource.Play();
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

			if (npcManager.Npc != null) {
				for (int i = 0; i < npcManager.Npc.Length; i++) {
					if (npcManager.Npc[i] != null) {
						randIndx = rng.Next(0, npcSpawnPoints.Count);
						NPCControl npcControl = npcManager.Npc[i].GetComponent<NPCControl>();
						npcControl.NpcInformation.ObjectEnabled = false;
						npcControl.InitializeNpcControl(npcSpawnPoints[randIndx].position, npcSpawnPoints[randIndx].rotation, npcManager.FirstFloor, npcManager.SecondFloor);
						npcControl.NpcInformation.ObjectEnabled = true;
						npcSpawnPoints.RemoveAt(randIndx);
					}
				}
			}

			initializeMainGame = true;
		}
	}

	private void GameStoryEnd() {
		SwitchGameState(global::GameState.MainGame);
	}
}

public struct FadeTransition {
	public FadeTransition(float duration, TransitionStart start, TransitionMid midT, TransitionEnd endT) {
		fadeDuration = duration;
		startTransition = start;
		midTransition = midT;
		endTransition = endT;
	}

	public FadeTransition(float duration) {
		fadeDuration = duration;
		startTransition = null;
		midTransition = null;
		endTransition = null;
	}

	public static FadeTransition none {
		get { return new FadeTransition(); }
	}

	private float fadeDuration;
	public float FadeDuration {
		get { return fadeDuration; }
	}

	private TransitionStart startTransition;
	public TransitionStart StartTransition {
		get { return startTransition; }
	}

	private TransitionMid midTransition;
	public TransitionMid Midtransition {
		get { return midTransition; }
	}

	private TransitionEnd endTransition;
	public TransitionEnd EndTransition {
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
	private Transform hideAndSeekSpawnPoints;
	public Transform HideAndSeekSpawnPoints {
		get { return hideAndSeekSpawnPoints; }
	}
}