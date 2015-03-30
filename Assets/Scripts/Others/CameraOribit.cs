using UnityEngine;
using System.Collections;

public class CameraOribit : MonoBehaviour {
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
			gameObject.SetActive(value);
		}
	}
	//--

	private void Awake() {
		cameraOrbit = GetComponent<Camera>();
	}

	private void Start() {
		originalPosition = transform.position;
		originalRotation = transform.rotation;
		cameraOrbitEnable = gameObject.activeInHierarchy;
	}

	private void Update() {
		if (cameraOrbitEnable) {
			if (cameraOrbit.pixelRect.Contains(Input.mousePosition)) {
				angleOrbit = Mathf.SmoothDamp(angleOrbit, Input.GetAxisRaw("Mouse ScrollWheel") * orbitSpeed, ref orbitVelocity, smoothOrbit);
				transform.RotateAround(target.position, Vector3.up, angleOrbit);
			}
		}
	}
}