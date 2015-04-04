using GameUtilities.PlayerUtility;
using UnityEngine;

public class PlayerControl : BasePlayer {
	# region Public Variables

	[SerializeField]
	private float directionDampTime = 0.25f;
	[SerializeField]
	private float directionSpeed = 1.5f;
	[SerializeField]
	private float rotationDegreePerSecond = 120f;
	[SerializeField]
	private Transform keypressCameraInitialPos;
	[SerializeField]
	private Transform keypressPlayerT;
	[SerializeField]
	private Transform keypressNpcT;
	[SerializeField]
	private Transform danceOffCameraInitialPos;
	[SerializeField]
	private Transform danceOffCameraMainPos;
	[SerializeField]
	private Transform danceOffCameraNpcPos;
	[SerializeField]
	private Transform danceOffPlayerPos;
	[SerializeField]
	private Transform danceOffNpcPos;

	# endregion Public Variables

	# region Private Variables

	private float speed;
	private float direction;
	private bool controlEnabled;

	// Controls
	private float horizontalInput;
	private float verticalInput;
	private float sprint;

	private bool sprintLocked;
	private float keyLockTime = -1f;

	# endregion Private Variables

	# region Reset Variables

	private float _directionDampTime;
	private float _directionSpeed;
	private float _rotationDegreePerSecond;

	# endregion Reset Variables

	// Public Properties
	public float Speed {
		get { return speed; }
	}

	public float Direction {
		get { return direction; }
	}

	public bool ControlEnabled {
		get { return controlEnabled; }
		set {
			controlEnabled = value;
			enabled = value;
		}
	}

	public Transform KeypressCameraInitialPos {
		get { return keypressCameraInitialPos; }
	}

	public Transform KeypressPlayerT {
		get { return keypressPlayerT; }
	}

	public Transform KeypressNpcT {
		get { return keypressNpcT; }
	}

	public Transform DanceOffCameraInitialPos {
		get { return danceOffCameraInitialPos; }
	}

	public Transform DanceOffCameraMainPos {
		get { return danceOffCameraMainPos; }
	}

	public Transform DanceOffCameraNpcPos {
		get { return danceOffCameraNpcPos; }
	}

	public Transform DanceOffPlayerPos {
		get { return danceOffPlayerPos; }
	}

	public Transform DanceOffNpcPos {
		get { return danceOffNpcPos; }
	}
	// --

	protected override void Awake() {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();
		if (playerAnimator) {
			if (CanMove()) {
				playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
				horizontalInput = Input.GetAxis(PlayerUtility.Horizontal);
				verticalInput = Input.GetAxis(PlayerUtility.Vertical);
				//sprint = Input.GetAxis(PlayerUtility.Sprint);
				JoystickToWorldspace(transform, pCamera.transform, ref direction, ref speed);

				if (Input.GetButtonDown(PlayerUtility.Vertical) && verticalInput > 0.1f) {
					if (keyLockTime > 0) {
						sprintLocked = true;
						//sprint = 1f;
					}
					else {
						keyLockTime = 0.5f;
					}
				}

				if (keyLockTime > 0f) {
					keyLockTime -= Time.deltaTime;
				}

				if (sprintLocked && Input.GetButtonUp(PlayerUtility.Vertical)) {
					//sprint = 0f;
					keyLockTime = 0f;
					sprintLocked = false;
				}

				sprint = (sprintLocked) ? 1f : 0f;
			}
			else {
				horizontalInput = 0f;
				verticalInput = 0f;
				speed = 0f;
				direction = 0f;
			}

			// Make direction inverse when walking backwards
			horizontalInput *= (verticalInput < 0) ? -1 : 1;
			playerAnimator.SetFloat(PlayerUtility.Speed, speed);
			playerAnimator.SetFloat(PlayerUtility.Direction, horizontalInput, _directionDampTime, Time.deltaTime);
		}
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
		if (IsInLocomotion() && ((direction >= 0f && horizontalInput >= 0f) || (direction < 0f && horizontalInput < 0f))) {
			Vector3 rotationAmount = Vector3.Lerp(
				Vector3.zero,
				new Vector3(0f, _rotationDegreePerSecond * (horizontalInput < 0f ? -1f : 1f), 0f),
				Mathf.Abs(horizontalInput)
			);
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			transform.rotation = (transform.rotation * deltaRotation);
		}
		else {
			transform.Rotate(0f, horizontalInput * _rotationDegreePerSecond * Time.deltaTime, 0f);
		}
		
	}

	protected override void Reset() {
		base.Reset();
		_directionDampTime = directionDampTime;
		_directionSpeed = directionSpeed;
		_rotationDegreePerSecond = rotationDegreePerSecond;

		speed = 0f;
		sprint = 0f;
		direction = 0f;
		horizontalInput = 0f;
		controlEnabled = false;
	}

	private void JoystickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut) {
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(horizontalInput * 0.5f, 0f, verticalInput * 0.75f);
		sprint = (stickDirection.sqrMagnitude > 0f) ? (sprint * 1.75f) : 0f;
		speed = ((verticalInput < 0f) ? verticalInput : (stickDirection.sqrMagnitude) + sprint);

		Vector3 cameraDirection = camera.forward;
		cameraDirection.y = 0f;
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		angleRootToMove /= 180f;
		directionOut = angleRootToMove * _directionSpeed;
	}

	private bool CanMove() {
		return controlEnabled && UserInterface.InactiveUI();
	}

	//public void SwitchToDialogueCamera() {
	//    dialogueCamera.gameObject.SetActive(true);
	//    pCamera.transform.position = dialogueCamera.transform.position;
	//    pCamera.transform.rotation = dialogueCamera.transform.rotation;
	//}

	//public void EndDialogueCamera() {
	//    dialogueCamera.gameObject.SetActive(false);
	//}
}