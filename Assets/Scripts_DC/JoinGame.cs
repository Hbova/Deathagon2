using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// use the Photon libraries
using Photon.Realtime;
using Photon.Pun;

public class JoinGame : MonoBehaviourPunCallbacks // override callback methods that Photon will call at certain connection events
{
	const int gameSceneIndex = 1;

	public TextMeshProUGUI label;

	void Start() {

		// keep the scenes of the different connected clients in sync with this one
		PhotonNetwork.AutomaticallySyncScene = true;

		// connect using default settings configured in the Photon Settings scriptable object
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster() {
		label.text = "Joining game...";
		// once connected to the master relay, join a random room
	    PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed( short returnCode, string message ) {
		label.text = "Creating game...";
		// no current game running, so join a random one
		PhotonNetwork.CreateRoom( null, new RoomOptions() { MaxPlayers = 20 }, null ); // create a RoomOptions object to control how players can join
	}

	public override void OnCreatedRoom() {
		label.text = "Created game...";
		SceneManager.LoadScene( gameSceneIndex );
	}

	public override void OnJoinedRoom()
	{
	    // succeeded in joining a room - load the appropriate scene
		label.text = "Joined room...";
		SceneManager.LoadScene( gameSceneIndex );
	}
}
