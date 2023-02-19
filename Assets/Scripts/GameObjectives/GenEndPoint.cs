using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class GenEndPoint : MonoBehaviour
{
    public GameObject stickman;

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
        //Vector3 spawnPosition = randomSpawnArea.transform.position;

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

        stickManPosition = new Vector3(xPos, 10, zPos);
        stickman.transform.position = stickManPosition;
        stickman.SetActive(true);

        if (Physics.Raycast(stickman.transform.position, stickman.transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, Mathf.Infinity))
        {
            //Debug.DrawRay(stickman.transform.position, stickman.transform.TransformDirection(Vector3.down) * hitInfo.distance, Color.yellow);
            stickman.transform.position = new Vector3(stickman.transform.position.x, stickman.transform.position.y - hitInfo.distance, stickman.transform.position.z);
        }
        GameTracker.Instance.setStartPoint(stickman.transform.localPosition);
        //Debug.Log(string.Format("Target is at: {0}", stickManPosition));
    }

    private void Update()
    {

    }

}
