using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;

public class NetworkedObjectsH : MonoBehaviour
{
    public Transform[] spawnPos = new Transform[8];

    [HideInInspector] public List<PhotonView> players = new List<PhotonView>();

    public static NetworkedObjectsH find;

    int seed; // only matters on the master client

    

    // singleton assignment
    void Awake()
    {
        find = this;
    }

    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            // try to create a truly random number to use as your starting random seed
            seed = DateTime.Now.Millisecond + System.Threading.Thread.CurrentThread.GetHashCode();
        }
        Debug.Log(players.Count);
        PhotonNetwork.Instantiate("Player2", spawnPos[players.Count].position, Quaternion.identity, 0);
    }

    public void AddPlayer(PhotonView player)
    {
        // add a player to the list of all tracked players
        players.Add(player);

        // only the "server" has authority over which color the player should be and its seed
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(players.Count);
            player.RPC("SetColor", RpcTarget.AllBuffered, players.Count-1); // buffer the color change so it applies to new arrivals in the room
        }
    }
}
