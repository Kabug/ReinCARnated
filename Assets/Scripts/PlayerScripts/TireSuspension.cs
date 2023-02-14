using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


// This script handles tire suspension, steering and acceleration
public class TireSuspension : MonoBehaviour
{
    private float suspensionLength = 0.8f;
    private float springStrength = 600f;
    private float springDamper = 100f;
    public bool turnable;
    public bool drivable;

    private float forwardSpeed = 3000f;
    private float backwardSpeed = 2000f;
    private float topSpeed = 27f;

    // Grip factor in range of 0-1
    private float gripFactor;
    private float defaultGrip = 0.8f;
    private float driftGrip = 0.6f;

    public LineRenderer line;

    public Rigidbody carRigidbody;
    public Transform carTransform;

    private bool rayCastHit;

    public AnimationCurve powerCurve;
    // Start is called before the first frame update
    void Start()
    {
        gripFactor = defaultGrip;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnable)
        {
            // Still need to cap max turn :|
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (transform.rotation.eulerAngles.y > 20 || transform.rotation.eulerAngles.y < 340)
                    {
                        transform.Rotate(-Vector3.up * 20 * Time.deltaTime);
                    }
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (transform.rotation.eulerAngles.y > 20 || transform.rotation.eulerAngles.y < 340)
                    {
                        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, carTransform.rotation, Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            gripFactor = driftGrip;
        }
        else
        {
            gripFactor = defaultGrip;
        }

    }

    private void FixedUpdate()
    {
        //line.SetPosition(0, transform.position);
        //line.SetPosition(1, transform.position);
        rayCastHit = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, suspensionLength))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.red);
            float vel = Vector3.Dot(transform.up, carRigidbody.GetPointVelocity(transform.position));
            float offset = suspensionLength - hitInfo.distance;
            float compressionRatio = (offset * springStrength) - (vel * springDamper);
            carRigidbody.AddForceAtPosition(transform.up * compressionRatio, transform.position);
            //line.SetPosition(1, transform.position + transform.up * compressionRatio / 100);

            rayCastHit = true;
        }

        if (rayCastHit)
        {
            Vector3 steeringDir = transform.right;

            Vector3 tireWorldVel = carRigidbody.GetPointVelocity(transform.position);

            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

            float desiredVelChange = -steeringVel * gripFactor;

            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            carRigidbody.AddForceAtPosition(steeringDir * 5f * desiredAccel, transform.position);
        }

        if (rayCastHit && drivable)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 accelDir = transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);


                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * forwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }

            if (Input.GetKey(KeyCode.S))
            {
                Vector3 accelDir = -transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);

                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * backwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }
        }
    }
}
