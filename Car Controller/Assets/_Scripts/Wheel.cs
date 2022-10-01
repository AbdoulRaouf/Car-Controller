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
    private Vector3 wheelVelocityLS;
    private float wheelAngle;
    private float Fx;
    private float Fy;

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
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime*Time.deltaTime);

        this.transform.localRotation= Quaternion.Euler(Vector3.up*wheelAngle);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.blue);
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

            wheelVelocityLS =transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            Fx = Input.GetAxis("Vertical") * sprinForce;
            Fy = wheelVelocityLS.x * sprinForce;

            rb.AddForceAtPosition(suspensionForce + (transform.forward * Fx) + (Fy * - transform.right), hit.point);
        }
        
    }
}
