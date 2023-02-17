using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn;
    //Vector3 myVector;
    // Start is called before the first frame update
    void Start()
    {
        //myVector = new Vector3(440f, 0f, 60f);
        Instantiate(objectToSpawn, new Vector3(440,0,60), transform.rotation);
        Instantiate(objectToSpawn, new Vector3(300,0,0), transform.rotation);
        Instantiate(objectToSpawn, new Vector3(450,0,-60), transform.rotation);
    }

    // Update is called once per frame
    
    void Update()
    {

    }
}
