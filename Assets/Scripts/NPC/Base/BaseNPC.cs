using GameUtilities.LayerManager;
using UnityEngine;

[RequireComponent(typeof(NPCInformation))]
[RequireComponent(typeof(NavMesh))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class BaseNPC : MonoBehaviour {
	# region Public Variables

	[SerializeField]
	private Transform npcRayPos;

	# endregion Public Variables

	# region Private Variables

	protected Animator npcAnim;
	protected AnimatorStateInfo npcAnimStateInfo;
	protected NPCInformation npcInformation;

	protected GameManager gameManager;
	protected DialogueManager dialogueManager;
	protected UserInterface userInterface;

	# endregion Private Variables

	// Public Properties
	public Transform NpcRayPos {
		get { return npcRayPos; }
	}

	public Animator npcAnimator {
		get { return npcAnim; }
	}

	public AnimatorStateInfo NpcAnimStateInfo {
		get {
			npcAnimStateInfo = npcAnim.GetCurrentAnimatorStateInfo(0);
			return npcAnimStateInfo;
		}
		set { npcAnimStateInfo = value; }
	}

	public NPCInformation NpcInformation {
		get { return npcInformation; }
	}
	// --

	protected virtual void Awake() {
		npcInformation = GetComponent<NPCInformation>();
	}

	private void OnEnable() {
		Reset();
	}

	protected virtual void Start() {
		gameManager = GameManager.current;
		dialogueManager = DialogueManager.current;
		userInterface = UserInterface.current;   

		npcAnim = GetComponent<Animator>();
		gameObject.layer = LayerManager.LayerNPC;

		if (npcAnim.layerCount >= 2) {
			npcAnim.SetLayerWeight(1, 1);
		}
	}

	protected virtual void Update() { }

	protected virtual void Reset() {
		npcInformation.Reset();
	}
}