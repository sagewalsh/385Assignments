using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCamera : MonoBehaviour
{
    private bool wayPointCameraOn = false;
    private GameObject currentWayPoint = null;
    private Transform target = null;

    void Start()
    {
        //GameObject.Find("WayPointCamera").camera.enabled = false;
    }

    void Update()
    {
        //Debug.Log("Update way point camera on");
        if (wayPointCameraOn)
        {
            //GameObject.Find("WayPointCamera").camera.enabled = true;
            Debug.Log("Enter if way point camera on");
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }

    public void wayPointCamera(bool onOff, GameObject gObject)
    {
        currentWayPoint = gObject;
        target = gObject.transform;
        wayPointCameraOn = onOff;
    }

}
