using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenUpBehavior : MonoBehaviour
{
        public Text enemyCountText = null;
        public float speed = 20f;

        // 90 Degrees in 2 seconds
        public float heroRotateSpeed = 90f / 2f;

        public bool followMousePos = true;

        private float cooldown = 0.2f;

        private float nextFire = 0f;

        private int planesTouched = 0;

        private GameController gameCon = null;

    // Start is called before the first frame update
    void Start()
    {
        gameCon = FindObjectOfType<GameController>();
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
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }

        // Use keyboard keys
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                pos += ((speed * Time.smoothDeltaTime) * transform.up);
            }

            if (Input.GetKey(KeyCode.S))
            {
                pos -= ((speed * Time.smoothDeltaTime) * transform.up);
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
            }

            nextFire = Time.time + cooldown;
        }

        // Update Position
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        planesTouched++;
        Destroy(collision.gameObject);
        enemyCountText.text = "Planes Touched: " + planesTouched;
        gameCon.EnemyDestroyed();
    }
}
