using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenUpBehavior : MonoBehaviour
{
        // Variables for UI text
        public Text enemyCountText = null;
        public Text heroControl = null;
        public Text coolText = null;

        
        // Variables to control Hero
        public float speed = 20f;
        public float heroRotateSpeed = 90f / 2f;
        private Rigidbody2D rb;
        public bool followMousePos = true;


        // Varaibles to Control Egg Shooting
        private float cooldown = 0.2f;
        private float nextFire = 0f;


        // Variables to control Plane Destruction
        private int planesTouched = 0;

        private GameController gameCon = null;



    void Start()
    {
        // Get the Game Controller
        gameCon = FindObjectOfType<GameController>();

        // Get this object's Rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Start text
        enemyCountText.text = "Touched(0)";
        coolText.text = "Fire Power Ready!";
    }

    void Update()
    {
        // Toggle between mouse and keyboard controls
        if(Input.GetKeyDown(KeyCode.M))
        {
            followMousePos = !followMousePos;
        }


        Vector3 pos = transform.position;

        // Follow Mouse
        if(followMousePos)
        {
            heroControl.text = "HERO: Control(Mouse)";
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }

        // Use keyboard keys
        else
        {
            heroControl.text = "HERO: Control(Keys)";

            // Speed up
            if (Input.GetKey(KeyCode.W))
            {
                speed += 0.1f;
            }

            // Speed up in opposite direction
            if (Input.GetKey(KeyCode.S))
            {            
                speed -= 0.1f;
            }
        }

        // Rotate Clockwise
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.forward, -heroRotateSpeed * Time.smoothDeltaTime);
        }

        // Rotate Counterclockwise
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.forward, heroRotateSpeed * Time.smoothDeltaTime);
        }

        // Shoot Eggs
        if(Time.time > nextFire)
        {
            coolText.text = "Fire Power Ready!";
            if(Input.GetKey(KeyCode.Space))
            {
                GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
                if(e != null)
                {
                    // Shoot eggs: from arrow pos, in arrow direction
                    e.transform.localPosition = transform.localPosition;
                    e.transform.rotation = transform.rotation;
                    gameCon.EggCreated();
                }

                // Cool down between egg shots
                nextFire = Time.time + cooldown;
            }
        }
        else
        {
            coolText.text = "Cooldown Fire Power.";
        }

        // Update Position and speed
        transform.position = pos;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If Arrow collided with a Plane
        if(collision.tag == "Plane")
        {
            planesTouched++;
            enemyCountText.text = "Touched(" + planesTouched + ")";

            // Delete Plane
            Destroy(collision.gameObject);
            gameCon.EnemyDestroyed();
        }
    }
}
