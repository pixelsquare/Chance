using UnityEngine;

public class BaseCamera : MonoBehaviour {
	private bool cameraEnabled;
	public bool CameraEnabled {
		get { return cameraEnabled; }
		set {
			cameraEnabled = value;
			GetComponent<Camera>().enabled = value;
			enabled = value;
			gameObject.SetActive(value);
		}
	}
}