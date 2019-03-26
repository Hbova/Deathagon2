﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform Destination;

    public float HitPoints = 50;
    
    // Update is called once per frame
    void Update()
    {
        agent.destination = Destination.position;
        if (HitPoints <= 0)
        {
            agent.transform.GetComponent<PlayerProperties>().KillEnemy(10);
            Destroy(this.gameObject);
        }
    }

    public void SetDestination(Transform destination)
    {
        Destination = destination;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT!!");
        HitPoints -= collision.transform.GetComponent<Bullet>().bulletDamage;
    }
}
