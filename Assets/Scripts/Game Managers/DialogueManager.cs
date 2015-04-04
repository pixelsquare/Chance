using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GUIDepth;
using GameUtilities.PlayerUtility;
using MiniGames;
using NPC;
using NPC.Database;
using UnityEngine;
using GameUtilities.LayerManager;

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

	//private NPCControl npcControl;
	//private NPCInformation npcInformation;

	private NPCData baseNPCData;
	private NPCDialogue[] npcDialogueList;
	private DialogueData[] npcDialogueData;

	//private Event e;
	private DialogueButton pressedButton;

	// Window Backdrop
	private Color guiOriginalColor;
	private Color textureColor;

	// Game Story Variables
	private bool isGameStory;
	//private Story[] storyLine;
	//private int storyLineIndx = 0;
	//private float skipTextAlpha = 0;

	private bool questionBoxEnabled;

	private Statistics totalStatistics;

	private Vector2 npcDescScrollPos;
	private GUIContent content = new GUIContent();

	private float spacebarAlpha;
	private float spacebarAlphaTime;
	private Color spacebarColor;

	private GameManager gameManager;
	//private NPCManager npcManager;
	private KeypressGameUI keypressGame;
	private DanceUI danceGame;

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

	public int DialogueDataIndx {
		get { return dialogueDataIndx; }
	}

	public DialogueData[] NpcDialogueData {
		get { return npcDialogueData; }
	}
	// --

	public static DialogueManager current;

	private void Awake() {
		current = this;
		//GameStory.Initialize();
	}

	private void Start() {
		gameManager = GameManager.current;
		//npcManager = NPCManager.current;
		keypressGame = KeypressGameUI.current;
		danceGame = DanceUI.current;

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
			//else if (gameManager.GameState == GameState.GameStory) {
				// Skip the story
				//if (Input.GetButtonDown(PlayerUtility.NextDialogue)) {
				//    gameManager.SwitchGameState(GameState.MainGame);
				//}

				//skipTextAlpha = Mathf.Lerp(0f, 1f, Mathf.PingPong(Time.time, 1f) / 1f);
			//}
		}
	}

	public void MainGUI(Event e) {
		if (dialogueEnabled && !questionBoxEnabled) {
			GUI.depth = GUIDepth.dialogueDepth;

			if (gameManager.GameState == GameState.GameStory && isGameStory) {
				//DrawGameStory(e);
			}
			else if (gameManager.GameState == GameState.MainGame || gameManager.GameState == GameState.Intro) {	// Remove Intro
				// Draw Dialogue
				DrawDialogue(e);

				// NPC Information
				if (showNPCInformation) {
					NpcInformationTooltip(e);
				}
			}
		}

		if (questionBoxEnabled) {
			MiniGameQuestionBox(e);
		}
	}

	/* Dialogue */
	private void DrawDialogue(Event e) {
		GUI.BeginGroup(mainRect);

		guiOriginalColor = GUI.color;
		GUI.color = textureColor;
		GUI.DrawTexture(mainRect, blackTexture);
		GUI.color = guiOriginalColor;

		// Main Character Avatar
		if (npcDialogueData[dialogueDataIndx].Texture != null) {
			Rect avatarRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.3f, mainRect.height * 0.5f);
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
						GUI.Box(buttonRect, npcDialogueData[dialogueDataIndx].Buttons[i].ButtonName, dialogueSkin.GetStyle("Dialogue Button Middle"));
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
							Debug.Log("[DIALOGUE] Wrong!");
							//DialogueEnabled = false;
							questionBoxEnabled = true;
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
							if (!dialogueEnabled) {
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

	private void NpcInformationTooltip(Event e) {
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
		UserInterface.ProgressBar(
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
		UserInterface.ProgressBar(
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
		UserInterface.ProgressBar(
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
		UserInterface.ProgressBar(
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
		UserInterface.SliderBox(
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

	private void MiniGameQuestionBox(Event e) {
		GUI.BeginGroup(mainRect);
		Rect questionBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.3f, mainRect.height * 0.2f);
		AnchorPoint.SetAnchor(ref questionBox, Anchor.MiddleCenter);
        GUI.Box(questionBox, string.Empty, dialogueSkin.GetStyle("Dialogue Mini Game Box BG"));

		GUI.BeginGroup(questionBox);
		Rect questionText = new Rect(questionBox.width * 0.5f, questionBox.height * 0.4f, questionBox.width * 0.9f, questionBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref questionText, Anchor.MiddleCenter);
        GUI.Box(questionText, "You have offended " + baseNPCData.NpcName, dialogueSkin.GetStyle("Dialogue Mini Game Text"));

		Rect buttonsRect = new Rect(questionBox.width * 0.5f, questionBox.height * 0.8f, questionBox.width * 0.9f, questionBox.height * 0.2f);
		AnchorPoint.SetAnchor(ref buttonsRect, Anchor.MiddleCenter);
		//GUI.Box(buttonsRect, string.Empty);

		GUI.BeginGroup(buttonsRect);
		Rect apologizeRect = new Rect(buttonsRect.width * 0.3f, buttonsRect.height * 0.5f, buttonsRect.width * 0.4f, buttonsRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref apologizeRect, Anchor.MiddleCenter);
        GUI.Box(apologizeRect, string.Empty, dialogueSkin.GetStyle("Dialogue Mini Game Apologize"));

		if (apologizeRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				Debug.Log("[DIALOGUE] Entering DANGER MODE!");
				DialogueEnabled = false;
				NPCManager.current.GetNpcControl(gameManager.BasePlayerData.PlayerInformation.InteractingTo).CurState = NPCState.MiniGame;
				gameManager.BasePlayerData.MiniGamesPlayed++;

				System.Random rng = new System.Random();
				int rand = 0;
				rand = rng.Next(0, 100);

				if (dialogueDataIndx >= 0 || dialogueDataIndx <= 5) {
					if (rand < 50) {
						if (pressedButton.KeypressLevel != KeypressLevel.None) {
							keypressGame.RunKeypress(
								pressedButton.KeypressLevel,
								baseNPCData.NpcSympathyText
							);
						}
					}
					else {
						danceGame.RunDance();
					}
				}
				else {
					if (rand <= 30) {
						if (pressedButton.KeypressLevel != KeypressLevel.None) {
							keypressGame.RunKeypress(
								pressedButton.KeypressLevel,
								baseNPCData.NpcSympathyText
							);
						}
					}
					else {
						danceGame.RunDance();
					}
				}

				questionBoxEnabled = false;
			}
		}

		Rect continueRect = new Rect(buttonsRect.width * 0.7f, buttonsRect.height * 0.5f, buttonsRect.width * 0.4f, buttonsRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref continueRect, Anchor.MiddleCenter);
        GUI.Box(continueRect, string.Empty, dialogueSkin.GetStyle("Dialogue Mini Game Ignore"));

		if (continueRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				DialogueEnabled = false;
				if (dialogueDataIndx < (npcDialogueData.Length - 1)) {
					AddTotalStatisticsToNpc();
				}
				else {
					DialogueEnabled = true;
				}

				questionBoxEnabled = false;
			}
		}

		GUI.EndGroup();

		GUI.EndGroup();

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
		//npcControl = npcManager.GetNpcControl(nameID);
		//npcInformation = npcManager.GetNpcInformation(nameID);
		baseNPCData = NPCDatabase.GetNpc(nameID);

		if (type == ReplyType.Accept) {
			npcDialogueData = NPCDatabase.GetNpcRandomAcceptedText(nameID);
		}
		else if (type == ReplyType.Decline) {
			npcDialogueData = NPCDatabase.GetNpcRandomDeclineText(nameID);
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
		//npcControl = npcManager.GetNpcControl(nameID);
		//npcInformation = npcManager.GetNpcInformation(nameID);
		baseNPCData = NPCDatabase.GetNpc(nameID);

		if (dialogueType == DialogueType.Continous) {
				Debug.Log("[DIALOGUE: CONTINOUS MODE] Initialize Continous Dialogue");
				npcDialogueData = NPCDatabase.GetNpcContinousDialogueData(nameID);
			}
		else if (dialogueType == DialogueType.Random) {
				Debug.Log("[DIALOGUE: RANDOM MODE] Initialize Random Dialogue");
				npcDialogueData = NPCDatabase.GetNpcRandomDialogueData(nameID);
			}
		else if (dialogueType == DialogueType.Selection) {
			Debug.Log("[DIALOGUE: SELECTION MODE] Initialize Selection Dialogue");
			npcDialogueList = NPCDatabase.GetNpcDialogueList(nameID);

			if (selectionIndx > -1 && selectionIndx < npcDialogueList.Length) {
				npcDialogueData = NPCDatabase.GetNpcSelectionDialogueData(selectionIndx, nameID);
			}
			else {
				Debug.Log("[DIALOGUE: SELECTION MODE] Invalid Index. -> [Fallback to Continous Dialogue]");
				npcDialogueData = NPCDatabase.GetNpcContinousDialogueData(nameID);
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
		//storyLineIndx = 0;

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
		NPCDatabase.AddNpcStatistics((NPCNameID)dialogueDBIndx, new Statistics(totalStatistics.Like, totalStatistics.Dislike));
		GameUtility.SetGameObjectLayerRecursively(NPCManager.current.GetNpc(baseNPCData.NpcNameID), LayerManager.LayerNPC);
		baseNPCData.NpcTalkedCount++;
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
			Debug.Log("[DIALOGUE] Wrong!");
			//DialogueEnabled = false;
			questionBoxEnabled = true;
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
			if (!dialogueEnabled) {
				AddTotalStatisticsToNpc();
			}
		}
	}
}