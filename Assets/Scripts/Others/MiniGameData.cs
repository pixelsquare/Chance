using System.Collections.Generic;
using GameUtilities.AnchorPoint;
using GameUtilities.GameGUI;
using UnityEngine;

namespace MiniGame {
	public enum ToughnessLevel {
		None,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
		Level6
	};

	public delegate void MiniGameStart();
	public delegate void MiniGameSuccess();
	public delegate void MiniGameFailed();
	public delegate void MiniGameEnd();

	namespace MiniGameDatabase {
		public struct MiniGameDatabase {
			private static MiniGameData[] itemDataInfoList = new MiniGameData[6];
			public static MiniGameData[] ItemDataInfoList {
				get { return itemDataInfoList; }
			}

			public static void Initialize() {
				itemDataInfoList[0] = new MiniGameData(
					40f,
					2f,
					3,
					5f,
					2f,
					10,
					ToughnessLevel.Level1,
					new Statistics(0, 10)
				);

				itemDataInfoList[1] = new MiniGameData(
					30f,
					1f,
					5,
					4f,
					2f,
					15,
					ToughnessLevel.Level2,
					new Statistics(0, 10)
				);

				itemDataInfoList[2] = new MiniGameData(
					30f,
					1f,
					8,
					4f,
					3f,
					15,
					ToughnessLevel.Level3,
					new Statistics(0, 10)
				);

				itemDataInfoList[3] = new MiniGameData(
					25f,
					1f,
					8,
					3f,
					3f,
					15,
					ToughnessLevel.Level4,
					new Statistics(0, 10)
				);

				itemDataInfoList[4] = new MiniGameData(
					20f,
					0.5f,
					8,
					2f,
					4f,
					20,
					ToughnessLevel.Level5,
					new Statistics(0, 10)
				);

				itemDataInfoList[5] = new MiniGameData(
					20f,
					0.5f,
					10,
					2f,
					5f,
					25,
					ToughnessLevel.Level6,
					new Statistics(0, 10)
				);

			}

			public static MiniGameData GetMiniGameMode(ToughnessLevel toughness) {
				MiniGameData miniGameData = new MiniGameData();
				for (int i = 0; i < itemDataInfoList.Length; i++) {
					if (itemDataInfoList[i].ToughnessLevel == toughness) {
						miniGameData = itemDataInfoList[i];
					}
				}

				Debug.Log("Mini Game Mode Not Found!");
				return miniGameData;
			}
		}
	}

	public struct MiniGameData {
		public MiniGameData(float time, float spawnRate, int spawnMax, float keyDur, float tPenalty, int kCorrectMax, 
			ToughnessLevel toughness, Statistics statPenalty) {
			gameTime = time;
			keySpawnRate = spawnRate;
			keySpawnMax = Mathf.Clamp(spawnMax, 0, 10);
			keyDuration = keyDur;
			timePenalty = tPenalty;
			correctKeysMax = kCorrectMax;
			toughnessLevel = toughness;
			gameOverPenalty = statPenalty;

			boxBgStyle = null;
			timerBgStyle = null;
			timerOverlayStyle = null;

			keyHolder = new KeyHolder[10];
		}

		private float gameTime;
		public float GameTime {
			get { return gameTime; }
		}

		private float keySpawnRate;
		public float KeySpawnRate {
			get { return keySpawnRate; }
		}

		private int keySpawnMax;
		public int KeySpawnMax {
			get { return keySpawnMax; }
		}

		private float keyDuration;
		public float KeyDuration {
			get { return keyDuration; }
		}

		private float timePenalty;
		public float TimePenalty {
			get { return timePenalty; }
		}

		private int correctKeysMax;
		public int CorrectKeysMax {
			get { return correctKeysMax; }
		}

		private ToughnessLevel toughnessLevel;
		public ToughnessLevel ToughnessLevel {
			get { return toughnessLevel; }
		}

		private Statistics gameOverPenalty;
		public Statistics GameOverPenalty {
			get { return gameOverPenalty; }
		}

		private KeyHolder[] keyHolder;
		public KeyHolder[] KeyHolder {
			get { return keyHolder; }
			set { keyHolder = value; }
		}

		private GUIStyle boxBgStyle;
		private GUIStyle timerBgStyle;
		private GUIStyle timerOverlayStyle;

		public void SetStyle(GUIStyle boxBg, GUIStyle timerBg, GUIStyle timerOverlay) {
			boxBgStyle = boxBg;
			timerBgStyle = timerBg;
			timerOverlayStyle = timerOverlay;
		}

		public void AddKey(Key key, string text, Vector2 keyPosition) {
			for (int i = 0; i < keyHolder.Length; i++) {
				if (!keyHolder[i].KeyEnabled) {
					keyHolder[i] = new KeyHolder(key, text, keyDuration, keyPosition, boxBgStyle, timerBgStyle, timerOverlayStyle);
					break;
				}
			}
		}

