using UnityEngine;

public class BaseCamera : MonoBehaviour {
	private bool cameraEnabled;
	public bool CameraEnabled {
		get { return cameraEnabled; }
		set {
			cameraEnabled = value;
			camera.enabled = value;
			enabled = value;
			gameObject.SetActive(value);
		}
	}
}