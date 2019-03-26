using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeSpan;

    public float bulletDamage;

    // Start is called before the first frame update
    void Start()
    {
        lifeSpan = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0) Destroy(this.gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * 20);
    }
}
