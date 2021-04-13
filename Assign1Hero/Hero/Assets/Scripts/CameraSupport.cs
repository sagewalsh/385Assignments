using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSupport : MonoBehaviour
{
    private Camera theCamera;
    private Bounds worldBounds;
    private Bounds bounds90;
    void Start()
    {
        theCamera = gameObject.GetComponent<Camera>();
        worldBounds = new Bounds();
        bounds90 = new Bounds();

        float maxY = theCamera.orthographicSize;
        float maxX = theCamera.orthographicSize * theCamera.aspect;

        float sizeY = 2 * maxY;
        float sizeX = 2 * maxX;

        float y90 = sizeY * 9 / 10;
        float x90 = sizeX * 9 / 10;

        Vector3 c = theCamera.transform.position;
        c.z = 0f;

        worldBounds.center = c;
        worldBounds.size = new Vector3(sizeX, sizeY, 1f);

        bounds90.center = c;
        bounds90.size = new Vector3(x90, y90, 1f);

        Debug.Log("WorldBounds: " + worldBounds);
        Debug.Log("PlaneBounds: " + bounds90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Bounds GetWorldBounds()
    {
        return worldBounds;
    }

    public Bounds Get90Bounds()
    {
        return bounds90;
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
