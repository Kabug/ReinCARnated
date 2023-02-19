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
    private float topSpeed = 38f;

    // Grip factor in range of 0-1
    private float gripFactor;
    private float defaultGrip = 0.8f;
    private float driftGrip = 0.1f;

    public Rigidbody carRigidbody;
    public Transform carTransform;

    private bool rayCastHit;

    public AnimationCurve powerCurve;

    public TrailRenderer trailRenderer;
    public ParticleSystem smokeParticles;
    public bool forceDrive = false;

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

    // Update is called once per frame
    void Update()
    {
        if(forceDrive)
        {
            forwardSpeed = Mathf.Lerp(0, forwardSpeedMaxPower, forwardTotal);
            forwardTotal += 0.5f * Time.deltaTime;
        }

        if (GameTracker.Instance.GAMESTATE == GameTracker.GameStates.Playing)
        {
            if (turnable)
            {
                if (GameTracker.Instance.isTurnLeft)
                {

                    transform.Rotate(-Vector3.up * 20 * Time.deltaTime);

                }

                if (GameTracker.Instance.isTurnRight)
                {
                    transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, carTransform.rotation, Time.deltaTime * 2.5f);
            }

            if (GameTracker.Instance.isDrift)
            {
                gripFactor = Mathf.Lerp(driftGrip, defaultGrip, Time.deltaTime);
            }
            else
            {
                gripFactor = Mathf.Lerp(defaultGrip, driftGrip, Time.deltaTime);
            }

            if (drivable)
            {
                if (GameTracker.Instance.isAccel)
                {
                    forwardSpeed = Mathf.Lerp(0, forwardSpeedMaxPower, forwardTotal);
                    forwardTotal += 0.5f * Time.deltaTime;
                }
                else
                {
                    forwardSpeed = Mathf.Lerp(forwardSpeedMaxPower, 0, forwardTotal);
                    forwardTotal = Time.deltaTime;
                }

                if (GameTracker.Instance.isDeccel)
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
            if (Mathf.Round(gripFactor * 10) < Mathf.Round(defaultGrip * 10) && 0.4f < Mathf.Clamp01(Mathf.Abs(Vector3.Dot(carTransform.forward, carRigidbody.velocity)) / topSpeed) && GameTracker.Instance.isDrift)
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

    }

    private void FixedUpdate()
    {
        if (forceDrive)
        {
            Vector3 accelDir = transform.forward;

            float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);

            float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * forwardSpeed;

            carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
        }
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

        if (GameTracker.Instance.GAMESTATE != GameTracker.GameStates.Playing)
        {
            return;
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
            if (GameTracker.Instance.isAccel)
            {
                Vector3 accelDir = transform.forward;

                float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);

                float availableToruqe = powerCurve.Evaluate(normalizedSpeed) * forwardSpeed;

                carRigidbody.AddForceAtPosition(accelDir * availableToruqe, transform.position);
            }

            if (GameTracker.Instance.isDeccel)
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
