using System.Collections.Generic;
using System.Linq;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using MiniGame;
using MiniGame.MiniGameDatabase;
using NPC;
using UnityEngine;

public class AngerGameUi : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private Texture2D blackTexture;
	[SerializeField]
	private Texture2D instructionTexture;

	# endregion Public Variables

	# region Private Variables

	// Screen Variables
	private Rect mainRect;
	private float oldScreenHeightRatio;
	private float screenHeightRatio;
	private Color originalColor;

	private bool dangerModeEnabled;
	private List<Key> keyDatabase;
	private List<Vector2> keyPositionDb;
	private List<string> sympathyText;

	private bool showGameInstruction = true;
	private bool gameHasStarted;
	private float startCountDown;

	private float gameTime;
	private float timePenalty;
	private float spawnRate;
	private int correctKey;
	private int correctKeyMax;
	private int keyPress;
	private KeyHolder pressedKey;

	private MiniGameFailed miniGameFailed;
	private MiniGameSuccess miniGameSuccess;
	private MiniGameEnd miniGameEnd;

	private ToughnessLevel toughnessLevel;

	private MiniGameData baseMiniGameMode;
	private DialogueManager dialogueManager;

	# endregion Private Variables

	// Public Properties
	public MiniGameData BaseMiniGameMode {
		get { return baseMiniGameMode; }
	}

	public ToughnessLevel ToughnessLevel {
		get { return toughnessLevel; }
	}

	public bool DangerModeEnabled {
		get { return dangerModeEnabled; }
		set {
			dangerModeEnabled = value;
			if (!value) {
				CancelInvoke("GenerateKeys");
			}
		}
	}

	public Texture2D InstructionTexture {
		get { return instructionTexture; }
	}
	// --

	public static AngerGameUi current;

	/// <summary>
	/// NOTE:
	/// Removing and addding key in the keydatabase
	/// to prevent having 2 letter at the same time
	/// </summary>

	private void Awake() {
		current = this;
	}

	private void Start() {
		MiniGameDatabase.Initialize();
		dialogueManager = DialogueManager.current;
		PopulateKeyDatabase();
		startCountDown = 3f;

		/* Debugging line */
		//sympathyText = new List<string>();
		//sympathyText.Add("SASD");
		//RunMiniGame(ToughnessLevel.Level5, sympathyText.ToArray<string>(), new MiniGameResult());
	}

	private void Update() {
		if (dangerModeEnabled) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (gameHasStarted) {
				// Go back to Dialogue State
				if (correctKey == 0 && dialogueManager.BaseNPCData.NpcNameID != NPCNameID.None) {
					showGameInstruction = false;
					DangerModeEnabled = false;

					if (miniGameSuccess != null) {
						miniGameSuccess();
					}

					if (miniGameEnd != null) {
						miniGameEnd();
					}
				}

				// Mini Game Timer
				if (gameTime > 0) {
					gameTime -= Time.deltaTime;
				}

				// End when time expires as well as if count has completed
				if (gameTime <= 0 || (correctKey == 0 && dialogueManager.BaseNPCData.NpcNameID == NPCNameID.None)) {
					showGameInstruction = false;
					DangerModeEnabled = false;

					if (miniGameFailed != null) {
						miniGameFailed();
					}

					if (miniGameEnd != null) {
						miniGameEnd();
					}
				}

				// Database is set in the parameter as pass by reference to add expired keys
				baseMiniGameMode.UpdateMode(Time.deltaTime, ref keyDatabase, ref keyPositionDb);

				// Check for player's input
				if (Input.inputString != string.Empty) {
					keyPress = Input.inputString.ToUpper()[0];
					pressedKey = baseMiniGameMode.KeyPress(keyPress);

					if (pressedKey.KeyEnabled) {
						correctKey--;
						keyDatabase.Add(pressedKey.KeyInput);
						keyPositionDb.Add(pressedKey.KeyPosition);
						baseMiniGameMode.RemoveKey(pressedKey);
					}
					else {
						gameTime -= timePenalty;
					}
				}
			}
			else {
				if (startCountDown > 0f) {
					startCountDown -= Time.deltaTime;
					startCountDown = Mathf.Clamp(startCountDown, 0f, 3f);
				}
				else {
					gameHasStarted = true;
					InvokeRepeating("GenerateKeys", 0.1f, spawnRate);
				}
			}

		}
	}

	private void OnGUI() {
		if (dangerModeEnabled) {
			GUI.depth = GUIDepth.dialogueDepth;

			originalColor = GUI.color;
			GUI.color = new Color(0f, 0f, 0f, 0.5f);
			GUI.DrawTexture(mainRect, blackTexture);
			GUI.color = originalColor;

			if (gameHasStarted) {
				DrawMiniGameMode();
				baseMiniGameMode.OnGUIMode(mainRect);
			}
			else {
				if (showGameInstruction) {
					DrawInstructions();
				}
				else {
					DrawCountDown();
				}
			}
		}
		
	}

	private void DrawMiniGameMode() {
		GUI.BeginGroup(mainRect);

		// Fade the Danger Texture
		//originalColor = GUI.color;
		//GUI.color = new Color(0f, 0f, 0f, 0.8f);
		Rect dangerRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.25f, mainRect.width * 0.1f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref dangerRect, Anchor.MiddleCenter);
		GUI.Box(dangerRect, "DANGER", tempSkin.GetStyle("Text"));
		//GUI.color = originalColor;

		Rect keyCountRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.3f, mainRect.width * 0.5f, mainRect.height * 0.02f);
		AnchorPoint.SetAnchor(ref keyCountRect, Anchor.MiddleCenter);
		GameGUI.ProgressBar(string.Empty, correctKey, correctKeyMax, keyCountRect, 
			progressBarSkin.GetStyle("Progress Bar BG"), progressBarSkin.GetStyle("Progress Bar Red Overlay"));

		Rect timerBoxRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.28f, mainRect.width * 0.1f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref timerBoxRect, Anchor.MiddleCenter);
		GUI.Box(timerBoxRect, GameUtility.ToHHMMSS(gameTime), tempSkin.GetStyle("Text"));

		GUI.EndGroup();
	}

	private void DrawInstructions() {
		GUI.BeginGroup(mainRect);
		Rect instructionBoxRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.7f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionBoxRect, Anchor.MiddleCenter);
		GUI.Box(instructionBoxRect, string.Empty, tempSkin.GetStyle("Block"));

		GUI.BeginGroup(instructionBoxRect);
		Rect instructionTitle = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.1f, instructionBoxRect.width * 0.9f, instructionBoxRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref instructionTitle, Anchor.MiddleCenter);
		GUI.Box(instructionTitle, "INSTRUCTIONS!", tempSkin.GetStyle("Block"));

		Rect instructionRect = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.5f, instructionBoxRect.width * 0.9f, instructionBoxRect.height * 0.6f);
		AnchorPoint.SetAnchor(ref instructionRect, Anchor.MiddleCenter);
		GUI.Box(instructionRect, "INSTRUCTIONS Description / Texture" + "\nMini Game Mode Level: " + toughnessLevel.ToString(), tempSkin.GetStyle("Block"));

		Rect countdownRect = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.9f, instructionBoxRect.width * 0.2f, instructionBoxRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref countdownRect, Anchor.MiddleCenter);
		GUI.Box(countdownRect, "STARTING IN \n" + startCountDown.ToString("F1"), tempSkin.GetStyle("Block"));
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
		GUI.Box(titleRect, "DANGER MODE!", tempSkin.GetStyle("Block"));

		Rect countdownRect = new Rect(countDownBox.width * 0.5f, countDownBox.height * 0.7f, countDownBox.width * 0.9f, countDownBox.height * 0.45f);
		AnchorPoint.SetAnchor(ref countdownRect, Anchor.MiddleCenter);
		GUI.Box(countdownRect, "STARTING IN \n" + startCountDown.ToString("F1"), tempSkin.GetStyle("Block"));

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void RunMiniGame(ToughnessLevel toughness, string[] npcSympathy, MiniGameResult result) {
		if (toughness != ToughnessLevel.None) {
			Reset();
			baseMiniGameMode = MiniGameDatabase.GetMiniGameMode(toughness);
			baseMiniGameMode.SetStyle(tempSkin.GetStyle("Block"), progressBarSkin.GetStyle("Progress Bar BG"), progressBarSkin.GetStyle("Progress Bar Overlay"));

			baseMiniGameMode.KeyHolder = new KeyHolder[10];
			PopulateKeyDatabase();

			toughnessLevel = toughness;
			spawnRate = baseMiniGameMode.KeySpawnRate;
			gameTime = baseMiniGameMode.GameTime;
			timePenalty = baseMiniGameMode.TimePenalty;
			correctKeyMax = baseMiniGameMode.CorrectKeysMax;
			correctKey = correctKeyMax;

			miniGameFailed = result.MiniGameFailed;
			miniGameSuccess = result.MiniGameSuccess;
			miniGameEnd = result.MiniGameEnd;

			if (result.MiniGameStart != null) {
				result.MiniGameStart();
			}

			if (npcSympathy != null) {
				sympathyText = npcSympathy.ToList<string>();
			}

			dangerModeEnabled = true;
		}
		else {
			Debug.Log("[MINI GAME MODE] Toughness Level set to None!");
		}
	}

	private void GenerateKeys() {
		// Clamp the random generator from 0 to database length
		if (!baseMiniGameMode.IsKeyHolderFull()) {
			System.Random rng = new System.Random();
			int indx = rng.Next(0, keyDatabase.Count - 1);
			int sympathyIndx = rng.Next(0, sympathyText.Count);
			int keyIndx = rng.Next(0, keyDatabase.Count);
			baseMiniGameMode.AddKey(
				keyDatabase[indx], 
				sympathyText[sympathyIndx], 
				new Vector2(keyPositionDb[keyIndx].x, (keyPositionDb[keyIndx].y)
			));

			// Remove the key in the database
			keyDatabase.RemoveAt(indx);

			// Remove key position
			keyPositionDb.RemoveAt(keyIndx);
		}
	}

	private void PopulateKeyDatabase() {
		keyDatabase = new List<Key>();
		int startIndx = 65;
		for (int i = 0; i < 26; i++) {
			keyDatabase.Add(new Key(startIndx, tempSkin.GetStyle("Block")));
			startIndx++;
		}

		keyPositionDb = new List<Vector2>();
		for (int y = 1; y < 4; y++) {
			for (int x = 1; x < 10; x++) {
				keyPositionDb.Add(new Vector2(x, y));
			}
		}
	}

	public void Reset() {
		dangerModeEnabled = false;

		gameHasStarted = false;
		startCountDown = 3f;

		spawnRate = 0f;
		correctKey = 0;
		correctKeyMax = 0;
		gameTime = 0f;
		timePenalty = 0f;
		keyPress = 0;
		pressedKey = new KeyHolder();

		toughnessLevel = ToughnessLevel.None;
		baseMiniGameMode = new MiniGameData();
	}
}