using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioNameID {
	None, MainMenuBGM, MainGameBGM, KeypressBGM, HideNSeekBGM, GameStoryBGM, HappyEnding, SadEnding, DanceOffBGM1, DanceOffBGM2,
	DanceOffCorrectBtn, DanceOffWrongBtn, GeneralBtn, HideNSeekSuccess, KeypressBtn1, KeypressBtn2, KeypressWrongBtn, MissionSuccess,
	MissionFailed, ItemPickup
}

public class AudioManager : MonoBehaviour {

	# region Public Variables

	[SerializeField]
	private AudioPool[] audioPool;

	# endregion Public Variables

	public static AudioManager current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		if (audioPool != null) {
			for (int i = 0; i < audioPool.Length; i++) {
				GameObject audioObj = new GameObject();
				audioObj.name = audioPool[i].AudioClip.name + " Parent";
				audioObj.transform.parent = transform;
				audioPool[i].Parent = audioObj.transform;

				audioPool[i].AudioPooled = new List<AudioSource>();
				for (int j = 0; j < audioPool[i].PooledAmount; j++) {
					GameObject obj = new GameObject();
					obj.name = audioPool[i].AudioClip.name;
					obj.transform.parent = audioObj.transform;
					AudioSource audioSource = obj.AddComponent<AudioSource>();
					audioSource.clip = audioPool[i].AudioClip;
					audioSource.gameObject.SetActive(false);
					audioPool[i].AudioPooled.Add(audioSource);
					obj.AddComponent<Audio>();
				}
			}
		}
	}

	public AudioSource GetAudioSource(AudioNameID nameID, bool autoDestruct) {
		AudioPool audio = GetAudioPool(nameID);

		for (int i = 0; i < audio.AudioPooled.Count; i++) {
			if (!audio.AudioPooled[i].gameObject.activeInHierarchy) {
				Audio audioScript = audio.AudioPooled[i].GetComponent<Audio>();
				if (autoDestruct) {
					audioScript.RunAudioSourceUpdate();
				}

				return audio.AudioPooled[i];
			}
		}
		
		if (audio.CanGrow) {
			GameObject obj = new GameObject();
			obj.name = audio.AudioClip.name;
			obj.transform.parent = audio.Parent;
			AudioSource audioSource = obj.AddComponent<AudioSource>();
			audioSource.clip = audio.AudioClip;
			audioSource.Play();

			audio.AudioPooled.Add(audioSource);
			Audio audioScript = obj.AddComponent<Audio>();
			if (autoDestruct) {
				audioScript.RunAudioSourceUpdate();
			}

			return audioSource;
		}

		return null;
	}

	public Audio GetAudio(AudioNameID nameID) {
		for (int i = 0; i < audioPool.Length; i++) {
			if (audioPool[i].AudioNameID == nameID) {
				return audioPool[i].AudioPooled[0].GetComponent<Audio>();
			}
		}

		return null;
	}

	private AudioPool GetAudioPool(AudioNameID nameID) {
		for (int i = 0; i < audioPool.Length; i++) {
			if (audioPool[i].AudioNameID == nameID) {
				return audioPool[i];
			}
		}
		return null;
	}
}

[System.Serializable]
public class AudioPool {
	[SerializeField]
	private AudioNameID audioNameID;
	public AudioNameID AudioNameID {
		get { return audioNameID; }
	}

	[SerializeField]
	private AudioClip audioClip;
	public AudioClip AudioClip {
		get { return audioClip; }
	}

	[SerializeField]
	private int pooledAmount = 1;
	public int PooledAmount {
		get { return pooledAmount; }
	}

	[SerializeField]
	private bool canGrow;
	public bool CanGrow {
		get { return canGrow; }
	}

	private Transform parent;
	public Transform Parent {
		get { return parent; }
		set { parent = value; }
	}

	private List<AudioSource> audioPooled;
	public List<AudioSource> AudioPooled {
		get { return audioPooled; }
		set { audioPooled = value; }
	}
}