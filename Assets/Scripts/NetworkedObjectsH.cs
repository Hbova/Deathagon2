﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Photon.Pun;
using Photon.Realtime;


public class NetworkedObjectsH : MonoBehaviour
{
    public Transform[] spawnPos = new Transform[8];

    public List<PhotonView> players = new List<PhotonView>();
    public List<List<int>> creepList = new List<List<int>>();

    public int myPlayerNumber;

    public int waveNumber = 0;
    public float waveTimer = 5;

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
        PhotonNetwork.Instantiate("Player2", spawnPos[players.Count].position, Quaternion.identity, 0);
    }

    private void Update()
    {
        waveTimer -= Time.deltaTime;
        if (PhotonNetwork.IsMasterClient)
        {
            if (waveTimer <= 0)
            {
                for(int i = 0; i < players.Count; i++)
                {
                    players[i].RPC("PayPlayers", RpcTarget.All);
                    if (i == 0) UnitSpawner.find.SpawnCreeps(creepList[players.Count - 1],i);
                    else UnitSpawner.find.SpawnCreeps(creepList[i], i);
                }
                for (int i = 0; i < creepList.Count; i++)
                {
                    creepList[i].Clear();
                }
                waveNumber += 1;
                waveTimer = 30;
            }
            
        }
    }

    public void AddPlayer(PhotonView player)
    {
        // add a player to the list of all tracked players
        myPlayerNumber = players.Count;
        players.Add(player);
        players[players.Count - 1].GetComponent<PlayerProperties>().playerNumber = myPlayerNumber;
        creepList.Add(new List<int>());

        // only the "server" has authority over which color the player should be and its seed
        if (PhotonNetwork.IsMasterClient)
        {
            player.RPC("SetColor", RpcTarget.AllBuffered, players.Count - 1); // buffer the color change so it applies to new arrivals in the room
            player.RPC("SetPosition", RpcTarget.AllBuffered, spawnPos[players.Count - 1].position);
        }
    }

    public void RemoveMe(int playerNumber)
    {
        if (playerNumber > myPlayerNumber) myPlayerNumber--;
        players.RemoveAt(playerNumber);
        creepList.RemoveAt(playerNumber);
    }

    public void AddToCreepList(int playerNumber,int creep)
    {
        creepList[playerNumber].Add(creep);
    }
}
