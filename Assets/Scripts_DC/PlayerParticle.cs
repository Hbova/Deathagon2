using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerParticle : MonoBehaviourPun
{
	public List<ParticleSystem> particlePrefabs; // a couple of random prefabs
	public Transform spawnTarget;
	public PlayerColor playerColor;

	System.Random random;

	void Update() {
		// if this client doesn't control this player, then don't let it spawn particles on input
		if( !photonView.IsMine ) return;

		if( Input.GetButtonDown( "Fire1" ) ) {
			// instantiate particles synchronously across the network
			photonView.RPC( "SpawnParticle", RpcTarget.All );
		}
	}

	[PunRPC]
	public void SetRandomSeed( int newSeed ) {
		random = new System.Random( newSeed );
	}

	[PunRPC]
	public void SpawnParticle() {
		// pick a random particle (note how the randomness means that a different particle might be picked on each client)
		ParticleSystem particlePrefab = particlePrefabs[ UnityEngine.Random.Range(0, particlePrefabs.Count) ];

		// pick a random particle (using a deterministic algorithm)
		// ParticleSystem particlePrefab = particlePrefabs[ random.Next(0, particlePrefabs.Count) ];
		ParticleSystem newSystem = Instantiate<ParticleSystem>( particlePrefab, spawnTarget.position, Quaternion.identity );

		// set the particle to be the same color as the player so it's identifiable as theirs
		var mainParticleModule = newSystem.main;
		mainParticleModule.startColor = playerColor.currentColor;
	}
}
