using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleWhenFinished : MonoBehaviour
{
	public ParticleSystem particle;
	public float checkInterval;

	void Start() {
		// destroy this spawned particle prefab after a certain amount of time
		Destroy( particle.gameObject, particle.main.duration );
	}
}
