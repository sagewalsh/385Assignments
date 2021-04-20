using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    // Variable for the Plane's Health
    private int hitsByEgg = 0;
    private float energy = 1f;

    // Game Controller
    private GameController gameCon = null;
    private GameObject[] waypoints;

    [SerializeField] public float speed;

    [SerializeField] public float rotateSpeed;

    private int currentTarget;

    void Start()
    {
        gameCon = FindObjectOfType<GameController>();

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        currentTarget = Random.Range(0, waypoints.Length - 1);
    }


    void Update()
    {
        float step = speed * Time.deltaTime;

        float rotStep = rotateSpeed * Time.deltaTime;

        Vector3 target = waypoints[currentTarget].transform.position;

        //transform.position = Vector2.MoveTowards(transform.position, target, step);

        transform.Translate(-1 * transform.up * speed * Time.smoothDeltaTime);

        // referenced https://www.codegrepper.com/code-examples/csharp/unity+2d+rotate+towards+direction
        Vector3 targetDirection = target - transform.position;

        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Testing");
        // find new target waypoint
        if (collision.tag == "Waypoint")
        {
            bool notChosen = true;
            int prev = currentTarget;
            while (notChosen)
            {
                Debug.Log("entered");
                currentTarget = Random.Range(0, waypoints.Length - 1);

                // make sure new target is different
                if (currentTarget != prev || waypoints.Length <= 1)
                {
                    notChosen = false;
                }
            }
            
        }
    }


    public void Hit()
    {
        // Increase hit count by 1
        hitsByEgg++;

        // Destroys after 4 hits
        if(hitsByEgg >= 4)
        {
            Destroy(gameObject); // Kill itself
            gameCon.EnemyDestroyed(); // Update Controller
        }
        else
        {
            ColorChange(); // Show Damage
        }
    }

    private void ColorChange()
    {
        // Each color change depletes plane color to 80%
        // of current color
        energy *= 0.8f;
        Color newColor = new Color(1f, 0f, 0f, energy);
        GetComponent<Renderer>().material.color = newColor;
    }
}
