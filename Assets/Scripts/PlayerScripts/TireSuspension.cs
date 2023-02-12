using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSuspension : MonoBehaviour
{
    private float suspensionLength = 1f;
    private float springStrength = 3f;
    private float springDamper = 1f;

    public LineRenderer line;

    public Rigidbody carRigidbody;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, suspensionLength))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.red);
            float vel = Vector3.Dot(transform.up, carRigidbody.GetPointVelocity(transform.position));
            float offset = suspensionLength - hitInfo.distance;
            float compressionRatio = (offset * springStrength) - (vel * springDamper);
            carRigidbody.AddForceAtPosition(transform.up * compressionRatio, transform.position);

            line.SetPosition(1, transform.position + transform.up * compressionRatio);
        }
    }
}
