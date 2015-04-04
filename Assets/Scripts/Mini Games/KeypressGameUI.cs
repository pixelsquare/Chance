using UnityEngine;
using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using MiniGames;
using System.Collections.Generic;
using NPC;
using MiniGames.Database;
using GameUtilities.LayerManager;

public class KeypressGameUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform miniGameCameraT;
    [SerializeField]
    private GUISkin keypressSkin;
	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private GUISkin progressSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float oldScreenHeightRatio;
	private float screenHeightRatio;
	private Color guiOriginalColor;

	private bool keypressEnabled;
	private bool keypressActive;
	private bool instructionEnabled;
	private bool informationEnabled;
	private bool miniGameSuccessful;
	private bool isCorrect;

	private float instructionCountDown;
	private const float instructionTime = 5f;

	private float keypressGameTime;
	private int correctKeysCount;

	private List<Key> keyDb;
	private List<Vector2> keyPositionDb;

	private KeypressData baseKeypressData;

	private Vector3 previousPlayerPos;
	private Quaternion previousPlayerRot;

	private Vector3 previousNpcPos;
	private Quaternion previousNpcRot;

	private Transform keypressCameraInitialPos;
	private Transform keypressPlayerT;
	private Transform keypressNpcT;

	private Animator playerAnimator;

	private Transform npcT;
	private NPCInformation npcInformation;
	private NPCControl npcControl;

	private Transform playerT;
	private PlayerInformation playerInformation;
	private PlayerControl playerControl;

	private GameManager gameManager;
	private NPCManager npcManager;
	private DialogueManager dialogueManager;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public bool KeypressEnabled {
		get { return keypressEnabled; }
	}
	// --

	public static KeypressGameUI current;

	private void Awake() {
		current = this;
		KeypressDatabase.Initialize();
	}

	private void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		dialogueManager = DialogueManager.current;

		PopulateDatabase();

		//string[] text = new string[1] { "Sorry" };
		//SympathyText testSympathyText = new SympathyText(text);
		//RunKeypress(KeypressLevel.Level1, testSympathyText);
	}

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

		if (keypressEnabled) {
			if (instructionEnabled) {
				if (instructionCountDown > 0f) {
					instructionCountDown -= Time.deltaTime;
					instructionCountDown = Mathf.Clamp(instructionCountDown, 0f, instructionTime);
				}
				else {
					Debug.Log("Start Initialize");
					instructionEnabled = false;
					informationEnabled = true;
					keypressActive = true;

					playerT = GameUtility.TransformCopy(playerT, keypressPlayerT);
					npcT = GameUtility.TransformCopy(npcT, keypressNpcT);

					miniGameCameraT = GameUtility.TransformCopy(miniGameCameraT, keypressCameraInitialPos);
					miniGameCameraT.gameObject.SetActive(true);

					npcControl.RunAnimationPreResult();

					// Start generating key
					if (baseKeypressData != null) {
						InvokeRepeating("GenerateKeys", 0.1f, baseKeypressData.KeySpawnRate);
					}
				}
			}
			else {
				if (keypressGameTime > 0f) {
					keypressGameTime -= Time.deltaTime;
					keypressGameTime = Mathf.Clamp(keypressGameTime, 0f, baseKeypressData.KeypressGameTime);
				}

				if (baseKeypressData != null) {
					// Update key timers
					baseKeypressData.UpdateKeypress(ref keyDb, ref keyPositionDb);

					// Check for player's input
					if (Input.inputString != string.Empty) {
						isCorrect = false;
						for (int i = 0; i < baseKeypressData.KeyHolder.Length; i++) {
							if (baseKeypressData.KeyHolder[i].KeyEnabled || baseKeypressData.KeyHolder[i].KeyHolderEnabled) {
								if (baseKeypressData.KeyHolder[i].KeyInput.KeyToInt == Input.inputString.ToUpper()[0]) {
									baseKeypressData.KeyHolder[i].KeyEnabled = false;
									baseKeypressData.KeyHolder[i].KeyHolderEnabled = false;
									correctKeysCount--;

									System.Random rng = new System.Random();
									int randomIndx = rng.Next(0, 2);

									if (randomIndx == 0) {
										AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.KeypressBtn1, true);
										audioSource.gameObject.SetActive(true);
										AudioManager.current.GetAudio(AudioNameID.KeypressBtn1).RunAudioSourceUpdate();
										audioSource.Play();
										
									}
									else {
										AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.KeypressBtn2, true);
										audioSource.gameObject.SetActive(true);
										AudioManager.current.GetAudio(AudioNameID.KeypressBtn2).RunAudioSourceUpdate();
										audioSource.Play();
									}

									isCorrect = true;
									break;
								}
							}
						}

						if (!isCorrect) {
							keypressGameTime -= baseKeypressData.KeypressTimePenalty;

							AudioSource audio = AudioManager.current.GetAudioSource(AudioNameID.KeypressWrongBtn, true);
							audio.gameObject.SetActive(true);
							AudioManager.current.GetAudio(AudioNameID.KeypressWrongBtn).RunAudioSourceUpdate();
							audio.Play();
						}
					}
				}
				if (keypressActive) {
					if (correctKeysCount <= 0 && keypressGameTime > 0f) {
						Debug.Log("Successful!");
						miniGameSuccessful = true;
						npcControl.RunAnimationGoodResult();

						Debug.Log("Game End");
						CancelInvoke("GenerateKeys");

						StartCoroutine(GameEnd());
						keypressActive = false;
					}

					if (keypressGameTime <= 0f && correctKeysCount > 0) {
						Debug.Log("Failed!");
						miniGameSuccessful = false;
						npcControl.RunAnimationBadResult();
						playerAnimator.SetTrigger("Cry");
						dialogueManager.AddTotalStatistics(baseKeypressData.KeypressStatisticsPenalty);

						Debug.Log("Game End");
						CancelInvoke("GenerateKeys");

						StartCoroutine(GameEnd());
						keypressActive = false;
					}
				}
			}
		}
	}

	public void MainGUI(Event e) {
		if (keypressEnabled) {
			if (instructionEnabled) {
				KeypressInstructions();
			}
			else {
				if (keypressActive) {
					if (baseKeypressData != null) {
						baseKeypressData.OnGUIDraw(mainRect);
					}

					if (informationEnabled) {
						KeypressInformation();
					}
				}
			}
		}
	}

	private void KeypressInstructions() {
		GUI.BeginGroup(mainRect);

		// Dim background
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.DrawTexture(mainRect, Resources.BlackTexture);
		GUI.color = guiOriginalColor;

		// Instruction Box
		Rect instructionsBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.5f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionsBox, Anchor.MiddleCenter);
		GUI.Box(instructionsBox, string.Empty, keypressSkin.GetStyle("Keypress BG"));

		GUI.BeginGroup(instructionsBox);

        //// Title
        //Rect titleRect = new Rect(instructionsBox.width * 0.5f, instructionsBox.height * 0.1f, instructionsBox.width * 0.9f, instructionsBox.height * 0.1f);
        //AnchorPoint.SetAnchor(ref titleRect, Anchor.MiddleCenter);
        //GUI.Box(titleRect, "TITLE GOES HERE!", tempSkin.GetStyle("Block"));

		// Desctiption
		Rect descRect = new Rect(instructionsBox.width * 0.5f, instructionsBox.height * 0.45f, instructionsBox.width * 0.9f, instructionsBox.height * 0.7f);
		AnchorPoint.SetAnchor(ref descRect, Anchor.MiddleCenter);
        GUI.DrawTexture(descRect, keypressSkin.GetStyle("Keypress Instruction").normal.background);
        //GUI.Box(descRect, string.Empty, keypressSkin.GetStyle("Keypress Instruction"));

		// Timer
		Rect timerRect = new Rect(instructionsBox.width * 0.5f, instructionsBox.height * 0.85f, instructionsBox.width * 0.8f, instructionsBox.height * 0.05f);
		AnchorPoint.SetAnchor(ref timerRect, Anchor.MiddleCenter);
        //GUI.Box(timerRect, "TIMER", tempSkin.GetStyle("Block"));

		UserInterface.ProgressBar(
			"Get Ready in " + (int)instructionCountDown + " ...",
			instructionCountDown,
			instructionTime,
			timerRect,
			progressSkin.GetStyle("Progress Bar BG"),
			progressSkin.GetStyle("Progress Bar Overlay")
		);

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void KeypressInformation() {
		GUI.BeginGroup(mainRect);
		Rect infoBoxRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.25f, mainRect.width * 0.5f, mainRect.height * 0.2f);
		AnchorPoint.SetAnchor(ref infoBoxRect, Anchor.MiddleCenter);
		//GUI.Box(infoBoxRect, string.Empty);

		GUI.BeginGroup(infoBoxRect);
		Rect titleRect = new Rect(infoBoxRect.width * 0.5f, infoBoxRect.height * 0.35f, infoBoxRect.width * 0.9f, infoBoxRect.height * 0.25f);
		AnchorPoint.SetAnchor(ref titleRect, Anchor.MiddleCenter);
        GUI.Box(titleRect, string.Empty, keypressSkin.GetStyle("Keypress Key Main Text"));

		Rect timerRect = new Rect(infoBoxRect.width * 0.5f, infoBoxRect.height * 0.5f, infoBoxRect.width * 0.9f, infoBoxRect.height * 0.25f);
		AnchorPoint.SetAnchor(ref timerRect, Anchor.MiddleCenter);
		GUI.Box(timerRect, GameUtility.ToHHMMSS(keypressGameTime), keypressSkin.GetStyle("Keypress Timer"));

		Rect distortBar = new Rect(infoBoxRect.width * 0.5f, infoBoxRect.height * 0.6f, infoBoxRect.width * 0.9f, infoBoxRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref distortBar, Anchor.MiddleCenter);
		//GUI.Box(distortBar, string.Empty);

		UserInterface.ProgressBar("NEGAMOTIONAL DISTORT", correctKeysCount, baseKeypressData.KeyCorrectMax, distortBar, 
			progressSkin.GetStyle("Progress Bar BG"), progressSkin.GetStyle("Progress Bar Red Overlay"));
		
		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void RunKeypress(KeypressLevel level, SympathyText sympathyText) {
		UserInterface.RunTitle("Watch Out!", "It's Mercy Game Time!", string.Empty, new FadeTransition(2f, null, null, InitializeInstruction));

		PopulateDatabase();

		if (playerT == null) {
			playerT = gameManager.BasePlayerData.PlayerT;
		}

		if (playerInformation == null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
		}

		if (playerControl == null) {
			playerControl = gameManager.BasePlayerData.PlayerControl;
		}

		playerAnimator = playerControl.PlayerAnimator;

		npcT = npcManager.GetNpc(playerInformation.InteractingTo);
		npcInformation = npcManager.GetNpcInformation(playerInformation.InteractingTo);
		npcControl = npcManager.GetNpcControl(playerInformation.InteractingTo);
		npcControl.CurState = NPCState.MiniGame;

		previousPlayerPos = new Vector3();
		previousPlayerPos = playerT.position;

		previousPlayerRot = new Quaternion();
		previousPlayerRot = playerT.rotation;

		previousNpcPos = new Vector3();
		previousNpcPos = npcT.position;

		previousNpcRot = new Quaternion();
		previousNpcRot = npcT.rotation;

		keypressCameraInitialPos = playerControl.KeypressCameraInitialPos;
		keypressPlayerT = playerControl.KeypressPlayerT;
		keypressNpcT = playerControl.KeypressNpcT;

		baseKeypressData = KeypressDatabase.GetMiniGameMode(level);
		baseKeypressData.KeySympathyText = sympathyText;

		keypressGameTime = baseKeypressData.KeypressGameTime;
		correctKeysCount = baseKeypressData.KeyCorrectMax;

		gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.KeypressBGM, false));

		miniGameSuccessful = false;
		instructionEnabled = false;
		keypressEnabled = true;
	}

	private void InitializeInstruction() {
		instructionCountDown = instructionTime;
		instructionEnabled = true;
	}

	private void PopulateDatabase() {
		keyDb = new List<Key>();
		for (int i = 65; i < 91; i++) {
			keyDb.Add(new Key(i, keypressSkin.GetStyle("Keypress Key Text")));
		}

		keyPositionDb = new List<Vector2>();
		for (int y = 1; y < 4; y++) {
			for (int x = 1; x < 9; x++) {
				keyPositionDb.Add(new Vector2(x, y));
			}
		}
	}

	private void GenerateKeys() {
		// Clamp the random generator from 0 to database length
		if (!baseKeypressData.IsKeyHolderFull()) {
			System.Random rng = new System.Random();
			int indx = rng.Next(0, keyDb.Count - 1);
			int keyIndx = rng.Next(0, keyPositionDb.Count);
			baseKeypressData.AddKey(
				keyDb[indx],
				new Vector2(keyPositionDb[keyIndx].x, keyPositionDb[keyIndx].y),
                keypressSkin.GetStyle("Keypress Key BG"),
                keypressSkin.GetStyle("Keypress Key Sympathy"),
				progressSkin.GetStyle("Progress Bar BG"),
				progressSkin.GetStyle("Progress Bar Overlay")
			);

			// Remove the key in the database
			keyDb.RemoveAt(indx);

			// Remove key position
			keyPositionDb.RemoveAt(keyIndx);
		}
	}

	private void MiniGameStart() {
		Debug.Log("Mini Game Start");
		npcControl.CurState = NPCState.MiniGame;
	}

	private IEnumerator GameEnd() {
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < baseKeypressData.KeyHolder.Length; i++) {
			baseKeypressData.KeyHolder[i].KeyEnabled = false;
			baseKeypressData.KeyHolder[i].KeyHolderEnabled = false;
		}

		playerT.position = previousPlayerPos;
		playerT.rotation = previousPlayerRot;

		npcT.position = previousNpcPos;
		npcT.rotation = previousNpcRot;

		ParticleManager.current.ParticlePoolReset(ParticleNameID.KeypressExplosion);

		if (dialogueManager.DialogueDataIndx < (dialogueManager.NpcDialogueData.Length - 1)) {
			dialogueManager.AddTotalStatisticsToNpc();
		}
		else {
			dialogueManager.DialogueEnabled = true;
		}

		if (miniGameSuccessful) {
			npcInformation.RunEmoticon(EmoticonNameID.Happy);
		}
		else {
			npcInformation.RunEmoticon(EmoticonNameID.Sad);
		}

		miniGameCameraT.gameObject.SetActive(false);
		GameUtility.SetGameObjectLayerRecursively(npcT, LayerManager.LayerNPC);
		npcControl.Reset();

		gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.MainGameBGM, false));

		informationEnabled = false;
		instructionEnabled = false;
		keypressEnabled = false;
	}
}