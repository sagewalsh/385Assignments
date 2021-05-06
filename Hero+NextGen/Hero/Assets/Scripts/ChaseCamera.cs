using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaseCamera : MonoBehaviour
{
    private GameObject Player;
    private int index = 0;
    private float distance;
    [SerializeField] public int ZoomOut; 
    [SerializeField] Camera cam;
    public Text enemyChaseCam = null;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        List<Vector2> planePositions = new List<Vector2>();
        int index = 0;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Plane"))
        {
           
            PlaneBehavior plane = obj.GetComponent<PlaneBehavior>();
            if(plane.mState == PlaneBehavior.EnemyState.eChaseState)
            {
                Debug.Log("adding plane");
                planePositions.Add(obj.transform.position);
                index++;
            } 
        }

        Vector2 averageEnemyVector = Vector2.zero;
        Vector3 playerPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, ZoomOut);
        Vector3 averageChaseCameraPosition = new Vector3();
        
        // enable camera
        if (index > 0)
        {
            cam.enabled = true;
            //cam.backgroundColor = Color32.LerpUnclamped()
            //cam.backgroundColor = Color.a = 0;
            for (int i = 0; i < index; i++)
            {
                averageEnemyVector += planePositions[i];
            }
            
            enemyChaseCam.text = "Chase Camera: \nActive";
            
            distance = Vector2.Distance(playerPosition, averageEnemyVector); //Added
            
            averageEnemyVector.x += playerPosition.x;

            averageEnemyVector.y += playerPosition.y;

            averageEnemyVector = (averageEnemyVector / (planePositions.Count + 1));

            averageChaseCameraPosition.x = averageEnemyVector.x;

            averageChaseCameraPosition.y = averageEnemyVector.y;

            averageChaseCameraPosition.z = ZoomOut - (distance);

            transform.position = averageChaseCameraPosition;        //averageChaseCameraPosition;

        }
        // disable cam
        else
        {
            enemyChaseCam.text = "Chase Camera: \nShut Off";
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
