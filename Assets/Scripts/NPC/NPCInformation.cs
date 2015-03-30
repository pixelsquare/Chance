using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using GameUtilities.PlayerUtility;
using Item;
using Item.Database;
using NPC;
using NPC.Database;
using UnityEngine;

public class NPCInformation : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private NPCNameID npcNameID;
	[SerializeField]
	private Emoticon emoticon;
	[SerializeField]
	private GUISkin npcSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion

	# region Private Variables

	private ItemData[] itemsHave;
	private Statistics npcStatistics;
	private bool npcEnable;
	private bool npcMenuEnable;

	private Vector3 informationBoxToScreen;
	private NPCData baseNPCData;

	private float distanceFromPlayer;
	private float angleFromPlayer;

	private Event e;
	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private NPCControl npcControl;
	private PlayerControl playerControl;
	private PlayerInformation playerInformation;

	private GameManager gameManager;
	private UserInterface userInterface;
	private DialogueManager dialogueManager;

	# endregion Private Variables

	# region Reset Variables

	private NPCNameID _npcNameID;

	# endregion Reset Variables

	// Public Properties
	public NPCNameID NpcNameID {
		get { return _npcNameID; }
	}

	public bool NPCEnable {
		get { return npcEnable; }
		set {
			npcEnable = value;
			gameObject.SetActive(value);
		}
	}

	public string NpcName {
		get { return baseNPCData.NpcName; }
	}

	public Statistics NpcStatistics {
		get { return npcStatistics; }
		set { baseNPCData.NpcStatistics = value; }
	}
	// --

	private void OnEnable() {
		BasePlayer.OnInteract += OnInteract;
	}

	private void OnDisable() {
		BasePlayer.OnInteract -= OnInteract;
	}

	private void Start() {
		gameManager = GameManager.current;
		userInterface = UserInterface.current;
		dialogueManager = DialogueManager.current;
		npcControl = GetComponent<NPCControl>();

		npcEnable = gameObject.activeInHierarchy;
		itemsHave = new ItemData[10];

		if (_npcNameID != NPCNameID.None) {
			baseNPCData = NPCDatabase.GetNPC(_npcNameID);
		}
	}

	private void Update() {
		if (playerInformation == null && gameManager.BasePlayerData != null) {
			playerInformation = gameManager.BasePlayerData.PlayerInformation;
			playerControl = gameManager.BasePlayerData.PlayerControl;
		}

		if (npcEnable && npcMenuEnable && baseNPCData != null) {
			distanceFromPlayer = Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position);
			angleFromPlayer = Vector3.Angle(gameManager.BasePlayerData.PlayerT.forward, (transform.position - gameManager.BasePlayerData.PlayerT.position));

			// Interact Key
			if (Input.GetButtonDown(PlayerUtility.Interact) && userInterface.InactiveUI() && !baseNPCData.NpcDialogueEnded) {
				playerInformation.AddNpcToList(baseNPCData.NpcNameID);
				playerInformation.InteractingTo = baseNPCData.NpcNameID;
				npcControl.CurState = NPCState.Interact;
				dialogueManager.RunDialogue(_npcNameID, baseNPCData.NpcNextDialogueIndx);
			}

			if (playerInformation.isNpcInCharactersFound(baseNPCData.NpcNameID)) {
				// Give Item Key
				if (Input.GetButtonDown(PlayerUtility.GiveItem) && (userInterface.InactiveUI() || userInterface.GivingItem)) {
					userInterface.GivingItem = true;
					userInterface.MainGameUi.ResetInventoryWindow();

					if (userInterface.GivingItem) {
						playerInformation.InteractingTo = baseNPCData.NpcNameID;
						npcControl.CurState = NPCState.Interact;
						userInterface.MainGameUi.HotkeyTextInit();
					}
					else {
						userInterface.MainGameUi.HotkeyTextEnd();
					}
				}
			}

			if (distanceFromPlayer > 3f || angleFromPlayer > playerControl.InteractViewAngle) {
				npcMenuEnable = false;
			}
			else {
				GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
				informationBoxToScreen = gameManager.BasePlayerData.PlayerControl.PCamera.WorldToScreenPoint(transform.position);

				if (baseNPCData.NpcStatistics != npcStatistics) {
					npcStatistics = baseNPCData.NpcStatistics;
				}
			}
		}
	}

	private void OnGUI() {
		if (npcEnable && npcMenuEnable && baseNPCData != null && userInterface.InactiveUI()) {
			GUI.depth = GUIDepth.npcMenuDepth;
			e = Event.current;
			NPCMenu();
		}
	}

	private void OnInteract(GameObject go) {
		npcMenuEnable = (gameObject == go);
	}

	private void NPCMenu() {
		Rect npcMenuRect = new Rect(informationBoxToScreen.x, Screen.height - informationBoxToScreen.y, mainRect.width * 0.1f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref npcMenuRect, Anchor.BottomLeft);
		GUI.Box(npcMenuRect, string.Empty, npcSkin.GetStyle("BG"));

		GUI.BeginGroup(npcMenuRect);

		if (!baseNPCData.NpcDialogueEnded) {
			# region Talk Button
			Rect talkRect = new Rect(npcMenuRect.width * 0.48f, npcMenuRect.height * 0.3f, npcMenuRect.width * 0.7f, npcMenuRect.height * 0.45f);
			AnchorPoint.SetAnchor(ref talkRect, Anchor.MiddleCenter);
			GUI.Box(talkRect, string.Empty, npcSkin.GetStyle("Talk Button"));

			if (talkRect.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					playerInformation.AddNpcToList(baseNPCData.NpcNameID);
					playerInformation.InteractingTo = baseNPCData.NpcNameID;
					npcControl.CurState = NPCState.Interact;
					dialogueManager.RunDialogue(_npcNameID, baseNPCData.NpcNextDialogueIndx);
				}
			}
			# endregion Talk Button
		}

		if (playerInformation.isNpcInCharactersFound(baseNPCData.NpcNameID)) {
			# region Give Item
			Rect giveItemRect = new Rect(npcMenuRect.width * 0.55f, npcMenuRect.height * 0.64f, npcMenuRect.width * 0.7f, npcMenuRect.height * 0.45f);
			AnchorPoint.SetAnchor(ref giveItemRect, Anchor.MiddleCenter);
			GUI.Box(giveItemRect, string.Empty, npcSkin.GetStyle("Give Item Button"));

			if (giveItemRect.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					playerInformation.InteractingTo = baseNPCData.NpcNameID;
					npcControl.CurState = NPCState.Interact;
					userInterface.GivingItem = !userInterface.GivingItem;
					userInterface.MainGameUi.ResetInventoryWindow();

					if (userInterface.GivingItem) {
						playerInformation.InteractingTo = baseNPCData.NpcNameID;
						npcControl.CurState = NPCState.Interact;
						userInterface.MainGameUi.HotkeyTextInit();
					}
					else {
						userInterface.MainGameUi.HotkeyTextEnd();
					}
				}
			}
			# endregion Give Item
		}

		GUI.EndGroup();

		if (userInterface.InactiveUI() && !userInterface.MainGameUi.ShowHotkeyText) {
			Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.94f, Screen.width * 0.5f, Screen.height * 0.1f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
			//GUI.Box(textRect, string.Empty);

			GUI.BeginGroup(textRect);

			if (playerInformation.isNpcInCharactersFound(baseNPCData.NpcNameID)) {
				Rect talkTextRect = new Rect(textRect.width * 0.41f, textRect.height * 0.5f, textRect.width * 0.25f, textRect.height * 0.9f);
				AnchorPoint.SetAnchor(ref talkTextRect, Anchor.MiddleCenter);
				GameGUI.HotkeyBox(talkTextRect, 0.52f, "E", "Talk", tempSkin.GetStyle("Text"));

				Rect giveItemTextRect = new Rect(textRect.width * 0.64f, textRect.height * 0.5f, textRect.width * 0.4f, textRect.height * 0.9f);
				AnchorPoint.SetAnchor(ref giveItemTextRect, Anchor.MiddleCenter);
				GameGUI.HotkeyBox(giveItemTextRect, 0.52f, "R", "Give Item", tempSkin.GetStyle("Text"));
			}
			else {
				Rect talkTextRect = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.25f, textRect.height * 0.9f);
				AnchorPoint.SetAnchor(ref talkTextRect, Anchor.MiddleCenter);
				GameGUI.HotkeyBox(talkTextRect, 0.52f, "E", "Talk", tempSkin.GetStyle("Text"));
			}

			GUI.EndGroup();
		}
	}

	public void RunEmoticon(EmoticonNameID state) {
		emoticon.RunEmoticon(state);
	}

	public void AddItemsHave(ItemNameID itemID) {
		int count = 0;
		for (int i = 0; i < itemsHave.Length; i++) {
			if (itemsHave[i] != null && itemsHave[i].ItemNameID != ItemNameID.None) {
				count++;
			}
		}

		if (count < itemsHave.Length) {
			itemsHave[count] = ItemDatabase.GetItem(itemID);
			Debug.Log("[ITEM RECIEVED] " + baseNPCData.NpcName + " has recieved " + itemsHave[count].ItemName + ".");
		}
	}

	public void Reset() {
		_npcNameID = npcNameID;

		informationBoxToScreen = Vector3.zero;
		npcMenuEnable = false;
	}
}