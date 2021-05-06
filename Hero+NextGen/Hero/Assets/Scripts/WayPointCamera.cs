using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointCamera : MonoBehaviour
{
    public bool wayPointCameraOn = false;
    
    public Text wayCamText = null;

    void Start()
    {
        transform.position = new Vector3(0,0,0);
    }

    void Update()
    {
        if(wayPointCameraOn)
        {
            wayCamText.text = "WayPoint Cam: Active";
        }
        else
        {
            wayCamText.text = "WayPoint Cam: Shut Off";
        }
    }

    public void cameraOn(Transform way)
    {
        Vector3 pos = way.position;
        pos.z = -10;
        transform.position = pos;
        wayPointCameraOn = true;
    }

    public void cameraOff()
    {
        transform.position = new Vector3(0,0,0);
        wayPointCameraOn = false;
    }

}
