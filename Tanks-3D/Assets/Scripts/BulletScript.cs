using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float travelTime = 3.0f;
    public float damage = 2.0f;

    private void Awake()
    {
        //Destroy bullet after travel time
        Destroy(gameObject, travelTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deduct player health with damage value
            collision.gameObject.GetComponent<PlayerNetwork>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy enemy in 1 hit
            Destroy(collision.gameObject);
        }

        // Destroy bullet
        Destroy(gameObject);
        
    }
}