		public void RemoveKey(KeyHolder holder) {
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyInput.KeyToInt == holder.KeyInput.KeyToInt) {
					keyHolder[i].Reset();
					keyHolder[i] = new KeyHolder();
				}
			}
		}

		public bool IsKeyHolderFull() {
			int count = 0;
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyEnabled) {
					count++;
				}
			}

			return (count == keySpawnMax);
		}

		public KeyHolder KeyPress(int input) {
			KeyHolder keyHold = new KeyHolder();
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyInput.KeyToInt == input) {
					keyHold = keyHolder[i];
				}
			}
			return keyHold;
		}

		public void UpdateMode(float dt, ref List<Key> keyDB, ref List<Vector2> keyPosDb) {
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyEnabled) {
					if (keyHolder[i].KeyTime > 0f) {
						keyHolder[i].KeyTime -= dt;
					}

					if (keyHolder[i].TimerHasExpired()) {
						keyDB.Add(keyHolder[i].KeyInput);
						keyPosDb.Add(keyHolder[i].KeyPosition);
						RemoveKey(keyHolder[i]);
					}
				}
			}
		}

		public void OnGUIMode(Rect mainRect) {
			for (int i = 0; i < keyHolder.Length; i++) {
				if (keyHolder[i].KeyEnabled) {
					keyHolder[i].DrawKey(mainRect);
				}
			}
		}
	}

	public struct KeyHolder {
		public KeyHolder(Key key, string text, float maxtime, Vector2 position, GUIStyle boxBg, GUIStyle timerBg, GUIStyle timerOverlay) {
			keyInput = key;
			keyPosition = position;
			keyMaxTime = maxtime;
			keyTime = keyMaxTime;

			boxBgStyle = boxBg;
			timerBgStyle = timerBg;
			timerOverlayStyle = timerOverlay;

			sympathyText = text;
			keyBoxRect = new Rect();

			keyEnabled = true;
		}

		private bool keyEnabled;
		public bool KeyEnabled {
			get { return keyEnabled; }
			set { keyEnabled = value; }
		}

		private Key keyInput;
		public Key KeyInput {
			get { return keyInput; }
		}

		private float keyTime;
		public float KeyTime {
			get { return keyTime; }
			set { keyTime = value; }
		}

		private float keyMaxTime;
		public float KeyMaxTime {
			get { return keyMaxTime; }
		}

		private Vector2 keyPosition;
		public Vector2 KeyPosition {
			get { return keyPosition; }
		}

		private Rect keyBoxRect;
		public Rect KeyBoxRect {
			get { return keyBoxRect; }
		}

		private string sympathyText;
		private GUIStyle boxBgStyle;
		private GUIStyle timerBgStyle;
		private GUIStyle timerOverlayStyle;

		public void DrawKey(Rect mainRect) {
			GUI.BeginGroup(mainRect);

			// position.x (0.05 ~ 0.95)
			// position.y (0.25 ~ 0.75
			Rect keyBoxRect = new Rect(
				(keyPosition.x * (mainRect.width * 0.05f) * 2.2f) - (mainRect.width * 0.025f), 
				keyPosition.y * (mainRect.height * 0.12f) + (mainRect.height * 0.3f), 
				mainRect.width * 0.12f, 
				mainRect.height * 0.12f);
			AnchorPoint.SetAnchor(ref keyBoxRect, Anchor.MiddleCenter);
			GUI.Box(keyBoxRect, string.Empty, boxBgStyle);

			# region KeyBoxRect
			GUI.BeginGroup(keyBoxRect);
			Rect keyRect = new Rect(keyBoxRect.width * 0.5f, keyBoxRect.height * 0.2f, keyBoxRect.width * 0.3f, keyBoxRect.height * 0.3f);
			AnchorPoint.SetAnchor(ref keyRect, Anchor.MiddleCenter);
			GUI.Box(keyRect, string.Empty + keyInput.KeyToChar, keyInput.KeyStyle);

			Rect sympathyRect = new Rect(keyBoxRect.width * 0.5f, keyBoxRect.height * 0.55f, keyBoxRect.width * 0.9f, keyBoxRect.height * 0.5f);
			AnchorPoint.SetAnchor(ref sympathyRect, Anchor.MiddleCenter);
			GUI.Box(sympathyRect, sympathyText, keyInput.KeyStyle);

			Rect timerBoxRect = new Rect(keyBoxRect.width * 0.5f, keyBoxRect.height * 0.85f, keyBoxRect.width * 0.75f, keyBoxRect.height * 0.12f);
			AnchorPoint.SetAnchor(ref timerBoxRect, Anchor.MiddleCenter);
			//GUI.Box(timerBoxRect, string.Empty);

			GameGUI.ProgressBar(string.Empty, keyTime, keyMaxTime, timerBoxRect, timerBgStyle, timerOverlayStyle);

			GUI.EndGroup();
			# endregion KeyBoxRect

			GUI.EndGroup();
		}

		public bool TimerHasExpired() {
			return (keyTime <= 0);
		}

		public void Reset() {
			keyTime = 0f;
			keyMaxTime = 0f;
			keyEnabled = false;
			keyPosition = Vector2.zero;
		}
	}

	public struct Key {
		public Key(int keyInt, GUIStyle style) {
			keyToInt = keyInt;
			keyStyle = style;
		}

		private int keyToInt;
		public int KeyToInt {
			get { return keyToInt; }
		}

		public char KeyToChar {
			get { return (char)keyToInt; }
		}

		private GUIStyle keyStyle;
		public GUIStyle KeyStyle {
			get { return keyStyle; }
		}
	}

	public struct MiniGameResult {
		public MiniGameResult(MiniGameStart start, MiniGameFailed failed, MiniGameSuccess success, MiniGameEnd end) {
			miniGameStart = start;
			miniGameFailed = failed;
			miniGameSuccess = success;
			miniGameEnd = end;
		}

		private MiniGameStart miniGameStart;
		public MiniGameStart MiniGameStart {
			get { return miniGameStart; }
		}

		private MiniGameFailed miniGameFailed;
		public MiniGameFailed MiniGameFailed {
			get { return miniGameFailed; }
		}

		private MiniGameSuccess miniGameSuccess;
		public MiniGameSuccess MiniGameSuccess {
			get { return miniGameSuccess; }
		}

		private MiniGameEnd miniGameEnd;
		public MiniGameEnd MiniGameEnd {
			get { return miniGameEnd; }
		}
	}
}