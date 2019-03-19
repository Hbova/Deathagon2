using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerProperties : MonoBehaviourPun
{

    public float[] playerIncomes;
    public int currentWallet;
    public float currentIncome { get; set; }

    [PunRPC]
    // synchronous assignment of income to the player from the "server"
    public void SetIncome(int order)
    {
        currentIncome = playerIncomes[order];
    }
    [PunRPC]
    public void PayPlayers(int order)
    {
        currentWallet += Mathf.RoundToInt(playerIncomes[order]);
    }
    [PunRPC]
    public void AddToIncome(int order,int amount)
    {
        playerIncomes[order] += amount;
    }
}
