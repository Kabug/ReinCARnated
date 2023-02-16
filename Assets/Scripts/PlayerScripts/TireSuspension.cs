using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


// This script handles tire suspension, steering and acceleration
public class TireSuspension : MonoBehaviour
{
    private float suspensionLength = 1f;
    private float springStrength = 600f;
    private float springDamper = 100f;
    public bool turnable;
    public bool drivable;

    private float forwardSpeedMaxPower = 3000f;
    private float forwardSpeed = 0f;
    private float backwardSpeedMaxPower = 2000f;
    private float backwardSpeed = 0f;
    private float forwardTotal = 0;
    private float backwardTotal = 0;
    private float topSpeed = 27f;

    // Grip factor in range of 0-1
    private float gripFactor;
    private float defaultGrip = 0.8f;
    private float driftGrip = 0.1f;

    public Rigidbody carRigidbody;
    public Transform carTransform;

    private bool rayCastHit;

    public AnimationCurve powerCurve;
    private CustomInput input = null;

    public TrailRenderer trailRenderer;
    public ParticleSystem smokeParticles;

    //Control States
    private bool isTurnLeft;
    private bool isTurnRight;
    private bool isDrift;
    private bool isAccel;
    private bool isDeccel;


    // Start is called before the first frame update
    void Start()
    {
        gripFactor = defaultGrip;
        if (trailRenderer)
        {
            trailRenderer.emitting = false;
        }

        if (smokeParticles)
        {
            var emission = smokeParticles.emission;
            emission.rateOverTime = 0;
        } 
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
        isTurnLeft = input.Player.TurnLeft.ReadValue<float>() > 0.1f;
        isTurnRight = input.Player.TurnRight.ReadValue<float>() > 0.1f;
        isDrift = input.Player.Drift.ReadValue<float>() > 0.1f;
        isAccel = input.Player.Accelerate.ReadValue<float>() > 0.1f;
        isDeccel = input.Player.Decelerate.ReadValue<float>() > 0.1f;

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

            transform.rotation = Quaternion.Slerp(transform.rotation, carTransform.rotation, Time.deltaTime * 2.5f);
        }
        
        if (isDrift)
        {
            gripFactor = Mathf.Lerp(driftGrip, defaultGrip, Time.deltaTime);
        }
        else
        {
            gripFactor = Mathf.Lerp(defaultGrip, driftGrip, Time.deltaTime);
        }

        if (trailRenderer)
        {
            if (Mathf.Round(gripFactor * 10) < Mathf.Round(defaultGrip * 10))
            {
                trailRenderer.emitting = true;
            }
            else
            {
                trailRenderer.emitting = false;
            }
        }
        if (smokeParticles)
        {
            if (Mathf.Round(gripFactor * 10) < Mathf.Round(defaultGrip * 10) && 0.4f < Mathf.Clamp01(Mathf.Abs(Vector3.Dot(carTransform.forward, carRigidbody.velocity)) / topSpeed) && isDrift)
            {
                var emission = smokeParticles.emission;
                emission.rateOverTime = 40;
            }
            else
            {
                var emission = smokeParticles.emission;
                emission.rateOverTime = 0;
            }
        }


        if (drivable)
        {
            if (isAccel)
            {
                forwardSpeed = Mathf.Lerp(0, forwardSpeedMaxPower, forwardTotal);
                forwardTotal += 0.5f * Time.deltaTime;
            }
            else
            {
                forwardSpeed = Mathf.Lerp(forwardSpeedMaxPower, 0, forwardTotal);
                forwardTotal = Time.deltaTime;
            }

            if (isDeccel)
            {
                backwardSpeed = Mathf.Lerp(0, backwardSpeedMaxPower, backwardTotal);
                backwardTotal += 0.5f * Time.deltaTime;
            }
            else
            {
                backwardSpeed = Mathf.Lerp(backwardSpeedMaxPower, 0, backwardTotal);
                backwardTotal = Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        rayCastHit = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, suspensionLength))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitInfo.distance, Color.red);
            float vel = Vector3.Dot(transform.up, carRigidbody.GetPointVelocity(transform.position));
            float offset = suspensionLength - hitInfo.distance;
            float compressionRatio = (offset * springStrength) - (vel * springDamper);
            carRigidbody.AddForceAtPosition(transform.up * compressionRatio, transform.position);

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

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);


                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * forwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }

            if (isDeccel)
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
