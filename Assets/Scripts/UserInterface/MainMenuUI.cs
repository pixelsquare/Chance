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
        Rect titleRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.4f, mainRect.width * 0.6f, mainRect.height * 0.3f);
        AnchorPoint.SetAnchor(ref titleRect, Anchor.MiddleCenter);
        GUI.Box(titleRect, string.Empty, menuSkin.GetStyle("Menu Title"));

		Rect startBtnRect = new Rect(mainRect.width * 0.5f, mainRect.height * 0.65f, mainRect.width * 0.2f, mainRect.height * 0.1f);
		AnchorPoint.SetAnchor(ref startBtnRect, Anchor.MiddleCenter);
		GUI.Box(startBtnRect, string.Empty, menuSkin.GetStyle("Menu Start Button"));

		if (startBtnRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				GameManager.current.SwitchGameState(GameState.CharacterSelection);
				AudioSource audioSource = AudioManager.current.GetAudioSource(AudioNameID.GeneralBtn, true);
				if (audioSource != null) {
					audioSource.gameObject.SetActive(true);
					audioSource.Play();
				}
			}
		}
		GUI.EndGroup();
	}
}