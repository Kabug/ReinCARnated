using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class GenEndPoint : MonoBehaviour
{
    public GameObject stickman;

    public GameObject arrow;

    public GameObject car;

    private Vector3 arrowPosition;

    private Vector3 stickManPosition;

    private void Start()
    {
        //GameObject[] openAreaObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.ToLower().Contains("01green") || obj.name.ToLower().Contains("01line") || obj.name.ToLower().Contains("01street")).ToArray();
        //int randomInt = Random.Range(0, openAreaObjects.Length);
        //GameObject randomSpawnArea = openAreaObjects[randomInt];
        //Renderer randomSpawnArea = randomSpawnArea.GetComponent<Renderer>();
        //Bounds randomSpawnAreaBounds = randomSpawnArea.bounds;
        //Vector3 spawnPosition = randomSpawnArea.transform.localPosition;

        Dictionary<int, List<int>> staticXPaths = new Dictionary<int, List<int>>();
        staticXPaths.Add(450, new List<int> {-1250, 1500});
        staticXPaths.Add(-330, new List<int> {-1350, 1350});

        Dictionary<int, List<int>> staticZPaths = new Dictionary<int, List<int>>();
        staticZPaths.Add(20, new List<int> {-1350, 450});
        staticZPaths.Add(-1510, new List<int> {-250, 700});

        int xPos;
        int zPos;

        if (Random.Range(0, 2) == 0)
        {
            KeyValuePair<int, List<int>> randomPair = staticXPaths.ElementAt(UnityEngine.Random.Range(0, staticXPaths.Count));
            xPos = randomPair.Key;
            zPos = Random.Range(randomPair.Value.ElementAt(0), randomPair.Value.ElementAt(1));
        }
        else
        {
            KeyValuePair<int, List<int>> randomPair = staticZPaths.ElementAt(UnityEngine.Random.Range(0, staticZPaths.Count));
            xPos = Random.Range(randomPair.Value.ElementAt(0), randomPair.Value.ElementAt(1));
            zPos = randomPair.Key;
        }

        arrowPosition = new Vector3(xPos, 175, zPos);
        GameObject arrowObject = Instantiate(arrow, arrowPosition, Quaternion.identity);
        stickManPosition = new Vector3(xPos, 10, zPos);
        GameObject stickManObject = Instantiate(stickman, stickManPosition, Quaternion.identity);

        Debug.Log(string.Format("Target is at: {0}", stickManPosition));
    }

    private void Update()
    {

    }

}
