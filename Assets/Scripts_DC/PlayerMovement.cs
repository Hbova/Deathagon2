using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
	public float speed;

    public Transform appearance;
	public Transform target;
    
	Vector3 lastSyncedPos;

	void Update() {

		// if this client owns this view, then control its movement using the input axes
		if( photonView.IsMine ) {
            
			// move the renderer for this player immediately to its ideal position
			appearance.position = target.position;
		}
		else {
			// interpolate from the renderer's current position to its ideal position
			appearance.position = Vector3.Lerp( appearance.position, target.position, speed * Time.deltaTime );

			// appearance.position = target.position; // for jerky but accurate movement
		}
	}

	// read and write to a serialized data stream to send this object's position information
	public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info ) {
		if( stream.IsWriting ) {

			// don't send redundant data, like an unchanged position, over the network
			if( lastSyncedPos != target.position ) {
				lastSyncedPos = target.position;

				// since there is new position data, serialize it to the data stream
				stream.SendNext( target.position );
			}
		}
		else {
			// receive data from the stream in *the same order* in which it was originally serialized
			target.position = (Vector3)stream.ReceiveNext();
		}
	}

    
}
