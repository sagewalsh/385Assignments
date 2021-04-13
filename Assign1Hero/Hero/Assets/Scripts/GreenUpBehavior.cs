using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenUpBehavior : MonoBehaviour
{
        // Variables for UI text
        public Text enemyCountText = null;
        public Text heroControl = null;

        
        // Variables to control Hero
        public float speed = 20f;

        // 90 Degrees in 2 seconds
        public float heroRotateSpeed = 90f / 2f;

        private Rigidbody2D rb;

        public bool followMousePos = true;


        // Varaibles to Control Egg Shooting
        private float cooldown = 0.2f;

        private float nextFire = 0f;


        // Variables to control Plane Destruction
        private int planesTouched = 0;

        private GameController gameCon = null;

    // Start is called before the first frame update
    void Start()
    {
        gameCon = FindObjectOfType<GameController>();
        rb = GetComponent<Rigidbody2D>();

        enemyCountText.text = "PlanesTouched(0)";
    }

    void Update()
    {

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
            if (Input.GetKey(KeyCode.W))
            {
                speed += 0.1f;
            }

            if (Input.GetKey(KeyCode.S))
            {            
                speed -= 0.1f;
            }
        }

        // Rotate
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.forward, -heroRotateSpeed * Time.smoothDeltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.forward, heroRotateSpeed * Time.smoothDeltaTime);
        }

        // Shoot Eggs
        if(Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
            if(e != null)
            {
                e.transform.localPosition = transform.localPosition;
                e.transform.rotation = transform.rotation;
                gameCon.EggCreated();
            }

            nextFire = Time.time + cooldown;
        }

        // Update Position
        transform.position = pos;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Plane")
        {
            planesTouched++;
            Destroy(collision.gameObject);
            enemyCountText.text = "PlanesTouched(" + planesTouched + ")";
            gameCon.EnemyDestroyed();
        }
    }
}
