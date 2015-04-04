using UnityEngine;
using System.Collections;
using GameUtilities.AnchorPoint;
using System.Collections.Generic;
using NPC;

namespace MiniGames {
	public enum KeypressLevel {
		None,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
		Level6
	};

	public enum ArrowKeyID {
		None,
		Vertical,
		Horizontal
	};

	public delegate void MiniGameStart();
	public delegate void MiniGameSuccess();
	public delegate void MiniGameFailed();
	public delegate void MiniGameEnd();

	//namespace Database {
	//    public struct KeypressDatabase {
	//        private static KeypressData[] keypressDataList = new KeypressData[6];
	//        public static KeypressData[] KeypressDataList {
	//            get { return keypressDataList; }
	//        }

	//        public static void Initialize() {
	//            keypressDataList[0] = new KeypressData(
	//                40f,
	//                1f,
	//                3,
	//                5f,
	//                2f,
	//                10,
	//                KeypressLevel.Level1,
	//                new Statistics(0, 10)
	//            );

	//            keypressDataList[1] = new KeypressData(
	//                30f,
	//                1f,
	//                5,
	//                4f,
	//                2f,
	//                15,
	//                KeypressLevel.Level2,
	//                new Statistics(0, 10)
	//            );

	//            keypressDataList[2] = new KeypressData(
	//                30f,
	//                1f,
	//                8,
	//                4f,
	//                3f,
	//                15,
	//                KeypressLevel.Level3,
	//                new Statistics(0, 10)
	//            );

	//            keypressDataList[3] = new KeypressData(
	//                25f,
	//                1f,
	//                8,
	//                3f,
	//                3f,
	//                15,
	//                KeypressLevel.Level4,
	//                new Statistics(0, 10)
	//            );

	//            keypressDataList[4] = new KeypressData(
	//                20f,
	//                0.5f,
	//                8,
	//                2f,
	//                4f,
	//                20,
	//                KeypressLevel.Level5,
	//                new Statistics(0, 10)
	//            );

	//            keypressDataList[5] = new KeypressData(
	//                20f,
	//                0.5f,
	//                10,
	//                2f,
	//                5f,
	//                25,
	//                KeypressLevel.Level6,
	//                new Statistics(0, 10)
	//            );

	//        }

	//        public static KeypressData GetMiniGameMode(KeypressLevel toughness) {
	//            KeypressData keypressData = new KeypressData();
	//            for (int i = 0; i < keypressDataList.Length; i++) {
	//                if (keypressDataList[i].ToughnessLevel == toughness) {
	//                    keypressData = keypressDataList[i];
	//                }
	//            }
	//            return keypressData;
	//        }
	//    }
	//}

	namespace Database {
		public class KeypressDatabase {
			public static KeypressData[] keypressDataList = new KeypressData[6];
			public static KeypressData[] KeypressDataList {
				get { return keypressDataList; }
			}

			public static void Initialize() {
				keypressDataList[0] = new KeypressData(
					40f,
					1f,
					5f,
					3,
					10,
					2f,
					KeypressLevel.Level1,
					Statistics.dislikeStat * 10
				);

				keypressDataList[1] = new KeypressData(
					30f,
					1f,
					4f,
					5,
					15,
					2f,
					KeypressLevel.Level2,
					Statistics.dislikeStat * 10
				);

				keypressDataList[2] = new KeypressData(
					30f,
					1f,
					4f,
					8,
					15,
					3f,
					KeypressLevel.Level3,
					Statistics.dislikeStat * 10
				);

				keypressDataList[3] = new KeypressData(
					25f,
					1f,
					3f,
					8,
					15,
					3f,
					KeypressLevel.Level4,
					Statistics.dislikeStat * 10
				);

				keypressDataList[4] = new KeypressData(
					20f,
					0.5f,
					2f,
					8,
					20,
					4f,
					KeypressLevel.Level5,
					Statistics.dislikeStat * 10
				);

				keypressDataList[5] = new KeypressData(
					20f,
					0.5f,
					2f,
					10,
					25,
					5f,
					KeypressLevel.Level6,
					Statistics.dislikeStat * 10
				);

			}

			public static KeypressData GetMiniGameMode(KeypressLevel level) {
				if (keypressDataList != null) {
					for (int i = 0; i < keypressDataList.Length; i++) {
						if (keypressDataList[i].KeypressGameLevel == level) {
							return keypressDataList[i];
						}
					}
				}
				return null;
			}
		}
	}

