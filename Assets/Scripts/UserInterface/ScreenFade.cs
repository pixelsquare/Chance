using System.Collections;
using GameUtilities;
using GameUtilities.GameGUI;
using UnityEngine;

public class ScreenFade : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Texture2D blackTexture;
	[SerializeField]
	private float fadeDuration = 0.25f;

	# endregion Public Variables

	# region Private Variables

	private Rect mainRect;
	private float screenHeightRatio;
	private float oldScreenHeightRatio;

	private Color guiOriginalColor;
	private Color textureColor;

	# endregion Private Variables

	// Public Properties
	public bool IsTransitioning { get; set; }
	//--

	public static ScreenFade current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		textureColor = new Color(0f, 0f, 0f, 1f);
	}

	private void Update() {
		GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);
	}

	private void OnGUI() {
		if (IsTransitioning) {
			GUI.depth = GUIDepth.transitionDepth;
			guiOriginalColor = GUI.color;
			GUI.color = textureColor;

			GUI.DrawTexture(mainRect, blackTexture);

			GUI.color = guiOriginalColor;
		}
	}

	public void Run(FadeTransition fadeT) {
		if (fadeT.StartTransition != null) {
			fadeT.StartTransition();
		}

		StartCoroutine("FadeOut", fadeT);
	}

	private IEnumerator FadeOut(FadeTransition fadeT) {
		IsTransitioning = true;
		float time = 0f;
		while (textureColor.a < 1f) {
			time += Time.deltaTime;
			textureColor.a = Mathf.Lerp(textureColor.a, 1.1f, time / (fadeDuration * 100f));
			yield return null;
		}

		if (fadeT.Midtransition != null) {
			fadeT.Midtransition();
		}

		StartCoroutine("FadeIn", fadeT);
		StopCoroutine("FadeOut");
	}

	private IEnumerator FadeIn(FadeTransition fadeT) {
		float time = 0f;
		while (textureColor.a > 0f) {
			time += Time.deltaTime;
			textureColor.a = Mathf.Lerp(textureColor.a, -0.1f, time / (fadeDuration * 100f));
			yield return null;
		}

		if (fadeT.EndTransition != null) {
			fadeT.EndTransition();
		}

		IsTransitioning = false;
		StopCoroutine("FadeIn");
	}
}