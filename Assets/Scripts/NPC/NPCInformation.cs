using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.PlayerUtility;
using NPC;
using NPC.Database;
using UnityEngine;
using GameUtilities.GUIDepth;
using GameUtilities.LayerManager;

public class NPCInformation : BaseInteractedObject {
	# region Public Variables

	[SerializeField]
	private NPCNameID npcNameID;
	[SerializeField]
	private Emoticon emoticon;
	[SerializeField]
	private GUISkin npcSkin;

	# endregion Public Variables

	# region Private Variables

	private NPCData baseNpcData;
	private bool isInCharactersFound;
	private string[] keyString = new string[2] { "[E] Talk", "[R] Give Item" };

	private NPCControl npcControl;

	# endregion Private Variables

	# region Reset Variables

	private NPCNameID _npcNameID;

	# endregion Reset Variables

	// Public Properties
	public NPCData BaseNpcData {
		get { return baseNpcData; }
	}
	// --

	protected override void Start() {
		base.Start();
		npcControl = GetComponent<NPCControl>();

		_npcNameID = npcNameID;
		baseNpcData = NPCDatabase.GetNpc(_npcNameID);
	}

	protected override void Update() {
		base.Update();
		if (gameManager.GameState == GameState.MainGame) {
			if(baseNpcData != null) {
				if (objectTooltipEnabled && UserInterface.InactiveUI()) {
					if (!baseNpcData.NpcFound) {
						baseNpcData.NpcFound = true;
					}

					// Interact Key
					if (Input.GetButtonDown(PlayerUtility.Interact) && !baseNpcData.NpcDialogueEnded) {
						playerInformation.AddNpcToList(baseNpcData.NpcNameID);
						playerInformation.InteractingTo = baseNpcData.NpcNameID;
						npcControl.CurState = NPCState.Interact;
						GameUtility.SetGameObjectLayerRecursively(transform, LayerManager.LayerNpcInteracted);
						dialogueManager.RunDialogue(_npcNameID, baseNpcData.NpcNextDialogueIndx);
					}

					isInCharactersFound = playerInformation.isNpcInCharactersFound(baseNpcData.NpcNameID);
					if (isInCharactersFound) {
						// Give Item Key
						if (Input.GetButtonDown(PlayerUtility.GiveItem) && (UserInterface.InactiveUI() || userInterface.GivingItem)) {
							userInterface.GivingItem = true;
							userInterface.MainGameUi.ResetInventoryWindow();

							if (userInterface.GivingItem) {
								playerInformation.InteractingTo = baseNpcData.NpcNameID;
								npcControl.CurState = NPCState.Interact;
								userInterface.MainGameUi.HotkeyTextInit();
							}
							else {
								userInterface.MainGameUi.HotkeyTextEnd();
							}
						}
					}
				}
			}
		}
	}

	protected override void OnGUI() {
		base.OnGUI();
		if (gameManager.GameState == GameState.MainGame) {
			if (objectTooltipEnabled && UserInterface.InactiveUI()) {
				if (!baseNpcData.NpcDialogueEnded) {
					e = Event.current;
					NPCMenu();
				}
			}
		}
	}

	public override void ResetObject() {
		base.ResetObject();
		_npcNameID = npcNameID;
		isInCharactersFound = false;
	}

	private void NPCMenu() {
		Rect npcMenuRect = new Rect(objectTooltipToScreen.x, Screen.height - objectTooltipToScreen.y, mainRect.width * 0.1f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref npcMenuRect, Anchor.BottomLeft);
		GUI.Box(npcMenuRect, string.Empty, npcSkin.GetStyle("BG"));

		GUI.BeginGroup(npcMenuRect);

		# region Talk Button
		Rect talkRect = new Rect(npcMenuRect.width * 0.48f, npcMenuRect.height * 0.3f, npcMenuRect.width * 0.7f, npcMenuRect.height * 0.45f);
		AnchorPoint.SetAnchor(ref talkRect, Anchor.MiddleCenter);
		GUI.Box(talkRect, string.Empty, npcSkin.GetStyle("Talk Button"));

		if (talkRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				playerInformation.AddNpcToList(baseNpcData.NpcNameID);
				playerInformation.InteractingTo = baseNpcData.NpcNameID;
				npcControl.CurState = NPCState.Interact;
				GameUtility.SetGameObjectLayerRecursively(transform, LayerManager.LayerNpcInteracted);
				dialogueManager.RunDialogue(_npcNameID, baseNpcData.NpcNextDialogueIndx);
			}
		}
		# endregion Talk Button

		if (isInCharactersFound) {
			# region Give Item
			Rect giveItemRect = new Rect(npcMenuRect.width * 0.55f, npcMenuRect.height * 0.64f, npcMenuRect.width * 0.7f, npcMenuRect.height * 0.45f);
			AnchorPoint.SetAnchor(ref giveItemRect, Anchor.MiddleCenter);
			GUI.Box(giveItemRect, string.Empty, npcSkin.GetStyle("Give Item Button"));

			if (giveItemRect.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					playerInformation.InteractingTo = baseNpcData.NpcNameID;
					npcControl.CurState = NPCState.Interact;
					userInterface.GivingItem = !userInterface.GivingItem;
					userInterface.MainGameUi.ResetInventoryWindow();

					if (userInterface.GivingItem) {
						playerInformation.InteractingTo = baseNpcData.NpcNameID;
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



		Rect keyText = new Rect();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
		//GUI.Box(textRect, string.Empty);

		GUI.BeginGroup(textRect);
		if (isInCharactersFound) {
			for (int i = 0; i < keyString.Length; i++) {
				keyText = new Rect((textRect.width * 0.125f) * i + (textRect.width * 0.425f), textRect.height * 0.5f, textRect.width * 0.2f, textRect.height);
				AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
				GUI.Box(keyText, keyString[i], tempSkin.GetStyle("Text"));
			}
		}
		else {
			Rect inventoryTextRect = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.26f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref inventoryTextRect, Anchor.MiddleCenter);
			GUI.Box(inventoryTextRect, "[E] Talk", tempSkin.GetStyle("Text"));
		}

		GUI.EndGroup();
	}

	public void RunEmoticon(EmoticonNameID state) {
		emoticon.RunEmoticon(state);
	}
}