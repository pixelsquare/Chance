using GameUtilities;
using GameUtilities.AnchorPoint;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin menuSkin;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	# endregion Private Variables

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
	}

	public void MenuWindow(Event e) {
		GUI.BeginGroup(mainRect);

		Rect startBtnRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width * 0.1f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref startBtnRect, Anchor.MiddleCenter);
		GUI.Box(startBtnRect, "Start", menuSkin.GetStyle("Menu Start Button"));

		if (startBtnRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				GameManager.current.SwitchGameState(GameState.CharacterSelection);
			}
		}
		GUI.EndGroup();
	}
}