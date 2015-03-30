using System.Collections;
using GameUtilities.LayerManager;
using NPC;
using UnityEngine;

public enum NPCState { Idle, Wander, Interact }

public class NPCControl : BaseNPC {
	# region Public Variables

	[SerializeField]
	private NPCState npcState;
	[SerializeField]
	private float randomWithinRadius = 5f;
	[SerializeField]
	private Transform headPivot;
	[SerializeField]
	private float minHeadTurn = -90f;
	[SerializeField]
	private float maxHeadTurn = 90f;
	[SerializeField]
	private float headTurnDampTime = 3f;
	[SerializeField]
	private float lookDistance = 5f;
	[SerializeField]
	private float minChangeDirTime = 3f;
	[SerializeField]
	private float maxChangeDirTime = 5f;
	[SerializeField]
	private float interactDistance = 1.5f;
	[SerializeField]
	private float obstructionDistance = 1f;
	[SerializeField]
	private float sphereCastRadius = 1.5f;

	# endregion Public Variables

	# region Private Variables

	private NPCState curState;
	private bool isInteracting;
	private Vector3 targetPos;
	private Vector3 randPoint;
	private float speed;
	private float stateRandTime;
	private NavMeshAgent navAgent;
	private AngerGameUi miniGameUi;

	# endregion Private Variables

	# region Reset Variables

	private NPCState _npcState;
	private float _randomWithinRadius;
	private float _minHeadTurn;
	private float _maxHeadTurn;
	private float _headTurnDampTime;
	private float _lookDistance;
	private float _minChangeDirTime;
	private float _maxChangeDirTime;
	private float _interactDistance;
	private float _obstructionDistance;
	private float _sphereCastRadius;

	# endregion Reset Variables

	// Public Properties
	public Transform HeadPivot {
		get { return headPivot; }
	}

	public NPCState CurState {
		get { return curState; }
		set { curState = value; }
	}

	public Vector3 TargetPos {
		get { return targetPos; }
		set { targetPos = value; }
	}
	// --

	protected override void Awake() {
		base.Awake();
	}
	                                                           
	protected override void Start() {                  
		base.Start();
		navAgent = GetComponent<NavMeshAgent>(); 
		miniGameUi = AngerGameUi.current;
	}
	              
	protected override void Update() {
		base.Update();
		if (curState == NPCState.Idle) {
			IdleState();
		}
		else if (curState == NPCState.Wander) {
			WanderState();
		}
		else if (curState == NPCState.Interact) {
			InteractState();
		}
		npcAnim.SetFloat("Speed", speed);
	}

	private void IdleState() {
		LookToPlayer();
		if (speed > 0) {
			speed = 0;
			navAgent.Stop(true);
			curState = NPCState.Idle;
		}
	}

	private void WanderState() {
		headPivot.rotation = Quaternion.Lerp(headPivot.rotation, Quaternion.LookRotation(NpcRayPos.forward), _headTurnDampTime * Time.deltaTime);
		if (Vector3.Distance(targetPos, transform.position) > 1f) {
			speed = 1;
			ObstructionAvoidance(ref targetPos);
			targetPos.y = 0f;
			navAgent.SetDestination(targetPos);
			curState = NPCState.Wander;
		}
		else {
			if (speed > 0f) {
				curState = NPCState.Idle;
				stateRandTime = Random.Range(_minChangeDirTime, _maxChangeDirTime);
			}

			StartCoroutine("SetNewTargetPosition");
		}

		Debug.DrawLine(transform.position, targetPos, Color.red);
	}

