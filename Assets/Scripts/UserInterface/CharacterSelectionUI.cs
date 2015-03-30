using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using Player;
using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Texture2D blackTexture;
	[SerializeField]
	private GUISkin charSelectionSkin;
	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;
	[SerializeField]
	private CameraOribit ninaIdleCamera;
	[SerializeField]
	private CameraOribit owenIdleCamera;
	[SerializeField]
	private Transform characterNina;
	[SerializeField]
	private Transform characterOwen;
	[SerializeField]
	private Texture2D ninaAvatar;
	[SerializeField]
	private Texture2D owenAvatar;

	# endregion

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;
	private Color guiOriginalColor;

	private float artProgVal = 0.5f;
	private float artStat = 50f;
	private float progStat = 50f;

	private float soundDesVal = 0.5f;
	private float designStat = 50f;
	private float soundStat = 50f;

	private float totalStat = 100f;

	private string playerName = "No Name";
	private Statistics playerStat;
	private bool isNinaShown = true;

	private PlayerInformation ninaPlayerInformation;
	private PlayerInformation owenPlayerInformation;

	private GameManager gameManager;
	private PlayerInformation playerInformation;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables
	public Transform CharacterNina {
		get { return characterNina; }
	}

	public Transform CharacterOwen {
		get { return characterOwen; }
	}

	public PlayerInformation NinaPlayerInformation {
		get { return ninaPlayerInformation; }
	}

	public PlayerInformation OwenPlayerInformation {
		get { return owenPlayerInformation; }
	}

	public static CharacterSelectionUI current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		gameManager = GameManager.current;

		ninaPlayerInformation = characterNina.GetComponent<PlayerInformation>();
		owenPlayerInformation = characterOwen.GetComponent<PlayerInformation>();
	}

	private void Update() {
		if (gameManager.GameState == GameState.CharacterSelection) {
			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (isNinaShown) {
				if (!ninaIdleCamera.CameraOrbitEnable) {
					ninaIdleCamera.CameraOrbitEnable = true;
					owenIdleCamera.CameraOrbitEnable = false;
				}
			}
			else {
				if (!owenIdleCamera.CameraOrbitEnable) {
					owenIdleCamera.CameraOrbitEnable = true;
					ninaIdleCamera.CameraOrbitEnable = false;
				}
			}
		}
	}

	public void CharacterSelectionWindow(Event e) {
		GUI.BeginGroup(mainRect);

		Rect selectionWindow = new Rect(mainRect.width * 0.275f, mainRect.height * 0.5f, mainRect.width * 0.45f, mainRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref selectionWindow, Anchor.MiddleCenter);
		GUI.Box(selectionWindow, string.Empty, charSelectionSkin.GetStyle("CS Menu BG"));

		GUI.BeginGroup(selectionWindow);
		Rect insertNameRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.1f, selectionWindow.width * 0.9f, selectionWindow.height * 0.1f);
		AnchorPoint.SetAnchor(ref insertNameRect, Anchor.MiddleCenter);
		GUI.Box(insertNameRect, "NAME", charSelectionSkin.GetStyle("CS Title"));

		Rect playerNameRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.2f, selectionWindow.width * 0.9f, selectionWindow.height * 0.1f);
		AnchorPoint.SetAnchor(ref playerNameRect, Anchor.MiddleCenter);
		playerName = GUI.TextArea(playerNameRect, playerName, 12, tempSkin.GetStyle("Block"));

		Rect statRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.3f, selectionWindow.width * 0.9f, selectionWindow.height * 0.1f);
		AnchorPoint.SetAnchor(ref statRect, Anchor.MiddleCenter);
		GUI.Box(statRect, "STATISTICS", charSelectionSkin.GetStyle("CS Title"));

		# region Artist and Programmer Slider
		Rect artistProgRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.4f, selectionWindow.width * 0.9f, selectionWindow.height * 0.15f);
		AnchorPoint.SetAnchor(ref artistProgRect, Anchor.MiddleCenter);
		GUI.Box(artistProgRect, string.Empty, charSelectionSkin.GetStyle("CS Attribute Box BG"));

		CharacterStatSlider(
			e,
			artistProgRect,
			ref artProgVal,
			"ARTIST [" + (int)artStat + "]",
			ref artStat,
			"PROGRAMMER [" + (int)progStat + "]",
			ref progStat
		);
		# endregion Artist and Programmer Slider

		# region Sound and Design Slider
		Rect soundDesRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.6f, selectionWindow.width * 0.9f, selectionWindow.height * 0.15f);
		AnchorPoint.SetAnchor(ref soundDesRect, Anchor.MiddleCenter);
		GUI.Box(soundDesRect, string.Empty, charSelectionSkin.GetStyle("CS Attribute Box BG"));

		CharacterStatSlider(
			e,
			soundDesRect,
			ref soundDesVal,
			"SOUND [" + (int)soundStat + "]",
			ref soundStat,
			"DESIGN [" + (int)designStat + "]",
			ref designStat
		);
		# endregion Sound and Design Slider

		Rect buttonBoxRect = new Rect(selectionWindow.width * 0.5f, selectionWindow.height * 0.8f, selectionWindow.width * 0.9f, selectionWindow.height * 0.15f);
		AnchorPoint.SetAnchor(ref buttonBoxRect, Anchor.MiddleCenter);
		//GUI.Box(buttonBoxRect, string.Empty, charSelectionSkin.GetStyle("Block"));

		# region Button Rect
		GUI.BeginGroup(buttonBoxRect);
		Rect backBtnRect = new Rect(buttonBoxRect.width * 0.25f, buttonBoxRect.height * 0.5f, buttonBoxRect.width * 0.4f, buttonBoxRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref backBtnRect, Anchor.MiddleCenter);
		GUI.Box(backBtnRect, "BACK!", charSelectionSkin.GetStyle("CS Back Button"));

		if (backBtnRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				gameManager.SwitchGameState(GameState.MainMenu);
			}
		}

		Rect okBtnRect = new Rect(buttonBoxRect.width * 0.75f, buttonBoxRect.height * 0.5f, buttonBoxRect.width * 0.4f, buttonBoxRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref okBtnRect, Anchor.MiddleCenter);
		GUI.Box(okBtnRect, "OK!!", charSelectionSkin.GetStyle("CS Ok Button"));

		if (okBtnRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				playerStat = new Statistics((int)artStat, (int)progStat, (int)designStat, (int)soundStat);
				PlayerData playerData = new PlayerData(playerName, null, playerStat);

				if (isNinaShown) {
					playerData.PlayerT = characterNina;
					playerData.PlayerAvatar = ninaAvatar;
					owenPlayerInformation.PlayerEnabled = false;
				}
				else {
					playerData.PlayerT = characterOwen;
					playerData.PlayerAvatar = owenAvatar;
					ninaPlayerInformation.PlayerEnabled = false;
				}

				gameManager.BasePlayerData = playerData;
				gameManager.SwitchGameState(GameState.GameStory);
			}
		}

		GUI.EndGroup();
		# endregion Button Rect

		GUI.EndGroup();

		# region Character Selection

		Rect selectionRect = new Rect(mainRect.width * 0.75f, mainRect.height * 0.75f, mainRect.width * 0.4f, mainRect.height * 0.05f);
		AnchorPoint.SetAnchor(ref selectionRect, Anchor.MiddleCenter);
		GUI.Box(selectionRect, string.Empty, charSelectionSkin.GetStyle("CS Name Box BG"));

		GUI.BeginGroup(selectionRect);
		Rect nameRect = new Rect(selectionRect.width * 0.5f, selectionRect.height * 0.5f, selectionRect.width * 0.5f, selectionRect.height * 0.5f);
		AnchorPoint.SetAnchor(ref nameRect, Anchor.MiddleCenter);
		GUI.Box(nameRect, playerName, charSelectionSkin.GetStyle("CS Character Name"));

		Rect leftButtonRect = new Rect(selectionRect.width * 0.15f, selectionRect.height * 0.5f, selectionRect.width * 0.15f, selectionRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref leftButtonRect, Anchor.MiddleCenter);
		GUI.Box(leftButtonRect, "<<", charSelectionSkin.GetStyle("CS Left Arrow Button"));

		if (leftButtonRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				isNinaShown = !isNinaShown;
			}
		}

		Rect rightButtonRect = new Rect(selectionRect.width * 0.85f, selectionRect.height * 0.5f, selectionRect.width * 0.15f, selectionRect.height * 0.9f);
		AnchorPoint.SetAnchor(ref rightButtonRect, Anchor.MiddleCenter);
		GUI.Box(rightButtonRect, ">>", charSelectionSkin.GetStyle("CS Right Arrow Button"));

		if (rightButtonRect.Contains(e.mousePosition)) {
			if (e.button == 0 && e.type == EventType.mouseUp) {
				isNinaShown = !isNinaShown;
			}
		}

		GUI.EndGroup();

		# endregion Character Selection

		GUI.EndGroup();
	}

	private void CharacterStatSlider(Event e, Rect bgRect, ref  float val, string leftStr, ref float leftVal, string rightStr, ref float rightVal) {
		GUI.BeginGroup(bgRect);
		Rect artistRect = new Rect(bgRect.width * 0.21f, bgRect.height * 0.25f, bgRect.width * 0.5f, bgRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref artistRect, Anchor.MiddleCenter);
		GUI.Box(artistRect, leftStr, charSelectionSkin.GetStyle("CS Attribute Text"));

		Rect progRect = new Rect(bgRect.width * 0.75f, bgRect.height * 0.25f, bgRect.width * 0.5f, bgRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref progRect, Anchor.MiddleCenter);
		GUI.Box(progRect, rightStr, charSelectionSkin.GetStyle("CS Attribute Text"));

		// --

		Rect sliderBoxRect = new Rect(bgRect.width * 0.5f, bgRect.height * 0.65f, bgRect.width * 0.8f, bgRect.height * 0.4f);
		AnchorPoint.SetAnchor(ref sliderBoxRect, Anchor.MiddleCenter);
		//GUI.Box(sliderBoxRect, string.Empty, charSelectionSkin.GetStyle("CS Attribute Left"));

		GameGUI.SliderBox(
			string.Empty, 
			val, 
			sliderBoxRect,
			charSelectionSkin.GetStyle("CS Attribute Left"),
			charSelectionSkin.GetStyle("CS Attribute Right"),
			tempSkin.GetStyle("Text1")
		);

		if (sliderBoxRect.Contains(e.mousePosition)) {
			if (e.button == 0 && (e.type == EventType.mouseDrag || e.type == EventType.mouseDown)) {
				val = ((e.mousePosition.x - sliderBoxRect.xMin) / (sliderBoxRect.xMax - sliderBoxRect.xMin));
				totalStat = val * Statistics.statMax;
				leftVal = Mathf.Round(totalStat);
				rightVal = Mathf.Round(Statistics.statMax - leftVal);
			}
		}

		GUI.EndGroup();
	}

	public void Reset() {
		playerStat = Statistics.zero;
		ninaIdleCamera.CameraOrbitEnable = false;
		owenIdleCamera.CameraOrbitEnable = false;

		ninaPlayerInformation.PlayerEnabled = false;
		owenPlayerInformation.PlayerEnabled = false;

		artProgVal = 0.5f;
		artStat = 50f;
		progStat = 50f;

		soundDesVal = 0.5f;
		designStat = 50f;
		soundStat = 50f;

		totalStat = 100f;
	}
}