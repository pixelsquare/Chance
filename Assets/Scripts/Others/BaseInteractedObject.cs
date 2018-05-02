using UnityEngine;
using System.Collections;
using GameUtilities;
using GameUtilities.GUIDepth;

public class BaseInteractedObject : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Material hoveredMaterial;
	[SerializeField]
	protected  GUISkin tempSkin;
	[SerializeField]
	private float interactDistance = 3f;
	[SerializeField]
	private float interactAngle = 60f;

	# endregion Public Variables

	# region Private Variables

	protected Event e;
	protected Rect mainRect;
	protected float screenHeightRatio;
	protected float oldScreenHeightRatio;
	protected Color guiOriginalColor;

	protected bool ojbectEnabled;
	protected bool objectTooltipEnabled;
	protected Vector2 objectTooltipToScreen;

	private float distanceToPlayer;
	private float angleToPlayer;

	private GameObject interactedObject;

	private Shader[] normalShaders;
	private Color[] originalColors;

	protected GameManager gameManager;
	protected NPCManager npcManager;
	protected UserInterface userInterface;
	protected DialogueManager dialogueManager;
	protected PlayerInformation playerInformation;

	# endregion Private Variables

	// Public Property
	public bool ObjectEnabled {
		get { return ojbectEnabled; }
		set { 
			ojbectEnabled = value;
			GetComponent<Collider>().enabled = value;

			if (GetComponent<Renderer>() != null) {
				GetComponent<Renderer>().enabled = value;
			}

			enabled = value;
			gameObject.SetActive(value);
		}
	}
	// --

	protected virtual void Start() {
		gameManager = GameManager.current;
		npcManager = NPCManager.current;
		userInterface = UserInterface.current;
		dialogueManager = DialogueManager.current;

		mainRect = new Rect();
		screenHeightRatio = 0f;
		oldScreenHeightRatio = 0f;
		guiOriginalColor = new Color();

		ObjectEnabled = gameObject.activeInHierarchy;
		objectTooltipEnabled = false;
		objectTooltipToScreen = new Vector2();

		distanceToPlayer = 0f;
		angleToPlayer = 0f;

		if (GetComponent<Renderer>() != null) {
			//normalMaterial = renderer.material;
			//currentMaterialColor = renderer.material.color;
			//originalMaterialColor = currentMaterialColor;

			normalShaders = new Shader[GetComponent<Renderer>().materials.Length];
			originalColors = new Color[GetComponent<Renderer>().materials.Length];
			for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++) {
				normalShaders[i] = GetComponent<Renderer>().materials[i].shader;
				originalColors[i] = GetComponent<Renderer>().materials[i].color;
			}

			//if (hoveredMaterial != null) {
			//    for (int i = 0; i < renderer.materials.Length; i++) {
			//        renderer.materials[i].shader = hoveredMaterial.shader;
			//    }
			//}
		}
	}

	private void OnEnable() {
		BasePlayer.OnInteract += OnInteract;
	}

	private void OnDisable() {
		BasePlayer.OnInteract -= OnInteract;
	}

	protected virtual void Update() {
		if (gameManager.GameState == GameState.MainGame) {
			if (playerInformation == null && gameManager.BasePlayerData != null) {
				playerInformation = gameManager.BasePlayerData.PlayerInformation;
			}

			if (GetComponent<Renderer>() != null) {
				//// Change the material when hovered
				//renderer.material = (objectTooltipEnabled && hoveredMaterial != null) ? hoveredMaterial : normalMaterial;

				//// Interpolate color
				//currentMaterialColor = Color.Lerp(originalMaterialColor, Color.gray, Mathf.PingPong(Time.time, 1f) / 1f);
				//renderer.material.color = currentMaterialColor;

				for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++) {
					if (objectTooltipEnabled && hoveredMaterial != null) {
						GetComponent<Renderer>().materials[i].shader = hoveredMaterial.shader;
					}
					else {
						GetComponent<Renderer>().materials[i].shader = normalShaders[i];
					}

					GetComponent<Renderer>().materials[i].color = Color.Lerp(originalColors[i], Color.gray, Mathf.PingPong(Time.time, 1f) / 1f);
				}
			}

			GameUtility.FitOnScreen(ref screenHeightRatio, ref oldScreenHeightRatio, ref mainRect);

			if (gameManager.BasePlayerData != null) {
				distanceToPlayer = Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position);
				angleToPlayer = Vector3.Angle(gameManager.BasePlayerData.PlayerControl.PlayerRayPosition.forward,
					(transform.position - gameManager.BasePlayerData.PlayerT.position));

				objectTooltipEnabled = (distanceToPlayer < interactDistance && angleToPlayer < interactAngle) && (interactedObject == gameObject);
			}

			if (objectTooltipEnabled) {
				if (UserInterface.InactiveUI()) {
					objectTooltipToScreen = gameManager.BasePlayerData.PlayerControl.PCamera.WorldToScreenPoint(transform.position);
				}
			}
		}
	}

	protected virtual void OnGUI() {
		if (gameManager.GameState == GameState.MainGame) {
			GUI.depth = GUIDepth.npcMenuDepth;
		}
	}

	public virtual void ResetObject() {
		mainRect = new Rect();
		screenHeightRatio = 0f;
		oldScreenHeightRatio = 0f;
		guiOriginalColor = new Color();

		ojbectEnabled = gameObject.activeInHierarchy;
		objectTooltipEnabled = false;
		objectTooltipToScreen = new Vector2();

		distanceToPlayer = 0f;
		angleToPlayer = 0f;

		if (GetComponent<Renderer>() != null) {
			//renderer.material = normalMaterial;
			//currentMaterialColor = renderer.material.color;
			//originalMaterialColor = currentMaterialColor;

			for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++) {
				GetComponent<Renderer>().materials[i].shader = normalShaders[i];
				GetComponent<Renderer>().materials[i].color = originalColors[i];
			}
		}
	}

	private void OnInteract(GameObject go) {
		interactedObject = go;
	}
}