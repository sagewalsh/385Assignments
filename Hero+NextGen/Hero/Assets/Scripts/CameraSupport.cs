using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSupport : MonoBehaviour
{
    // Game Camera
    private Camera theCamera;
    private GameObject currentWayPoint = null;

    // World boundaries
    private Bounds worldBounds;

    // 90% of the World boundaries
    private Bounds bounds90;


    void Start()
    {
        theCamera = gameObject.GetComponent<Camera>();
        worldBounds = new Bounds();
        bounds90 = new Bounds();

        // Boundary range is -max to +max
        float maxY = theCamera.orthographicSize;
        float maxX = theCamera.orthographicSize * theCamera.aspect;

        float sizeY = 2 * maxY;
        float sizeX = 2 * maxX;

        // 90% of the world 
        float y90 = sizeY * 9 / 10;
        float x90 = sizeX * 9 / 10;

        Vector3 c = theCamera.transform.position;
        c.z = 0f;

        // Store the world boundaries
        worldBounds.center = c;
        worldBounds.size = new Vector3(sizeX, sizeY, 1f);

        // Store 90% of the world boundaries
        bounds90.center = c;
        bounds90.size = new Vector3(x90, y90, 1f);

    }

    void Update(){}

    // Returns boundaries of the world
    public Bounds GetWorldBounds()
    {
        return worldBounds;
    }

    // Returns boundaries of 90% of the world
    public Bounds Get90Bounds()
    {
        return bounds90;
    }

    // Returns if boundaries given are within the world
    public bool isInside(Bounds b1)
    {
        return isInsideBounds(b1, worldBounds);
    }

    // Returns if boundaries given (b1) are within the 
    // other boundaries given (b2) -- returns if b1 in b2
    public bool isInsideBounds(Bounds b1, Bounds b2)
    {
        return (b1.min.x < b2.min.x) && (b1.max.x > b2.max.x) &&
               (b1.min.y < b2.min.y) && (b1.max.y > b2.max.y);
    }

}
