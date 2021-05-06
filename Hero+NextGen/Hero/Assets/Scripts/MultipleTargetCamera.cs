using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    [Header("==General Chase Camera Settings==")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] List<Transform> planePositions;


    private void Update()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Plane"))
        {
            PlaneBehavior plane = obj.GetComponent<PlaneBehavior>();
            if (plane.mState == PlaneBehavior.EnemyState.eChaseState)
            {
                Debug.Log("adding plane");
                planePositions.Add(obj.transform);
            }
        }
    }

    private void LateUpdate()
    {
        if (planePositions.Count == 0)
        {
            return;
        }
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = centerPoint;
    }

    private Vector3 GetCenterPoint()
    {
        if (planePositions.Count == 1)
        {
            return planePositions[0].position;
        }
        var bounds = new Bounds(planePositions[0].position, Vector3.zero);

        for (int i = 0; i < planePositions.Count; i++)
        {
            bounds.Encapsulate(planePositions[i].position);
        }
        return bounds.center;
    }
}