	/* Keypress Game */
	public class KeypressData {
		public KeypressData() {
			keypressGameTime = 0f;
			keySpawnRate = 0f;
			keyDuration = 0f;
			keyCorrectMax = 0;
			keypressTimePenalty = 0f;
			keypressGamelevel = KeypressLevel.None;
			keypressStatisticsPenalty = Statistics.zero;
			keyHolder = null;
			keySympathyText = new SympathyText();
		}

		public KeypressData(float gameTime, float spawnRate, float duration, int keyMax, int keyCorrect, float timePenalty, KeypressLevel level, Statistics statPenalty) {
			keypressGameTime = gameTime;
			keySpawnRate = spawnRate;
			keyDuration = duration;
			keyCorrectMax = keyCorrect;
			keypressTimePenalty = timePenalty;
			keypressStatisticsPenalty = statPenalty;
			keypressGamelevel = level;
			keyHolder = new KeyHolder[keyMax];
			keySympathyText = new SympathyText();
		}

		private float keypressGameTime;
		public float KeypressGameTime {
			get { return keypressGameTime; }
		}

		private float keySpawnRate;
		public float KeySpawnRate {
			get { return keySpawnRate; }
		}

		private float keyDuration;
		public float KeyDuration {
			get { return keyDuration; }
		}

		private int keyCorrectMax;
		public int KeyCorrectMax {
			get { return keyCorrectMax; }
		}

		private float keypressTimePenalty;
		public float KeypressTimePenalty {
			get { return keypressTimePenalty; }
		}

		private Statistics keypressStatisticsPenalty;
		public Statistics KeypressStatisticsPenalty {
			get { return keypressStatisticsPenalty; }
		}

		private KeypressLevel keypressGamelevel;
		public KeypressLevel KeypressGameLevel {
			get { return keypressGamelevel; }
		}

		private SympathyText keySympathyText;
		public SympathyText KeySympathyText {
			get { return keySympathyText; }
			set { keySympathyText = value; }
		}

		private KeyHolder[] keyHolder;
		public KeyHolder[] KeyHolder {
			get { return keyHolder; }
		}

		public void AddKey(Key key, Vector2 position, GUIStyle holderStyle, GUIStyle textStyle, GUIStyle tBaseStyle, GUIStyle tOverlayStyle) {
			for (int i = 0; i < keyHolder.Length; i++) {
				if (!keyHolder[i].KeyActive) {
					keyHolder[i] = new KeyHolder(key, position, keyDuration, GetRandomText());
					keyHolder[i].KeyEnabled = true;
					keyHolder[i].KeyHolderEnabled = true;
					keyHolder[i].SetStyle(holderStyle, textStyle, tBaseStyle, tOverlayStyle);
					break;
				}
			}
		}

