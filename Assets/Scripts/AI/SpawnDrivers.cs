using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnDrivers : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshTriangulation navMesh = UnityEngine.AI.NavMesh.CalculateTriangulation();
        Vector3[] vertices = navMesh.vertices;
        spawnRoadOne();
        spawnRoadTwo();
        spawnRoadThree(vertices);
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

    void spawnRoadThree(Vector3[] vertices)
    {
        for (int i = 0; i < 200; i++)
        {
            int randomVertex = Random.Range(0, vertices.Length);
            Vector3 randomPoint = vertices[randomVertex];
            UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas);
            Vector3 randomPosition = hit.position;

            GameObject vehicle = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
            Instantiate(vehicle, randomPosition, transform.rotation);
        }
    }
}
