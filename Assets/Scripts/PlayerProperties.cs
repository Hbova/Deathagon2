using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerProperties : MonoBehaviourPun
{
    public int playerNumber;
    public int currentWallet;
    public float currentIncome { get; set; }
    public float playerBulletDamage = 25;

    public void KillEnemy(int worth)
    {
        currentWallet += worth;
    }

    public void SpawnLevel1()
    {
        NetworkedObjectsH.find.AddToCreepList(playerNumber, 1);
        if (photonView.IsMine)
        {
            currentWallet -= 50;
            currentIncome += 20;
        }
    }

    [PunRPC]
    public void PayPlayers()
    {
        currentWallet += Mathf.RoundToInt(currentIncome);
    }
}
