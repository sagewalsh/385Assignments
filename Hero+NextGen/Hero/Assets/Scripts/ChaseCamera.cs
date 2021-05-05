using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{

    [Header("==General Chase Camera Settings==")]
    [SerializeField] private Transform target;
    [SerializeField] Camera chaseCamera;
    [SerializeField] private float smoothSpeed = 0.4f;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        chaseCamera.enabled = false;
    }


    private void FixedUpdate()
    {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
    }

    private void CameraPower()
    {
            chaseCamera.enabled = !chaseCamera; 
    }
}
