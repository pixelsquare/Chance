using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
	# region Public Variables

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float orbitSpeed = 100f;
	[SerializeField]
	private float smoothOrbit = 0.3f;

	# endregion

	# region Private Variables

	private bool cameraOrbitEnable;
	private Camera cameraOrbit;

	private float angleOrbit;
	private float orbitVelocity;
	private Vector3 originalPosition;
	private Quaternion originalRotation;

	# endregion Private Variables

	# region Reset Variables

	private float _orbitSpeed;
	private float _smoothOrbit;

	# endregion Reset Variables

	// Public Properties
	public bool CameraOrbitEnable {
		get { return cameraOrbitEnable; }
		set {
			cameraOrbitEnable = value;
			cameraOrbit.enabled = value;
			if (value) {
				transform.position = originalPosition;
				transform.rotation = originalRotation;
			}
			enabled = value;
			gameObject.SetActive(value);
		}
	}
	//--

	private void Awake() {
		cameraOrbit = GetComponent<Camera>();
	}

	private void Start() {
		_orbitSpeed = orbitSpeed;
		_smoothOrbit = smoothOrbit;

		originalPosition = transform.position;
		originalRotation = transform.rotation;
		cameraOrbitEnable = gameObject.activeInHierarchy;

		angleOrbit = 0f;
		orbitVelocity = 0f;
	}

	private void Update() {
		if (cameraOrbitEnable) {
			if (cameraOrbit.pixelRect.Contains(Input.mousePosition)) {
				angleOrbit = Mathf.SmoothDamp(angleOrbit, Input.GetAxisRaw("Mouse ScrollWheel") * _orbitSpeed, ref orbitVelocity, _smoothOrbit);
				transform.RotateAround(target.position, Vector3.up, angleOrbit);
			}
		}
	}

	public void ResetCameraOrbit() {
		_orbitSpeed = orbitSpeed;
		_smoothOrbit = smoothOrbit;

		originalPosition = transform.position;
		originalRotation = transform.rotation;
		cameraOrbitEnable = gameObject.activeInHierarchy;

		angleOrbit = 0f;
		orbitVelocity = 0f;
	}
}