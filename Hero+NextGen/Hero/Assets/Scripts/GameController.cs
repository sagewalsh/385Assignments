using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Variables to track planes in world
    private int maxPlanes = 10;
    private int numberOfPlanes = 0;
    private int planesDestroyed = 0;
    public int heroHit = 0;


    // Variable to track eggs in world
    private int numberOfEggs = 0;

    // UI Text Variables
    public Text enemyText = null;
    public Text eggText = null;
    public Text waypoint = null;
    public Text heroHits = null;

    public GameObject[] waypoints;

    CameraSupport s = null;

    public bool isRandom;

    void Start()
    {
        isRandom = false;

        s = Camera.main.GetComponent<CameraSupport>();
        enemyText.text = "ENEMY: Count(" + numberOfPlanes +
                         ") Destroyed(" + planesDestroyed + ")";

        waypoint.text = "Waypoints: Sequential";

        heroHits.text = "HERO: Hit(" + heroHit + ")";
    }


    void Update()
    {
        // Press Q to quit game application
        if(Input.GetKey(KeyCode.Q))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isRandom)
            {
                Debug.Log("Random is off");
                waypoint.text = "Waypoints: Sequential";
                isRandom = false;
            }

            else
            {
                Debug.Log("Random is on");
                waypoint.text = "Waypoints: Random";
                isRandom = true;
            }
        }

        // Create more planes to maintain maxPlanes in the world
        if(numberOfPlanes < maxPlanes)
        {
            // Create a plane
            GameObject e = Instantiate(Resources.Load("Prefabs/PlaneEnemy") as GameObject);
            
            // Spawn plane to random position within 90% of world boundaries
            Vector3 pos;
            pos.x = s.Get90Bounds().min.x + Random.value * s.Get90Bounds().size.x;
            pos.y = s.Get90Bounds().min.y + Random.value * s.Get90Bounds().size.y;
            pos.z = 0;

            e.transform.localPosition = pos;
            numberOfPlanes++; // Increase plane count
        }

        // Update the UI Text
        enemyText.text = "ENEMY: Count(" + numberOfPlanes +
                         ") Destroyed(" + planesDestroyed + ")";

        eggText.text = "EGG: OnScreen(" + numberOfEggs + ")";

        heroHits.text = "HERO: Hit(" + heroHit + ")";
    }

    // Update plane tracking variables
    public void EnemyDestroyed()
    {
        numberOfPlanes--;
        planesDestroyed++;
    }

    public void EggCreated()
    {
        numberOfEggs++;
    }

    public void EggDestroyed()
    {
        numberOfEggs--;
    }
}