		public void UpdateKeypress(ref List<Key> keyDb, ref List<Vector2> keyPositionDb) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (keyHolder[i].KeyActive) {
						keyHolder[i].UpdateKeyTimer(ref keyDb, ref keyPositionDb);
					}
				}
			}
		}

		public void OnGUIDraw(Rect mainRect) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (keyHolder[i].KeyActive) {
						keyHolder[i].DrawKey(mainRect);
					}
				}
			}
		}
		
		private string GetRandomText() {
			int randTextIndx = 0;
			System.Random rng = new System.Random();
			randTextIndx = rng.Next(0, keySympathyText.Text.Length);
			return keySympathyText.Text[randTextIndx];
		}

		public bool IsKeyHolderFull() {
			int count = 0;
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyActive) {
					count++;
				}
			}

			return count == keyHolder.Length;
		}

		public bool IsKeyHolderEmpty() {
			int count = 0;
			for (int i = 0; i < keyHolder.Length; i++) {
				if (!keyHolder[i].KeyActive) {
					count++;
				}
			}

			return count == keyHolder.Length;
		}
	}

	public struct KeyHolder {
		public KeyHolder(Key key, Vector2 position, float timerMax, string text) {
			keyInput = key;
			keyPosition = position;

			keyActive = true;
			keyHolderEnabled = false;
			keyEnabled = false;
			keyTextEnabled = false;
			keyBoxRect = new Rect();

			keyHolderStyle = null;
			keyTextStyle = null;
			timerBaseStyle = null;
			timerOverlayStyle = null;

			keyTimerMax = timerMax;
			keyTimer = keyTimerMax;

			keyText = text;

			guiOriginalColor = new Color();
			keyTextAlpha = 1f;
			keyVelocity = 0f;
			keyYPos = 0f;
		}

		private bool keyActive;
		public bool KeyActive {
			get { return keyActive; }
			set { keyActive = value; }
		}

		private bool keyHolderEnabled;
		public bool KeyHolderEnabled {
			get { return keyHolderEnabled; }
			set { keyHolderEnabled = value; }
		}

		private bool keyEnabled;
		public bool KeyEnabled {
			get { return keyEnabled; }
			set { keyEnabled = value; }
		}

		private bool keyTextEnabled;
		public bool KeyTextEnabled {
			get { return keyTextEnabled; }
		}

		private float keyTimer;
		public float KeyTimer {
			get { return keyTimer; }
		}

		private float keyTimerMax;
		public float KeyTimerMax {
			get { return keyTimerMax; }
		}

		private Key keyInput;
		public Key KeyInput {
			get { return keyInput; }
		}

		private Vector2 keyPosition;
		public Vector2 KeyPosition {
			get { return keyPosition; }
		}

		private Rect keyBoxRect;
		public Rect KeyBoxRect {
			get { return keyBoxRect; }
		}

		private string keyText;
		public string KeyText {
			get { return keyText; }
		}

		private GUIStyle keyHolderStyle;
		public GUIStyle KeyHolderStyle {
			get { return keyHolderStyle; }
			set { keyHolderStyle = value; }
		}

		private GUIStyle keyTextStyle;
		public GUIStyle KeyTextStyle {
			get { return keyTextStyle; }
			set { keyTextStyle = value; }
		}

		private GUIStyle timerBaseStyle;
		public GUIStyle TimerBaseStyle {
			get { return timerBaseStyle; }
			set { timerBaseStyle = value; }
		}

		private GUIStyle timerOverlayStyle;
		public GUIStyle TimerOverlayStyle {
			get { return timerOverlayStyle; }
			set { timerOverlayStyle = value; }
		}

		private Color guiOriginalColor;
		private float keyTextAlpha;
		private float keyVelocity;
		private float keyYPos;

		public void SetStyle(GUIStyle holderStyle, GUIStyle textStyle, GUIStyle tBaseStyle, GUIStyle tOverlayStyle) {
			keyHolderStyle = holderStyle;
			keyTextStyle = textStyle;
			timerBaseStyle = tBaseStyle;
			timerOverlayStyle = tOverlayStyle;
		}

		public void UpdateKeyTimer(ref List<Key> keyDb, ref List<Vector2> keyPositionDb) {
			if (keyActive) {
				if (keyTimer > 0f) {
					keyTimer -= Time.deltaTime;
				}
				else {
					keyEnabled = false;
					keyHolderEnabled = false;

				}

				if(keyTimer <= 0f || (!keyEnabled || !keyHolderEnabled)) {
					if (!keyTextEnabled) {

						if (keyTextAlpha > 0f) {
							// Spawn Explosion particle for the key
							ParticleSystem particle = ParticleManager.current.GetPooledParticle(ParticleNameID.KeypressExplosion);
							float posY = (keyPosition.y > 2) ? -(keyBoxRect.height * 0.25f) : keyBoxRect.height * 0.25f;
							particle.transform.position = ParticleManager.current.ParticleCamera.ScreenToWorldPoint(
								new Vector3(
									keyBoxRect.x + keyPosition.x + keyBoxRect.width * 0.5f,
									(keyBoxRect.y / keyPosition.y) + posY,
									particle.transform.position.z
								)
							);
							particle.gameObject.SetActive(true);
						}

						keyTextEnabled = true;
					}
					else {
						if (keyTextAlpha > 0f) {
							keyTextAlpha -= Time.deltaTime;
						}
						else {
							keyDb.Add(keyInput);
							keyPositionDb.Add(keyPosition);
							keyActive = false;
						}

						// Move the text upwards in slow motion
						keyYPos = Mathf.SmoothDamp(keyYPos, -20f, ref keyVelocity, 0.3f);
					}
				}
			}
		}

		public void DrawKey(Rect mainRect) {
			if (keyActive) {
				GUI.BeginGroup(mainRect);

				// Draw key box holder
				if (keyHolderEnabled) {
					// position.x (0.05 ~ 0.95)
					// position.y (0.25 ~ 0.75
					keyBoxRect = new Rect(
						(keyPosition.x * (mainRect.width * 0.05f) * 2.2f) - (mainRect.width * 0.025f),
						keyPosition.y * (mainRect.height * 0.12f) + (mainRect.height * 0.3f),
						mainRect.width * 0.1f,
						mainRect.height * 0.1f);
					AnchorPoint.SetAnchor(ref keyBoxRect, Anchor.MiddleCenter);
					GUI.Box(keyBoxRect, string.Empty, keyHolderStyle);

					// Draw Key
					GUI.BeginGroup(keyBoxRect);
					if (keyEnabled) {
						Rect keyRect = new Rect(keyBoxRect.width * 0.5f, keyBoxRect.height * 0.4f, keyBoxRect.width * 0.9f, keyBoxRect.height * 0.7f);
						AnchorPoint.SetAnchor(ref keyRect, Anchor.MiddleCenter);
						GUI.Box(keyRect, string.Empty + keyInput.KeyToChar, keyInput.KeyStyle);
					}

					// Draw Timer box
					Rect timerBoxRect = new Rect(keyBoxRect.width * 0.5f, keyBoxRect.height * 0.8f, keyBoxRect.width * 0.9f, keyBoxRect.height * 0.2f);
					AnchorPoint.SetAnchor(ref timerBoxRect, Anchor.MiddleCenter);
					UserInterface.ProgressBar(keyTimer.ToString("F1"), keyTimer, keyTimerMax, timerBoxRect, timerBaseStyle, timerOverlayStyle);
					GUI.EndGroup();
				}

				// Draw key text
				if (keyTextEnabled && keyTimer > 0f) {
					guiOriginalColor = GUI.color;
					GUI.color = new Color(1f, 1f, 1f, keyTextAlpha);

					GUI.BeginGroup(keyBoxRect);
					Rect keyTextRect = new Rect(keyBoxRect.width * 0.5f, (keyBoxRect.height * 0.4f) + keyYPos, keyBoxRect.width, keyBoxRect.height);
					AnchorPoint.SetAnchor(ref keyTextRect, Anchor.MiddleCenter);
					GUI.Box(keyTextRect, keyText, keyTextStyle);
					GUI.EndGroup();

					GUI.color = guiOriginalColor;
				}

				GUI.EndGroup();
			}
		}
	}

	public struct Key {
		public Key(int keyInt, GUIStyle style) {
			keyToInt = keyInt;
			keyToChar = (char)keyInt;
			keyStyle = style;
		}

		private int keyToInt;
		public int KeyToInt {
			get { return keyToInt; }
		}

		private char keyToChar;
		public char KeyToChar {
			get { return keyToChar; }
		}

		private GUIStyle keyStyle;
		public GUIStyle KeyStyle {
			get { return keyStyle; }
		}

		public static bool operator ==(Key lhs, Key rhs) {
			return lhs.keyToInt == rhs.keyToInt;
		}

		public static bool operator !=(Key lhs, Key rhs) {
			return lhs.keyToInt != rhs.keyToInt;
		}

		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override string ToString() {
			return base.ToString();
		}
	}

	/* Dance Game */

	public class DanceData {
		public DanceData(int holderCount) {
			keyHolder = new ArrowKeyHolder[holderCount];
			danceSuccess = false;
			danceDone = false;
		}

		private ArrowKeyHolder[] keyHolder;
		public ArrowKeyHolder[] KeyHolder {
			get { return keyHolder; }
			set { keyHolder = value; }
		}

		private bool danceDone;
		public bool DanceDone {
			get { return danceDone; }
			set { danceDone = value; }
		}

		private bool danceSuccess;
		public bool DanceSuccess {
			get { return danceSuccess; }
			set { danceSuccess = value; }
		}

		public void Reset() {
			danceSuccess = false;
			danceDone = false;
		}

		public void AddKey(ArrowKey key) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (!keyHolder[i].DanceKeyHolderEnabled) {
						keyHolder[i].DanceKeyInput = key;
						break;
					}
				}
			}
		}

		public void KeyHolderActive(ArrowKey key, bool active) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (keyHolder[i].DanceKeyInput.Equals(key)) {
						keyHolder[i].DanceKeyHolderEnabled = active;
						break;
					}
				}
			}
		}

		public void KeyActive(ArrowKey key, bool active) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (keyHolder[i].DanceKeyInput.Equals(key)) {
						keyHolder[i].DanceKeyEnabled = active;
						break;
					}
				}
			}
		}

		public void OnGUIDraw(Rect mainRect) {
			if (keyHolder != null) {
				for (int i = 0; i < keyHolder.Length; i++) {
					if (keyHolder[i].DanceKeyHolderEnabled || keyHolder[i].DanceKeyEnabled) {
						keyHolder[i].DrawDanceKey(mainRect);
					}
				}
			}
		}
	}

	public struct ArrowKeyHolder {
		public ArrowKeyHolder(ArrowKey key) {
			danceKeyInput = key;
			danceKeyPosition = new Vector2();

			danceKeyHolderEnabled = false;
			danceKeyEnabled = false;
			danceKeyBoxRect = new Rect();
			danceKeyHolderStyle = null;
		}

		public ArrowKeyHolder(ArrowKey key, Vector2 pos) {
			danceKeyInput = key;
			danceKeyPosition = pos;

			danceKeyHolderEnabled = false;
			danceKeyEnabled = false;
			danceKeyBoxRect = new Rect();
			danceKeyHolderStyle = null;
		}

		private bool danceKeyHolderEnabled;
		public bool DanceKeyHolderEnabled {
			get { return danceKeyHolderEnabled; }
			set { danceKeyHolderEnabled = value; }
		}

		private bool danceKeyEnabled;
		public bool DanceKeyEnabled {
			get { return danceKeyEnabled; }
			set { danceKeyEnabled = value; }
		}

		private ArrowKey danceKeyInput;
		public ArrowKey DanceKeyInput {
			get { return danceKeyInput; }
			set { danceKeyInput = value; }
		}

		private Rect danceKeyBoxRect;
		public Rect DanceKeyBoxRect {
			get { return danceKeyBoxRect; }
		}

		private Vector2 danceKeyPosition;
		public Vector2 DanceKeyPosition {
			get { return danceKeyPosition; }
		}

		private GUIStyle danceKeyHolderStyle;
		public GUIStyle DanceKeyHolderStyle {
			get { return danceKeyHolderStyle; }
			set { danceKeyHolderStyle = value; }
		}

		public void DrawDanceKey(Rect mainRect) {
			if (danceKeyHolderEnabled) {
				GUI.BeginGroup(mainRect);

				// position.x (0.05 ~ 0.95)
				// position.y (0.25 ~ 0.75
				danceKeyBoxRect = new Rect(
					(danceKeyPosition.x * (mainRect.width * 0.05f) * 2.2f) - (mainRect.width * 0.025f),
					danceKeyPosition.y * (mainRect.height * 0.12f) + (mainRect.height * 0.3f),
					mainRect.width * 0.1f,
					mainRect.height * 0.1f);
				AnchorPoint.SetAnchor(ref danceKeyBoxRect, Anchor.MiddleCenter);
				GUI.Box(danceKeyBoxRect, string.Empty, danceKeyHolderStyle);

				# region KeyBoxRect
				if (danceKeyEnabled) {
					GUI.BeginGroup(danceKeyBoxRect);
					Rect keyRect = new Rect(danceKeyBoxRect.width * 0.5f, danceKeyBoxRect.height * 0.5f, danceKeyBoxRect.width * 0.9f, danceKeyBoxRect.height * 0.9f);
					AnchorPoint.SetAnchor(ref keyRect, Anchor.MiddleCenter);
					GUI.Box(keyRect, string.Empty, danceKeyInput.KeyStyle);
					GUI.EndGroup();
				}
				# endregion KeyBoxRect

				GUI.EndGroup();
			}
		}
	}

	public struct ArrowKey {
		public ArrowKey(int axis, ArrowKeyID keyID, GUIStyle style) {
			keyAxis = axis;
			keyId = keyID;
			keyStyle = style;
			keyTexture = null;
		}

		private int keyAxis;
		public int KeyAxis {
			get { return keyAxis; }
		}

		private ArrowKeyID keyId;
		public ArrowKeyID KeyID {
			get { return keyId; }
		}

		private Texture2D keyTexture;
		public Texture2D KeyTexture {
			get { return keyTexture; }
			set { keyTexture = value; }
		}

		private GUIStyle keyStyle;
		public GUIStyle KeyStyle {
			get { return keyStyle; }
			set { keyStyle = value; }
		}

		public static bool operator ==(ArrowKey lhs, ArrowKey rhs) {
			return ((lhs.keyAxis == rhs.keyAxis) && (lhs.keyId == rhs.keyId));
		}

		public static bool operator!=(ArrowKey lhs, ArrowKey rhs) {
			return ((lhs.keyAxis != rhs.keyAxis) && (lhs.keyId != rhs.keyId));
		}

		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override string ToString() {
			string result = string.Empty;
			if (keyId == ArrowKeyID.Horizontal) {
				if (keyAxis == 1) {
					result = "Right Arrow";
				}
				else if (keyAxis == -1) {
					result = "Left Arrow";
				}
			}
			else if (keyId == ArrowKeyID.Vertical) {
				if (keyAxis == 1) {
					result = "Up Arrow";
				}
				else if (keyAxis == -1) {
					result = "Down Arrow";
				}
			}
			return result;
		}
	}
}