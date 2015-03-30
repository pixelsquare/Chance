using UnityEngine;
using System.Collections;
using Item;
using Item.Database;
using GameUtilities;
using GameUtilities.GameGUI;
using GameUtilities.AnchorPoint;
using GameUtilities.PlayerUtility;

public class ItemInformation : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private ItemNameID itemNameID;
	[SerializeField]
	private GUISkin itemSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;
	private Color guiOriginalColor;

	private bool itemEnable;
	private bool itemTooltipEnable;
	private Vector3 tooltipToScreen;

	private float distanceFromPlayer;
	private float angleFromPlayer;

	private Color materialColor;
	private Color originalMatColor;

	private ItemData baseItemData;
	private GameManager gameManager;
	private UserInterface userInterface;
    //private PlayerControl playerControl;
	private PlayerInformation playerInformation;

	# endregion Private Variables

	# region Reset Variables

	private ItemNameID _itemNameID;

	# endregion Reset Variables

	// Public Variables
	public ItemNameID ItemNameID {
		get { return _itemNameID; }
	}

	public bool ItemEnabled {
		get { return itemEnable; }
		set {
			itemEnable = value;
			gameObject.SetActive(value);
		}
	}
	// --

	private void OnEnable() {
		Reset();
		BasePlayer.OnInteract += OnInteract;
	}

	private void OnDisable() {
		BasePlayer.OnInteract -= OnInteract;
	}

	private void Start() {
		gameManager = GameManager.current;
		userInterface = UserInterface.current;

		materialColor = gameObject.renderer.material.color;
		originalMatColor = materialColor;
		itemEnable = gameObject.activeInHierarchy;
		baseItemData = ItemDatabase.GetItem(_itemNameID);
	}

	private void Update() {
		if (gameManager.GameState == GameState.MainGame) {
			if (playerInformation == null && gameManager.BasePlayerData != null) {
				playerInformation = gameManager.BasePlayerData.PlayerInformation;
                //playerControl = gameManager.PlayerControl;
			}

			materialColor = Color.Lerp(originalMatColor, Color.gray, Mathf.PingPong(Time.time, 1f) / 1f);
			gameObject.renderer.material.color = materialColor;

			if (itemTooltipEnable || (itemEnable && baseItemData.ItemNameID != ItemNameID.None && gameManager.BasePlayerData != null)) {
				distanceFromPlayer = Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position);
				//angleFromPlayer = Vector3.Angle(gameManager.Player.forward, (transform.position - gameManager.Player.position));

				//if (distanceFromPlayer > 3f || angleFromPlayer > 60f) {
				//    itemTooltipEnable = false;
				//}
				if (distanceFromPlayer > 2f) {
					itemTooltipEnable = false;
				}
				else {
					GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
					tooltipToScreen = gameManager.BasePlayerData.PlayerControl.PCamera.WorldToScreenPoint(transform.position);

					if (Input.GetButtonDown(PlayerUtility.ItemPickup) && userInterface.InactiveUI() && itemTooltipEnable) {
						if (!playerInformation.IsInventoryFull()) {
							if (playerInformation.GetItemCount() > 8) {
								userInterface.MainGameUi.RunInventoryFullIndicator("Inventory is Almost Full!");
							}
							playerInformation.AddInventoryItem(_itemNameID);
							ItemEnabled = false;
						}
						else {
							userInterface.MainGameUi.RunInventoryFullIndicator("Inventory is Full!");
						}
					}
				}
			}
		}
	}

	private void OnGUI() {
		if (itemTooltipEnable && itemEnable && baseItemData.ItemNameID != ItemNameID.None) {
			GUI.depth = GUIDepth.npcMenuDepth;

			if (userInterface.InactiveUI()) {
				ItemTooltip();
			}
		}
	}

	private void OnInteract(GameObject go) {
		itemTooltipEnable = (gameObject == go);
	}

	private void ItemTooltip() {
		Rect itemTooltipRect = new Rect(tooltipToScreen.x, Screen.height - tooltipToScreen.y, mainRect.width * 0.22f, mainRect.height * 0.04f);
		AnchorPoint.SetAnchor(ref itemTooltipRect, Anchor.BottomCenter);
		//GUI.Box(itemTooltipRect, string.Empty, itemSkin.GetStyle("Item Name Hover"));

		GUI.BeginGroup(itemTooltipRect);
		Rect keyInfoRect = new Rect(itemTooltipRect.width * 0.5f, itemTooltipRect.height * 0.5f, itemTooltipRect.width * 0.95f, itemTooltipRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref keyInfoRect, Anchor.MiddleCenter);
		GUI.Box(keyInfoRect, baseItemData.ItemName, itemSkin.GetStyle("Item Name Hover"));
		GUI.EndGroup();

		if (!userInterface.MainGameUi.ShowHotkeyText) {
			Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.94f, Screen.width * 0.5f, Screen.height * 0.1f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);
			//GUI.Box(textRect, string.Empty);

			GUI.BeginGroup(textRect);

			Rect pickupItemTextRect = new Rect(textRect.width * 0.51f, textRect.height * 0.5f, textRect.width * 0.45f, textRect.height * 0.9f);
			AnchorPoint.SetAnchor(ref pickupItemTextRect, Anchor.MiddleCenter);
			GameGUI.HotkeyBox(pickupItemTextRect, 0.52f, "E", "Pickup Item", tempSkin.GetStyle("Text"));

			GUI.EndGroup();
		}
	}

	private void Reset() {
		_itemNameID = itemNameID;

		tooltipToScreen = Vector3.zero;
		itemTooltipEnable = false;
	}
}