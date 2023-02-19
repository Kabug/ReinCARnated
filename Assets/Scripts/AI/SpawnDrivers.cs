using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDrivers : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;
    //Vector3 myVector;
    // Start is called before the first frame update
    void Start()
    {
        spawnRoadOne();
        spawnRoadTwo();
    }

    // Update is called once per frame

    void Update()
    {

    }

    void spawnRoadOne()
    {
        int z1 = -1600;
        int z2 = 805;
        int interval = 100;
        for (int z = z1; z < z2; z += interval)
        {
            GameObject vehicle = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
            Instantiate(vehicle, new Vector3(445, 3, z), transform.rotation);
            Instantiate(vehicle, new Vector3(435, 3, z), transform.rotation);
        }
    }

    void spawnRoadTwo()
    {
        int x1 = -1300;
        int x2 = 1350;
        int interval = 100;
        for (int x = x1; x < x2; x += interval)
        {
            GameObject vehicle = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
            Instantiate(vehicle, new Vector3(x, 3, -5), transform.rotation);
            Instantiate(vehicle, new Vector3(x, 3, 8), transform.rotation);
        }
    }
}
