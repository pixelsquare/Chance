using System.Collections.Generic;
using UnityEngine;

namespace GameUtilities {
	// General Utilities for the game
	public class GameUtility {
		public static Vector3 GetWorldScale(Transform root) {
			Vector3 worldScale = root.localScale;
			Transform tmpParent = root.parent;

			while (tmpParent != null) {
				worldScale = Vector3.Scale(worldScale, tmpParent.localScale);
				tmpParent = tmpParent.parent;
			}

			return worldScale;
		}

		// Converts milliseconds to HHMMSS format
		public static string ToHHMMSS(float ms) {
			int sec = (int)((ms % 3600f) % 60f);
			string seconds = (sec < 10) ? ("0" + sec) : (string.Empty + sec);

			int min = (int)((ms % 3600f) / 60f);
			string minutes = (min < 10) ? ("0" + min) : (string.Empty + min);

			//int hr = (int)(ms / 3600);
			//string hours = (hr < 10) ? ("0" + hr) : (string.Empty + hr);

			return minutes + ":" + seconds;
		}

		// Recursively look for renderers in a gameobject to enable or disable
		public static void SetRendererActiveRecursively(Transform root, bool active) {
			foreach (Transform obj in root) {
				if (obj.renderer != null) {
					obj.renderer.enabled = active;
				}
				SetRendererActiveRecursively(obj, active);
			}
		}

		// Recursively change gameobject layer
		public static void SetGameObjectLayerRecursively(Transform root, int layer) {
			foreach (Transform obj in root) {
				if (obj != null) {
					obj.gameObject.layer = layer;
				}
				SetGameObjectLayerRecursively(obj, layer);
			}
			root.gameObject.layer = layer;
		}

		// Recursively look for Transform's children
		public static void GetChildrenRecursively(Transform root, ref List<Transform> children) {
			foreach (Transform obj in root) {
				if (obj != null) {
					children.Add(obj);
				}
				GetChildrenRecursively(obj, ref children);
			}
		}

