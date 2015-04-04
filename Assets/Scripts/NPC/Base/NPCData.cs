using Item;
using UnityEngine;
using MiniGames;

namespace NPC {
	/*
	 *	Dialogue Data classes
	 *	NOTE: Do not edit anything
	 */

	public class NPCData {
		public NPCData() {
			npcNickname = string.Empty;
			npcName = string.Empty;
			npcAvatar = null;
			npcNameID = NPCNameID.None;
			npcDesc = string.Empty;
			npcStatistics = Statistics.zero;
			npcTalkedCount = 0;	
			npcItemsNeeded = null;
			npcItemsHave = new ItemData[20];
			npcDialogue = null;
			npcSympathyText = new SympathyText();
			npcItemAcceptDialogue = null;
			npcItemDeclineDialogue = null;
			npcDialogueEnded = false;
			npcNextDialogueindx = 0;
			npcDialogueIndx = -1;
			npcFound = false;
		}

		public NPCData(string nickname, string name, Texture2D avatar, NPCNameID nameID, string desc, Statistics stat, 
			ItemsNeeded[] items, NPCDialogue[] dialogue, SympathyText sympathyText, ItemAcceptDialogue[] itemAccept, ItemDeclineDialogue[] itemDecline) {
				npcNickname = nickname;
				npcName = name;
				npcAvatar = avatar;
				npcNameID = nameID;
				npcDesc = desc;
				npcStatistics = stat;
				npcItemsNeeded = items;
				npcItemsHave = new ItemData[20];
				npcDialogue = dialogue;
				npcSympathyText = sympathyText;
				npcItemAcceptDialogue = itemAccept;
				npcItemDeclineDialogue = itemDecline;
				npcDialogueEnded = false;
				npcNextDialogueindx = 0;
				npcDialogueIndx = -1;
				npcTalkedCount = 0;
				npcFound = false;
		}

		private string npcNickname;
		public string NpcNickname {
			get { return npcNickname; }
		}

		private string npcName;
		public string NpcName {
			get { return npcName; }
		}

		private Texture2D npcAvatar;
		public Texture2D NpcAvatar {
			get { return npcAvatar; }
		}

		private NPCNameID npcNameID;
		public NPCNameID NpcNameID {
			get { return npcNameID; }
		}

		private string npcDesc;
		public string NpcDesc {
			get { return npcDesc; }
		}

		private Statistics npcStatistics;
		public Statistics NpcStatistics {
			get { return npcStatistics; }
			set { npcStatistics = value; }
		}

		private int npcTalkedCount;
		public int NpcTalkedCount {
			get { return npcTalkedCount; }
			set { npcTalkedCount = value; }
		}

		private ItemsNeeded[] npcItemsNeeded;
		public ItemsNeeded[] NpcItemsNeeded {
			get { return npcItemsNeeded; }
		}

		private ItemData[] npcItemsHave;
		public ItemData[] NpcItemsHave {
			get { return npcItemsHave; }
			set { npcItemsHave = value; }
		}

		private NPCDialogue[] npcDialogue;
		public NPCDialogue[] NpcDialogue {
			get { return npcDialogue; }
			set { npcDialogue = value; }
		}

		private SympathyText npcSympathyText;
		public SympathyText NpcSympathyText {
			get { return npcSympathyText; }
		}

		private ItemAcceptDialogue[] npcItemAcceptDialogue;
		public ItemAcceptDialogue[] NpcItemAcceptDialogue {
			get { return npcItemAcceptDialogue; }
			set { npcItemAcceptDialogue = value; }
		}

		private ItemDeclineDialogue[] npcItemDeclineDialogue;
		public ItemDeclineDialogue[] NpcItemDeclineDialogue {
			get { return npcItemDeclineDialogue; }
			set { npcItemDeclineDialogue = value; }
		}

		private bool npcDialogueEnded;
		public bool NpcDialogueEnded {
			get { return npcDialogueEnded; }
			set { npcDialogueEnded = value; }
		}

		private int npcNextDialogueindx;
		public int NpcNextDialogueIndx {
			get { return npcNextDialogueindx; }
			set { npcNextDialogueindx = value; }
		}

		private int npcDialogueIndx;
		public int NpcDialogueIndx {
			get { return npcDialogueIndx; }
			set { npcDialogueIndx = value; }
		}

		private bool npcFound;
		public bool NpcFound {
			get { return npcFound; }
			set { npcFound = value; }
		}
	}

	public struct NPCDialogue {
		public NPCDialogue(DialogueData[] data) {
			dialogueData = data;
		}

