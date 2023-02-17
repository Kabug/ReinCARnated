using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCube : MonoBehaviour
{
    public GameObject pickupEffect;
    public HealthBar healthBar;
    public float multiplier = 1.4f;
    public float duration = 4f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider player)
    {
        Debug.Log("power up!");

        GameObject clone = Instantiate(pickupEffect, transform.position, transform.rotation);

        player.transform.localScale *= multiplier;

        GameTracker stats = player.GetComponent<GameTracker>();
        stats.maxHealth *= multiplier;

        GetComponent<Collider>().enabled =false;
        GetComponent<MeshRenderer>().enabled =false;

        Destroy(clone, 2f);

        yield return new WaitForSeconds(duration);

        player.transform.localScale /= multiplier;
        stats.maxHealth /= multiplier;


    }
}
