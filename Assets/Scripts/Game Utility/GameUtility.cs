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

			if (root.renderer != null) {
				root.renderer.enabled = active;
			}
		}

		// Recursively change the material of an object
		public static void SetRendererMaterialRecursively(Transform root, Material material) {
			foreach (Transform obj in root) {
				if (obj.renderer != null && obj.renderer.material != null) {
					obj.renderer.material = material;
				}
				SetRendererMaterialRecursively(obj, material);
			}

			if (root.renderer != null && root.renderer.material != null) {
				root.renderer.material = material;
			}
		}

		// Recursively change the color of the material
		public static void SetRendererMaterialColorRecursively(Transform root, Color color) {
			foreach (Transform obj in root) {
				if (obj.renderer != null && obj.renderer.material != null) {
					obj.renderer.material.color = color;
				}
				SetRendererMaterialColorRecursively(obj, color);
			}

			if (root.renderer != null && root.renderer.material != null) {
				root.renderer.material.color = color;
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

		public static Transform TransformCopy(Transform t1, Transform t2) {
			Transform t = t1;
			t.position = t2.position;
			t.rotation = t2.rotation;

			//t.localPosition = t2.localPosition;
			//t.localRotation = t2.localRotation;
			//t.localScale = t2.localScale;

			//t.localEulerAngles = t2.localEulerAngles;

			return t;
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
			public static string ArrowVertical = "Arrow Vertical";
			public static string ArrowHorizontal = "Arrow Horizontal";
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
	namespace GUIDepth {
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