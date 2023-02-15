using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinish : MonoBehaviour
{

    public GameObject popupUI;
    private bool isPopupActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().tag == "endPoint")
        {
            Debug.Log("endpoint reached");

            if (!isPopupActive)
            {
                popupUI.SetActive(true);
                isPopupActive = true;
            }
        }
    }
}
