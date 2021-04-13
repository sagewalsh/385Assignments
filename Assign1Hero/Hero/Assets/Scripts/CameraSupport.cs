using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSupport : MonoBehaviour
{
    private Camera theCamera;
    private Bounds worldBounds;
    void Start()
    {
        theCamera = gameObject.GetComponent<Camera>();
        worldBounds = new Bounds();

        float maxY = theCamera.orthographicSize;
        float maxX = theCamera.orthographicSize * theCamera.aspect;
        float sizeY = 2 * maxY;
        float sizeX = 2 * maxX;

        Vector3 c = theCamera.transform.position;
        c.z = 0f;

        worldBounds.center = c;
        worldBounds.size = new Vector3(sizeX, sizeY, 1f);

        Debug.Log("WorldBounds: " + worldBounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Bounds GetWorldBounds()
    {
        return worldBounds;
    }

    public bool isInside(Bounds b1)
    {
        return isInsideBounds(b1, worldBounds);
    }

    public bool isInsideBounds(Bounds b1, Bounds b2)
    {
        return (b1.min.x < b2.min.x) && (b1.max.x > b2.max.x) &&
               (b1.min.y < b2.min.y) && (b1.max.y > b2.max.y);
    }
}
