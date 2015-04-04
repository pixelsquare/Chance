using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ParticleNameID {
	None, KeypressExplosion
};

public class ParticleManager : MonoBehaviour {
	[SerializeField]
	private Camera particleCamera;

	[SerializeField]
	private ParticlePool[] particlePooled;

	public Camera ParticleCamera {
		get { return particleCamera; }
	}

	public static ParticleManager current;

	private void Awake() {
		current = this;
	}

	private void Start() {
		if (particlePooled != null) {
			for (int i = 0; i < particlePooled.Length; i++) {
				GameObject particleObj = new GameObject();
				particleObj.name = particlePooled[i].Particle.name + " Parent";
				particleObj.transform.parent = transform;
				particlePooled[i].Parent = particleObj.transform;

				particlePooled[i].PooledParticle = new List<ParticleSystem>();
				for (int j = 0; j < particlePooled[i].PooledAmount; j++) {
					ParticleSystem particle = (ParticleSystem)Instantiate(particlePooled[i].Particle);
					particle.playOnAwake = true;
					particle.gameObject.SetActive(false);
					particle.name = particlePooled[i].Particle.name;
					particle.transform.parent = particlePooled[i].Parent;
					particlePooled[i].PooledParticle.Add(particle);
				}
			}
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			ParticleSystem particle = GetPooledParticle(ParticleNameID.KeypressExplosion);
			particle.gameObject.SetActive(true);
		}

		if (particlePooled != null) {
			for (int i = 0; i < particlePooled.Length; i++) {
				particlePooled[i].Update();
			}
		}
	}

	public ParticleSystem GetPooledParticle(ParticleNameID nameID) {
		ParticlePool particlePool = GetParticlePool(nameID);
		for (int i = 0; i < particlePool.PooledParticle.Count; i++) {
			if (!particlePool.PooledParticle[i].gameObject.activeInHierarchy) {
				return particlePool.PooledParticle[i];
			}
		}

		if (particlePool.CanGrow) {
			ParticleSystem particle = (ParticleSystem)Instantiate(particlePool.Particle);
			particle.playOnAwake = true;
			particle.gameObject.SetActive(false);
			particle.name = particlePool.Particle.name;
			particle.transform.parent = particlePool.Parent;
			particlePool.PooledParticle.Add(particle);
			return particle;
		}

		return null;
	}

	public void ParticlePoolReset(ParticleNameID nameID) {
		ParticlePool particlePool = GetParticlePool(nameID);
		for (int i = 0; i < particlePool.PooledParticle.Count; i++) {
			if (particlePool.PooledParticle[i].gameObject.activeInHierarchy) {
				particlePool.PooledParticle[i].gameObject.SetActive(false);
			}
		}
	}

	private ParticlePool GetParticlePool(ParticleNameID nameID) {
		for (int i = 0; i < particlePooled.Length; i++) {
			if (particlePooled[i].ParticleNameID == nameID) {
				return particlePooled[i];
			}
		}
		return null;
	}
}

[System.Serializable]
public class ParticlePool {
	[SerializeField]
	private ParticleNameID particleNameID;
	public ParticleNameID ParticleNameID {
		get { return particleNameID; }
	}

	[SerializeField]
	private ParticleSystem particle;
	public ParticleSystem Particle {
		get { return particle; }
	}

	[SerializeField]
	private int pooledAmount = 10;
	public int PooledAmount {
		get { return pooledAmount; }
		set { pooledAmount = value; }
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

	private List<ParticleSystem> pooledParticle;
	public List<ParticleSystem> PooledParticle {
		get { return pooledParticle; }
		set { pooledParticle = value; }
	}

	public void Update() {
		for (int i = 0; i < pooledParticle.Count; i++) {
			if (!pooledParticle[i].IsAlive()) {
				// Move it away from the camera
				pooledParticle[i].transform.position = new Vector3(5f, 5f, 5f);
				pooledParticle[i].gameObject.SetActive(false);
			}
		}
	}
}