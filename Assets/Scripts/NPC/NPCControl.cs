using System.Collections;
using GameUtilities.LayerManager;
using NPC;
using UnityEngine;

public enum NPCState { Idle, Wander, Interact, MiniGame }

public class NPCControl : BaseNPC {
	# region Public Variables

	[SerializeField]
	private NPCState npcState;
	[SerializeField]
	private Transform headPivot;
	[SerializeField]
	private float randomWithinRadius = 5f;
	[SerializeField]
	private float minHeadTurn = -15f;
	[SerializeField]
	private float maxHeadTurn = 15f;
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
	private float sphereCastRadius = 0.5f;

	# endregion Public Variables

	# region Private Variables

	private NPCState curState;
	private bool isInteracting;
	private float speed;
	private float stateRandTime;
	private Vector3 targetPos;
	private Vector3 randPoint;
	private BoxCollider firstFloor;
	private BoxCollider secondFloor;
	private UnityEngine.AI.NavMeshAgent navAgent;

	private int randomLocationIndx;
	private bool newDirection;
	private bool aiHasStarted;

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
	public NPCState CurState {
		get { return curState; }
		set { curState = value; }
	}
	// --

	protected override void Awake() {
		base.Awake();
	}
	                                                           
	protected override void Start() {                  
		base.Start();
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
	}
	              
	protected override void Update() {
		base.Update();
		if (aiHasStarted) {
			if (curState == NPCState.Idle) {
				IdleState();
			}
			else if (curState == NPCState.Wander) {
				WanderState();
			}
			else if (curState == NPCState.Interact) {
				InteractState();
			}
			else if (curState == NPCState.MiniGame) {
				MiniGameState();
			}
			npcAnim.SetFloat("Speed", speed);
		}
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
			ObstructionAvoidance(ref targetPos);
			speed = 1;
			navAgent.SetDestination(targetPos);
			curState = NPCState.Wander;
		}
		else {
			if (speed > 0f) {
				curState = NPCState.Idle;
			}

			if (!newDirection) {
				StartCoroutine("SetNewTargetPosition");
				newDirection = true;
			}
		}

		Debug.DrawLine(transform.position, targetPos, Color.red);
	}

	private void InteractState() {
		if (npcInformation.BaseNpcData.NpcNameID == gameManager.BasePlayerData.PlayerInformation.InteractingTo) {
			LookToPlayer();

			if (userInterface.GivingItem || dialogueManager.DialogueEnabled || curState == NPCState.MiniGame) {
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

	private void MiniGameState() {
		LookToPlayer();
		if (speed > 0) {
			speed = 0;
			navAgent.Stop(true);
			StopCoroutine("SetNewTargetPosition");
			curState = NPCState.MiniGame;
		}
	}

	public void InitializeNpcControl(Vector3 position, Quaternion rotation, BoxCollider firstFlr, BoxCollider secondFlr) {
		transform.position = position;
		transform.rotation = rotation;
		targetPos = transform.position;
		firstFloor = firstFlr;
		secondFloor = secondFlr;
		aiHasStarted = true;
	}

	private IEnumerator SetNewTargetPosition() {
		stateRandTime = Random.Range(_minChangeDirTime, _maxChangeDirTime);
		yield return new WaitForSeconds(stateRandTime);

		if (npcAnim.GetFloat("Speed") < 0.01f) {
			System.Random rng = new System.Random();
			randomLocationIndx = rng.Next(0, 100);
			if (randomLocationIndx >= 0 && randomLocationIndx <= 25) {
				randPoint.x = Random.Range(firstFloor.bounds.min.x, firstFloor.bounds.max.x);
				randPoint.y = firstFloor.transform.position.y;
				randPoint.z = Random.Range(firstFloor.bounds.min.z, firstFloor.bounds.max.z);
				targetPos = randPoint;
			}
			else if(randomLocationIndx > 25 && randomLocationIndx <= 50) {
				randPoint.x = Random.Range(secondFloor.bounds.min.x, secondFloor.bounds.max.x);
				randPoint.y = secondFloor.transform.position.y;
				randPoint.z = Random.Range(secondFloor.bounds.min.z, secondFloor.bounds.max.z);
				targetPos = randPoint;
			}
			else if (randomLocationIndx > 50) {
				randPoint = Random.insideUnitSphere * _randomWithinRadius;
				randPoint.y = 0f;
				targetPos = transform.position + randPoint;
			}

			curState = NPCState.Wander;
		}

		newDirection = false;
		yield return null;
	}

	private void ObstructionAvoidance(ref Vector3 targetPosition) {
		RaycastHit hit;
		if (Physics.SphereCast(NpcRayPos.position, _sphereCastRadius, transform.forward, out hit, _obstructionDistance,
			1 << LayerManager.LayerWalls | 1 << LayerManager.LayerNPC | 1 << LayerManager.LayerObject |
			1 << LayerManager.LayerNina | 1 << LayerManager.LayerOwen | 1 << LayerManager.LayerNpcInteracted)) {
			Vector3 inverseDir = Vector3.Reflect(hit.point - transform.position, hit.normal);
			targetPosition = transform.position + (inverseDir * Random.Range(1f, 5f));
			targetPosition.y = 0f;
			Debug.DrawLine(hit.point, targetPosition, Color.green);
		}
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

		if (gameManager.BasePlayerData != null && gameManager.BasePlayerData.PlayerT != null) {
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

	public void RunAnimationPreResult() {
		npcAnim.SetFloat("Anger", 0.5f);
	}

	public void RunAnimationBadResult() {
		StartCoroutine(AnimationBadResult());
	}

	public IEnumerator AnimationBadResult() {
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

	public void RunAnimationGoodResult() {
		npcAnim.SetFloat("Anger", 0f);
	}

	public override void Reset() {
		base.Reset();
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

		isInteracting = false;
		curState = _npcState;
		targetPos = transform.position;
		randPoint = Vector3.zero;
		speed = 0f;
		stateRandTime = 0f;
		stateRandTime = GetRandomDirTime();
		if (curState == NPCState.Wander) {
			StartCoroutine("SetNewTargetPosition");
		}
	}
}