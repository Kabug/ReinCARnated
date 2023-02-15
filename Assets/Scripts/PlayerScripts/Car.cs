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

    private float topSpeed = 27f;
    // Start is called before the first frame update
    void Start()
    {
        profile.TryGet(out chromaticAbberation);
    }

    // Update is called once per frame
    void Update()
    {
        float carSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
        abberation = Mathf.Clamp01(Mathf.Lerp(normalizedSpeed, 2, Time.deltaTime));
        chromaticAbberation.intensity.Override(abberation);
    }
}
