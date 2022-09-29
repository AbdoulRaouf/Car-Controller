using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Wheel[] wheelTab;

    [Header("*----Specification----*")]
    [SerializeField] private float wheelsBase;
    [SerializeField] private float rearTrack;
    [SerializeField] private float turnRadius;
    

    [Header("*----Inputs----*")]
    [SerializeField] private float steerInput;

    [SerializeField] private float ackermannAngleLeft;
    [SerializeField] private float ackermannAngleRight;

    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0)
        {
            ackermannAngleLeft  = Mathf.Rad2Deg * Mathf.Atan(wheelsBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelsBase / (turnRadius - (rearTrack / 2))) * steerInput;

        }
        else if (steerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelsBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelsBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach (var w in wheelTab)
        {
            if (w.wheelFL)
            {
                w.steerAngle = ackermannAngleLeft;
            }
            if (w.wheelFR)
            {
                w.steerAngle = ackermannAngleRight;
            }
        }
                
    }
}
