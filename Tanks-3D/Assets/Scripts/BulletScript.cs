using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float travelTime = 3;

    private void Awake()
    {
        //Destroy bullet after travel time
        Destroy(gameObject, travelTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy collided object
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        //Destroy(collision.gameObject);
        
        // Destroy bullet
        Destroy(gameObject);
        
    }
}