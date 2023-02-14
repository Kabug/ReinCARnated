using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSuspension : MonoBehaviour
{
    private float suspensionLength = 1f;
    private float springStrength = 600f;
    private float springDamper = 100f;
    public bool turnable;
    public bool drivable;

    public LineRenderer line;

    public Rigidbody carRigidbody;
    public Transform carTransform;

    private bool rayCastHit;

    public AnimationCurve powerCurve;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        rayCastHit = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, suspensionLength))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.red);
            float vel = Vector3.Dot(transform.up, carRigidbody.GetPointVelocity(transform.position));
            float offset = suspensionLength - hitInfo.distance;
            float compressionRatio = (offset * springStrength) - (vel * springDamper);
            carRigidbody.AddForceAtPosition(transform.up * compressionRatio, transform.position);

            line.SetPosition(1, transform.position + transform.up * compressionRatio / 100);

            rayCastHit = true;
        }
    }

    private void FixedUpdate()
    {
        if (turnable)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(-Vector3.up * 20 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up * 20 * Time.deltaTime);
            }

            if (rayCastHit)
            {

                Vector3 steeringDir = transform.forward;

                Vector3 tireWorldVel = carRigidbody.GetPointVelocity(transform.position);

                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

                float desiredVelChange = -steeringVel * 0.8f;

                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                carRigidbody.AddForceAtPosition(steeringDir * 5f * desiredAccel, transform.position);
            }
            
            else{
                
            }
        }

        if (rayCastHit && drivable)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 accelDir = transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / 100f);

                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * 10000f;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }
        }

    }
}
