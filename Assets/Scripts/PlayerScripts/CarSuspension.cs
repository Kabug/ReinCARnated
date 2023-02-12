using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSuspension : MonoBehaviour
{
    private float suspensionLength = 1f;
    public List<Vector3> suspensionLocations = new List<Vector3>(4);
    private float tireOffset = 0.1f;
    private float springStrength = 2f;
    private float springDamper = 1f;

    Dictionary<string, bool> areWheelsGrounded = new Dictionary<string, bool>()
    {
        { "fr", false },
        { "br:", false },
        { "fl", false },
        { "bl", false },
    };

    Rigidbody m_Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        // Clean this up later ;)
        suspensionLocations.Add(new Vector3());
        suspensionLocations.Add(new Vector3());
        suspensionLocations.Add(new Vector3());
        suspensionLocations.Add(new Vector3());

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;

        //Transform position doesn't take into account rotation
        suspensionLocations[0] = new Vector3(transform.position.x + transform.localScale.x / 2 - tireOffset, transform.position.y, transform.position.z + transform.localScale.z / 2 - tireOffset);
        suspensionLocations[1] = new Vector3(transform.position.x + transform.localScale.x / 2 - tireOffset, transform.position.y, transform.position.z - transform.localScale.z / 2 + tireOffset);
        suspensionLocations[2] = new Vector3(transform.position.x - transform.localScale.x / 2 + tireOffset, transform.position.y, transform.position.z + transform.localScale.z / 2 - tireOffset);
        suspensionLocations[3] = new Vector3(transform.position.x - transform.localScale.x / 2 + tireOffset, transform.position.y, transform.position.z - transform.localScale.z / 2 + tireOffset);

        //Front Right
        if (Physics.Raycast(suspensionLocations[0], transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength))
        {
            areWheelsGrounded["fr"] = true;
            Debug.DrawRay(suspensionLocations[0], transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.red);
            compressionRatioForce(hitInfo.distance, suspensionLocations[0]);
        }
        else
        {
            areWheelsGrounded["fr"] = false;
        }

        // Back Right 
        if (Physics.Raycast(suspensionLocations[1], transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength))
        {
            areWheelsGrounded["br"] = true;
            Debug.DrawRay(suspensionLocations[1], transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.yellow);
            compressionRatioForce(hitInfo.distance, suspensionLocations[1]);
        }
        else
        {
            areWheelsGrounded["br"] = false;
        }

        //Front Left
        if (Physics.Raycast(suspensionLocations[2], transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength))
        {
            areWheelsGrounded["fl"] = true;
            Debug.DrawRay(suspensionLocations[2], transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.blue);
            compressionRatioForce(hitInfo.distance, suspensionLocations[2]);
        }
        else
        {
            areWheelsGrounded["fl"] = false;
        }

        // Back Left
        if (Physics.Raycast(suspensionLocations[3], transform.TransformDirection(Vector3.down), out hitInfo, suspensionLength)) {
            areWheelsGrounded["bl"] = true;
            Debug.DrawRay(suspensionLocations[3], transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.green);
            compressionRatioForce(hitInfo.distance, suspensionLocations[3]);
        }
        else
        {
            areWheelsGrounded["bl"] = false;
        }
    }

    void FixedUpdate()
     {
        if (areWheelsGrounded.ContainsValue(true))
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

    void compressionRatioForce (float hitDistance, Vector3 position)
    {
        float vel = Vector3.Dot(transform.up, m_Rigidbody.GetPointVelocity(position));
        float offset = suspensionLength - hitDistance;
        float compressionRatio = (offset * springStrength) - (vel - springDamper);
        m_Rigidbody.AddForceAtPosition(transform.up * compressionRatio, position);
    }
}
