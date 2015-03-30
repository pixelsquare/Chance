using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using GameUtilities.LayerManager;
using GameUtilities.PlayerUtility;
using MiniGame;
using NPC;
using NPC.Database;
using UnityEngine;


public class DialogueManager : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin gameStorySkin;
	[SerializeField]
	private GUISkin dialogueSkin;
	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private Texture2D blackTexture;
	[SerializeField]
	private float textPrintDelay = 0.03f;
	[SerializeField]
	private int maxTextCount = 99;
	[SerializeField]
	private DialogueType dialogueType;

	# endregion Public Variables

	# region Private Variables

	// Screen Variables
	private Rect mainRect;
	private float oldScreenHeightRatio;
	private float screenHeightRatio;

	private bool dialogueEnabled;

	private int errorIndices;					// Computes for the excess indexes to get quick print
	private int charCount;						// Value that needed for the error
	private int charIndx;						// Index that iterates the string

	private string dialogueInputText = string.Empty;			// Input Text
	private string dialogueOutputText = string.Empty;			// Output Text

	private bool isPrinting;
	private bool donePrinting;

	private int dialogueDBIndx;
	private int dialogueDataIndx;
	private int dialogueMaxCount;

	private bool showNPCInformation;

	private Transform npcT;
	private NPCControl npcControl;
	private NPCInformation npcInformation;

	private NPCData baseNPCData;
	private NPCDialogue[] npcDialogueList;
	private DialogueData[] npcDialogueData;

	private Event e;
	private DialogueButton pressedButton;

	// Window Backdrop
	private Color guiOriginalColor;
	private Color textureColor;

	// Game Story Variables
	private bool isGameStory;
	private Story[] storyLine;
	private int storyLineIndx = 0;
	private float skipTextAlpha = 0;

	private Statistics totalStatistics;

	private Vector2 npcDescScrollPos;
	private GUIContent content = new GUIContent();

	private float spacebarAlpha;
	private float spacebarAlphaTime;
	private Color spacebarColor;

	private GameManager gameManager;
	private AngerGameUi miniGameUi;
	private NPCManager npcManager;
	private MissionManager missionManager;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public bool DialogueEnabled {
		get { return dialogueEnabled; }
		set { 
			dialogueEnabled = value; 

			// Pause printing when dialogue is disabled
			if(value){
				StartPrinting();
			}
			else {
				StopCoroutine("NormalPrint");
			}
		}
	}

	public bool IsGameStory {
		get { return isGameStory; }
		set { isGameStory = value; }
	}

	public NPCData BaseNPCData {
		get { return baseNPCData; }
	}
	// --

	public static DialogueManager current;

	private void Awake() {
		current = this;
		GameStory.Initialize();
	}

	private void Start() {
		gameManager = GameManager.current;
		miniGameUi = AngerGameUi.current;
		npcManager = NPCManager.current;
		missionManager = MissionManager.current;
		textureColor = new Color(0f, 0f, 0f, 0.1f);
		//RunDialogue(NPCNameID.Andy, DialogueType.Normal, 0); // Run In Intro not in main game (gameManager Resets Dialogue thus it will not work)
	}

	private void Update() {
		if (dialogueEnabled) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (gameManager.GameState == GameState.MainGame || gameManager.GameState == GameState.Intro) { // Remove Intro
				if (!isGameStory) {
					if (Input.GetButtonDown(PlayerUtility.NextDialogue)) {
						NextDialogue();
						spacebarAlphaTime = 0f;
					}

					// Keyboard input for dialogue buttons
					if (npcDialogueData[dialogueDataIndx].Buttons != null) {
						if (npcDialogueData[dialogueDataIndx].Buttons.Length > 2) {
							if (Input.GetButtonDown(PlayerUtility.DialogueButton1)) {
								DialogueButtonDown(0);
							}
							if (Input.GetButtonDown(PlayerUtility.DialogueButton2)) {
								DialogueButtonDown(1);
							}
							if (Input.GetButtonDown(PlayerUtility.DialogueButton3)) {
								DialogueButtonDown(2);
							}
						}
						else {
							if (Input.GetButtonDown(PlayerUtility.DialogueButton1)) {
								DialogueButtonDown(0);
							}
							if (Input.GetButtonDown(PlayerUtility.DialogueButton2)) {
								DialogueButtonDown(1);
							}
						}
					}

					if (donePrinting) {
						spacebarAlphaTime += Time.deltaTime;
						spacebarAlpha = Mathf.Lerp(1f, 0.1f, Mathf.PingPong(spacebarAlphaTime, 0.5f) / 0.5f);
					}
				}
			}
			else if (gameManager.GameState == GameState.GameStory) {
				// Skip the story
				if (Input.GetButtonDown(PlayerUtility.NextDialogue)) {
					gameManager.SwitchGameState(GameState.MainGame);
				}

				skipTextAlpha = Mathf.Lerp(0f, 1f, Mathf.PingPong(Time.time, 1f) / 1f);
			}
		}
	}

	private void OnGUI() {
		if (dialogueEnabled) {
			e = Event.current;
			GUI.depth = GUIDepth.dialogueDepth;

			if (gameManager.GameState == GameState.GameStory && isGameStory) {
				DrawGameStory();
			}
			else if (gameManager.GameState == GameState.MainGame || gameManager.GameState == GameState.Intro) {	// Remove Intro
				// Draw Dialogue
				DrawDialogue();

				// NPC Information
				if (showNPCInformation) {
					NpcInformationTooltip();
				}
			}
		}
	}

	/* Game Story */
	public void RunGameStory() {
		isGameStory = true;
		dialogueEnabled = true;
		storyLine = GameStory.StoryText;
		StartCoroutine("PrintGameStory");
	}

	private void DrawGameStory() {
		GUI.BeginGroup(mainRect);

		// Dialogue Backdrop
		GUI.DrawTexture(mainRect, blackTexture);

		Rect textRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.9f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		GUI.Box(textRect, "> " + dialogueOutputText + " <", gameStorySkin.GetStyle("Story Text"));

		GUI.EndGroup();

		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, skipTextAlpha);

		Rect skipTextRect = new Rect(Screen.width * 0.5f, Screen.height * 0.95f, Screen.width * 0.3f, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref skipTextRect, Anchor.MiddleCenter);
		GUI.Box(skipTextRect, "<<< Press (Spacebar) to SKIP >>>", gameStorySkin.GetStyle("Story Skip Text"));
		GUI.color = guiOriginalColor;
	}

	private IEnumerator PrintGameStory() {
		yield return new WaitForSeconds(1.5f);
		while (storyLineIndx < storyLine.Length) {
			if (!isPrinting) {
				dialogueInputText = storyLine[storyLineIndx].Text;
				isPrinting = true;
				charCount = 0;
				StartCoroutine("NormalPrint");

				if (donePrinting) {
					yield return new WaitForSeconds(storyLine[storyLineIndx].Duration);
					dialogueOutputText = string.Empty;
					storyLineIndx++;
					charCount = 0;
					charIndx = 0;
					donePrinting = false;
				}
			}
			yield return null;
		}

		gameManager.SwitchGameState(GameState.MainGame);
	}

	/* Dialogue */
	private void DrawDialogue() {
		GUI.BeginGroup(mainRect);

		guiOriginalColor = GUI.color;
		GUI.color = textureColor;
		GUI.DrawTexture(mainRect, blackTexture);
		GUI.color = guiOriginalColor;

		// Main Character Avatar
		if (npcDialogueData[dialogueDataIndx].Texture != null) {
			Rect avatarRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.5f, mainRect.height * 0.5f);
			AnchorPoint.SetAnchor(ref avatarRect, Anchor.MiddleCenter);
			GUI.DrawTexture(avatarRect, npcDialogueData[dialogueDataIndx].Texture);
		}

		// Dialogue box
		Rect dialogueRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.71f, mainRect.width * 0.95f, mainRect.height * 0.18f);
		AnchorPoint.SetAnchor(ref dialogueRect, Anchor.MiddleCenter);
		//GUI.Box(dialogueRect, string.Empty, dialogueSkin.GetStyle(dialogueSkin.name + " Background"));
		GUI.Box(dialogueRect, string.Empty, dialogueSkin.GetStyle("Dialogue Box"));

		// Dialogue Text
		Rect textRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.71f, mainRect.width * 0.8f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, dialogueOutputText);
		GUI.Box(textRect, dialogueOutputText, dialogueSkin.GetStyle("Dialogue Box Text"));

		// Dialogue Button (If there are any)
		if (donePrinting && npcDialogueData[dialogueDataIndx].Buttons != null) {
			for (int i = 0; i < npcDialogueData[dialogueDataIndx].Buttons.Length; i++) {
				Rect buttonRect;
				if (npcDialogueData[dialogueDataIndx].Buttons.Length > 2) {
					buttonRect = new Rect(mainRect.width * 0.18f + (i * (mainRect.width * 0.32f)), mainRect.height * 0.79f, mainRect.width * 0.3f, mainRect.height * 0.06f);
				}
				else {
					buttonRect = new Rect(mainRect.width * 0.23f + (i * (mainRect.width * 0.55f)), mainRect.height * 0.78f, mainRect.width * 0.4f, mainRect.height * 0.07f);
				}
				AnchorPoint.SetAnchor(ref buttonRect, Anchor.MiddleCenter);

				if (npcDialogueData[dialogueDataIndx].Buttons.Length > 2) {
					if (i == 0) {
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Button Left"));
					}
					else if (i == 1) {
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Name BG"));
					}
					else if (i == 2) {
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Button Right"));
					}
				}
				else {
					if (i == 0) {
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Button Left"));
					}
					else if (i == 1) {
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Button Right"));
					}
				}

				//GUI.BeginGroup(buttonRect);
				//Rect hotkeyRect = new Rect(buttonRect.width * 0.5f, buttonRect.height * 0.1f, buttonRect.width * 0.1f, buttonRect.height * 0.1f);
				//AnchorPoint.SetAnchor(ref hotkeyRect, Anchor.MiddleCenter);
				//GUI.Box(hotkeyRect, "[" + (i + 1) + "]", tempSkin.GetStyle("Block"));
				//GUI.EndGroup();


				if (buttonRect.Contains(e.mousePosition)) {
					// When button was pressed
					if (e.button == 0 && e.type == EventType.mouseUp) {
						pressedButton = npcDialogueData[dialogueDataIndx].Buttons[i];
						Debug.Log("[DIALOGUE BUTTON] " + pressedButton.ButtonName);

						baseNPCData.NpcNextDialogueIndx = pressedButton.GoToDialogueIndx;
						if (pressedButton.GoToDialogueIndx == -1) {
							Debug.Log("[DIALOGUE ENDED] " + baseNPCData.NpcName);
							baseNPCData.NpcDialogueEnded = true;
						}
						else {
							Debug.Log("[DIALOGUE JUMP] " + baseNPCData.NpcName + " next dialogue is " + baseNPCData.NpcNextDialogueIndx);
						}

						if (pressedButton.ButtonType == ButtonType.Correct) {
							Debug.Log("[DIALOGUE] Correct!");
						}
						else if (pressedButton.ButtonType == ButtonType.Wrong) {
							Debug.Log("[DIALOGUE] Wrong! Entering DANGER MODE!");
							DialogueEnabled = false;

							if (pressedButton.ToughnessLevel != ToughnessLevel.None) {
								miniGameUi.RunMiniGame(
									pressedButton.ToughnessLevel,
									baseNPCData.NpcSympathyText.Text,
									new MiniGameResult(DialogueMiniGameStart, DialogueMiniGameFailed, DialogueMiniGameSuccess, DialogueMiniGameEnd)
								);
							}
						}

						// Add relationship stat to NPC
						Statistics stat = pressedButton.AddedStatistics;
						if (stat != Statistics.zero) {
							AddTotalStatistics(new Statistics(stat.Like, stat.Dislike));
						}						

						// End Dialogue when the hierarchy stop; else iterate hieharchy
						if (pressedButton.ButtonDialogueData != null) {
							// Allocate new Dialogue data that is within the button parameters
							npcDialogueData = pressedButton.ButtonDialogueData;
							dialogueDataIndx = 0;
							dialogueInputText = npcDialogueData[dialogueDataIndx].Dialogue;
							dialogueMaxCount = npcDialogueData.Length;
							errorIndices = charCount = charIndx = 0;
							donePrinting = false;
							StartPrinting();
							break;
						}
						else {
							dialogueEnabled = false;
							if (!dialogueEnabled && !miniGameUi.DangerModeEnabled) {
								AddTotalStatisticsToNpc();
							}
						}
					}
				}
			}
		}

		Rect nameBoxRect = new Rect(mainRect.width * 0.12f, mainRect.height * 0.63f, mainRect.width * 0.1f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref nameBoxRect, Anchor.MiddleCenter);
		//GUI.Box(nameBoxRect, string.Empty);

		# region Name Box
		GUI.BeginGroup(nameBoxRect);

		// Dialogue Name BG
		Rect nameBgRect = new Rect(nameBoxRect.width * 0.5f, nameBoxRect.height * 0.5f, nameBoxRect.width, nameBoxRect.height);
		AnchorPoint.SetAnchor(ref nameBgRect, Anchor.MiddleCenter);
		GUI.Box(nameBgRect, string.Empty, dialogueSkin.GetStyle("Dialogue Name BG"));

		// Dialogue Name 
		Rect nameRect = new Rect(nameBoxRect.width * 0.5f, nameBoxRect.height * 0.5f, nameBoxRect.width, nameBoxRect.height);
		AnchorPoint.SetAnchor(ref nameRect, Anchor.MiddleCenter);
		GUI.Box(nameRect, baseNPCData.NpcName, dialogueSkin.GetStyle("Dialogue Name"));

		GUI.EndGroup();
		# endregion Name Box

		# region Name Box Tooltip
		// NPC Information Tooltip
		Rect infoTooltipRect = new Rect(mainRect.width * 0.18f, mainRect.height * 0.63f, mainRect.width * 0.015f, mainRect.height * 0.015f);
		AnchorPoint.SetAnchor(ref infoTooltipRect, Anchor.MiddleCenter);
		GUI.Box(infoTooltipRect, "?", tempSkin.GetStyle("Button"));

		if (infoTooltipRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				showNPCInformation = !showNPCInformation;
			}
		}

		if (infoTooltipRect.Contains(e.mousePosition)) {
			Rect toolTipRect = new Rect(e.mousePosition.x, e.mousePosition.y, mainRect.width * 0.12f, mainRect.height * 0.025f);
			AnchorPoint.SetAnchor(ref toolTipRect, Anchor.MiddleLeft);
			GUI.Box(toolTipRect, "Information", tempSkin.GetStyle("Block"));
		}
		# endregion Name Box Tooltip

		if (donePrinting && npcDialogueData[dialogueDataIndx].Buttons == null) {
			spacebarColor = dialogueSkin.GetStyle("Dialogue Spacebar Text").normal.textColor;
			guiOriginalColor = GUI.color;
			GUI.color = new Color(spacebarColor.r, spacebarColor.g, spacebarColor.b, spacebarAlpha);
			Rect spacebarRect = new Rect(mainRect.width * 0.85f, mainRect.height * 0.765f, mainRect.width * 0.3f, mainRect.height * 0.025f);
			AnchorPoint.SetAnchor(ref spacebarRect, Anchor.MiddleCenter);
			GUI.Box(spacebarRect, "SPACEBAR ►", dialogueSkin.GetStyle("Dialogue Spacebar Text"));
			GUI.color = guiOriginalColor;
		}

		GUI.EndGroup();
	}

	private void NpcInformationTooltip() {
		GUI.BeginGroup(mainRect);

		// Information Box holder
		Rect informationBoxRect = new Rect(mainRect.width * 0.13f, mainRect.height * 0.45f, mainRect.width * 0.2f, mainRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref informationBoxRect, Anchor.MiddleCenter);
		//GUI.Box(informationBoxRect, string.Empty, dialogueSkin.GetStyle(dialogueSkin.name + " Info BG"));
		GUI.Box(informationBoxRect, string.Empty, dialogueSkin.GetStyle("Dialogue NPC BG"));

		# region NPC Statistics

		GUI.BeginGroup(informationBoxRect);

		// NPC Name
		Rect nameRect = new Rect(informationBoxRect.width * 0.5f, informationBoxRect.height * 0.09f, informationBoxRect.width * 0.9f, informationBoxRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref nameRect, Anchor.MiddleCenter);
		GUI.Box(nameRect, baseNPCData.NpcName, dialogueSkin.GetStyle("Dialogue NPC Name"));

		// NPC Background Story
		Rect bgStoryRect = new Rect(informationBoxRect.width * 0.45f, informationBoxRect.height * 0.31f, informationBoxRect.width * 0.9f, informationBoxRect.height * 0.35f);
		// Make the text center when text is less than 102
		bgStoryRect.x = (baseNPCData.NpcDesc.Length > 102) ? informationBoxRect.width * 0.45f : informationBoxRect.width * 0.5f;
		AnchorPoint.SetAnchor(ref bgStoryRect, Anchor.MiddleCenter);

		Rect npcDescScrollView = new Rect(0f, 0f, informationBoxRect.width * 0.8f, informationBoxRect.height * 0.8f);
		content.text = baseNPCData.NpcDesc;
		npcDescScrollView.height = dialogueSkin.GetStyle("Dialogue NPC Desc").CalcHeight(content, informationBoxRect.width * 0.69f);
		npcDescScrollPos = GUI.BeginScrollView(bgStoryRect, npcDescScrollPos, npcDescScrollView);

		Rect bgStoryText = new Rect(
			informationBoxRect.width * 0.09f,
			0f,
			informationBoxRect.width * 0.75f,
			dialogueSkin.GetStyle("Dialogue NPC Desc").CalcHeight(content, informationBoxRect.width * 0.69f)
		);

		GUI.Label(bgStoryText, baseNPCData.NpcDesc, dialogueSkin.GetStyle("Dialogue NPC Desc"));
		GUI.EndScrollView();

		Rect npcProgressStatRect = new Rect(informationBoxRect.width * 0.5f, informationBoxRect.height * 0.73f, informationBoxRect.width * 0.9f, informationBoxRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref npcProgressStatRect, Anchor.MiddleCenter);
		GUI.Box(npcProgressStatRect, string.Empty, progressBarSkin.GetStyle("Progress BG"));

		# region NPC Progress bar stats

		GUI.BeginGroup(npcProgressStatRect);
		// Art bar
		Rect artRect = new Rect(npcProgressStatRect.width * 0.5f, npcProgressStatRect.height * 0.15f, npcProgressStatRect.width * 0.85f, npcProgressStatRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref artRect, Anchor.MiddleCenter);
		GameGUI.ProgressBar(
			"Art [" + (float)baseNPCData.NpcStatistics.Art + "]",
			(float)baseNPCData.NpcStatistics.Art,
			(float)Statistics.statMax,
			artRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Programming bar
		Rect progRect = new Rect(npcProgressStatRect.width * 0.5f, npcProgressStatRect.height * 0.32f, npcProgressStatRect.width * 0.85f, npcProgressStatRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref progRect, Anchor.MiddleCenter);
		GameGUI.ProgressBar(
			"Programming [" + (float)baseNPCData.NpcStatistics.Programming + "]",
			(float)baseNPCData.NpcStatistics.Programming,
			(float)Statistics.statMax,
			progRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Design bar
		Rect designRect = new Rect(npcProgressStatRect.width * 0.5f, npcProgressStatRect.height * 0.49f, npcProgressStatRect.width * 0.85f, npcProgressStatRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref designRect, Anchor.MiddleCenter);
		GameGUI.ProgressBar(
			"Design [" + (float)baseNPCData.NpcStatistics.Design + "]",
			(float)baseNPCData.NpcStatistics.Design,
			(float)Statistics.statMax,
			designRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Sound bar
		Rect soundRect = new Rect(npcProgressStatRect.width * 0.5f, npcProgressStatRect.height * 0.65f, npcProgressStatRect.width * 0.85f, npcProgressStatRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref soundRect, Anchor.MiddleCenter);
		GameGUI.ProgressBar(
			"Sound [" + (float)baseNPCData.NpcStatistics.Sound + "]",
			(float)baseNPCData.NpcStatistics.Sound,
			(float)Statistics.statMax,
			soundRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Like / Dislike
		Rect likeDislikeRect = new Rect(npcProgressStatRect.width * 0.5f, npcProgressStatRect.height * 0.82f, npcProgressStatRect.width * 0.85f, npcProgressStatRect.height * 0.15f);
		AnchorPoint.SetAnchor(ref likeDislikeRect, Anchor.MiddleCenter);
		GameGUI.SliderBox(
			"Like / Dislike",
			(float)baseNPCData.NpcStatistics.Like / (float)Statistics.statMax,
			likeDislikeRect,
			progressBarSkin.GetStyle("Progress Bar Overlay"),
			progressBarSkin.GetStyle("Progress Bar Red Overlay"),
			tempSkin.GetStyle("Text1")
		);

		GUI.EndGroup(); // npcInfoRect

		# endregion NPC Progress bar stats

		GUI.EndGroup(); // informationBoxRect

		# endregion NPC Statistics

		GUI.EndGroup();
	}

	private bool IsEndDialogue() {
		return dialogueDataIndx == (dialogueMaxCount - 1) && donePrinting &&
			(npcDialogueData[dialogueDataIndx].Buttons == null);
	}

	public void RunDialogue(NPCNameID nameID, int selectionIndx) {
		Reset();
		dialogueEnabled = true;
		showNPCInformation = true;
		
		SetDataName(nameID, selectionIndx);
		StartPrinting();
	}

	public void RunReplyDialogue(NPCNameID nameID, ReplyType type, string itemName) {
		Reset();
		dialogueEnabled = true;
		showNPCInformation = true;

		dialogueDBIndx = (int)nameID;
		npcT = npcManager.GetMainNpc(nameID);
		npcControl = npcManager.GetMainNpcControl(nameID);
		npcInformation = npcManager.GetMainNpcInformation(nameID);
		baseNPCData = NPCDatabase.GetNPC(nameID);

		if (type == ReplyType.Accept) {
			npcDialogueData = NPCDatabase.GetNPCRandomAcceptedText(nameID);
		}
		else if (type == ReplyType.Decline) {
			npcDialogueData = NPCDatabase.GetNPCRandomDeclineText(nameID);
		}

		if (npcDialogueData == null || baseNPCData.NpcNameID == NPCNameID.None) {
			dialogueEnabled = false;
		}
		else {
			dialogueInputText = npcDialogueData[dialogueDataIndx].Dialogue + " " + itemName;
			dialogueMaxCount = npcDialogueData.Length;
		}

		StartPrinting();
	}

	private void SetDataName(NPCNameID nameID, int selectionIndx) {
		dialogueDBIndx = (int)nameID;
		npcT = npcManager.GetMainNpc(nameID);
		npcControl = npcManager.GetMainNpcControl(nameID);
		npcInformation = npcManager.GetMainNpcInformation(nameID);
		baseNPCData = NPCDatabase.GetNPC(nameID);

		if (dialogueType == DialogueType.Continous) {
				Debug.Log("[DIALOGUE: CONTINOUS MODE] Initialize Continous Dialogue");
				npcDialogueData = NPCDatabase.GetNPCContinousDialogueData(nameID);
			}
		else if (dialogueType == DialogueType.Random) {
				Debug.Log("[DIALOGUE: RANDOM MODE] Initialize Random Dialogue");
				npcDialogueData = NPCDatabase.GetNPCRandomDialogueData(nameID);
			}
		else if (dialogueType == DialogueType.Selection) {
			Debug.Log("[DIALOGUE: SELECTION MODE] Initialize Selection Dialogue");
			npcDialogueList = NPCDatabase.GetNPCDialogueList(nameID);

			if (selectionIndx > -1 && selectionIndx < npcDialogueList.Length) {
				npcDialogueData = NPCDatabase.GetNPCSelectionDialogueData(selectionIndx, nameID);
			}
			else {
				Debug.Log("[DIALOGUE: SELECTION MODE] Invalid Index. -> [Fallback to Continous Dialogue]");
				npcDialogueData = NPCDatabase.GetNPCContinousDialogueData(nameID);
			}
		}

		// Disable dialogue window if the data is empty
		if (npcDialogueData == null || baseNPCData.NpcNameID == NPCNameID.None) {
			dialogueEnabled = false;
		}
		else {
			dialogueInputText = npcDialogueData[dialogueDataIndx].Dialogue;
			dialogueMaxCount = npcDialogueData.Length;
		}
	}

	private void StartPrinting() {
		if (dialogueEnabled) {
			isPrinting = true;
			dialogueOutputText = string.Empty;
			errorIndices = 0;
			charCount = 0;
			charIndx = 0;
			donePrinting = false;
			StartCoroutine("NormalPrint");
		}
	}

	public void Reset() {
		StopAllCoroutines();
		dialogueInputText = string.Empty;
		dialogueOutputText = string.Empty;
		
		errorIndices = 0;
		charCount = 0;
		charIndx = 0;
		dialogueMaxCount = 0;

		showNPCInformation = false;
		isPrinting = false;
		donePrinting = false;
		dialogueEnabled = false;

		dialogueDBIndx = 0;
		dialogueDataIndx = 0;
		npcDialogueData = null;

		baseNPCData = new NPCData();
		isGameStory = false;
		storyLineIndx = 0;

		totalStatistics = new Statistics();
	}

	/* Printing */
	private void NextDialogue() {
		if (IsEndDialogue()) {
			dialogueEnabled = false;
			if (!dialogueEnabled) {
				AddTotalStatisticsToNpc();
			}
			
		}
		//dialogueEnabled = !IsEndDialogue();

		if (!isPrinting) {
			if (donePrinting && dialogueDataIndx < (npcDialogueData.Length - 1)) {
				dialogueDataIndx++;
				dialogueInputText = npcDialogueData[dialogueDataIndx].Dialogue;

				StartPrinting();
			}
		}
		else {
			QuickPrint();
		}
	}

	private void QuickPrint() {
		errorIndices = charCount;
		if (!donePrinting) {
			isPrinting = true;
			StopCoroutine("NormalPrint");
			dialogueOutputText = string.Empty;
			charCount = 0;
			charIndx -= errorIndices;

			for (int i = charIndx; i < dialogueInputText.Length; i++) {
				if (charCount > maxTextCount && dialogueInputText[i] == ' ') {
					charIndx++;
					charCount++;
					isPrinting = false;
					break;
				}
				dialogueOutputText += dialogueInputText[i];
				charIndx++;
				charCount++;
			}
		}

		if (charIndx >= dialogueInputText.Length) {
			donePrinting = true;
			isPrinting = false;
		}
	}

	private IEnumerator NormalPrint() {
		if (!donePrinting && dialogueEnabled) {
			for (int i = charIndx; i < dialogueInputText.Length; i++) {
				// Check for text overflow
				if (charCount > maxTextCount && dialogueInputText[i] == ' ') {
					charIndx++;
					charCount++;
					isPrinting = false;
					break;
				}

				dialogueOutputText += dialogueInputText[i];
				charIndx++;
				charCount++;
				yield return new WaitForSeconds(textPrintDelay);
			}
		}

		if (charIndx >= dialogueInputText.Length) {
			donePrinting = true;
			isPrinting = false;
		}
	}

	public void AddTotalStatistics(Statistics addedStat) {
		Statistics beforeStat = totalStatistics;
		Statistics afterStat = totalStatistics + addedStat;
		if (addedStat.Like > 0) {
			afterStat.Dislike -= addedStat.Like;
		}
		else if (addedStat.Dislike > 0) {
			afterStat.Like -= addedStat.Dislike;
		}
		afterStat.ClampAllValues();
		totalStatistics = afterStat;
		totalStatistics.ClampAllValues();

		Debug.Log("[NPC STATISTICS: TOTAL STATS GAINED] " + addedStat.ToString() + " has been added to total statistics " + 
			beforeStat.ToString() + " -> " + totalStatistics.ToString());
	}

	public void AddTotalStatisticsToNpc() {
		NPCDatabase.AddNPCStats((NPCNameID)dialogueDBIndx, new Statistics(totalStatistics.Like, totalStatistics.Dislike));
		missionManager.AddTalkedToNpc(baseNPCData.NpcNameID);
		baseNPCData.NpcTalkedCount++;
	}

	/* Others */
	private void DialogueMiniGameStart() {
		Debug.Log("[DIALOGUE MINI GAME] START");
		npcControl.RunPreResult();
		GameUtility.SetGameObjectLayerRecursively(npcT, LayerManager.LayerNpcInteracted);
		gameManager.BasePlayerData.PlayerControl.SwitchToDialogueCamera();
	}

	private void DialogueMiniGameEnd() {
		Debug.Log("[DIALOGUE MINI GAME] END");
		GameUtility.SetGameObjectLayerRecursively(NPCManager.current.GetMainNpc(baseNPCData.NpcNameID), LayerManager.LayerNPC);
		gameManager.BasePlayerData.PlayerControl.EndDialogueCamera();
	}

	private void DialogueMiniGameFailed() {
		Debug.Log("[DIALOGUE MINI GAME] RESULT: FALED");
		Statistics penaltyStat = miniGameUi.BaseMiniGameMode.GameOverPenalty;
		AddTotalStatistics(penaltyStat);

		Debug.Log("[MINI GAME PENALTY] " + baseNPCData.NpcName + " has recieved a penalty of " +
			penaltyStat.ToString() + " because of mini game undone. [" + miniGameUi.ToughnessLevel.ToString() + "]");
		AddTotalStatisticsToNpc();
		NPCManager.current.GetMainNpcControl(baseNPCData.NpcNameID).RunPostBadResult();
		npcInformation.RunEmoticon(EmoticonNameID.Sad);
	}

	private void DialogueMiniGameSuccess() {
		Debug.Log("[DIALOGUE MINI GAME] SUCCESS");
		NPCManager.current.GetMainNpcControl(baseNPCData.NpcNameID).RunPostGoodResult();
		npcInformation.RunEmoticon(EmoticonNameID.Happy);
		if (dialogueDataIndx < (npcDialogueData.Length - 1)) {
			AddTotalStatisticsToNpc();
		}
		else {
			DialogueEnabled = true;
		}
	}

	private void DialogueButtonDown(int indx) {
		pressedButton = npcDialogueData[dialogueDataIndx].Buttons[indx];
		Debug.Log("[DIALOGUE BUTTON] " + pressedButton.ButtonName);

		baseNPCData.NpcNextDialogueIndx = pressedButton.GoToDialogueIndx;
		if (pressedButton.GoToDialogueIndx == -1) {
			Debug.Log("[DIALOGUE ENDED] " + baseNPCData.NpcName);
			baseNPCData.NpcDialogueEnded = true;
		}
		else {
			Debug.Log("[DIALOGUE JUMP] " + baseNPCData.NpcName + " next dialogue is " + baseNPCData.NpcNextDialogueIndx);
		}

		if (pressedButton.ButtonType == ButtonType.Correct) {
			Debug.Log("[DIALOGUE] Correct!");
		}
		else if (pressedButton.ButtonType == ButtonType.Wrong) {
			Debug.Log("[DIALOGUE] Wrong! Entering DANGER MODE!");
			DialogueEnabled = false;

			if (pressedButton.ToughnessLevel != ToughnessLevel.None) {
				miniGameUi.RunMiniGame(
					pressedButton.ToughnessLevel,
					baseNPCData.NpcSympathyText.Text,
					new MiniGameResult(DialogueMiniGameStart, DialogueMiniGameFailed, DialogueMiniGameSuccess, DialogueMiniGameEnd)
				);
			}
		}

		// Add relationship stat to NPC
		Statistics stat = pressedButton.AddedStatistics;
		if (stat != Statistics.zero) {
			AddTotalStatistics(new Statistics(stat.Like, stat.Dislike));
		}

		// End Dialogue when the hierarchy stop; else iterate hieharchy
		if (pressedButton.ButtonDialogueData != null) {
			// Allocate new Dialogue data that is within the button parameters
			npcDialogueData = pressedButton.ButtonDialogueData;
			dialogueDataIndx = 0;
			dialogueInputText = npcDialogueData[dialogueDataIndx].Dialogue;
			dialogueMaxCount = npcDialogueData.Length;
			errorIndices = charCount = charIndx = 0;
			donePrinting = false;
			StartPrinting();
		}
		else {
			dialogueEnabled = false;
			if (!dialogueEnabled && !miniGameUi.DangerModeEnabled) {
				AddTotalStatisticsToNpc();
			}
		}
	}
}