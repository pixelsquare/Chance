using System.Collections.Generic;
using GameUtilities.LayerManager;
using GameUtilities.PlayerUtility;
using UnityEngine;

// Every player must have these components
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInformation))]

public class BasePlayer : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform playerCameraT;
	[SerializeField]
	private Transform playerRayPos;
	[SerializeField]
	private Transform playerHead;
	[SerializeField]
	private float interactViewAngle = 60f;
	[SerializeField]
	private float interactRadius = 2f;
	[SerializeField]
	private float locomotionThreshold = 0.1f;

	# endregion Public Variables

	# region Private Variables

	protected Camera pCamera;
	protected Animator playerAnimator;
	protected AnimatorStateInfo playerStateInfo;
	protected PlayerCamera playerCamera;
	protected int locomotionID;

	private float viewAngle;
	private Collider[] sphereHit;
	private List<ObjectHit> objectHit;

	protected GameManager gameManager;
	protected UserInterface userInterface;

	# endregion Private Variables

	# region Reset Variables

	private float _interactViewAngle;
	private float _interactRadius;
	private float _locomotionThreshold;

	# endregion Reset Variables

	// Public Properties
	public Transform PlayerHead {
		get { return playerHead; }
	}

	public Transform PlayerRayPosition {
		get { return playerRayPos; }
	}

	public float InteractViewAngle {
		get { return _interactViewAngle; }
	}

	public Camera PCamera {
		get { return pCamera; }
	}

	public Animator PlayerAnimator {
		get { return playerAnimator; }
	}

	public float LocomotionThreshold {
		get { return _locomotionThreshold; }
	}
	// --

	public delegate void Interact(GameObject go);
	public static event Interact OnInteract;

	protected virtual void Awake() { }

	protected virtual void Start() {
        playerAnimator = GetComponent<Animator>();
		playerCamera = playerCameraT.GetComponent<PlayerCamera>();
		playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
		pCamera = playerCameraT.GetComponent<Camera>();
		locomotionID = Animator.StringToHash(PlayerUtility.AnimatorHashID);

		gameManager = GameManager.current;
		userInterface = UserInterface.current;

		// Reset all variables including those class
		// that inherits this base class
		Reset();

		if (playerAnimator.layerCount >= 2) {
			playerAnimator.SetLayerWeight(1, 1);
		}
	}

	protected virtual void Update() {
		if (gameManager.GameState == GameState.MainGame) {
			// Sphere checking for NPC, Item and Theatre
			sphereHit = Physics.OverlapSphere(transform.position, _interactRadius, 
				1 << LayerManager.LayerNPC | 1 << LayerManager.LayerItem | 1 << LayerManager.LayerTheatre);
			
			// Add them in Object Hit list
			objectHit = new List<ObjectHit>();
			if (sphereHit != null && objectHit != null) {
				for (int i = 0; i < sphereHit.Length; i++) {
					if (sphereHit[i] != null) {
						viewAngle = Vector3.Angle(playerRayPos.forward, (sphereHit[i].transform.localPosition - transform.position));
						if (viewAngle < _interactViewAngle) {
							objectHit.Add(new ObjectHit(sphereHit[i].gameObject, Vector3.Distance(transform.position, sphereHit[i].transform.position)));
						}
					}
				}

				// Sort list by their distance
				objectHit.Sort(SortDistance);

				// Get the nearest object
				if (sphereHit.Length > 0 && objectHit.Count > 0) {
					if (OnInteract != null && UserInterface.InactiveUI()) {
						OnInteract(objectHit[0].Obj);
					}
				}
			}
		}
	}

	protected virtual void FixedUpdate() { }

	protected virtual void Reset() {
		interactViewAngle = Mathf.Clamp(interactViewAngle, 0f, 360f);
		_interactViewAngle = interactViewAngle;
		_interactRadius = interactRadius;
		_locomotionThreshold = locomotionThreshold;

		viewAngle = 0f;
		sphereHit = null;
		objectHit = null;
	}

	private int SortDistance(ObjectHit a, ObjectHit b) {
		return a.Distance.CompareTo(b.Distance);
	}

	public bool IsInLocomotion() {
		return playerStateInfo.nameHash == locomotionID;
	}
}

public class ObjectHit {
	public ObjectHit(GameObject go, float dist) {
		obj = go;
		distance = dist;
	}

	private GameObject obj;
	public GameObject Obj {
		get { return obj; }
	}

	private float distance;
	public float Distance {
		get { return distance; }
		set { distance = value; }
	}
}