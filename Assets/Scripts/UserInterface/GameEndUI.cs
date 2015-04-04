using GameUtilities;
using GameUtilities.AnchorPoint;
using UnityEngine;

public class GameEndUI : MonoBehaviour {
	# region Public Variables

    [SerializeField]
    private GUISkin gameEndSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private GameEnding gameEnding;

	# endregion Private Variables

	private void Start() {
		gameEnding = GameEnding.current;
	}

	private void Update() {
		if (gameEnding.BaseGameEndingData != null) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
		}
	}

	public void GameEndWindow(Event e) {
		if (gameEnding.BaseGameEndingData != null) {
			GUI.BeginGroup(mainRect);
			Rect endBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.5f, mainRect.height * 0.5f);
			AnchorPoint.SetAnchor(ref endBox, Anchor.MiddleCenter);
			GUI.Box(endBox, string.Empty, gameEndSkin.GetStyle("Game End BG"));

			GUI.BeginGroup(endBox);
            //Rect gameOverRect = new Rect(endBox.width * 0.5f, endBox.height * 0.125f, endBox.width * 0.9f, endBox.height * 0.15f);
            //AnchorPoint.SetAnchor(ref gameOverRect, Anchor.MiddleCenter);
            //GUI.Box(gameOverRect, "GAME OVER!", tempSkin.GetStyle("Block"));

			if (gameEnding.BaseGameEndingData.EndingType != EndingType.None) {
                //Rect endDescRect = new Rect(endBox.width * 0.5f, endBox.height * 0.25f, endBox.width * 0.9f, endBox.height * 0.1f);
                //AnchorPoint.SetAnchor(ref endDescRect, Anchor.MiddleCenter);
                //GUI.Box(endDescRect, gameEnding.BaseGameEndingData.EndingTitle, tempSkin.GetStyle("Block"));

				Rect endTitleRect = new Rect(endBox.width * 0.5f, endBox.height * 0.5f, endBox.width * 0.8f, endBox.height * 0.8f);
				AnchorPoint.SetAnchor(ref endTitleRect, Anchor.MiddleCenter);
                GUI.Box(endTitleRect, string.Empty, gameEndSkin.GetStyle("Game End Summary"));

                GUI.BeginGroup(endTitleRect);
                Rect endDescRect = new Rect(endTitleRect.width * 0.5f, endTitleRect.height * 0.5f, endTitleRect.width * 0.9f, endTitleRect.height * 0.9f);
                AnchorPoint.SetAnchor(ref endDescRect, Anchor.MiddleCenter);
                GUI.Box(endDescRect, gameEnding.BaseGameEndingData.EndingDesc, gameEndSkin.GetStyle("Game End Desc"));
                GUI.EndGroup();
			}

			Rect menuBtnRect = new Rect(endBox.width * 0.5f, endBox.height * 0.8f, endBox.width * 0.3f, endBox.height * 0.05f * screenHeightRatio);
			AnchorPoint.SetAnchor(ref menuBtnRect, Anchor.MiddleCenter);
            GUI.Box(menuBtnRect, string.Empty, gameEndSkin.GetStyle("Main Menu Btn"));

			if (menuBtnRect.Contains(e.mousePosition)) {
				if (e.button == 0 && e.type == EventType.mouseUp) {
					GameManager.current.SwitchGameState(GameState.MainMenu);
				}
			}

			GUI.EndGroup();

			GUI.EndGroup();
		}
	}
}