	private void InteractState() {
		if (npcInformation.NpcNameID == gameManager.BasePlayerData.PlayerInformation.InteractingTo) {
			LookToPlayer();

			if (userInterface.GivingItem || dialogueManager.DialogueEnabled || miniGameUi.DangerModeEnabled) {
				Vector3 lookAt = gameManager.BasePlayerData.PlayerT.position - transform.position;
				// Rotate towards the player
				if (Vector3.Angle(transform.forward, lookAt) > 0f) {
					Quaternion rotationShift = Quaternion.LookRotation(lookAt);
					rotationShift.x = 0f;
					rotationShift.z = 0f;
					transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationShift, 5f);
				}

				if (!isInteracting) {
					// Walk towards the player
					if (Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position) > _interactDistance) {
						targetPos = gameManager.BasePlayerData.PlayerT.position;
						targetPos.y = 0f;
						speed = 1f;
						navAgent.SetDestination(targetPos);
					}
					else {
						speed = 0;
						isInteracting = true;
						navAgent.Stop(true);
						StopAllCoroutines();
					}
				}
			}
			else {
				// Reset the player's AI
				curState = _npcState;
				targetPos = transform.position;
				targetPos.y = 0f;
				randPoint = Vector3.zero;
				speed = 0f;
				isInteracting = false;
				gameManager.BasePlayerData.PlayerInformation.InteractingTo = NPCNameID.None;

				if (curState == NPCState.Wander) {
					stateRandTime = 0f;
					stateRandTime = GetRandomDirTime();
					StartCoroutine("SetNewTargetPosition");
				}
			}
		}
	}

	private IEnumerator SetNewTargetPosition() {
		yield return new WaitForSeconds(stateRandTime);

		if (npcAnim.GetFloat("Speed") < 0.01f) {
			randPoint = Random.insideUnitSphere * _randomWithinRadius;
			randPoint.y = 0f;
			targetPos = transform.position + randPoint;
			curState = NPCState.Wander;
		}

		StopCoroutine("SetNewTargetPosition");
	}

	private void ObstructionAvoidance(ref Vector3 targetPosition) {
		RaycastHit hit;
		if (Physics.SphereCast(transform.position, _sphereCastRadius, transform.forward, out hit, _obstructionDistance,
			1 << LayerManager.LayerWalls | 1 << LayerManager.LayerNPC | 1 << LayerManager.LayerObject | 
		    1 << LayerManager.LayerNina | 1 << LayerManager.LayerOwen)) {
				Vector3 inverseDir = Vector3.Reflect(hit.point - transform.position, hit.normal);
				targetPosition = transform.position + (inverseDir * Random.Range(1f, 5f));
				targetPosition.y = 0f;
				Debug.DrawLine(hit.point, targetPosition, Color.green);
		}

		//RaycastHit hit;
		//Debug.DrawLine(NpcRayPos.position, NpcRayPos.position + (NpcRayPos.forward * _obstructionDistance), Color.blue);
		//// Make the AI run the opposite of where it is facing. When one of this layers are detected by raycast
		//// this prevents the AI to make unecessary movements. thus, changing its path when bumping to something.
		//if (Physics.Raycast(NpcRayPos.position, NpcRayPos.position + NpcRayPos.forward, out hit, _obstructionDistance, 
		//    1 << LayerManager.LayerWalls | 1 << LayerManager.LayerNPC | 1 << LayerManager.LayerObject | 
		//    1 << LayerManager.LayerNina | 1 << LayerManager.LayerOwen)) {
		//    Vector3 inverseDir = Vector3.Reflect(hit.point - transform.position, hit.normal);
		//    targetPosition = transform.position + (inverseDir * 5f);
		//    targetPosition.y = 0f;
		//    Debug.DrawLine(hit.point, targetPosition, Color.green);
		//}
	}

	private void LookToPlayer() {
		# region raw angle calculation
			//Vector3 direction = gameManager.Player.position - transform.position;
			//Vector3 forward = transform.forward;
			//direction.Normalize();
			//forward.Normalize();

			//float angleRad = Vector3.Dot(forward, direction);
			//float angle = Mathf.Acos(angleRad) * Mathf.Rad2Deg;
		# endregion

		if (gameManager.BasePlayerData != null) {
			float angle = Vector3.Angle(transform.forward, (gameManager.BasePlayerData.PlayerT.position - transform.position));
			Vector3 headDir = gameManager.BasePlayerData.PlayerControl.PlayerHead.position - headPivot.position;

			if ((angle > _minHeadTurn && angle < _maxHeadTurn) && (Vector3.Distance(transform.position, gameManager.BasePlayerData.PlayerT.position) < _lookDistance)) {
				headPivot.rotation = Quaternion.Lerp(headPivot.rotation, Quaternion.LookRotation(headDir), _headTurnDampTime * Time.deltaTime);
			}
			else {
				headPivot.rotation = Quaternion.Lerp(headPivot.rotation, Quaternion.LookRotation(NpcRayPos.forward), _headTurnDampTime * Time.deltaTime);
			}
		}
	}

	private float GetRandomDirTime() {
		return Random.Range(_minChangeDirTime, _maxChangeDirTime);
	}

	public void RunPreResult() {
		npcAnim.SetFloat("Anger", 0.5f);
	}

	public void RunPostBadResult() {
		StartCoroutine(PostBadResult());
	}

	public IEnumerator PostBadResult() {
		float time = 0f;
		float anger = npcAnim.GetFloat("Anger");
		while (anger < 1f) {
			time += Time.deltaTime;
			anger = Mathf.Lerp(anger, 1f, time);
			npcAnim.SetFloat("Anger", anger);
			yield return null;
		}

		time = 0f;
		npcAnim.SetFloat("Anger", 0f);
	}

	public void RunPostGoodResult() {
		npcAnim.SetFloat("Anger", 0f);
	}

	protected override void Reset() {
		base.Reset();
		StopAllCoroutines();
		_npcState = npcState;
		_randomWithinRadius = randomWithinRadius;
		_minHeadTurn = minHeadTurn;
		_maxHeadTurn = maxHeadTurn;
		_headTurnDampTime = headTurnDampTime;
		_lookDistance = lookDistance;
		_minChangeDirTime = minChangeDirTime;
		_maxChangeDirTime = maxChangeDirTime;
		_interactDistance = interactDistance;
		_obstructionDistance = obstructionDistance;
		_sphereCastRadius = sphereCastRadius;

		curState = _npcState;
		targetPos = transform.position;
		randPoint = Vector3.zero;
		speed = 0f;
		stateRandTime = 0f;
		stateRandTime = GetRandomDirTime();
	}
}