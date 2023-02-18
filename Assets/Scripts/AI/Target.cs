using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody[] rigidBodies;

    private void Awake()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        enableRagdoll(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableRagdoll(bool val)
    {
        val = !val;
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = val;
        }
        //if (val)
        //{
        //    foreach (var rigidBody in rigidBodies)
        //    {
        //        rigidBody.useGravity = !val;
        //    }
        //}
    }
}
