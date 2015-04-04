using UnityEngine;
using System.Collections;
using GameUtilities;
using MiniGames;
using System.Collections.Generic;
using GameUtilities.PlayerUtility;
using GameUtilities.AnchorPoint;
using GameUtilities.LayerManager;

public class DanceUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform miniGameCameraT;
    [SerializeField]
    private GUISkin danceSkin;
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

	private bool danceEnabled;
	private bool instructionEnabled;
	private bool informationEnabled;
	private bool gameEnded;

	private float cameraSmoothDampTime = 0.3f;

	private const float instructionTime = 5f;
	private float instructionCountDown;

	private List<ArrowKey> arrowKeyDb;
	private DanceData[] danceDataList;
	private int[] danceKeyCount = new int[5] { 3, 4, 5, 6, 7 };
	private Vector2[] danceKeyPos = new Vector2[5] {
		new Vector2(4f, 3f),
		new Vector2(3.5f, 3f),
		new Vector2(3f, 3f),
		new Vector2(2.5f, 3f),
		new Vector2(1.75f, 3f)
	};

	private int danceDataIndx;
	private DanceData baseDanceData;

	private int keyPointerIndx;
	private ArrowKey keyPointer;

	private Transform danceOffCameraInitialPos;
	private Transform danceOffCameraMainPos;
	private Transform danceOffCameraNpcPos;
	private Transform danceOffNpcPos;
	private Transform danceOffPlayerPos;

	private Vector3 previousPlayerPos;
	private Quaternion previousPlayerRot;

	private Vector3 previousNpcPos;
	private Quaternion previousNpcRot;

	private Statistics statisticsPenalty;

	private float arrowH;
	private float arrowV;
	private bool isPlayerTurn;
	private bool isPlayerDancing;

	private Transform playerT;
	private PlayerControl playerControl;
	private PlayerInformation playerInformation;
	private Animator playerAnimator;

	private Transform npcT;
	private NPCControl npcControl;
	private Animator npcAnimator;

	private GameManager gameManager;
	private NPCManager npcManager;
	private DialogueManager dialogueManager;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public bool DanceEnabled {
		get { return danceEnabled; }
	}
	// --

	public static DanceUI current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		dialogueManager = DialogueManager.current;

		PopulateDatabase();
	}

	private void Update() {
		if (danceEnabled) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (instructionEnabled) {
				if (instructionCountDown > 0f) {
					instructionCountDown -= Time.deltaTime;
					instructionCountDown = Mathf.Clamp(instructionCountDown, 0f, instructionTime);
				}
				else {
					// Initialize Game after Instruction
					Debug.Log("Start Initialize");
					instructionEnabled = false;
					informationEnabled = true;

					playerT = GameUtility.TransformCopy(playerT, danceOffPlayerPos);
					npcT = GameUtility.TransformCopy(npcT, danceOffNpcPos);
					miniGameCameraT = GameUtility.TransformCopy(miniGameCameraT, danceOffCameraInitialPos);
					miniGameCameraT.gameObject.SetActive(true);
					StartCoroutine(NpcDancingTurn());
				}
			}
			else {
				if (isPlayerTurn) {
					arrowV = Input.GetAxisRaw(PlayerUtility.Vertical);
					arrowH = Input.GetAxisRaw(PlayerUtility.Horizontal);

					if (Input.GetButtonDown(PlayerUtility.Vertical) || Input.GetButtonDown(PlayerUtility.Horizontal)) {

						for (int i = 0; i < baseDanceData.KeyHolder.Length; i++) {
							if (!baseDanceData.KeyHolder[i].DanceKeyEnabled) {
								if (baseDanceData.KeyHolder[i].DanceKeyInput == keyPointer) {
									if (baseDanceData.KeyHolder[i].DanceKeyInput.KeyID == ArrowKeyID.Vertical &&
										baseDanceData.KeyHolder[i].DanceKeyInput.KeyAxis == arrowV) {
										baseDanceData.KeyHolder[i].DanceKeyEnabled = true;

										keyPointerIndx++;
										keyPointer = (keyPointerIndx < baseDanceData.KeyHolder.Length)
											? baseDanceData.KeyHolder[keyPointerIndx].DanceKeyInput
											: new ArrowKey();

										AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.DanceOffCorrectBtn, true);
										audioSource.gameObject.SetActive(true);
										audioSource.Play();

										StartCoroutine("NpcPlayerDance");
										break;
									}

									if (baseDanceData.KeyHolder[i].DanceKeyInput.KeyID == ArrowKeyID.Horizontal &&
										baseDanceData.KeyHolder[i].DanceKeyInput.KeyAxis == arrowH) {
										baseDanceData.KeyHolder[i].DanceKeyEnabled = true;

										keyPointerIndx++;
										keyPointer = (keyPointerIndx < baseDanceData.KeyHolder.Length)
											? baseDanceData.KeyHolder[keyPointerIndx].DanceKeyInput
											: new ArrowKey();

										AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.DanceOffCorrectBtn, true);
										audioSource.gameObject.SetActive(true);
										audioSource.Play();

										StartCoroutine("NpcPlayerDance");
										break;
									}
								}

								// If nothing breaks it at the top, count it as wrong immediately
								if (danceDataIndx < danceDataList.Length - 1) {
									Debug.Log("WRONG!");
									StopCoroutine("NpcPlayerDance");
									baseDanceData.DanceSuccess = false;
									baseDanceData.DanceDone = true;

									// Reset Animations to Idle state
									npcAnimator.SetFloat("Dancing", 0f);
									playerAnimator.SetFloat("Dancing", 0f);

									// Assign next dance data
									danceDataIndx++;
									baseDanceData = danceDataList[danceDataIndx];

									AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.DanceOffWrongBtn, true);
									audioSource.gameObject.SetActive(true);
									audioSource.Play();
				
									StartCoroutine(NpcDancingTurn());
									break;
								}
								else {
									if (!gameEnded) {
										baseDanceData.DanceSuccess = false;
										baseDanceData.DanceDone = true;
										StartCoroutine(GameEnd());
										gameEnded = true;
									}
								}
							}
						}
					}

					if (IsPlayerTurnDone() && !isPlayerDancing) {
						if (danceDataIndx < danceDataList.Length - 1) {
							StopCoroutine("NpcPlayerDance");
							baseDanceData.DanceSuccess = true;
							baseDanceData.DanceDone = true;

							// Reset Animations to Idle state
							npcAnimator.SetFloat("Dancing", 0f);
							playerAnimator.SetFloat("Dancing", 0f);

							// Assign next dance data
							danceDataIndx++;
							baseDanceData = danceDataList[danceDataIndx];

							StartCoroutine(NpcDancingTurn());
						}
						else {
							if (!gameEnded) {
								baseDanceData.DanceSuccess = true;
								baseDanceData.DanceDone = true;
								StartCoroutine(GameEnd());
								gameEnded = true;
							}
						}
					}
				}
			}
		}
	}

	public void MainGUI(Event e) {
		if (danceEnabled) {
			if (instructionEnabled) {
				DanceInstructions();
			}
			else {
				if (baseDanceData != null) {
					baseDanceData.OnGUIDraw(mainRect);
				}

				if (informationEnabled) {
					DanceInformation();
				}
			}
		}
	}

	private void DanceInstructions() {
		GUI.BeginGroup(mainRect);

		// Dim background
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.DrawTexture(mainRect, Resources.BlackTexture);
		GUI.color = guiOriginalColor;

		// Instruction Box
		Rect instructionsBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.5f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref instructionsBox, Anchor.MiddleCenter);
		GUI.Box(instructionsBox, string.Empty, danceSkin.GetStyle("Dance BG"));

		GUI.BeginGroup(instructionsBox);

        //// Title
        //Rect titleRect = new Rect(instructionsBox.width * 0.5f, instructionsBox.height * 0.1f, instructionsBox.width * 0.9f, instructionsBox.height * 0.1f);
        //AnchorPoint.SetAnchor(ref titleRect, Anchor.MiddleCenter);
        //GUI.Box(titleRect, "TITLE GOES HERE!", tempSkin.GetStyle("Block"));

		// Desctiption
		Rect descRect = new Rect(instructionsBox.width * 0.5f, instructionsBox.height * 0.45f, instructionsBox.width * 0.9f, instructionsBox.height * 0.7f);
		AnchorPoint.SetAnchor(ref descRect, Anchor.MiddleCenter);
        GUI.DrawTexture(descRect, danceSkin.GetStyle("Dance Instruction").normal.background);
        //GUI.Box(descRect, string.Empty, danceSkin.GetStyle("Dance Instruction"));

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

	private void DanceInformation() {
		GUI.BeginGroup(mainRect);
		Rect instructionBoxRect = new Rect(mainRect.width * 0.85f, mainRect.height * 0.4f, mainRect.width * 0.25f, mainRect.height * 0.4f);
		AnchorPoint.SetAnchor(ref instructionBoxRect, Anchor.MiddleCenter);
        GUI.Box(instructionBoxRect, string.Empty, danceSkin.GetStyle("Dance Info BG"));

		GUI.BeginGroup(instructionBoxRect);
        //Rect instructionTitle = new Rect(instructionBoxRect.width * 0.5f, instructionBoxRect.height * 0.1f, instructionBoxRect.width * 0.9f, instructionBoxRect.height * 0.15f);
        //AnchorPoint.SetAnchor(ref instructionTitle, Anchor.MiddleCenter);
        //GUI.Box(instructionTitle, "Levels", danceSkin.GetStyle("Dance Info Title"));

		for (int i = 0; i < danceDataList.Length; i++) {
			Rect levelNum = new Rect(
				instructionBoxRect.width * 0.3f,
				(instructionBoxRect.height * 0.15f) * i + (instructionBoxRect.height * 0.225f),
				instructionBoxRect.width * 0.4f,
				instructionBoxRect.height * 0.15f
			);
			AnchorPoint.SetAnchor(ref levelNum, Anchor.MiddleCenter);
            GUI.Box(levelNum, string.Empty + (i + 1), danceSkin.GetStyle("Dance Info Number Box"));

			if (danceDataList[i].DanceDone) {
				Rect levelStat = new Rect(
					instructionBoxRect.width * 0.7f,
					(instructionBoxRect.height * 0.15f) * i + (instructionBoxRect.height * 0.225f),
					instructionBoxRect.width * 0.25f,
					instructionBoxRect.height * 0.125f
				);
				AnchorPoint.SetAnchor(ref levelStat, Anchor.MiddleCenter);


				if (danceDataList[i].DanceSuccess) {
                    GUI.DrawTexture(levelStat, danceSkin.GetStyle("Dance Info Ok").normal.background);
                    //GUI.Box(levelStat, string.Empty, danceSkin.GetStyle("Dance Info Ok"));
				}
				else {
                    GUI.DrawTexture(levelStat, danceSkin.GetStyle("Dance Info Fail").normal.background);
                    //GUI.Box(levelStat, string.Empty, danceSkin.GetStyle("Dance Info Fail"));
				}
			}
		}
		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void RunDance() {
		UserInterface.RunTitle(string.Empty, "It's Time to Groove!", string.Empty, new FadeTransition(2f, null, null, InitializeInstruction));

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
		npcControl = npcManager.GetNpcControl(playerInformation.InteractingTo);
		npcControl.CurState = NPCState.MiniGame;
		npcAnimator = npcControl.NpcAnimator;

		previousPlayerPos = new Vector3();
		previousPlayerPos = playerT.position;

		previousPlayerRot = new Quaternion();
		previousPlayerRot = playerT.rotation;

		previousNpcPos = new Vector3();
		previousNpcPos = npcT.position;

		previousNpcRot = new Quaternion();
		previousNpcRot = npcT.rotation;

		danceOffCameraInitialPos = playerControl.DanceOffCameraInitialPos;
		danceOffCameraMainPos = playerControl.DanceOffCameraMainPos;
		danceOffCameraNpcPos = playerControl.DanceOffCameraNpcPos;
		danceOffNpcPos = playerControl.DanceOffNpcPos;
		danceOffPlayerPos = playerControl.DanceOffPlayerPos;


		System.Random rng = new System.Random();
		int randKeyIndx = 0;

		int[] randKeys = new int[danceKeyCount[4]];
		for (int i = 0; i < randKeys.Length; i++) {
			randKeyIndx = rng.Next(0, arrowKeyDb.Count);
			randKeys[i] = randKeyIndx;
		}

		int k = 0;
		danceDataList = new DanceData[5];
		for (int i = 0; i < danceDataList.Length; i++) {
			danceDataList[i] = new DanceData(danceKeyCount[i]);
			k = 0;
			for (int j = 0; j < danceDataList[i].KeyHolder.Length; j++) {
				danceDataList[i].KeyHolder[j] = new ArrowKeyHolder(arrowKeyDb[randKeys[k]], danceKeyPos[i] + (Vector2.right * j));
				danceDataList[i].KeyHolder[j].DanceKeyHolderStyle = danceSkin.GetStyle("Dance Key Box");
				danceDataList[i].KeyHolder[j].DanceKeyHolderEnabled = false;
				danceDataList[i].KeyHolder[j].DanceKeyEnabled = false;
				k++;
			}
			danceDataList[i].Reset();
		}


		danceDataIndx = 0;
		baseDanceData = danceDataList[danceDataIndx];

		int randomAudio = 0;
		randomAudio = rng.Next(0, 2);
		if (randomAudio == 0) {
			gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.DanceOffBGM1, false));
		}
		else {
			gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.DanceOffBGM2, false));
		}

		gameEnded = false;
		instructionEnabled = false;
		danceEnabled = true;
	}

	private void PopulateDatabase() {
		arrowKeyDb = new List<ArrowKey>();
		arrowKeyDb.Add(new ArrowKey(1, ArrowKeyID.Vertical, danceSkin.GetStyle("Dance Key Up")));
        arrowKeyDb.Add(new ArrowKey(-1, ArrowKeyID.Vertical, danceSkin.GetStyle("Dance Key Down")));
        arrowKeyDb.Add(new ArrowKey(1, ArrowKeyID.Horizontal, danceSkin.GetStyle("Dance Key Right")));
        arrowKeyDb.Add(new ArrowKey(-1, ArrowKeyID.Horizontal, danceSkin.GetStyle("Dance Key Left")));

		System.Random rng = new System.Random();
		int randKeyIndx = 0;

		danceDataList = new DanceData[5];
		for (int i = 0; i < danceDataList.Length; i++) {
			danceDataList[i] = new DanceData(danceKeyCount[i]);
			for (int j = 0; j < danceDataList[i].KeyHolder.Length; j++) {
				randKeyIndx = rng.Next(0, arrowKeyDb.Count);
				danceDataList[i].KeyHolder[j] = new ArrowKeyHolder(arrowKeyDb[randKeyIndx], danceKeyPos[i] + (Vector2.right * j));
                danceDataList[i].KeyHolder[j].DanceKeyHolderStyle = danceSkin.GetStyle("Dance Key Box");
			}
		}
	}

	private void InitializeInstruction() {
		instructionCountDown = instructionTime;
		instructionEnabled = true;
	}

	private IEnumerator NpcDancingTurn() {
		isPlayerTurn = false;

		// Hide keys
		for (int i = 0; i < baseDanceData.KeyHolder.Length; i++) {
			baseDanceData.KeyHolder[i].DanceKeyHolderEnabled = false;
			baseDanceData.KeyHolder[i].DanceKeyEnabled = false;
		}

		// Start moving the camera from initial position to it's regular position
		float distance = 0f;
		Vector3 cameraVelocity = Vector3.zero;
		distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraMainPos.position);

		while (distance > 0.1f) {
			miniGameCameraT.position = Vector3.SmoothDamp(miniGameCameraT.position, danceOffCameraMainPos.position, ref cameraVelocity, cameraSmoothDampTime);
			distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraMainPos.position);
			yield return null;
		}

		yield return new WaitForSeconds(1f);

		// Start moving the camera to Npc
		distance = 0;
		distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraNpcPos.position);
		cameraVelocity = Vector3.zero;

		while (distance > 0.1f) {
			miniGameCameraT.position = Vector3.SmoothDamp(miniGameCameraT.position, danceOffCameraNpcPos.position, ref cameraVelocity, cameraSmoothDampTime);
			distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraNpcPos.position);
			yield return null;
		}

		yield return new WaitForSeconds(0.1f);

		// Npc dances as well as the keyinput is shown
		float time = 0f;
		int keyIndx = 0;

		float danceRoutine = 0f;
		float danceRoutineTarget = 0f;
		danceRoutineTarget = danceRoutine + 1;

		while (keyIndx < baseDanceData.KeyHolder.Length) {
			if (danceRoutine >= danceRoutineTarget) {
				baseDanceData.KeyHolder[keyIndx].DanceKeyHolderEnabled = true;
				baseDanceData.KeyHolder[keyIndx].DanceKeyEnabled = true;
				keyIndx++;

				time = 0f;
				danceRoutine = danceRoutineTarget;
				danceRoutineTarget = danceRoutine + 1;
				yield return new WaitForSeconds(0.5f);
			}

			time += Time.deltaTime;
			danceRoutine = Mathf.Lerp(danceRoutine, danceRoutineTarget, Mathf.PingPong(time, 1f) / 1f);
			npcAnimator.SetFloat("Dancing", danceRoutine);
			yield return null;
		}

		danceRoutine = 0f;
		npcAnimator.SetFloat("Dancing", danceRoutine);

		yield return new WaitForSeconds(0.1f);

		// Hide keys
		for (int i = 0; i < baseDanceData.KeyHolder.Length; i++) {
			baseDanceData.KeyHolder[i].DanceKeyHolderEnabled = false;
			baseDanceData.KeyHolder[i].DanceKeyEnabled = false;
		}

		// Start moving back to orginal position
		distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraMainPos.position);
		cameraVelocity = Vector3.zero;

		while (distance > 0.1f) {
			miniGameCameraT.position = Vector3.SmoothDamp(miniGameCameraT.position, danceOffCameraMainPos.position, ref cameraVelocity, cameraSmoothDampTime);
			distance = Vector3.Distance(miniGameCameraT.position, danceOffCameraMainPos.position);
			yield return null;
		}

		yield return new WaitForSeconds(0.1f);

		// Show key holders
		for (int i = 0; i < baseDanceData.KeyHolder.Length; i++) {
			baseDanceData.KeyHolder[i].DanceKeyHolderEnabled = true;
		}

		keyPointerIndx = 0;
		keyPointer = baseDanceData.KeyHolder[keyPointerIndx].DanceKeyInput;

		isPlayerTurn = true;
	}

	private IEnumerator NpcPlayerDance() {
		isPlayerDancing = true;
		float danceRoutine = 0f;
		float time = 0f;
		danceRoutine = npcAnimator.GetFloat("Dancing");

		while (danceRoutine < keyPointerIndx) {
			time += Time.deltaTime;
			danceRoutine = Mathf.Lerp(danceRoutine, keyPointerIndx, Mathf.PingPong(time, 1f) / 1f);
			npcAnimator.SetFloat("Dancing", danceRoutine);
			playerAnimator.SetFloat("Dancing", danceRoutine);
			yield return null;
		}

		isPlayerDancing = false;
	}

	private bool IsPlayerTurnDone() {
		return (baseDanceData.KeyHolder.Length == keyPointerIndx);
	}

	private IEnumerator GameEnd() {
		yield return new WaitForSeconds(0.5f);
		Debug.Log("END");
		StopCoroutine("NpcPlayerDance");

		playerT.position = previousPlayerPos;
		playerT.rotation = previousPlayerRot;

		npcT.position = previousNpcPos;
		npcT.rotation = previousNpcRot;

		// Reset Animations to Idle state
		npcAnimator.SetFloat("Dancing", 0f);
		playerAnimator.SetFloat("Dancing", 0f);

		// Hide keys
		for (int i = 0; i < baseDanceData.KeyHolder.Length; i++) {
			baseDanceData.KeyHolder[i].DanceKeyHolderEnabled = false;
			baseDanceData.KeyHolder[i].DanceKeyEnabled = false;
		}

		// Count the numbers of correct
		int count = 0;
		for (int i = 0; i < danceDataList.Length; i++) {
			if (danceDataList[i] != null && danceDataList[i].DanceSuccess) {
				count++;
			}
		}

		// Designate penalty for corrects
		if (count == danceDataList.Length) {
			statisticsPenalty = Statistics.zero;
		}
		else if (count == 3 || count == 4) {
			statisticsPenalty = Statistics.dislikeStat * 5;
		}
		else if (count == 1 || count == 2) {
			statisticsPenalty = Statistics.dislikeStat * 10;
		}
		else if (count == 0) {
			statisticsPenalty = Statistics.dislikeStat * 15;
		}

		dialogueManager.AddTotalStatistics(statisticsPenalty);
		if (dialogueManager.DialogueDataIndx < (dialogueManager.NpcDialogueData.Length - 1)) {
			dialogueManager.AddTotalStatisticsToNpc();
		}
		else {
			dialogueManager.DialogueEnabled = true;
		}

		GameUtility.SetGameObjectLayerRecursively(npcT, LayerManager.LayerNPC);
		npcControl.Reset();

		danceDataIndx = 0;
		baseDanceData = danceDataList[danceDataIndx];

		keyPointerIndx = 0;
		keyPointer = baseDanceData.KeyHolder[keyPointerIndx].DanceKeyInput;

		arrowH = 0;
		arrowV = 0;

		isPlayerTurn = false;
		isPlayerDancing = false;

		miniGameCameraT.gameObject.SetActive(false);

		gameManager.SetAudioSource(AudioManager.current.GetAudioSource(AudioNameID.MainGameBGM, false));

		informationEnabled = false;
		danceEnabled = false;
	}
}