		private DialogueData[] dialogueData;
		public DialogueData[] DialogueData {
			get { return dialogueData; }
			set { dialogueData = value; }
		}
	}

	public struct SympathyText {
		public SympathyText(string[] sympathy) {
			text = sympathy;
		}

		private string[] text;
		public string[] Text {
			get { return text; }
		}
	}

	public struct ItemAcceptDialogue {
		public ItemAcceptDialogue(DialogueData[] data) {
			dialogueData = data;
		}

		private DialogueData[] dialogueData;
		public DialogueData[] DialogueData {
			get { return dialogueData; }
		}
	}

	public struct ItemDeclineDialogue {
		public ItemDeclineDialogue(DialogueData[] data) {
			dialogueData = data;
		}

		private DialogueData[] dialogueData;
		public DialogueData[] DialogueData {
			get { return dialogueData; }
		}
	}

	public class ItemsNeeded {
		public ItemsNeeded() {
			itemID = ItemNameID.None;
			addedStatistics = new Statistics();
			itemRecieved = false;
		}

		public ItemsNeeded(ItemNameID item) {
			itemID = item;
			addedStatistics = new Statistics();
			itemRecieved = false;
		}

		public ItemsNeeded(ItemNameID item, Statistics addedStat) {
			itemID = item;
			addedStatistics = addedStat;
			itemRecieved = false;
		}

		private ItemNameID itemID;
		public ItemNameID ItemID {
			get { return itemID; }
			set { itemID = value; }
		}

		private Statistics addedStatistics;
		public Statistics AddedStatistics {
			get { return addedStatistics; }
			set { addedStatistics = value; }
		}

		private bool itemRecieved;
		public bool ItemRecieved {
			get { return itemRecieved; }
			set { itemRecieved = value; }
		}
	}

	public struct DialogueData {
		public DialogueData(string text) {
			dialogue = text;
			buttons = null;
			texture = null;
		}

		public DialogueData(Texture2D tex, string text) {
			dialogue = text;
			buttons = null;
			texture = tex;
		}

		public DialogueData(string text, DialogueButton[] btns) {
			dialogue = text;
			buttons = btns;
			texture = null;
		}

		public DialogueData(Texture2D tex, string text, DialogueButton[] btns) {
			dialogue = text;
			buttons = btns;
			texture = tex;
		}

		private string dialogue;
		public string Dialogue {
			get { return dialogue; }
		}

		private Texture2D texture;
		public Texture2D Texture {
			get { return texture; }
		}

		private DialogueButton[] buttons;
		public DialogueButton[] Buttons {
			get { return buttons; }
		}
	}

	public struct DialogueButton {
		public DialogueButton(string name, ButtonType type) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, KeypressLevel toughness) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, KeypressLevel toughness) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, KeypressLevel toughness) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, KeypressLevel toughness) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = Statistics.zero;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, KeypressLevel toughness, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, KeypressLevel toughness, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, KeypressLevel toughness, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, KeypressLevel toughness, Statistics addedStat) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = null;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, KeypressLevel toughness, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, KeypressLevel toughness, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, KeypressLevel toughness, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, KeypressLevel toughness, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			buttonDialogueData = data;
			addedStatistics = Statistics.zero;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, ButtonType type, KeypressLevel toughness, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = -1;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, ButtonType type, KeypressLevel toughness, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = -1;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = KeypressLevel.None;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		public DialogueButton(string name, int goToIndx, ButtonType type, KeypressLevel toughness, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = goToIndx;
			texture = null;
		}

		public DialogueButton(Texture2D tex, string name, int goToIndx, ButtonType type, KeypressLevel toughness, Statistics addedStat, DialogueData[] data) {
			buttonName = name;
			buttonType = type;
			toughnessLevel = toughness;
			addedStatistics = addedStat;
			buttonDialogueData = data;
			goToDialogueIndx = goToIndx;
			texture = tex;
		}

		private string buttonName;
		public string ButtonName {
			get { return buttonName; }
		}

		private ButtonType buttonType;
		public ButtonType ButtonType {
			get { return buttonType; }
		}

		private KeypressLevel toughnessLevel;
		public KeypressLevel KeypressLevel {
			get { return toughnessLevel; }
		}

		private Statistics addedStatistics;
		public Statistics AddedStatistics {
			get { return addedStatistics; }
		}

		private Texture2D texture;
		public Texture2D Texture {
			get { return texture; }
		}

		private DialogueData[] buttonDialogueData;
		public DialogueData[] ButtonDialogueData {
			get { return buttonDialogueData; }
		}

		private int goToDialogueIndx;
		public int GoToDialogueIndx {
			get { return goToDialogueIndx; }
		}
	}
}