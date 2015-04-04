using GameUtilities.AnchorPoint;
using GameUtilities.PlayerUtility;
using Item;
using Item.Database;
using UnityEngine;

public class ItemInformation : BaseInteractedObject {
	# region Public Variables

	[SerializeField]
	private ItemNameID itemNameID;
	[SerializeField]
	private bool hasMiniGame;
	[SerializeField]
	private GUISkin itemSkin;

	# endregion Public Variables

	# region Private Variables

	private ItemData baseItemData;

	# endregion Private Variables

	# region Reset Variables

	private ItemNameID _itemNameID;

	# endregion Reset Variables

	// Public Properties

	// -- 

	protected override void Start() {
		base.Start();
		_itemNameID = itemNameID;
		baseItemData = ItemDatabase.GetItem(_itemNameID);
	}

	protected override void Update() {
		base.Update();
		if (gameManager.GameState == GameState.MainGame) {
			if (objectTooltipEnabled && UserInterface.InactiveUI()) {
				if (Input.GetButtonDown(PlayerUtility.ItemPickup)) {

					if (!HideNSeekUI.current.SideMissionActive) {
						AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.ItemPickup, true);
						audioSource.gameObject.SetActive(true);

						audioSource.Play();
						if (hasMiniGame) {
							HideNSeekUI.current.RunHideAndSeek(MiniGameReward);
						}
						else {
							// Check if we can still place an item in our inventory
							if (!playerInformation.IsInventoryFull()) {
								// Show an indicator telling our inventory is almost full
								if (playerInformation.GetItemCount() > 8) {
									UserInterface.RunTextIndicator(new Color(1f, 1f, 0f), "Inventory Almost Full!");
								}

								// Add the object to player's inventory
								playerInformation.AddInventoryItem(_itemNameID);
								ObjectEnabled = false;
							}
							else {
								// Show an indicator telling our inventory is full
								UserInterface.RunTextIndicator(Color.red, "Inventory is Full!");
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
				ItemTooltip();
			}
		}
	}

	public override void ResetObject() {
		base.ResetObject();
		_itemNameID = itemNameID;
	}

	private void MiniGameReward() {
		playerInformation.AddInventoryItem(_itemNameID);
		ObjectEnabled = false;
	}

	private void ItemTooltip() {
		Rect itemTooltipRect = new Rect(objectTooltipToScreen.x, Screen.height - objectTooltipToScreen.y, mainRect.width * 0.22f, mainRect.height * 0.04f);
		AnchorPoint.SetAnchor(ref itemTooltipRect, Anchor.BottomCenter);

		GUI.BeginGroup(itemTooltipRect);
		Rect keyInfoRect = new Rect(itemTooltipRect.width * 0.5f, itemTooltipRect.height * 0.5f, itemTooltipRect.width * 0.95f, itemTooltipRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref keyInfoRect, Anchor.MiddleCenter);
		GUI.Box(keyInfoRect, baseItemData.ItemName, itemSkin.GetStyle("Item Name Hover"));
		GUI.EndGroup();

		Rect textRect = new Rect(Screen.width * 0.5f, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref textRect, Anchor.MiddleCenter);

		GUI.BeginGroup(textRect);
		Rect keyText = new Rect(textRect.width * 0.5f, textRect.height * 0.5f, textRect.width * 0.2f, textRect.height);
		AnchorPoint.SetAnchor(ref keyText, Anchor.MiddleCenter);
		GUI.Box(keyText, "[E] Pickup Item", tempSkin.GetStyle("Text"));
		GUI.EndGroup();
	}
}