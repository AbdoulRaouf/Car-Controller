using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Rigidbody rb;
    public bool wheelFL;
    public bool wheelFR;
    public bool wheelRL;
    public bool wheelRR;


    [Header("*----Suspension----*")]
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float springStiffness;
    [SerializeField] private float dampingStiffness;

    private float springLength;
    private float maxLength;
    private float lastLength;
    private float minLength;
    private float springVelocity;
    private float dampingForce;
    private float sprinForce;

    private Vector3 suspensionForce;
    private float wheelAngle;

    [Header("*----wheel----*")]
    [SerializeField] private float wheelRadius;
    public float steerAngle;
    public float steerTime;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        maxLength = restLength + springTravel;
        minLength = restLength - springTravel;
    }

    void Update()
    {
        this.transform.localRotation= Quaternion.Euler(transform.localRotation.x,transform.localRotation.y+steerAngle,transform.localRotation.z);
    }

    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, (maxLength + wheelRadius))){
            lastLength = springLength;
            springLength= hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength)/ Time.fixedDeltaTime;
            sprinForce = springStiffness * (restLength - springLength);
            dampingForce = dampingStiffness * springVelocity;
            suspensionForce = (sprinForce + dampingForce) * transform.up;

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }
}
