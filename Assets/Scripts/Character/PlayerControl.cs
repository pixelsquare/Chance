using GameUtilities.LayerManager;
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
	private Camera dialogueCamera;

	# endregion Public Variables

	# region Private Variables

	private float speed;
	private float direction;
	private bool enableControl;
	private bool isGrounded;

	// Controls
	private float horizontalInput;
	private float verticalInput;
	private float sprint;

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

	public bool EnableControl {
		get { return enableControl; }
		set { enableControl = value; }
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
				sprint = Input.GetAxis(PlayerUtility.Sprint);
				JoystickToWorldspace(transform, PlayerCamera.transform, ref direction, ref speed);
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
		speed = 0f;
		direction = 0f;
		horizontalInput = 0f;
		verticalInput = 0f;

		_directionDampTime = directionDampTime;
		_directionSpeed = directionSpeed;
		_rotationDegreePerSecond = rotationDegreePerSecond;
	}

	public void SwitchToDialogueCamera() {
		dialogueCamera.gameObject.SetActive(true);
		pCamera.transform.position = dialogueCamera.transform.position;
		pCamera.transform.rotation = dialogueCamera.transform.rotation;
	}

	public void EndDialogueCamera() {
		dialogueCamera.gameObject.SetActive(false);
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
		return enableControl && userInterface.InactiveUI();
	}
}