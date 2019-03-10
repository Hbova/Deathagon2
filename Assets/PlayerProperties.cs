using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class PlayerProperties : MonoBehaviourPun
{

    public float[] playerIncomes;
    public float currentIncome { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    // synchronous assignment of income to the player from the "server"
    public void SetIncome(int order)
    {
        currentIncome = playerIncomes[order];
    }
}
