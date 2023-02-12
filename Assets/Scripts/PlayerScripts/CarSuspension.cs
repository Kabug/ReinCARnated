using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSuspension : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
     {
        if (Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.AddForce(transform.forward * 30f, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_Rigidbody.AddTorque(transform.up * 5f * -1, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_Rigidbody.AddTorque(transform.up * 5f, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.AddForce(-transform.forward * 20f, ForceMode.Acceleration);
        }
    }
}
