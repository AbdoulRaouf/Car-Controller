using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera Camera;
    public GameObject CameraFollower;
    void Start()
    {
        Camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.position= CameraFollower.transform.position + new Vector3(.4f,3.3f,-6f);
    }
}
