using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private int planeMax = 10;
    int planeCount = 0;

    void start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (planeCount < planeMax)
        {
            SpawnObject();
            planeCount++;
        }
    }

    private void SpawnObject()
    {
        bool objSpawned = false;
        while (!objSpawned)
        {
            Vector3 objPosition = new Vector3(Random.Range(-245f, 245f), Random.Range(-90f, 90f), 0);

            // ensures new object is spawned far enough away
            if ((objPosition - transform.position).magnitude < 3)
            {
                continue;
            }
            else
            {
                objSpawned = true;
                
                // Destroys on a 10 second timer
                Destroy(Instantiate(enemy, objPosition, Quaternion.identity), 10.0f);
            }
        }
        
    }
}
