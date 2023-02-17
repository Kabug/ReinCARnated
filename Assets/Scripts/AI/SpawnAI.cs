using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAI : MonoBehaviour
{
    private float nextActionTime = 0f;
    public float period = 5f;

    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    
    void Update()
    {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            // execute block of code here
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
