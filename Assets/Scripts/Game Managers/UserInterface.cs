using System.Collections;
using GameUtilities;
using GameUtilities.AnchorPoint;
using GameUtilities.GUIDepth;
using UnityEngine;
using System.Collections.Generic;
using GameUtilities.PlayerUtility;

public class UserInterface : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private GUISkin userInterfaceSkin;
	[SerializeField]
	private GUISkin playerProfileSkin;
	[SerializeField]
	private GUISkin progressBarSkin;
	[SerializeField]
	private GUISkin tempSkin;

	# endregion Public Variables

	# region Private Variables

	private Event e;
	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private Color guiOriginalColor;
	private Color textureColor;

	private bool showIndicator;
	private string indicatorText;
	private float indicatorAlpha;
	private Color originalIndicatorColor;
	private Color currentIndicatorColor;

	private bool showTitle;
	private float titleAlpha;
	private string titleText;
	private string titleNameText;
	private string titleDescText;

	private float skipTextAlpha;

	private MainMenuUI mainMenuUi;
	private CharacterSelectionUI characterSelectionUi;
	private MainGameUI mainGameUi;
	private GameEndUI gameEndUi;
	private MissionUi missionUi;
	private DanceUI danceUi;
	private KeypressGameUI keypressUi;
	private HideNSeekUI hideNSeekUi;

	private DialogueManager dialogueManager;
	private GameManager gameManager;
	private GameEnding gameEnding;

	# endregion Private Variables

	# region Reset Variables

	# endregion Reset Variables

	// Public Properties
	public int InventorySlotMax { get; set; }
	public bool ShowUI { get; set; }
	public bool ShowPlayerProfile { get; set; }
	public bool ShowInventory { get; set; }
	public bool ShowSettings { get; set; }
	public bool GivingItem { get; set; }
	public bool IsTransitioning { get; set; }

	public MainMenuUI MainMenuUi {
		get { return mainMenuUi; }
	}

	public CharacterSelectionUI CharacterSelectionUi {
		get { return characterSelectionUi; }
	}

	public MainGameUI MainGameUi {
		get { return mainGameUi; }
	}

	public GameEndUI GameEndUi {
		get { return gameEndUi; }
	}
	// --

	public static UserInterface current;

	private void Awake() {
		current= this;
	}

	private void Start() {
		dialogueManager = DialogueManager.current;
		gameManager = GameManager.current;
		gameEnding = GameEnding.current;

		textureColor = new Color(0f, 0f, 0f, 1f);

		mainMenuUi = GetComponent<MainMenuUI>();
		characterSelectionUi = GetComponent<CharacterSelectionUI>();
		mainGameUi = GetComponent<MainGameUI>();
		gameEndUi = GetComponent<GameEndUI>();
		missionUi = GetComponent<MissionUi>();
		danceUi = GetComponent<DanceUI>();
		keypressUi = GetComponent<KeypressGameUI>();
		hideNSeekUi = GetComponent<HideNSeekUI>();
	}

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

		if (dialogueManager.DialogueEnabled) {
			ShowUI = false;
		}
		else {
			ShowUI = true;
		}

		if (gameManager.GameState == GameState.GameStory) {
			if (Input.GetButtonDown(PlayerUtility.NextDialogue)) {
				gameManager.GameStory.StopCutscene();
			}

			skipTextAlpha = Mathf.Lerp(0f, 1f, Mathf.PingPong(Time.time, 1f) / 1f);
		}
		else if (gameManager.GameState == GameState.EndingResult) {
			if (Input.GetButtonDown(PlayerUtility.NextDialogue)) {
				gameManager.Theater.BaseCutscene.StopCutscene();
			}

			skipTextAlpha = Mathf.Lerp(0f, 1f, Mathf.PingPong(Time.time, 1f) / 1f);
		}
	}

	private void OnGUI() {
		e = Event.current;
		GUI.depth = GUIDepth.userInterfaceDepth;

		if (gameManager.GameState == GameState.Intro) {

		}
		else if (gameManager.GameState == GameState.MainMenu) {
			mainMenuUi.MenuWindow(e);
		}
		else if (gameManager.GameState == GameState.CharacterSelection) {
			characterSelectionUi.CharacterSelectionWindow(e);
		}
		else if (gameManager.GameState == GameState.MainGame) {
			if (ShowUI) {
				mainGameUi.MainGUI(e);
			}

			if (ShowPlayerProfile) {
				mainGameUi.PlayerProfileWindow(e);
			}

			if (ShowSettings) {
				mainGameUi.SettingsWindow(e);
			}

			if (ShowInventory || GivingItem) {
				mainGameUi.InventoryWindow(e);
			}

			mainGameUi.HotKeyText();
			missionUi.MissionGUI(e);
			danceUi.MainGUI(e);
			keypressUi.MainGUI(e);
			hideNSeekUi.MainGUI(e);
			dialogueManager.MainGUI(e);

			if (showIndicator) {
				DrawIndicator();
			}
		}
		else if (gameManager.GameState == GameState.GameEnd) {
			gameEndUi.GameEndWindow(e);
		}
		else if (gameManager.GameState == GameState.GameStory || gameManager.GameState == GameState.EndingResult) {
			DrawSkipText();
		}

		if (IsTransitioning) {
			GUI.depth = GUIDepth.transitionDepth;
			guiOriginalColor = GUI.color;
			GUI.color = textureColor;

			GUI.DrawTexture(mainRect, Resources.BlackTexture);

			GUI.color = guiOriginalColor;
		}

		if (showTitle) {
			DrawTitle();
		}
	}

	public void ResetMainGame() {
		mainGameUi.ResetStatisticWindow();
		mainGameUi.ResetInventoryWindow();
		mainGameUi.ResetSettingsWindow();
		ShowUI = true;
		ShowPlayerProfile = false;
		ShowInventory = false;
		ShowSettings = false;
	}

	private void DrawSkipText() {
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, skipTextAlpha);

		Rect skipTextRect = new Rect(Screen.width * 0.5f, Screen.height * 0.95f, Screen.width * 0.3f, Screen.height * 0.1f * screenHeightRatio);
		AnchorPoint.SetAnchor(ref skipTextRect, Anchor.MiddleCenter);
		GUI.Box(skipTextRect, "<<< Press (Spacebar) to SKIP >>>", tempSkin.GetStyle("Skip Text"));
		GUI.color = guiOriginalColor;
	}

	private void DrawIndicator() {
		originalIndicatorColor = userInterfaceSkin.GetStyle("Player Text Indicator").normal.textColor;
		userInterfaceSkin.GetStyle("Player Text Indicator").normal.textColor = currentIndicatorColor;

		GUI.BeginGroup(mainRect);
		GUI.color = new Color(1f, 1f, 1f, indicatorAlpha);
		Rect inventoryFullText = new Rect(mainRect.width * 0.5f, mainRect.height * 0.75f, mainRect.width * 0.3f, mainRect.height * 0.3f);
		AnchorPoint.SetAnchor(ref inventoryFullText, Anchor.MiddleCenter);
		GUI.Box(inventoryFullText, indicatorText, userInterfaceSkin.GetStyle("Player Text Indicator"));
		GUI.EndGroup();

		userInterfaceSkin.GetStyle("Player Text Indicator").normal.textColor = originalIndicatorColor;
	}

	private IEnumerator Fade(FadeTransition fadeT) {
		IsTransitioning = true;
		float time = 0f;
		while (textureColor.a < 1f) {
			time += Time.deltaTime;
			textureColor.a = Mathf.Lerp(textureColor.a, 1.1f, Mathf.PingPong(time, fadeT.FadeDuration) / fadeT.FadeDuration);
			yield return null;
		}

		if (fadeT.Midtransition != null) {
			fadeT.Midtransition();
		}

		time = 0f;
		while (textureColor.a > 0f) {
			time += Time.deltaTime;
			textureColor.a = Mathf.Lerp(textureColor.a, -0.1f, Mathf.PingPong(time, fadeT.FadeDuration) / fadeT.FadeDuration);
			yield return null;
		}

		if (fadeT.EndTransition != null) {
			fadeT.EndTransition();
		}

		IsTransitioning = false;
	}

	public static bool InactiveUI() {
		return !current.dialogueManager.DialogueEnabled && !current.gameEnding.ShowTheaterWindow && !current.mainGameUi.ShowHotkeyText && 
			!KeypressGameUI.current.KeypressEnabled && !HideNSeekUI.current.ShowingHideNSeekInstruction && !DanceUI.current.DanceEnabled &&
			(!current.ShowInventory && !current.ShowSettings && !current.ShowPlayerProfile && !current.GivingItem);
	}

	private void DrawTitle() {
		GUI.BeginGroup(mainRect);
		Rect missionTitleBox = new Rect(mainRect.width * 0.5f, mainRect.height * 0.5f, mainRect.width, mainRect.height * 0.2f);
		AnchorPoint.SetAnchor(ref missionTitleBox, Anchor.MiddleCenter);

		GUI.BeginGroup(missionTitleBox);
		guiOriginalColor = GUI.color;
		GUI.color = new Color(1f, 1f, 1f, titleAlpha);

		Rect missionTitle = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.1f, missionTitleBox.width * 0.9f, missionTitleBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref missionTitle, Anchor.MiddleCenter);
		GUI.Box(missionTitle, titleText, userInterfaceSkin.GetStyle("Screen Title"));

		Rect missionNameTitle = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.3f, missionTitleBox.width * 0.9f, missionTitleBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref missionNameTitle, Anchor.MiddleCenter);
		GUI.Box(missionNameTitle, titleNameText, userInterfaceSkin.GetStyle("Screen Title Name"));

		Rect missionDivider = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.45f, missionTitleBox.width * 0.8f, missionTitleBox.height * 0.08f);
		AnchorPoint.SetAnchor(ref missionDivider, Anchor.MiddleCenter);
		GUI.Box(missionDivider, string.Empty, userInterfaceSkin.GetStyle("Screen Divider"));

		Rect missionName = new Rect(missionTitleBox.width * 0.5f, missionTitleBox.height * 0.6f, missionTitleBox.width * 0.9f, missionTitleBox.height * 0.5f);
		AnchorPoint.SetAnchor(ref missionName, Anchor.MiddleCenter);
		GUI.Box(missionName, titleDescText, userInterfaceSkin.GetStyle("Screen Desc"));
		GUI.color = guiOriginalColor;

		GUI.EndGroup();

		GUI.EndGroup();
	}

	private IEnumerator IndicatorFade() {
		showIndicator = true;
		float time = 0f;
		while (indicatorAlpha < 1f) {
			time += Time.deltaTime;
			indicatorAlpha = Mathf.Lerp(indicatorAlpha, 1.1f, Mathf.PingPong(time, 0.2f) / 0.2f);
			yield return null;
		}

		yield return new WaitForSeconds(1f);
		time = 0f;
		while (indicatorAlpha > 0f) {
			time += Time.deltaTime;
			indicatorAlpha = Mathf.Lerp(indicatorAlpha, -0.1f, Mathf.PingPong(time, 1) / 1);
			yield return null;
		}

		showIndicator = false;
	}

	private IEnumerator TitleFade(FadeTransition fadeT) {
		showTitle = true;
		float time = 0f;
		while (time < 1f) {
			time += Time.deltaTime;
			titleAlpha = Mathf.Lerp(titleAlpha, 1f, Mathf.PingPong(time, 3f) / 3f);
			yield return null;
		}

		if (fadeT.Midtransition != null) {
			fadeT.Midtransition();
		}

		float waitTime = (fadeT.FadeDuration > 0f) ? fadeT.FadeDuration : 1f;
		yield return new WaitForSeconds(waitTime);

		time = 0f;
		while (time < 1f) {
			time += Time.deltaTime;
			titleAlpha = Mathf.Lerp(titleAlpha, 0f, Mathf.PingPong(time, 3f) / 3f);
			yield return null;
		}

		if (fadeT.EndTransition != null) {
			fadeT.EndTransition();
		}

		showTitle = false;
	}

	public static void RunTransitionFade(FadeTransition fadeT) {
		if (fadeT.StartTransition != null) {
			fadeT.StartTransition();
		}

		current.StartCoroutine(current.Fade(fadeT));
	}

	public static void ProgressBar(string name, float val, float max, Rect barRect, GUIStyle baseStyle, GUIStyle overlayStyle) {
		GUI.BeginGroup(barRect);

		// Progress bar Background
		Rect bgRect = new Rect(0f, 0f, barRect.width, barRect.height);
		GUI.Box(bgRect, name, baseStyle);

		// Convert value to decimal place
		float value = val / max;

		# region Progress bar Overlay
		GUI.BeginGroup(new Rect(0f, 0f, barRect.width * value, barRect.height));

		Rect overlayRect = new Rect(0f, 0f, barRect.width, barRect.height);
		GUI.Box(overlayRect, name, overlayStyle);

		GUI.EndGroup();
		# endregion Progress bar Overlay

		GUI.EndGroup();
	}

	public static void SliderBox(string name, float value, Rect bgRect, GUIStyle leftStyle, GUIStyle rightStyle, GUIStyle textStyle) {
		GUI.BeginGroup(bgRect);

		# region Left Slider
		Rect leftRect = new Rect(0f, bgRect.height * 0.5f, bgRect.width, bgRect.height);
		AnchorPoint.SetAnchor(ref leftRect, Anchor.MiddleLeft);
		//GUI.Box(leftRect, string.Empty);

		GUI.BeginGroup(new Rect(0f, 0f, bgRect.width * value, bgRect.height));
		Rect leftValRect = new Rect(0f, leftRect.height * 0.5f, leftRect.width, leftRect.height);
		AnchorPoint.SetAnchor(ref leftValRect, Anchor.MiddleLeft);
		GUI.Box(leftValRect, string.Empty, leftStyle);
		GUI.EndGroup();
		# endregion Left Slider

		# region Right Slider
		Rect rightRect = new Rect(bgRect.width, bgRect.height * 0.5f, bgRect.width, bgRect.height);
		AnchorPoint.SetAnchor(ref rightRect, Anchor.MiddleRight);
		//GUI.Box(rightRect, string.Empty);

		GUI.BeginGroup(new Rect(rightRect.width * value, 0f, rightRect.width, rightRect.height));
		Rect rightValRect = new Rect(rightRect.width * (1 - value), rightRect.height * 0.5f, rightRect.width, rightRect.height);
		AnchorPoint.SetAnchor(ref rightValRect, Anchor.MiddleRight);
		GUI.Box(rightValRect, string.Empty, rightStyle);
		GUI.EndGroup();
		# endregion Right Slider

		GUI.EndGroup();
		GUI.Box(bgRect, name, textStyle);
	}

	public static void RunTextIndicator(Color color, string text) {
		current.indicatorText = text;
		current.currentIndicatorColor = color;
		current.indicatorAlpha = 0f;
		current.StopCoroutine("IndicatorFade");
		current.StartCoroutine("IndicatorFade");
	}

	public static void RunTitle(string title, string name, string desc, FadeTransition fadeT) {
		current.titleText = title;
		current.titleNameText = name;
		current.titleDescText = desc;
		current.titleAlpha = 0f;

		current.StartCoroutine(current.TitleFade(fadeT));
	}
}

//public struct Title {
//    private string titleText;
//    public string TitleText {
//        get { return titleText; }
//    }

//    private string titleName;
//    public string TitleName {
//        get { return titleName; }
//    }

//    private string titleDesc;
//    public string TitleDesc {
//        get { return titleDesc; }
//    }

//    private float titleAlpha;
//    public float TitleAlpha {
//        get { return titleAlpha; }
//    }
//}