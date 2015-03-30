using UnityEngine;

public class BaseCamera : MonoBehaviour {
	private bool enableCamera;
	public bool EnableCamera {
		get { return enableCamera; }
		set {
			enableCamera = value;
			gameObject.SetActive(value);
		}
	}
}