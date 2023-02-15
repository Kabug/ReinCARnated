using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEndPoint : MonoBehaviour
{
    public GameObject endMarker;

    public GameObject ground;

    public GameObject car;

    private Vector3 randomPosition;

    private void Start()
    {
        Renderer groundRenderer = ground.GetComponent<Renderer>();
        Bounds groundBounds = groundRenderer.bounds;

        Renderer endMarkerRenderer = endMarker.GetComponent<MeshRenderer>();
        Bounds endMarkerBounds = endMarkerRenderer.bounds;

        randomPosition = new Vector3(
            Random.Range(groundBounds.min.x, groundBounds.max.x),
            groundBounds.max.y + 100,
            Random.Range(groundBounds.min.z, groundBounds.max.z)
        );

        GameObject spawnedObject = Instantiate(endMarker, randomPosition, Quaternion.identity);

        Debug.Log("Target marker is at: " + randomPosition);
    }

    private void Update()
    {

    }

}
