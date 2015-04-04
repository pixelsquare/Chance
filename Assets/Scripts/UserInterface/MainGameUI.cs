using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.PlayerUtility;
using Item;
using NPC;
using NPC.Database;
using UnityEngine;
using Mission.Database;

public class MainGameUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin userInterfaceSkin;
	[SerializeField]
	private GUISkin playerProfileSkin;
	[SerializeField]
	private GUISkin inventorySkin;
    [SerializeField]
    private GUISkin missionProgressSkin;

	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private Vector2 inventorySize;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private bool showHoveredStats;
	private bool showHoveredInventory;
	private bool showHoveredSettings;

	// Inventory Variables
	private ItemData[] inventorySlot;
	private Rect slotRect;

	private bool isDraggingItem;
	private ItemData draggedItem;

	private float selectedBoxAlpha;
	private float selectedBoxAlphaTime;
	private bool hasPickedItem;
	private Rect selectedItemRect;
	private ItemData selectedItem;

	private int hoveredItemIndx;
	private Rect hoveredItemRect;
	private ItemData hoveredItem;

	private bool mouseOnInventoryBox;
	private bool mouseOnInventoryWindow;
	private int selectedItemGiveIndx;
	private int previousItemIndx;

	// Window Backdrop
	private Color guiOriginalColor;
	private Color textureColor;

	// Player Profile Variables
	private Vector2 charactersFoundScroll;
	private int selectedNpcIndx;
	private NPCData selectedNpc;
	private Rect selectedNpcRect;
	private float selectedNpcAlpha;
	private float selectedNpcAlphaTime;

	// Hotkey Text
	private bool showHotkeyText;
	private float hotkeyTextAlpha;
	private float hotkeyFadeDelay = 0.2f;

	private float likeAverage;
	private float oldLikeAverage;

	// Camera Status Text
	private string cameraStatus;

	// Progress Window
	private int completedMissions;
	private int failedMissions;
	private int miniGameCount;
	private int charactersFound;

	private float missionProgress;
	private float missionProgressTime;

	private string[] windowGeneralKeys = new string[4] { "[I] Inventory", "[C] Player Profile", "[X] Settings", "[ESC] Exit" };
	private string[] givingItemKeys = new string[2] { "[Enter] Give Item", "[ESC] Cancel" };

	private UserInterface userInterface;
	private GameManager gameManager;
	private NPCManager npcManager;
	private PlayerInformation playerInformation;

	private Statistics teamDynamicsStat;

	# endregion Private Variables

	// Public Properties
	public bool ShowHotkeyText {
		get { return showHotkeyText; }
	}
	// --

	private void Start() {
		userInterface = UserInterface.current;
		gameManager = GameManager.current;
		npcManager = NPCManager.current;

		textureColor = new Color(0f, 0f, 0f, 0.5f);
		charactersFoundScroll = Vector2.zero;

		userInterface.InventorySlotMax = (int)(inventorySize.x * inventorySize.y) - 1;
		inventorySlot = new ItemData[userInterface.InventorySlotMax];
	}

	private void Update() {
		if (playerInformation == null && gameManager.BasePlayerData != null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
		}

		if (gameManager.GameState == GameState.MainGame) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			// Keyboard function for Player Profile
			if (Input.GetButtonDown(PlayerUtility.PlayerProfile) && (UserInterface.InactiveUI() || userInterface.ShowPlayerProfile)) {
				selectedNpcIndx = 0;
				selectedNpcAlphaTime = 0;
				selectedNpc = new NPCData();
				likeAverage = (float)npcManager.GetNpcAverageStatistics().Like / (float)Statistics.statMax;
				ResetStatisticWindow();
				HotkeyTextInit();
				userInterface.ShowPlayerProfile = true;

				//userInterface.ShowPlayerProfile = true;
				//selectedNpcIndx = 0;
				//selectedNpcAlphaTime = 0;
				//selectedNpc = new NPCData();
			}

			// Keyboard function for Inventory
			if (Input.GetButtonDown(PlayerUtility.Inventory) && (UserInterface.InactiveUI() || userInterface.ShowInventory)) {
				ResetInventoryWindow();
				userInterface.ShowInventory = true;
				
			}

			// Keyboard Function for Settings
			if (Input.GetButtonDown(PlayerUtility.Settings) && (UserInterface.InactiveUI() || userInterface.ShowSettings)) {
				InitializeProgressInfo();
				userInterface.ShowSettings = true;
			}

			Time.timeScale = (userInterface.ShowSettings) ? 0f : 1f;

			// Escape for windows
			if (Input.GetButtonDown(PlayerUtility.ExitWindow)) {
				if (userInterface.ShowInventory || userInterface.ShowPlayerProfile || userInterface.ShowSettings) {
					HotkeyTextEnd();
				}

				userInterface.ShowPlayerProfile = false;
				userInterface.ShowInventory = false;
				userInterface.ShowSettings = false;
			}

			// Keyboard tooltip text at the bottom of the screen
			if (userInterface.ShowPlayerProfile || userInterface.ShowInventory || userInterface.ShowSettings) {
				HotkeyTextInit();
			}

			// If either inventory or giving item
			if (userInterface.ShowInventory || userInterface.GivingItem) {
				// Hide and reset hover box and information when using mouse and mouse is outside the inventory window
				if (mouseOnInventoryBox || mouseOnInventoryWindow) {
					hoveredItemIndx = -1;
					hoveredItem = new ItemData();
					hoveredItemRect = new Rect();
				}

				// Blinking effect on selected slot box
				if (selectedItem != null) {
					selectedBoxAlphaTime += Time.deltaTime;
					selectedBoxAlpha = Mathf.Lerp(1f, 0.1f, Mathf.PingPong(selectedBoxAlphaTime, 0.5f) / 0.5f);
				}
			}

			if (userInterface.ShowInventory) {
				// Move the hover box in all inventory slots using keyboard
				if (Input.GetButtonDown(PlayerUtility.Horizontal)) {
					hoveredItemIndx += (int)Input.GetAxisRaw(PlayerUtility.Horizontal);
					if (hoveredItemIndx > inventorySlot.Length - 1) {
						hoveredItemIndx = 0;
					}
					if (hoveredItemIndx < 0) {
						hoveredItemIndx = inventorySlot.Length - 1;
					}
				}

				if (Input.GetButtonDown(PlayerUtility.SelectItem)) {
					if (hasPickedItem) {
						// Drop / Swap Item
						hasPickedItem = false;
						selectedItemRect = new Rect();
						SwapItems(previousItemIndx, hoveredItemIndx);
						selectedItem = new ItemData();
					}
					else {
						// Pick / Select Item
						if (hoveredItem != null) {
							hasPickedItem = true;
							previousItemIndx = hoveredItemIndx;
							selectedItem = hoveredItem;
							selectedItemRect = hoveredItemRect;
						}
					}
				}
			}
			else if (userInterface.GivingItem) {
				// Be able to move through the inventory when player haven't picked up an item
				if (!hasPickedItem) {
					// Move the hover box in all inventory slots using keyboard
					if (Input.GetButtonDown(PlayerUtility.Horizontal)) {
						hoveredItemIndx += (int)Input.GetAxisRaw(PlayerUtility.Horizontal);
						if (hoveredItemIndx > inventorySlot.Length - 1) {
							hoveredItemIndx = 0;
						}
						if (hoveredItemIndx < 0) {
							hoveredItemIndx = inventorySlot.Length - 1;
						}
					}
				}

				if (Input.GetButtonDown(PlayerUtility.ExitWindow)) {
					if (!hasPickedItem && userInterface.GivingItem) {
						HotkeyTextEnd();
						userInterface.GivingItem = false;
					}
				}

				if (Input.GetButtonDown(PlayerUtility.CancelSelectedItem)) {
					// Cancel an item selected
					if (hasPickedItem) {
						hoveredItemIndx = selectedItemGiveIndx;
						hoveredItem = selectedItem;
						hoveredItemRect = selectedItemRect;

						hasPickedItem = false;
						selectedItem = new ItemData();
						selectedItemRect = new Rect();
					}
				}

				if (Input.GetButtonDown(PlayerUtility.SelectItem)) {
					if (hasPickedItem) {
						// Give an Item
						NPCNameID npcName = playerInformation.InteractingTo;
						NPCDatabase.GiveNpcItem(npcName, selectedItem.ItemNameID, selectedItemGiveIndx);

						hoveredItemIndx = selectedItemGiveIndx;
						hoveredItem = selectedItem;
						hoveredItemRect = selectedItemRect;

						hasPickedItem = false;
						selectedItem = new ItemData();
						selectedItemRect = new Rect();
					}
					else {
						// Pick / Select Item
						if (hoveredItem != null) {
							hasPickedItem = true;
							previousItemIndx = hoveredItemIndx;
							selectedItem = hoveredItem;
							selectedItemRect = hoveredItemRect;

							selectedItemGiveIndx = hoveredItemIndx;
							hoveredItemIndx = -1;
							hoveredItemRect = new Rect();
						}
					}
				}
			}

			if (userInterface.ShowPlayerProfile) {
				// Move Hover box in the character's found list using keyboard
				if (Input.GetButtonDown(PlayerUtility.Vertical)) {
					if (playerInformation.GetNpcCount() > 0) {
						selectedNpcIndx -= (int)Input.GetAxisRaw(PlayerUtility.Vertical);
						if (selectedNpcIndx > playerInformation.GetNpcCount() - 1) {
							selectedNpcIndx = 0;
						}

						if (selectedNpcIndx < 0) {
							selectedNpcIndx = playerInformation.GetNpcCount() - 1;
						}
					}
				}

				// Blinking effect on selected Npc box
				if (playerInformation.GetNpcCount() > 0) {
					selectedNpcAlphaTime += Time.deltaTime;
					selectedNpcAlpha = Mathf.Lerp(1f, 0.1f, Mathf.PingPong(selectedNpcAlphaTime, 0.5f) / 0.5f);
				}
			}

			if (userInterface.ShowSettings) {
				missionProgressTime += Time.deltaTime;
				missionProgress = Mathf.Lerp(0f, (float)completedMissions, missionProgressTime * 5f);
			}
		}
	}

	public void MainGUI(Event e) {
		if (UserInterface.InactiveUI()) {
			Rect playerInfoRect = new Rect(mainRect.width * 0.2f, mainRect.height * 0.08f, mainRect.height * 0.4f, mainRect.height * 0.16f);
			AnchorPoint.SetAnchor(ref playerInfoRect, Anchor.MiddleCenter);
			//GUI.Box(playerInfoRect, string.Empty);

			GUI.BeginGroup(playerInfoRect);

			// Avatar BG
			Rect playerAvatarRect = new Rect(playerInfoRect.width * 0.17f, playerInfoRect.height * 0.5f, playerInfoRect.width * 0.25f, playerInfoRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref playerAvatarRect, Anchor.MiddleCenter);
			GUI.Box(playerAvatarRect, string.Empty, userInterfaceSkin.GetStyle("Player Avatar BG"));

			if (gameManager.BasePlayerData != null) {
				if (gameManager.BasePlayerData.PlayerAvatar != null) {
					GUI.BeginGroup(playerAvatarRect);
					// Avatar
					Rect playerAvatarFrameRect = new Rect(playerAvatarRect.width * 0.6f, playerAvatarRect.height * 0.55f, playerAvatarRect.width * 0.55f, playerAvatarRect.height * 0.45f);
					AnchorPoint.SetAnchor(ref playerAvatarFrameRect, Anchor.MiddleCenter);
					GUI.DrawTexture(playerAvatarFrameRect, gameManager.BasePlayerData.PlayerAvatar);
					GUI.EndGroup();
				}

				// Name BG
				Rect nameRect = new Rect(playerInfoRect.width * 0.25f, playerInfoRect.height * 0.8f, playerInfoRect.width * 0.35f, playerInfoRect.height * 0.2f);
				AnchorPoint.SetAnchor(ref nameRect, Anchor.MiddleCenter);
				GUI.Box(nameRect, string.Empty, userInterfaceSkin.GetStyle("Player Name BG"));

				// Name
				Rect nameFrameRect = new Rect(playerInfoRect.width * 0.25f, playerInfoRect.height * 0.8f, playerInfoRect.width * 0.4f, playerInfoRect.height * 0.3f);
				AnchorPoint.SetAnchor(ref nameFrameRect, Anchor.MiddleCenter);
				GUI.Box(nameFrameRect, gameManager.BasePlayerData.PlayerName, userInterfaceSkin.GetStyle("Player Name"));
			}

			# region Player Information Button
			Rect statsRect = new Rect(playerInfoRect.width * 0.3f, playerInfoRect.height * 0.2f, mainRect.width * 0.03f, mainRect.height * 0.03f);
			AnchorPoint.SetAnchor(ref statsRect, Anchor.MiddleCenter);
			GUI.Box(statsRect, string.Empty, userInterfaceSkin.GetStyle("Player Profile Btn"));

			if (statsRect.Contains(e.mousePosition) && UserInterface.InactiveUI()) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					selectedNpcIndx = 0;
					selectedNpcAlphaTime = 0;
					selectedNpc = new NPCData();
					likeAverage = (float)npcManager.GetNpcAverageStatistics().Like / (float)Statistics.statMax;
					userInterface.ShowPlayerProfile = true;
					ResetHovers();
					ResetStatisticWindow();
					HotkeyTextInit();
				}
			}
			# endregion Player Information Button

			# region Inventory Button
			Rect inventoryRect = new Rect(playerInfoRect.width * 0.3f, playerInfoRect.height * 0.4f, mainRect.width * 0.03f, mainRect.height * 0.03f);
			AnchorPoint.SetAnchor(ref inventoryRect, Anchor.MiddleCenter);
			GUI.Box(inventoryRect, string.Empty, userInterfaceSkin.GetStyle("Player Inventory"));

			if (inventoryRect.Contains(e.mousePosition) && UserInterface.InactiveUI()) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					userInterface.ShowInventory = true;
					ResetHovers();
					ResetInventoryWindow();
					HotkeyTextInit();
				}
			}
			# endregion Inventory Button

			# region Settings Button
			Rect settingsRect = new Rect(playerInfoRect.width * 0.3f, playerInfoRect.height * 0.6f, mainRect.width * 0.03f, mainRect.height * 0.03f);
			AnchorPoint.SetAnchor(ref settingsRect, Anchor.MiddleCenter);
			GUI.Box(settingsRect, string.Empty, userInterfaceSkin.GetStyle("Player Settings"));

			if (settingsRect.Contains(e.mousePosition) && UserInterface.InactiveUI()) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					userInterface.ShowSettings = true;
					ResetHovers();
					ResetSettingsWindow();
					HotkeyTextInit();
					InitializeProgressInfo();
				}
			}
			# endregion Settings Button


			showHoveredStats = statsRect.Contains(e.mousePosition);
			showHoveredInventory = inventoryRect.Contains(e.mousePosition);
			showHoveredSettings = settingsRect.Contains(e.mousePosition);

			GUI.EndGroup();

			// Makes the button hover in full scale

			if (showHoveredStats && !showHoveredInventory && !showHoveredSettings) {
				Rect statsTooltipRect = new Rect(e.mousePosition.x + 10f, e.mousePosition.y, mainRect.width * 0.12f, mainRect.height * 0.025f);
				AnchorPoint.SetAnchor(ref statsTooltipRect, Anchor.TopLeft);
				GUI.Box(statsTooltipRect, "Player Statistics", tempSkin.GetStyle("Block"));
			}

			if (showHoveredInventory && !showHoveredStats && !showHoveredSettings) {
				Rect inventoryTooltipRect = new Rect(e.mousePosition.x + 10f, e.mousePosition.y, mainRect.width * 0.1f, mainRect.height * 0.025f);
				AnchorPoint.SetAnchor(ref inventoryTooltipRect, Anchor.TopLeft);
				GUI.Box(inventoryTooltipRect, "Inventory", tempSkin.GetStyle("Block"));
			}

			if (showHoveredSettings && !showHoveredStats && !showHoveredInventory) {
				Rect settingsTooltipRect = new Rect(e.mousePosition.x + 10f, e.mousePosition.y, mainRect.width * 0.07f, mainRect.height * 0.025f);
				AnchorPoint.SetAnchor(ref settingsTooltipRect, Anchor.TopLeft);
				GUI.Box(settingsTooltipRect, "Settings", tempSkin.GetStyle("Block"));
			}

			if (playerInformation != null) {
				if (playerInformation.PlayerCamera.CameraState == CameraState.Behind) {
					cameraStatus = "Locked";
				}
				else if (playerInformation.PlayerCamera.CameraState == CameraState.OrbitFollow) {
					cameraStatus = "Free";
				}
				else if (playerInformation.PlayerCamera.CameraState == CameraState.FirstPerson) {
					cameraStatus = "FPV";
				}
			}

			Rect cameraTextRect = new Rect(Screen.width * 0.1f, Screen.height * 0.95f, Screen.width * 0.3f, Screen.height * 0.1f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref cameraTextRect, Anchor.MiddleCenter);
			GUI.Box(cameraTextRect, "[Q] Camera:", userInterfaceSkin.GetStyle("Player Camera Text"));

			Rect cameraStatusRect = new Rect(Screen.width * 0.025f, Screen.height * 0.95f, Screen.width * 0.3f, Screen.height * 0.1f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref cameraStatusRect, Anchor.MiddleLeft);
			GUI.Box(cameraStatusRect, cameraStatus, userInterfaceSkin.GetStyle("Player Camera Text"));
		}
	}

	public void PlayerProfileWindow(Event e) {
		GUI.BeginGroup(mainRect);
		guiOriginalColor = GUI.color;
		GUI.color = textureColor;
		GUI.DrawTexture(mainRect, Resources.BlackTexture);
		GUI.color = guiOriginalColor;

		// Character Stat Background
		Rect charStatBGRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.7f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref charStatBGRect, Anchor.MiddleCenter);
		GUI.Box(charStatBGRect, string.Empty, playerProfileSkin.GetStyle("Profile BG"));

		GUI.BeginGroup(charStatBGRect);

		# region Left Panel

		Rect leftPanel = new Rect(charStatBGRect.width * 0.17f, charStatBGRect.height * 0.53f, charStatBGRect.width * 0.3f, charStatBGRect.height * 0.87f);
		AnchorPoint.SetAnchor(ref leftPanel, Anchor.MiddleCenter);
		GUI.Box(leftPanel, string.Empty, playerProfileSkin.GetStyle("Profile Left Panel"));

		GUI.BeginGroup(leftPanel);

		// Character Avatar BG
		Rect charAvatarBgRect = new Rect(leftPanel.width * 0.5f, leftPanel.height * 0.23f, leftPanel.width * 0.9f, leftPanel.height * 0.4f);
		AnchorPoint.SetAnchor(ref charAvatarBgRect, Anchor.MiddleCenter);
		GUI.Box(charAvatarBgRect, string.Empty, playerProfileSkin.GetStyle("Profile Avatar BG"));

		if (gameManager.BasePlayerData.PlayerAvatar != null) {
			// Character Avatar
			GUI.BeginGroup(charAvatarBgRect);
			Rect charAvatarRect = new Rect(charAvatarBgRect.width * 0.5f, charAvatarBgRect.height * 0.58f, charAvatarBgRect.width * 0.55f, charAvatarBgRect.height * 0.8f);
			AnchorPoint.SetAnchor(ref charAvatarRect, Anchor.MiddleCenter);
			GUI.DrawTexture(charAvatarRect, gameManager.BasePlayerData.PlayerAvatar);
			GUI.EndGroup();
		}

		// Player Name BG
		Rect nameBgRect = new Rect(leftPanel.width * 0.5f, leftPanel.height * 0.47f, leftPanel.width * 0.9f, leftPanel.height * 0.08f);
		AnchorPoint.SetAnchor(ref nameBgRect, Anchor.MiddleCenter);
		GUI.Box(nameBgRect, string.Empty, playerProfileSkin.GetStyle("Profile Name BG"));

		// Player Name
		Rect nameRect = new Rect(leftPanel.width * 0.5f, leftPanel.height * 0.47f, leftPanel.width * 0.9f, leftPanel.height * 0.08f);
		AnchorPoint.SetAnchor(ref nameRect, Anchor.MiddleCenter);
		GUI.Box(nameRect, gameManager.BasePlayerData.PlayerName, playerProfileSkin.GetStyle("Profile Name"));

		# region Player Statistics

		Rect statLRect = new Rect(leftPanel.width * 0.5f, leftPanel.height * 0.73f, leftPanel.width * 0.93f, leftPanel.height * 0.46f);
		AnchorPoint.SetAnchor(ref statLRect, Anchor.MiddleCenter);
		GUI.Box(statLRect, string.Empty, progressBarSkin.GetStyle("Progress BG"));

		GUI.BeginGroup(statLRect);

		Rect playerStatRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.125f, statLRect.width * 0.9f, statLRect.height * 0.2f);
		AnchorPoint.SetAnchor(ref playerStatRect, Anchor.MiddleCenter);
		GUI.Box(playerStatRect, string.Empty, playerProfileSkin.GetStyle("Profile Player Statistics Title"));

		// Art
		Rect artRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.3f, statLRect.width * 0.88f, statLRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref artRect, Anchor.MiddleCenter);
		UserInterface.ProgressBar(
			"Art",
			(float)gameManager.BasePlayerData.PlayerStatistics.Art,
			(float)Statistics.statMax,
			artRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Programming
		Rect progRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.44f, statLRect.width * 0.88f, statLRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref progRect, Anchor.MiddleCenter);
		UserInterface.ProgressBar(
			"Programming",
			(float)gameManager.BasePlayerData.PlayerStatistics.Programming,
			(float)Statistics.statMax,
			progRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Design
		Rect designRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.58f, statLRect.width * 0.88f, statLRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref designRect, Anchor.MiddleCenter);
		UserInterface.ProgressBar(
			"Design",
			(float)gameManager.BasePlayerData.PlayerStatistics.Design,
			(float)Statistics.statMax,
			designRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Sound
		Rect soundRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.72f, statLRect.width * 0.88f, statLRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref soundRect, Anchor.MiddleCenter);
		UserInterface.ProgressBar(
			"Sound",
			(float)gameManager.BasePlayerData.PlayerStatistics.Sound,
			(float)Statistics.statMax,
			soundRect,
			progressBarSkin.GetStyle("Progress Bar BG"),
			progressBarSkin.GetStyle("Progress Bar Overlay")
		);

		// Like / Dislike
		Rect likeDislikeRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.86f, statLRect.width * 0.88f, statLRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref likeDislikeRect, Anchor.MiddleCenter);
		UserInterface.SliderBox(
			"Overall Like",
			likeAverage,
			likeDislikeRect,
			progressBarSkin.GetStyle("Progress Bar Overlay"),
			progressBarSkin.GetStyle("Progress Bar Red Overlay"),
			tempSkin.GetStyle("Text1")
		);

		GUI.EndGroup();

		# endregion Player Statistics

		GUI.EndGroup();	// Left Panel

		# endregion Left Panel

		# region Middle Panel

		Rect middlePanel = new Rect(charStatBGRect.width * 0.5f, charStatBGRect.height * 0.53f, charStatBGRect.width * 0.35f, charStatBGRect.height * 0.87f);
		AnchorPoint.SetAnchor(ref middlePanel, Anchor.MiddleCenter);
		GUI.Box(middlePanel, string.Empty, playerProfileSkin.GetStyle("Profile Middle Panel"));

		GUI.BeginGroup(middlePanel);
		// Characters Found Logo
		Rect charFoundRect = new Rect(middlePanel.width * 0.5f, middlePanel.height * 0.075f, middlePanel.width * 0.9f, middlePanel.height * 0.1f);
		AnchorPoint.SetAnchor(ref charFoundRect, Anchor.MiddleCenter);
		GUI.Box(charFoundRect, string.Empty, playerProfileSkin.GetStyle("Profile Player Found Title"));

		//charactersFoundScroll.y = Mathf.Clamp(charactersFoundScroll.y, 30f, Mathf.Infinity);
		if (playerInformation.NpcFound != null) {
			Rect scrollRect = new Rect(middlePanel.width * 0.5f, middlePanel.height * 0.55f, middlePanel.width * 0.9f, middlePanel.height * 0.85f);
			AnchorPoint.SetAnchor(ref scrollRect, Anchor.MiddleCenter);

			Rect scrollViewRect = new Rect(0f, 0f, middlePanel.width * 0.8f, (middlePanel.height * 0.15f) * playerInformation.GetNpcCount());
			//AnchorPoint.SetAnchor(ref scrollViewRect, Anchor.MiddleCenter);
			//GUI.Box(scrollViewRect, string.Empty);

			// Scroller
			charactersFoundScroll = GUI.BeginScrollView(scrollRect, charactersFoundScroll, scrollViewRect);

			// Iterate through player's related NPCs
			for (int i = 0; i < playerInformation.NpcFound.Length; i++) {
				if (playerInformation.NpcFound[i] != null) {
					Rect playerBoxRect = new Rect(middlePanel.width * 0.425f, ((middlePanel.height * 0.15f) * (i + 1)) - (middlePanel.height * 0.07f),
							middlePanel.width * 0.8f, middlePanel.height * 0.15f);

					playerBoxRect.x = (playerInformation.GetNpcCount() < 6) ? middlePanel.width * 0.45f : middlePanel.width * 0.425f;
					playerBoxRect.width = (playerInformation.GetNpcCount() < 6) ? middlePanel.width * 0.9f : middlePanel.width * 0.8f;

					AnchorPoint.SetAnchor(ref playerBoxRect, Anchor.MiddleCenter);
					GUI.Box(playerBoxRect, string.Empty, playerProfileSkin.GetStyle("Profile Friend BG"));

					CharacterList(
						e,
						i,
						playerInformation.NpcFound[i].NpcName,
						playerInformation.NpcFound[i].NpcAvatar,
						playerBoxRect,
						playerProfileSkin.GetStyle("Profile Friend Avatar BG"),
						playerProfileSkin.GetStyle("Profile Friend Name BG")
					);

					if (playerBoxRect.Contains(e.mousePosition) && (e.button == 0 && e.type == EventType.mouseDown)) {
						selectedNpcIndx = i;
					}

					if (selectedNpcIndx == i) {
						selectedNpc = playerInformation.NpcFound[i];
						selectedNpcRect = new Rect(middlePanel.width * 0.425f, ((middlePanel.height * 0.15f) * (i + 1)) - (middlePanel.height * 0.07f),
							middlePanel.width * 0.8f, middlePanel.height * 0.15f);

						selectedNpcRect.x = (playerInformation.GetNpcCount() < 6) ? middlePanel.width * 0.45f : middlePanel.width * 0.425f;
						selectedNpcRect.width = (playerInformation.GetNpcCount() < 6) ? middlePanel.width * 0.9f : middlePanel.width * 0.8f;

						AnchorPoint.SetAnchor(ref selectedNpcRect, Anchor.MiddleCenter);
					}
				}
			}

			if (playerInformation.GetNpcCount() > 0) {
				guiOriginalColor = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, selectedNpcAlpha);
				GUI.Box(selectedNpcRect, string.Empty, playerProfileSkin.GetStyle("Profile Selected Npc"));
				GUI.color = guiOriginalColor;
			}

			GUI.EndScrollView();
		}

		GUI.EndGroup();

		# endregion Middle Panel

		#region Right Panel
		Rect rightPanel = new Rect(charStatBGRect.width * 0.83f, charStatBGRect.height * 0.53f, charStatBGRect.width * 0.3f, charStatBGRect.height * 0.87f);
		AnchorPoint.SetAnchor(ref rightPanel, Anchor.MiddleCenter);
		if (playerInformation.GetNpcCount() < 0) {
			GUI.Box(rightPanel, "Select\nA\nCharacter", playerProfileSkin.GetStyle("Profile Right Panel"));
		}
		else {
			GUI.Box(rightPanel, string.Empty, playerProfileSkin.GetStyle("Profile Right Panel"));
		}

		GUI.BeginGroup(rightPanel);
		if (playerInformation.GetNpcCount() > 0) {
			Rect npcTitle = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.08f, rightPanel.width * 0.9f, rightPanel.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcTitle, Anchor.MiddleCenter);
			GUI.Box(npcTitle, string.Empty, playerProfileSkin.GetStyle("Profile Npc Info Title"));
		}

		if (selectedNpc != null && selectedNpc.NpcAvatar != null) {
			Rect npcAvatarBg = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.275f, rightPanel.width * 0.9f, rightPanel.height * 0.3f);
			AnchorPoint.SetAnchor(ref npcAvatarBg, Anchor.MiddleCenter);
			GUI.Box(npcAvatarBg, string.Empty, playerProfileSkin.GetStyle("Profile Npc Info Avatar Bg"));

			GUI.BeginGroup(npcAvatarBg);
			Rect npcAvatar = new Rect(npcAvatarBg.width * 0.5f, npcAvatarBg.height * 0.5f, npcAvatarBg.width * 0.45f, npcAvatarBg.height * 0.6f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref npcAvatar, Anchor.MiddleCenter);
			GUI.DrawTexture(npcAvatar, selectedNpc.NpcAvatar);
			//GUI.Box(npcAvatar, string.Empty);
			GUI.EndGroup();

			Rect npcNameBg = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.47f, rightPanel.width * 0.9f, rightPanel.height * 0.08f);
			AnchorPoint.SetAnchor(ref npcNameBg, Anchor.MiddleCenter);
			GUI.Box(npcNameBg, selectedNpc.NpcName, playerProfileSkin.GetStyle("Profile Npc Info Name Bg"));

			Rect npcName = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.47f, rightPanel.width * 0.9f, rightPanel.height * 0.08f);
			AnchorPoint.SetAnchor(ref npcName, Anchor.MiddleCenter);
			GUI.Box(npcName, string.Empty, playerProfileSkin.GetStyle("Profile Npc Info Name"));

			//Rect npcDesc = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.75f, rightPanel.width * 0.9f, rightPanel.height * 0.4f);
			//AnchorPoint.SetAnchor(ref npcDesc, Anchor.MiddleCenter);
			//GUI.Box(npcDesc, selectedNpc.NpcDesc, playerProfileSkin.GetStyle("Profile Npc Info Desc"));

			# region Npc Statistics

			Rect npcStatRect = new Rect(rightPanel.width * 0.5f, rightPanel.height * 0.73f, rightPanel.width * 0.93f, rightPanel.height * 0.46f);
			AnchorPoint.SetAnchor(ref npcStatRect, Anchor.MiddleCenter);
			GUI.Box(npcStatRect, string.Empty, progressBarSkin.GetStyle("Progress BG"));

			GUI.BeginGroup(statLRect);

			Rect npcStatTitle = new Rect(statLRect.width * 0.5f, statLRect.height * 0.125f, statLRect.width * 0.9f, statLRect.height * 0.2f);
			AnchorPoint.SetAnchor(ref npcStatTitle, Anchor.MiddleCenter);
			GUI.Box(npcStatTitle, string.Empty, playerProfileSkin.GetStyle("Profile Player Statistics Title"));

			// Art
			Rect npcArtRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.3f, statLRect.width * 0.88f, statLRect.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcArtRect, Anchor.MiddleCenter);
			UserInterface.ProgressBar(
				"Art",
				(float)selectedNpc.NpcStatistics.Art,
				(float)Statistics.statMax,
				npcArtRect,
				progressBarSkin.GetStyle("Progress Bar BG"),
				progressBarSkin.GetStyle("Progress Bar Overlay")
			);

			// Programming
			Rect npcProgRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.44f, statLRect.width * 0.88f, statLRect.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcProgRect, Anchor.MiddleCenter);
			UserInterface.ProgressBar(
				"Programming",
				(float)selectedNpc.NpcStatistics.Programming,
				(float)Statistics.statMax,
				npcProgRect,
				progressBarSkin.GetStyle("Progress Bar BG"),
				progressBarSkin.GetStyle("Progress Bar Overlay")
			);

			// Design
			Rect npcDesignRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.58f, statLRect.width * 0.88f, statLRect.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcDesignRect, Anchor.MiddleCenter);
			UserInterface.ProgressBar(
				"Design",
				(float)selectedNpc.NpcStatistics.Design,
				(float)Statistics.statMax,
				npcDesignRect,
				progressBarSkin.GetStyle("Progress Bar BG"),
				progressBarSkin.GetStyle("Progress Bar Overlay")
			);

			// Sound
			Rect npcSoundRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.72f, statLRect.width * 0.88f, statLRect.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcSoundRect, Anchor.MiddleCenter);
			UserInterface.ProgressBar(
				"Sound",
				(float)selectedNpc.NpcStatistics.Sound,
				(float)Statistics.statMax,
				npcSoundRect,
				progressBarSkin.GetStyle("Progress Bar BG"),
				progressBarSkin.GetStyle("Progress Bar Overlay")
			);

			// Like / Dislike
			Rect npcLikeDislikeRect = new Rect(statLRect.width * 0.5f, statLRect.height * 0.86f, statLRect.width * 0.88f, statLRect.height * 0.1f);
			AnchorPoint.SetAnchor(ref npcLikeDislikeRect, Anchor.MiddleCenter);
			UserInterface.SliderBox(
				"Like / Dislike",
				(float)selectedNpc.NpcStatistics.Like / (float)Statistics.statMax,
				npcLikeDislikeRect,
				progressBarSkin.GetStyle("Progress Bar Overlay"),
				progressBarSkin.GetStyle("Progress Bar Red Overlay"),
				tempSkin.GetStyle("Text1")
			);

			GUI.EndGroup();

			# endregion Npc Statistics
		}

		GUI.EndGroup();

		# endregion Right Panel

		# region Exit
		Rect exitRect = new Rect(charStatBGRect.width * 0.95f, charStatBGRect.height * 0.08f, mainRect.width * 0.03f, mainRect.height * 0.03f);
		AnchorPoint.SetAnchor(ref exitRect, Anchor.MiddleCenter);
		GUI.Box(exitRect, string.Empty, playerProfileSkin.GetStyle("Profile Exit Btn"));

		if (exitRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				userInterface.ShowPlayerProfile = false;
				ResetStatisticWindow();
				HotkeyTextEnd();
			}
		}
		# endregion Exit

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void InventoryWindow(Event e) {
		GUI.BeginGroup(mainRect);
		guiOriginalColor = GUI.color;
		GUI.color = textureColor;
		// Dialogue Backdrop
		GUI.DrawTexture(mainRect, Resources.BlackTexture);
		GUI.color = guiOriginalColor;

		Rect inventoryBGRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.45f, mainRect.width * 0.4f, mainRect.height * 0.4f);
		AnchorPoint.SetAnchor(ref inventoryBGRect, Anchor.MiddleCenter);
		GUI.Box(inventoryBGRect, string.Empty, inventorySkin.GetStyle("Inventory BG"));

		mouseOnInventoryWindow = inventoryBGRect.Contains(e.mousePosition);

		GUI.BeginGroup(inventoryBGRect);

		# region Inventory

		Rect inventoryRect = new Rect(inventoryBGRect.width * 0.5f, inventoryBGRect.height * 0.53f, inventoryBGRect.width * 0.9f, inventoryBGRect.height * 0.8f);
		AnchorPoint.SetAnchor(ref inventoryRect, Anchor.MiddleCenter);
		GUI.Box(inventoryRect, string.Empty, inventorySkin.GetStyle("Inventory Box BG"));

		mouseOnInventoryBox = inventoryRect.Contains(e.mousePosition);

		GUI.BeginGroup(inventoryRect);

		# region Inventory Swap and Switching
		int itemIndx = 0;
		for (int y = 0; y < inventorySize.y; y++) {
			// This makes the inventory 4 3 4 slots
			inventorySize.x = (y == 1) ? 3 : 4;

			for (int x = 0; x < inventorySize.x; x++) {
				// Make the second column 3 slots only and others 4 slots
				slotRect = GetInventorySlotRect(x, y, inventoryRect);
				AnchorPoint.SetAnchor(ref slotRect, Anchor.MiddleCenter);
				GUI.Box(slotRect, string.Empty, inventorySkin.GetStyle("Inventory Slot BG"));

				// Replicate player's inventory to UI's inventory
				if (playerInformation.Inventory != null) {
					inventorySlot[itemIndx] = playerInformation.Inventory[itemIndx];
				}

				// When mouse is hovering any slot (filled or empty)
				// Set hovered item index
				if (slotRect.Contains(e.mousePosition)) {
					hoveredItemIndx = itemIndx;
				}

				// Hovering Used for keyboard
				if (hoveredItemIndx == itemIndx) {
					hoveredItem = inventorySlot[itemIndx];
					hoveredItemRect = GetInventorySlotRect(x, y, inventoryRect);
					AnchorPoint.SetAnchor(ref hoveredItemRect, Anchor.MiddleCenter);
				}

				// Draw inventory items if it's not empty
				if (inventorySlot[itemIndx] != null) {
					// [Slot with Items]

					// Draw item icon
					if (inventorySlot[itemIndx].ItemIcon != null) {
						GUI.DrawTexture(slotRect, inventorySlot[itemIndx].ItemIcon);
					}

					// Determine if the mouse is Over the rect with items on it
					if (slotRect.Contains(e.mousePosition)) {
						// When the player starts dragging the item
						if (e.button == 0 && e.type == EventType.mouseDrag) {
							if (!isDraggingItem) {
								// Start dragging
								isDraggingItem = true;
								previousItemIndx = itemIndx;
								draggedItem = inventorySlot[itemIndx];
								playerInformation.Inventory[itemIndx] = new ItemData();

								// Reset picking up properties
								hasPickedItem = false;
								selectedItemRect = new Rect();
								selectedItem = new ItemData();
							}
						}

						// Dropping the dragged item (Item Swap)
						if (e.button == 0 && e.type == EventType.mouseUp) {
							if (isDraggingItem) {
								isDraggingItem = false;
								selectedItemRect = new Rect();
								SwapItems(previousItemIndx, itemIndx);
								playerInformation.Inventory[itemIndx] = draggedItem;
								draggedItem = new ItemData();
							}
						}

						// Selecting a Slot using mouse left button
						if (e.button == 0 && e.type == EventType.mouseDown) {
							if (!hasPickedItem) {
								// Pick Item
								hasPickedItem = true;
								previousItemIndx = itemIndx;
								selectedItem = inventorySlot[itemIndx];
								selectedItemRect = GetInventorySlotRect(x, y, inventoryRect);
								AnchorPoint.SetAnchor(ref selectedItemRect, Anchor.MiddleCenter);
								selectedItemGiveIndx = hoveredItemIndx;
								selectedBoxAlphaTime = 0f;
							}
							else {
								// Drop Item
								hasPickedItem = false;
								selectedItemRect = new Rect();
								SwapItems(previousItemIndx, itemIndx);
								selectedItem = new ItemData();
							}
						}
					}
				}
				else {
					// [Slot without Items]
					if (slotRect.Contains(e.mousePosition)) {
						// Dropping item in an empty slot
						if (e.button == 0 && e.type == EventType.mouseUp) {
							if (hasPickedItem) {
								hasPickedItem = false;
								selectedItemRect = new Rect();
								SwapItems(previousItemIndx, itemIndx);
								selectedItem = new ItemData();
							}

							if (isDraggingItem) {
								isDraggingItem = false;
								selectedItemRect = new Rect();
								SwapItems(previousItemIndx, itemIndx);
								playerInformation.Inventory[itemIndx] = draggedItem;
								draggedItem = new ItemData();
							}
						}
					}
				}

				itemIndx++;
			}
		}
		# endregion Inventory Swap and Switching

		# region Inside Inventory Box
		if (selectedItem != null && selectedItem.ItemNameID != ItemNameID.None) {
			guiOriginalColor = GUI.color;
			GUI.color = new Color(1f, 1f, 1f, selectedBoxAlpha);
			GUI.Box(selectedItemRect, string.Empty, inventorySkin.GetStyle("Inventory Slot Hover Selected"));
			GUI.color = guiOriginalColor;
		}

		if (hoveredItemIndx > -1) {
			GUI.Box(hoveredItemRect, string.Empty, inventorySkin.GetStyle("Inventory Slot Hover"));
		}
		# endregion Inside Inventory Box

		GUI.EndGroup();

		# endregion Inventory

		# region Inside Inventory Window

		# region Exit
		Rect exitRect = new Rect(inventoryBGRect.width * 0.9f, inventoryBGRect.height * 0.1f, mainRect.width * 0.03f, Screen.height * 0.03f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref exitRect, Anchor.MiddleCenter);
		GUI.Box(exitRect, string.Empty, inventorySkin.GetStyle("Inventory Exit Button"));

		if (exitRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				userInterface.ShowInventory = false;
				userInterface.GivingItem = false;
				ResetInventoryWindow();
				HotkeyTextEnd();
			}
		}
		# endregion Exit

		# endregion Inside Inventory Window

		GUI.EndGroup();

		# region Outside Inventory Window
		// Item is being dragged
		if (isDraggingItem) {
			Rect draggedItemRect = new Rect(e.mousePosition.x, e.mousePosition.y, slotRect.width, slotRect.height);
			AnchorPoint.SetAnchor(ref draggedItemRect, Anchor.MiddleCenter);
			if (draggedItem.ItemIcon != null) {
				GUI.DrawTexture(draggedItemRect, draggedItem.ItemIcon);
			}

			// When player tries to release mouse button from dragging
			if (e.button == 0 && e.type == EventType.mouseUp) {
				isDraggingItem = false;
				playerInformation.Inventory[previousItemIndx] = draggedItem;
				draggedItem = new ItemData();
			}
		}


		# region Item Information
		if (hoveredItem != null && hoveredItem.ItemIcon != null) {
			Rect itemInfoBox = new Rect(mainRect.width * 0.8f, mainRect.height * 0.45f, mainRect.width * 0.2f, mainRect.height * 0.3f);
			AnchorPoint.SetAnchor(ref itemInfoBox, Anchor.MiddleCenter);
			GUI.Box(itemInfoBox, string.Empty, inventorySkin.GetStyle("Inventory Item Info BG"));

			GUI.BeginGroup(itemInfoBox);
			// Item Avatar Bg
			Rect itemAvatarBgRect = new Rect(itemInfoBox.width * 0.5f, itemInfoBox.height * 0.225f, itemInfoBox.width * 0.9f, itemInfoBox.height * 0.35f);
			AnchorPoint.SetAnchor(ref itemAvatarBgRect, Anchor.MiddleCenter);
			GUI.Box(itemAvatarBgRect, string.Empty, inventorySkin.GetStyle("Inventory Item Icon BG"));

			GUI.BeginGroup(itemAvatarBgRect);
			// Item Avatar
			Rect itemAvatarRect = new Rect(itemAvatarBgRect.width * 0.5f, itemAvatarBgRect.height * 0.5f,
				itemAvatarBgRect.width * 0.3f, itemAvatarBgRect.height * 0.3f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref itemAvatarRect, Anchor.MiddleCenter);
			GUI.DrawTexture(itemAvatarRect, hoveredItem.ItemIcon);
			GUI.EndGroup();

			// Item Name
			Rect itemNameRect = new Rect(itemInfoBox.width * 0.5f, itemInfoBox.height * 0.45f, itemInfoBox.width * 0.9f, itemInfoBox.height * 0.15f);
			AnchorPoint.SetAnchor(ref itemNameRect, Anchor.MiddleCenter);
			GUI.Box(itemNameRect, hoveredItem.ItemName, inventorySkin.GetStyle("Inventory Item Name"));

			// Item Description
			Rect itemDescRect = new Rect(itemInfoBox.width * 0.5f, itemInfoBox.height * 0.725f, itemInfoBox.width * 0.9f, itemInfoBox.height * 0.45f);
			AnchorPoint.SetAnchor(ref itemDescRect, Anchor.MiddleCenter);
			GUI.Box(itemDescRect, hoveredItem.ItemDesc, inventorySkin.GetStyle("Inventory Item Desc"));

			GUI.EndGroup();
		}
		# endregion Item Information

		GUI.EndGroup();
		# endregion Outside Inventory Window

		if (userInterface.GivingItem) {
			if (selectedItem != null && selectedItem.ItemNameID != ItemNameID.None) {
				ItemQuestionBox(e);
				showHotkeyText = false;
			}
			else {
				showHotkeyText = true;
			}
		}
	}

	public void SettingsWindow(Event e) {
		GUI.BeginGroup(mainRect);

		guiOriginalColor = GUI.color;
		GUI.color = textureColor;
		// Dialogue Backdrop
		GUI.DrawTexture(mainRect, tempSkin.GetStyle("Black BG").normal.background);
		GUI.color = guiOriginalColor;

		Rect pauseText = new Rect(mainRect.width * 0.5f, mainRect.height * 0.25f, mainRect.width * 0.4f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref pauseText, Anchor.MiddleCenter);
        GUI.DrawTexture(pauseText, userInterfaceSkin.GetStyle("Pause Texture").normal.background);
        //GUI.Box(pauseText, "PAUSED", tempSkin.GetStyle("Text"));

		Rect settingsBGRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.4f, mainRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref settingsBGRect, Anchor.MiddleCenter);
        GUI.Box(settingsBGRect, string.Empty, missionProgressSkin.GetStyle("Mission Progress BG"));

		GUI.BeginGroup(settingsBGRect);

        //Rect missionTitle = new Rect(settingsBGRect.width * 0.5f, settingsBGRect.height * 0.15f, settingsBGRect.width * 0.9f, settingsBGRect.height * 0.1f);
        //AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
        //GUI.Box(missionTitle, "MISSION PROGRESS", tempSkin.GetStyle("Text"));

		Rect progressInfo = new Rect(settingsBGRect.width * 0.5f, settingsBGRect.height * 0.5f, settingsBGRect.width * 0.9f, settingsBGRect.height * 0.8f);
		AnchorPoint.SetAnchor(ref progressInfo, Anchor.MiddleCenter);
		GUI.Box(progressInfo, string.Empty, missionProgressSkin.GetStyle("Mission Progress Box"));

		GUI.BeginGroup(progressInfo);

            Rect missionProgressRect = new Rect(progressInfo.width * 0.5f, progressInfo.height * 0.2f, progressInfo.width * 0.9f, progressInfo.height * 0.1f);
            AnchorPoint.SetAnchor(ref missionProgressRect, Anchor.MiddleCenter);
            UserInterface.ProgressBar(
                string.Empty,
                missionProgress,
                (float)MissionDatabase.MissionDataList.Length,
                missionProgressRect,
                progressBarSkin.GetStyle("Progress Bar BG"),
                progressBarSkin.GetStyle("Progress Bar Overlay"));

            Rect flagRect = new Rect(progressInfo.width * 0.9f, progressInfo.height * 0.2f, progressInfo.width * 0.15f, progressInfo.height * 0.15f);
            AnchorPoint.SetAnchor(ref flagRect, Anchor.MiddleCenter);
            GUI.DrawTexture(flagRect, missionProgressSkin.GetStyle("Mission Progress Flag").normal.background);

            Rect informationRect = new Rect(progressInfo.width * 0.5f, progressInfo.height * 0.5f, progressInfo.width * 0.9f, progressInfo.height * 0.5f);
            AnchorPoint.SetAnchor(ref informationRect, Anchor.MiddleCenter);
            //GUI.Box(informationRect, string.Empty);

            GUI.BeginGroup(informationRect);
            Rect missionCompletedRect = new Rect(informationRect.width * 0.5f, informationRect.height * 0.2f, informationRect.width * 0.9f, informationRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref missionCompletedRect, Anchor.MiddleCenter);
			GUI.Box(missionCompletedRect, "Missions Completed: " + completedMissions, tempSkin.GetStyle("Text1"));

            Rect missionFailedRect = new Rect(informationRect.width * 0.5f, informationRect.height * 0.4f, informationRect.width * 0.9f, informationRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref missionFailedRect, Anchor.MiddleCenter);
			GUI.Box(missionFailedRect, "Missions Failed: " + failedMissions, tempSkin.GetStyle("Text1"));

            Rect miniGamesRect = new Rect(informationRect.width * 0.5f, informationRect.height * 0.6f, informationRect.width * 0.9f, informationRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref miniGamesRect, Anchor.MiddleCenter);
			GUI.Box(miniGamesRect, "Mini Games Played: " + miniGameCount, tempSkin.GetStyle("Text1"));

            Rect charactersFoundRect = new Rect(informationRect.width * 0.5f, informationRect.height * 0.8f, informationRect.width * 0.9f, informationRect.height * 0.15f);
			AnchorPoint.SetAnchor(ref charactersFoundRect, Anchor.MiddleCenter);
			GUI.Box(charactersFoundRect, "Characters Found: " + charactersFound, tempSkin.GetStyle("Text1"));
            GUI.EndGroup();

            Rect buttonsRect = new Rect(progressInfo.width * 0.5f, progressInfo.height * 0.85f, progressInfo.width * 0.9f, progressInfo.height * 0.15f);
            AnchorPoint.SetAnchor(ref buttonsRect, Anchor.MiddleCenter);

            GUI.BeginGroup(buttonsRect);
            Rect endBtn = new Rect(buttonsRect.width * 0.25f, buttonsRect.height * 0.5f, buttonsRect.width * 0.3f, buttonsRect.height * 0.9f);
            AnchorPoint.SetAnchor(ref endBtn, Anchor.MiddleCenter);
            GUI.Box(endBtn, string.Empty, missionProgressSkin.GetStyle("Mission Progress End Btn"));

            if (endBtn.Contains(e.mousePosition))
            {
                if (e.button == 0 && e.type == EventType.mouseUp)
                {
                    userInterface.ShowSettings = false;
                    ResetSettingsWindow();
                    HotkeyTextEnd();
                    Application.Quit();
                }
            }

            Rect cancelBtn = new Rect(buttonsRect.width * 0.75f, buttonsRect.height * 0.5f, buttonsRect.width * 0.3f, buttonsRect.height * 0.9f);
            AnchorPoint.SetAnchor(ref cancelBtn, Anchor.MiddleCenter);
            GUI.Box(cancelBtn, string.Empty, missionProgressSkin.GetStyle("Mission Progress Cancel Btn"));

            if (cancelBtn.Contains(e.mousePosition))
            {
                if (e.button == 0 && e.type == EventType.mouseUp)
                {
                    userInterface.ShowSettings = false;
                    ResetSettingsWindow();
                    HotkeyTextEnd();
                }
            }

            GUI.EndGroup();
		GUI.EndGroup();

		//# region Exit
		//Rect exitRect = new Rect(settingsBGRect.width * 0.95f, settingsBGRect.height * 0.05f, mainRect.width * 0.02f, Screen.height * 0.02f * screenHeightRatio);
		//AnchorPoint.SetAnchor(ref exitRect, Anchor.MiddleCenter);
		//GUI.Box(exitRect, "X", tempSkin.GetStyle("Button"));

		//if (exitRect.Contains(e.mousePosition)) {
		//    if (e.button == 0 && e.type == EventType.mouseUp) {
		//        userInterface.ShowSettings = false;
		//        ResetSettingsWindow();
		//        HotkeyTextEnd();
		//    }
		//}
		//# endregion Exit

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void ItemQuestionBox(Event e) {
		GUI.BeginGroup(mainRect);
		Rect questionBoxRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.7f, mainRect.width * 0.23f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref questionBoxRect, Anchor.MiddleCenter);
		GUI.Box(questionBoxRect, string.Empty, tempSkin.GetStyle("Block1"));

		GUI.BeginGroup(questionBoxRect);
		Rect questionRect = new Rect(questionBoxRect.width * 0.5f, questionBoxRect.height * 0.3f, questionBoxRect.width, questionBoxRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref questionRect, Anchor.MiddleCenter);
		GUI.Box(questionRect, "Do you want to give \n" + selectedItem.ItemName, tempSkin.GetStyle("Text1"));

		Rect submitRect = new Rect(questionBoxRect.width * 0.5f, questionBoxRect.height * 0.75f, questionBoxRect.width * 0.3f, questionBoxRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref submitRect, Anchor.MiddleCenter);
		GUI.Box(submitRect, "Ok!", tempSkin.GetStyle("Button"));

		if (submitRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				NPCNameID npcName = playerInformation.InteractingTo;
				NPCDatabase.GiveNpcItem(npcName, selectedItem.ItemNameID, selectedItemGiveIndx);
			}
		}

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private void CharacterList(Event e, int i, string name, Texture2D avatar, Rect rect, GUIStyle avatarBg, GUIStyle nameBg) {
		GUI.BeginGroup(new Rect(rect.x, rect.y, rect.width, rect.height));

		// NPC Avatar BG
		Rect avatarBgRect = new Rect(rect.width * 0.15f, rect.height * 0.5f, mainRect.width * 0.07f, mainRect.height * 0.07f);
		AnchorPoint.SetAnchor(ref avatarBgRect, Anchor.MiddleCenter);
		GUI.Box(avatarBgRect, string.Empty, avatarBg);

		GUI.BeginGroup(avatarBgRect);
		// NPC Avatar
		Rect avatarRect = new Rect(avatarBgRect.width * 0.5f, avatarBgRect.height * 0.5f, avatarBgRect.width * 0.6f, avatarBgRect.height * 0.6f);
		AnchorPoint.SetAnchor(ref avatarRect, Anchor.MiddleCenter);
        GUI.DrawTexture(avatarRect, avatar);
        //GUI.Box(avatarRect, avatar, playerProfileSkin.GetStyle("Profile Friend Avatar"));
		GUI.EndGroup();

		// NPC Name BG
		Rect npcNameBgRect = new Rect(rect.width * 0.62f, rect.height * 0.5f, rect.width * 0.68f, rect.height * 0.3f);
		AnchorPoint.SetAnchor(ref npcNameBgRect, Anchor.MiddleCenter);
		GUI.Box(npcNameBgRect, string.Empty, nameBg);

		// NPC Name
		Rect npcNameRect = new Rect(rect.width * 0.62f, rect.height * 0.5f, rect.width * 0.68f, rect.height * 0.3f);
		AnchorPoint.SetAnchor(ref npcNameRect, Anchor.MiddleCenter);
		GUI.Box(npcNameRect, name, playerProfileSkin.GetStyle("Profile Friend Name"));

		GUI.EndGroup();
	}

	private void InitializeProgressInfo() {
		completedMissions = gameManager.BasePlayerData.MissionsCompleted;
		missionProgress = completedMissions;
		failedMissions = gameManager.BasePlayerData.MissionsFailed;
		miniGameCount = gameManager.BasePlayerData.MiniGamesPlayed;
		charactersFound = playerInformation.NpcFound.Length;
		missionProgressTime = 0f;
	}

	public void HotKeyText() {
		Rect keyText = new Rect();
		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);

		if (showHotkeyText) {
			guiOriginalColor = GUI.color;
			GUI.color = new Color(1f, 1f, 1f, hotkeyTextAlpha);

			GUI.BeginGroup(textRect);
			for (int i = 0; i < windowGeneralKeys.Length; i++) {
				keyText = new Rect((textRect.width * 0.2f) * i + (textRect.width * 0.2f), textRect.height * 0.5f, textRect.width * 0.2f, textRect.height);
				AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
				GUI.Box(keyText, windowGeneralKeys[i], tempSkin.GetStyle("Text"));
			}
			GUI.EndGroup();

			textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.85f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);

			GUI.BeginGroup(textRect);
			if (userInterface.ShowInventory) {
				keyText = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.25f, textRect.height);
				AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
				if (!hasPickedItem) {
					GUI.Box(keyText, "[Enter] Select Item", tempSkin.GetStyle("Text"));
				}
				else {
					GUI.Box(keyText, "[Enter] Swap Item", tempSkin.GetStyle("Text"));
				}
			}
			GUI.EndGroup();

			GUI.color = guiOriginalColor;
		}

		textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.85f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);

		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, hotkeyTextAlpha);

		GUI.BeginGroup(textRect);
		if (userInterface.GivingItem) {
			if (!hasPickedItem) {
				keyText = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.5f, textRect.height * 0.9f);
				AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
				GUI.Box(keyText, "[Enter] Select Item", tempSkin.GetStyle("Text"));
			}
			else {
				for (int i = 0; i < givingItemKeys.Length; i++) {
					keyText = new Rect((textRect.width * 0.2f) * i + (textRect.width * 0.4f), textRect.height * 0.8f, textRect.width * 0.2f, textRect.height);
					AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
					GUI.Box(keyText, givingItemKeys[i], tempSkin.GetStyle("Text"));
				}
			}
		}
		GUI.EndGroup();

		GUI.color = guiOriginalColor;
	}

	public void HotkeyTextInit() {
		StopCoroutine("HotkeyTextFadeOut");
		showHotkeyText = true;
		hotkeyTextAlpha = 0.8f;
	}

	public void HotkeyTextEnd() {
		showHotkeyText = true;
		hotkeyTextAlpha = 0.8f;
		StartCoroutine("HotkeyTextFadeOut");
	}

	private IEnumerator HotkeyTextFadeOut() {
		yield return new WaitForSeconds(hotkeyFadeDelay);
		float time = 0f;
		while (hotkeyTextAlpha > 0f) {
			time += Time.deltaTime;
			hotkeyTextAlpha = Mathf.Lerp(hotkeyTextAlpha, -0.1f, Mathf.PingPong(time, 1f) / 1f);
			yield return null;
		}
		hotkeyTextAlpha = 0f;
		showHotkeyText = false;
	}

	private Rect GetInventorySlotRect(int x, int y, Rect inventoryRect) {
		return (y == 1)
		? new Rect(
			(inventoryRect.width * 0.2f) + x * (inventoryRect.width * 0.3f),
			(inventoryRect.height * 0.2f) + y * (inventoryRect.height * 0.3f),
			mainRect.width * 0.06f,
			mainRect.height * 0.06f
		)
		: new Rect(
			(inventoryRect.width * 0.2f) + x * (inventoryRect.width * 0.2f),
			(inventoryRect.height * 0.2f) + y * (inventoryRect.height * 0.3f),
			mainRect.width * 0.06f,
			mainRect.height * 0.06f
		);
	}

	private void SwapItems(int previousIndx, int curIndx) {
		ItemData previousItem = playerInformation.Inventory[previousIndx];
		playerInformation.Inventory[previousIndx] = playerInformation.Inventory[curIndx];
		playerInformation.Inventory[curIndx] = previousItem;
	}

	private void ResetHovers() {
		showHoveredInventory = false;
		showHoveredSettings = false;
		showHoveredStats = false;
	}

	public void ResetStatisticWindow() {
		charactersFoundScroll = Vector2.zero;
		showHoveredStats = false;
		showHoveredSettings = false;
		showHoveredInventory = false;
	}

	public void ResetInventoryWindow() {
		slotRect = new Rect();

		isDraggingItem = false;
		draggedItem = new ItemData();

		selectedBoxAlpha = 1f;
		selectedBoxAlphaTime = 0f;
		hasPickedItem = false;
		selectedItemRect = new Rect();
		selectedItem = new ItemData();

		hoveredItemIndx = 0;
		hoveredItemRect = new Rect();
		hoveredItem = new ItemData();

		mouseOnInventoryBox = false;
		mouseOnInventoryWindow = false;
		selectedItemGiveIndx = -1;
		previousItemIndx = -1;

	}

	public void ResetSettingsWindow() {

	}
}