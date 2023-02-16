using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCube : MonoBehaviour
{
    public GameObject pickupEffect;

    void onCollisionEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("power up!");

        Instantiate(pickupEffect, transform.position, transform.rotation);

        Destroy(gameObject);

    }
}
