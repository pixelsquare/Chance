using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	# region Private Variables

	private AudioSource audioSource;

	# endregion Private Variables

	private void OnEnable() {
		audioSource = GetComponent<AudioSource>();
	}

	public void RunAudioSourceUpdate() {
		gameObject.SetActive(true);
		StartCoroutine(AudioSourceUpdate());
	}

	private IEnumerator AudioSourceUpdate() {
		yield return new WaitForSeconds(audioSource.clip.length);
		gameObject.SetActive(false);
	}
}