		// Update GUI Scale and position when window is resized
		public static void FitOnScreen(ref float screenHeightRatio, ref float oldScreenHeightRatio, ref Rect mainRect) {
			if (screenHeightRatio == 0) {
				screenHeightRatio = (float)Screen.width / Screen.height;
			}
			
			if (screenHeightRatio != oldScreenHeightRatio) {
				screenHeightRatio = (float)Screen.width / Screen.height;
				oldScreenHeightRatio = screenHeightRatio;
			}

			if (mainRect.width != Screen.width && mainRect.height != (Screen.height * screenHeightRatio)) {
				mainRect = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width, Screen.height * screenHeightRatio);
				AnchorPoint.AnchorPoint.SetAnchor(ref mainRect, AnchorPoint.Anchor.MiddleCenter);
			}
		}
	}

	// Used as reference for layers in gameobjects
	// Used in casting rays or detecting collisions
	namespace LayerManager {
		public class LayerManager {
			private static int layerPlayer = 8;
			public static int LayerPlayer {
				get { return layerPlayer; }
			}

			private static int layerNPC = 9;
			public static int LayerNPC {
				get { return layerNPC; }
			}

			private static int layerWalls = 10;
			public static int LayerWalls {
				get { return layerWalls; }
			}

			private static int layerItem = 11;
			public static int LayerItem {
				get { return layerItem; }
			}

			private static int layerNina = 12;
			public static int LayerNina {
				get { return layerNina; }
			}

			private static int layerOwen = 13;
			public static int LayerOwen {
				get { return layerOwen; }
			}

			private static int layerObject = 14;
			public static int LayerObject {
				get { return layerObject; }
			}

			private static int layerTheatre = 15;
			public static int LayerTheatre {
				get { return layerTheatre; }
			}

			private static int layerNpcInteracted = 16;
			public static int LayerNpcInteracted {
				get { return layerNpcInteracted; }
			}

			private static int layerNpcWanderer = 17;
			public static int LayerNpcWanderer {
				get { return layerNpcWanderer; }
			}

			private static int layerGround = 31;
			public static int LayerGround {
				get { return layerGround; }
			}
		}
	}

	// Reference variables for player's input and animation
	namespace PlayerUtility {
		public class PlayerUtility {
			public static string Horizontal = "Horizontal";
			public static string Vertical = "Vertical";
			public static string Speed = "Speed";
			public static string Direction = "Direction";
			public static string AnimatorHashID = "Base Layer.Locomotion";

			public static string FPVKey = "FPV Key";
			public static string FocusBehind = "Focus Behind";
			public static string Interact = "Interact";
			public static string GiveItem = "Give Item";
			public static string ItemPickup = "Item Pickup";
			public static string PlayerProfile = "Player Profile";
			public static string Inventory = "Inventory";
			public static string Settings = "Settings";
			public static string ItemSwap = "Item Swap";
			public static string SelectItem = "Select Item";
			public static string CancelSelectedItem = "Cancel Selected Item";
			public static string EnterTheatre = "Enter Theatre";
			public static string JoinGameJam = "Join Game Jam";
			public static string NextDialogue = "Next Dialogue";
			public static string ExitWindow = "Exit Window";
			public static string DialogueButton1 = "Dialogue Button 1";
			public static string DialogueButton2 = "Dialogue Button 2";
			public static string DialogueButton3 = "Dialogue Button 3";
			public static string Sprint = "Sprint";
		}
	}
	
	// Reference variables for npc animation and emoticons
	namespace NpcUtility {
		public class NpcUtility {
			public static string EmoticonHashID = "Base Layer.None";
			public static string EmoticonHappy = "Happy";
			public static string EmoticonSad = "Sad";
		}
	}

	// Mainly used for GUI's
	// Used to change the anchor point of Rect class
	namespace AnchorPoint {
		public enum Anchor {
			TopLeft, TopCenter, TopRight,
			MiddleLeft, MiddleCenter, MiddleRight,
			BottomLeft, BottomCenter, BottomRight
		}

		public class AnchorPoint {
			public static void SetAnchor(ref Rect rect, Anchor anchor) {
				if (anchor == Anchor.TopLeft) {
					rect.Set(rect.x, rect.y, rect.width, rect.height);
				}
				else if (anchor == Anchor.TopCenter) {
					rect.Set(rect.x - (rect.width * 0.5f), rect.y, rect.width, rect.height);
				}
				else if (anchor == Anchor.TopRight) {
					rect.Set(rect.x - rect.width, rect.y, rect.width, rect.height);
				}
				else if (anchor == Anchor.MiddleLeft) {
					rect.Set(rect.x, rect.y - (rect.height * 0.5f), rect.width, rect.height);
				}
				else if (anchor == Anchor.MiddleCenter) {
					rect.Set(rect.x - (rect.width * 0.5f), rect.y - (rect.height * 0.5f), rect.width, rect.height);
				}
				else if (anchor == Anchor.MiddleRight) {
					rect.Set(rect.x - rect.width, rect.y - (rect.height * 0.5f), rect.width, rect.height);
				}
				else if (anchor == Anchor.BottomLeft) {
					rect.Set(rect.x, rect.y - rect.height, rect.width, rect.height);
				}
				else if (anchor == Anchor.BottomCenter) {
					rect.Set(rect.x - (rect.width * 0.5f), rect.y - rect.height, rect.width, rect.height);
				}
				else if (anchor == Anchor.BottomRight) {
					rect.Set(rect.x - rect.width, rect.y - rect.height, rect.width, rect.height);
				}
			}
		}
	}

	// Progress bar and Slider
	namespace GameGUI {
		public class GameGUI {
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
				AnchorPoint.AnchorPoint.SetAnchor(ref leftRect, AnchorPoint.Anchor.MiddleLeft);
				//GUI.Box(leftRect, string.Empty);

				GUI.BeginGroup(new Rect(0f, 0f, bgRect.width * value, bgRect.height));
				Rect leftValRect = new Rect(0f, leftRect.height * 0.5f, leftRect.width, leftRect.height);
				AnchorPoint.AnchorPoint.SetAnchor(ref leftValRect, AnchorPoint.Anchor.MiddleLeft);
				GUI.Box(leftValRect, string.Empty, leftStyle);
				GUI.EndGroup();
				# endregion Left Slider

				# region Right Slider
				Rect rightRect = new Rect(bgRect.width, bgRect.height * 0.5f, bgRect.width, bgRect.height);
				AnchorPoint.AnchorPoint.SetAnchor(ref rightRect, AnchorPoint.Anchor.MiddleRight);
				//GUI.Box(rightRect, string.Empty);

				GUI.BeginGroup(new Rect(rightRect.width * value, 0f, rightRect.width, rightRect.height));
				Rect rightValRect = new Rect(rightRect.width * (1 - value), rightRect.height * 0.5f, rightRect.width, rightRect.height);
				AnchorPoint.AnchorPoint.SetAnchor(ref rightValRect, AnchorPoint.Anchor.MiddleRight);
				GUI.Box(rightValRect, string.Empty, rightStyle);
				GUI.EndGroup();
				# endregion Right Slider

				GUI.EndGroup();
				GUI.Box(bgRect, name, textStyle);
			}

			public static void HotkeyBox(Rect bgRect, float xOffset, string key, string keyText, GUIStyle textSkin) {
				GUI.BeginGroup(bgRect);
				Rect keyRect = new Rect(bgRect.width * 0.2f, bgRect.height * 0.5f, bgRect.width * 0.3f, bgRect.height * 0.9f);
				AnchorPoint.AnchorPoint.SetAnchor(ref keyRect, AnchorPoint.Anchor.MiddleCenter);
				GUI.Box(keyRect, "[" + key + "]", textSkin);

				Rect keyTextRect = new Rect(bgRect.width * xOffset, bgRect.height * 0.5f, bgRect.width * 0.6f, bgRect.height * 0.9f);
				AnchorPoint.AnchorPoint.SetAnchor(ref keyTextRect, AnchorPoint.Anchor.MiddleCenter);
				GUI.Box(keyTextRect, keyText, textSkin);

				GUI.EndGroup();
			}
		}

		public class GUIDepth {
			public static int transitionDepth = 0;
			public static int dialogueDepth = 1;
			public static int userInterfaceDepth = 2;
			public static int npcMenuDepth = 3;
		}
	}

	// Used for creating and parenting a gameobject
	namespace Xform {
		public class XForm {
			private Vector3 position;
			private Transform transform;

			public XForm(string name, Vector3 pos, Transform trans, Transform parent) {
				position = pos;
				transform = trans;
				transform.name = name;
				transform.parent = parent;
				transform.localPosition = position;
			}
		}
	}
}