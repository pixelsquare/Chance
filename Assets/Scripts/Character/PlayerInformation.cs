using GameUtilities;
using Item;
using Item.Database;
using NPC;
using NPC.Database;
using Player;
using UnityEngine;

public class PlayerInformation : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private PlayerCamera playerCamera;

	# endregion Public Variables

	# region Private Variables

	private NPCNameID interactingTo;
	private ItemData[] inventory;
	private NPCData[] npcFound;
	private bool playerEnabled;

	private PlayerControl playerControl;
	private GameManager gameManager;
	private NPCManager npcManager;

	# endregion Private Variables

	// Public Properties
	public NPCNameID InteractingTo {
		get { return interactingTo; }
		set { interactingTo = value; }
	}

	public PlayerCamera PlayerCamera {
		get { return playerCamera; }
	}

	public ItemData[] Inventory {
		get { return inventory; }
	}

	public NPCData[] NpcFound {
		get { return npcFound; }
	}

	public bool PlayerEnabled {
		get { return playerEnabled; }
		set {
			playerEnabled = value;
			SetPlayerInformationActive(value);
		}
	}
	// --

	private void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		playerControl = GetComponent<PlayerControl>();

		inventory = new ItemData[11];
		npcFound = new NPCData[npcManager.Npc.Length];

        //AddInventoryItem(ItemNameID.GrassJellyMilkTea);
        //AddInventoryItem(ItemNameID.HotCoffee);
        //AddInventoryItem(ItemNameID.IcedTea);
		//AddInventoryItem(ItemNameID.Milk);
		//AddInventoryItem(ItemNameID.OrangeJuice);
		//AddInventoryItem(ItemNameID.StrawberryMilkshake);
		//AddInventoryItem(ItemNameID.USB);
		//AddInventoryItem(ItemNameID.WhiteEarphones);
		//AddInventoryItem(ItemNameID.GuitarPick);

		//AddInventoryItem(ItemNameID.FanfictionNotebook);
		//AddInventoryItem(ItemNameID.Headset);

        //AddNpcToList(NPCNameID.Noelle);
        //AddNpcToList(NPCNameID.Andy);
        //AddNpcToList(NPCNameID.Bart);
        //AddNpcToList(NPCNameID.Beta);
        //AddNpcToList(NPCNameID.Franz);
        //AddNpcToList(NPCNameID.Jenevieve);
        //AddNpcToList(NPCNameID.Maxine);

        PlayerEnabled = false;
	}

	private void Update() {
	//    if (Input.GetKeyDown(KeyCode.U)) {
	//        AddNpcToList(NPCNameID.Noelle);
	//        AddNpcToList(NPCNameID.Andy);
	//        AddNpcToList(NPCNameID.Bart);
	//        AddNpcToList(NPCNameID.Beta);
	//        AddNpcToList(NPCNameID.Franz);
	//        AddNpcToList(NPCNameID.Jenevieve);
	//        AddNpcToList(NPCNameID.Maxine);
	//        for (int i = 0; i < npcManager.Npc.Length; i++) {
	//            NPCInformation npcInformation = npcManager.Npc[i].GetComponent<NPCInformation>();
	//            npcInformation.BaseNpcData.NpcFound = true;
	//        }
	//    }
	}

	private void SetPlayerInformationActive(bool active) {
		collider.enabled = active;
		playerControl.PlayerAnimator.enabled = active;
		playerControl.ControlEnabled = active;
		playerCamera.CameraEnabled = active;
		GameUtility.SetRendererActiveRecursively(transform, active);
		gameObject.SetActive(active);
	}

	public void AddNpcToList(NPCNameID nameID) {
		int count = 0; int indx = 0;
		for (int i = 0; i < npcFound.Length; i++) {
			if (npcFound[i] != null) {
				if (npcFound[i].NpcNameID == nameID) {
					count++;
				}
				indx++;
			}
		}

		if (count < 1 && indx < npcFound.Length) {
			npcFound[indx] = NPCDatabase.GetNpc(nameID);
			Debug.Log("[PLAYER] " + npcFound[indx].NpcName + " has been added to your Characters Found");
		}
	}

	public void RemoveNpcToList(NPCNameID nameID) {
		for (int i = 0; i < npcFound.Length; i++) {
			if (npcFound[i] != null && npcFound[i].NpcNameID == nameID) {
				Debug.Log("[CHARACTER FOUND] " + npcFound[i].NpcName + " has been removed.");
				npcFound[i] = new NPCData();
			}
		}
	}

	public void AddInventoryItem(ItemNameID itemID) {
		int indx = 0;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] == null) {
				indx = i;
				break;
			}
		}

		if (indx < inventory.Length && !IsInventoryFull()) {
			inventory[indx] = ItemDatabase.GetItem(itemID);
			Debug.Log("[PLAYER INVENTORY] " + inventory[indx].ItemName + " has been added to your Inventory");
		}
	}

	public void RemoveInventoryItem(ItemNameID itemID, int inventoryIndx) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null && inventory[i].ItemNameID == itemID) {
				Debug.Log("[INVENTORY] An item has been removed. [" + inventory[i].ItemName + "]");
				inventory[i] = null;
				break;
			}
		}
	}

	public void AddStatistics(Statistics addedStat) {
		Statistics beforeStat = gameManager.BasePlayerData.PlayerStatistics;
		Statistics afterStat = gameManager.BasePlayerData.PlayerStatistics + addedStat;
		if (addedStat.Like > 0) {
			afterStat.Dislike -= addedStat.Like;
		}
		else if (addedStat.Dislike > 0) {
			afterStat.Like -= addedStat.Dislike;
		}

		afterStat.ClampAllValues();
		PlayerData tmpData = gameManager.BasePlayerData;
		tmpData.PlayerStatistics = afterStat;
		gameManager.BasePlayerData = tmpData;
		gameManager.BasePlayerData.PlayerStatistics.ClampAllValues();

		Debug.Log("[PLAYER STATISTICS] " + addedStat.ToString() + " has been added to " +
			gameManager.BasePlayerData.PlayerName + " " + beforeStat.ToString() + " -> " + gameManager.BasePlayerData.PlayerStatistics.ToString());
	}

	public bool isNpcInCharactersFound(NPCNameID nameID) {
		for (int i = 0; i < npcFound.Length; i++) {
			if (npcFound[i] != null && npcFound[i].NpcNameID == nameID) {
				return true;
			}
		}
		return false;
	}

	public bool IsInventoryFull() {
		int count = 0;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				count++;
			}
		}

		return (count == inventory.Length);
	}

	public int GetItemCount() {
		int count = 0;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				count++;
			}
		}
		return count;
	}

	public int GetNpcCount() {
		int count = 0;
		for (int i = 0; i < npcFound.Length; i++) {
			if (npcFound[i] != null) {
				count++;
			}
		}
		return count;
	}

	public void ResetPlayerInformation() {
		interactingTo = NPCNameID.None;
		PlayerEnabled = false;
	}
}