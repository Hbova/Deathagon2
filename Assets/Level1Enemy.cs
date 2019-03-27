using System.Collections;
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
            Destination.GetComponent<PlayerProperties>().KillEnemy(10);
            Destroy(this.gameObject);
        }
    }

    public void SetDestination(Transform destination)
    {
        Destination = destination;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("HIT!!");
        Bullet bullet = col.transform.GetComponent<Bullet>();

        if( bullet ) {
            HitPoints -= bullet.bulletDamage;
        }
    }
}
