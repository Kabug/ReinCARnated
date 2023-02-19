using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Car : MonoBehaviour
{
    public VolumeProfile profile;
    ChromaticAberration chromaticAbberation;
    private float abberation = 0;

    public Rigidbody rigidBody;

    public float topSpeed = 37f;

    public Target targetScript;
    public Target startTargetScript;

    // main menu controls
    private bool startSequenceOver = false;
    public List<TireSuspension> tireScripts;
    public  MainMenuManager mainMenuManager;


    // Start is called before the first frame update
    void Start()
    {
        profile.TryGet(out chromaticAbberation);
        //Vector3 startPosition = new Vector3(PlayerPrefs.GetFloat("TargetX"), PlayerPrefs.GetFloat("TargetY"), PlayerPrefs.GetFloat("TargetZ"));
        //if (startPosition != new Vector3(0, 0, 0))
        //{
        //    Debug.Log(startPosition);
        //    transform.position = startPosition;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        float carSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);
        GameTracker.Instance.setCarSpeed(carSpeed);
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
        abberation = Mathf.Clamp01(Mathf.Lerp(normalizedSpeed, 2, Time.deltaTime));
        chromaticAbberation.intensity.Override(abberation);

        for (var i = 0; i < tireScripts.Count; i++)
        {
            tireScripts[i].forceDrive = !startSequenceOver;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,new Vector3(0, -1, 0), out hitInfo, Mathf.Infinity))
        {
        }
        Debug.Log(hitInfo.distance);
        if (GameTracker.Instance.isFlip && GameTracker.Instance.GAMESTATE == GameTracker.GameStates.Playing && (hitInfo.distance == 0 || hitInfo.distance < 2))
        {
            rigidBody.isKinematic = true;
            transform.position = transform.position + new Vector3(0, 1, 0);
            transform.rotation = Quaternion.identity;
            rigidBody.isKinematic = false;

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "endPoint")
        {
            targetScript.enableRagdoll(true);
            foreach (ContactPoint contact in collision.contacts)
            {
                collision.rigidbody.AddForceAtPosition(new Vector3(-collision.relativeVelocity.x, collision.relativeVelocity.y, -collision.relativeVelocity.z) * 150 + Vector3.up * 45, contact.point);
            }
        }


        if (collision.gameObject.tag == "mainMenuTarget")
        {
            startTargetScript.enableRagdoll(true);
            foreach (ContactPoint contact in collision.contacts)
            {
                collision.rigidbody.AddForceAtPosition(new Vector3(-collision.relativeVelocity.x, collision.relativeVelocity.y, -collision.relativeVelocity.z) * 150 + Vector3.up * 45, contact.point);
            }
            startSequenceOver = true;
            mainMenuManager.startTransition();
        }

        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
    }
}
