using UnityEngine;
using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameEnd;
using GameEnd.Database;

public class GameEndUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private GameEndData baseGameEndData;

	# endregion Private Variables

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
	}

	public void GameEndWindow(Event e) {
		GUI.BeginGroup(mainRect);
		Rect endBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.9f, mainRect.height * 0.6f);
		AnchorPoint.SetAnchor(ref endBox, Anchor.MiddleCenter);
		GUI.Box(endBox, string.Empty, tempSkin.GetStyle("Block"));

		GUI.BeginGroup(endBox);
		Rect gameOverRect = new Rect(endBox.width * 0.5f, endBox.height * 0.125f, endBox.width * 0.9f, endBox.height * 0.15f);
		AnchorPoint.SetAnchor(ref gameOverRect, Anchor.MiddleCenter);
		GUI.Box(gameOverRect, "GAME OVER!", tempSkin.GetStyle("Block"));

		if (baseGameEndData.EndingType != EndingType.None) {
			Rect endDescRect = new Rect(endBox.width * 0.5f, endBox.height * 0.25f, endBox.width * 0.9f, endBox.height * 0.1f);
			AnchorPoint.SetAnchor(ref endDescRect, Anchor.MiddleCenter);
			GUI.Box(endDescRect, baseGameEndData.EndingTitle, tempSkin.GetStyle("Block"));

			Rect endTitleRect = new Rect(endBox.width * 0.5f, endBox.height * 0.5f, endBox.width * 0.9f, endBox.height * 0.4f);
			AnchorPoint.SetAnchor(ref endTitleRect, Anchor.MiddleCenter);
			GUI.Box(endTitleRect, baseGameEndData.EndingDesc, tempSkin.GetStyle("Block"));
		}

		Rect menuBtnRect = new Rect(endBox.width * 0.5f, endBox.height * 0.8f, endBox.width * 0.15f, endBox.height * 0.075f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref menuBtnRect, Anchor.MiddleCenter);
		GUI.Box(menuBtnRect, "Go back to Menu", tempSkin.GetStyle("Button"));

		if (menuBtnRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				GameManager.current.SwitchGameState(GameState.MainMenu);
			}
		}

		GUI.EndGroup();

		GUI.EndGroup();
	}

	public void SetBaseGameEnd(EndingType type) {
		baseGameEndData = GameEndDatabase.GetGameEnd(type);
	}
}