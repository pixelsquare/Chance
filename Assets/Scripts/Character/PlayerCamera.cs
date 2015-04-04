using GameUtilities.LayerManager;
using GameUtilities.PlayerUtility;
using UnityEngine;

public enum CameraState { OrbitFollow, Behind, FirstPerson }

public class PlayerCamera : BaseCamera {
	# region Public Variables

	[SerializeField]
	private float distanceAway = 5f;
	[SerializeField]
	private float distanceUp = 2f;
	[SerializeField]
	private Transform followTarget;
	[SerializeField]
	private Transform FPVCameraPos;
	[SerializeField]
	private float FPVThreshold = 0.5f;
	[SerializeField]
	private CameraState camState;
	[SerializeField]
	private float FPVRotationDegreePerSecond = 120f;

	// Camera Smoothing
	[SerializeField]
	private float cameraSmoothDampTime;
	[SerializeField]
	private float lookDirectionDampTime;

	# endregion Public Variables

	# region Private Variables

	private Vector3 cameraVelocitySmooth;
	private Vector3 targetPosition;

	private Vector3 lookDirection;
	private Vector3 curLookDirection;
	private Vector3 velocityLookDirection;

	private float fpvInput;
	private float horizontalInput;
	private float verticalInput;

	private PlayerControl playerControl;
	private GameManager gameManager;

	# endregion Private Variables

	# region Reset Variables

	private float _distanceAway;
	private float _distanceUp;
	private float _FPVThreshold;
	private CameraState _camState;
	private float _FPVRotationDegreePerSecond;
	private float _cameraSmoothDampTime;
	private float _lookDirectionDampTime;

	# endregion Reset Variables

	public CameraState CameraState {
		get { return _camState; }
	}

	private const float TARGETING_THRESHOLD = 0.01f;

	private void Start() {
		gameManager = GameManager.current;
		playerControl = followTarget.GetComponent<PlayerControl>();

		_distanceAway = distanceAway;
		_distanceUp = distanceUp;
		_FPVThreshold = FPVThreshold;
		_camState = camState;
		_FPVRotationDegreePerSecond = FPVRotationDegreePerSecond;
		_cameraSmoothDampTime = cameraSmoothDampTime;
		_lookDirectionDampTime = lookDirectionDampTime;

		cameraVelocitySmooth = Vector3.zero;
		targetPosition = Vector3.zero;

		lookDirection = followTarget.forward;
		curLookDirection = lookDirection;
		velocityLookDirection = Vector3.zero;

		fpvInput = 0f;
		verticalInput = 0f;
		horizontalInput = 0f;
	}

	private void LateUpdate() {
		if (gameManager.GameState == GameState.MainGame) {
			if (gameManager.BasePlayerData != null) {
				fpvInput = Input.GetAxis(PlayerUtility.FPVKey);
				horizontalInput = Input.GetAxis(PlayerUtility.Horizontal);
				verticalInput = -Input.GetAxis(PlayerUtility.Vertical);

				Vector3 characterOffset = followTarget.position + new Vector3(0f, _distanceUp, 0f);
				Vector3 lookAt = characterOffset;

				if (Input.GetButtonDown(PlayerUtility.FocusBehind) && UserInterface.InactiveUI()) {
					_camState = (_camState == CameraState.OrbitFollow) ? CameraState.Behind : CameraState.OrbitFollow;
				}
				else {
					if (fpvInput > _FPVThreshold && !playerControl.IsInLocomotion()) {
						_camState = CameraState.FirstPerson;
					}
					if (_camState == CameraState.FirstPerson && fpvInput < -TARGETING_THRESHOLD) {
						_camState = CameraState.OrbitFollow;
					}
				}

				if (_camState == CameraState.OrbitFollow) {
					if (playerControl.Speed > playerControl.LocomotionThreshold && playerControl.IsInLocomotion()) {
						lookDirection = Vector3.Lerp(followTarget.right * (horizontalInput < 0 ? 1f : -1f),
							followTarget.forward * (verticalInput < 0 ? -1f : 1f),
							Mathf.Abs(Vector3.Dot(transform.forward, followTarget.forward)));

						curLookDirection = Vector3.Normalize(characterOffset - transform.position);
						curLookDirection.y = 0f;

						curLookDirection = Vector3.SmoothDamp(curLookDirection, lookDirection, ref velocityLookDirection, _lookDirectionDampTime);
					}
					targetPosition = characterOffset + followTarget.up * distanceUp - Vector3.Normalize(curLookDirection) * distanceAway;
				}
				else if (_camState == CameraState.Behind) {
					lookDirection = followTarget.forward;
					curLookDirection = lookDirection;
					targetPosition = characterOffset + followTarget.up * _distanceUp - lookDirection * _distanceAway;
				}
				else if (_camState == CameraState.FirstPerson) {
					Quaternion rotationShift = Quaternion.FromToRotation(transform.forward, FPVCameraPos.forward);
					transform.rotation = rotationShift * transform.rotation;

					Vector3 rotationAmount = Vector3.Lerp(
						Vector3.zero, 
						new Vector3(0f, _FPVRotationDegreePerSecond * (horizontalInput < 0f ? -1f : 1f), 0f), 
						Mathf.Abs(horizontalInput)
					);

					Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
					followTarget.transform.rotation = (followTarget.transform.rotation * deltaRotation);

					targetPosition = FPVCameraPos.position;
					lookAt = Vector3.Lerp(targetPosition + followTarget.forward, transform.position + transform.forward, _cameraSmoothDampTime * Time.deltaTime);
					lookAt = Vector3.Lerp(transform.position + transform.forward, lookAt, Vector3.Distance(transform.position, FPVCameraPos.position));
				}

				CompensateForWalls(characterOffset, ref targetPosition);
				SmoothPosition(transform.position, targetPosition);
				transform.LookAt(lookAt);
			}
		}
	}

	private void SmoothPosition(Vector3 from, Vector3 to) {
		transform.position = Vector3.SmoothDamp(from, to, ref cameraVelocitySmooth, _cameraSmoothDampTime);
	}

	private void CompensateForWalls(Vector3 from, ref Vector3 to) {
		RaycastHit wallHit = new RaycastHit();
		int layers = (1 << LayerManager.LayerWalls | 1 << LayerManager.LayerGround | 1 << LayerManager.LayerObject | 1 << LayerManager.LayerTheatre);
		if (Physics.Linecast(from, to, out wallHit, layers)) {
			to = new Vector3(wallHit.point.x, to.y, wallHit.point.z);
		}
	}

	private void ResetPlayerCamera() {
		_distanceAway = distanceAway;
		_distanceUp = distanceUp;
		_FPVThreshold = FPVThreshold;
		_camState = camState;
		_FPVRotationDegreePerSecond = FPVRotationDegreePerSecond;
		_cameraSmoothDampTime = cameraSmoothDampTime;
		_lookDirectionDampTime = lookDirectionDampTime;

		cameraVelocitySmooth = Vector3.zero;
		targetPosition = Vector3.zero;

		lookDirection = followTarget.forward;
		curLookDirection = lookDirection;
		velocityLookDirection = Vector3.zero;

		fpvInput = 0f;
		verticalInput = 0f;
		horizontalInput = 0f;
	}
}