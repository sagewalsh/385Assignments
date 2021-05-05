using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCamera : MonoBehaviour
{
    public bool wayPointCameraOn = false;
    public Transform target;

    void update()
    {
        if (wayPointCameraOn)
        {
            transform.position = target.position;
        }
    }

    public void wayPointCamera(bool onOff)
    {
        wayPointCameraOn = onOff;
    }
}
