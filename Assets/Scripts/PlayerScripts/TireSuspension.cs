using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TireSuspension : MonoBehaviour
{
    private float suspensionLength = 0.8f;
    private float springStrength = 600f;
    private float springDamper = 100f;
    public bool turnable;
    public bool drivable;

    private float forwardSpeed = 6000f;
    private float backwardSpeed = 2000f;

    // Grip factor in range of 0-1
    private float gripFactor = 0.8f;

    public LineRenderer line;

    public Rigidbody carRigidbody;
    public Transform carTransform;

    private bool rayCastHit;

    public AnimationCurve powerCurve;
    private CustomInput input = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        input = new CustomInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        bool isTurnLeft = input.Player.TurnLeft.ReadValue<float>() > 0.1f;
        bool isTurnRight = input.Player.TurnRight.ReadValue<float>() > 0.1f;

        if (turnable)
        {
            if (isTurnLeft)
            {
                transform.Rotate(-Vector3.up * 20 * Time.deltaTime);
            }

            if (isTurnRight)
            {
                transform.Rotate(Vector3.up * 20 * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        bool isAccel = input.Player.Accelerate.ReadValue<float>() > 0.1f;
        bool isDeccel = input.Player.Decelerate.ReadValue<float>() > 0.1f;
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
            if (isAccel)
            {
                Vector3 accelDir = transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / 500f);

                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * forwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }

            if (isDeccel)
            {
                Vector3 accelDir = -transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / 100f);

                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * backwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }
        }
    }
}
