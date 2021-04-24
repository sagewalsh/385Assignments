using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    // Travel speed of the egg
    public const float eggSpeed = 40f;

    // Game Controller
    private GameController gameCon = null;

    // Camera Script
    private CameraSupport s = null;



    void Start()
    {
        gameCon = FindObjectOfType<GameController>();
        s = Camera.main.GetComponent<CameraSupport>();
    }



    void Update()
    {
        // Travel
        transform.position += transform.up * (eggSpeed * Time.smoothDeltaTime);
        
        // If outside the game window: kill itself
        Vector3 pos = transform.position;
        if( !s.GetWorldBounds().Contains(pos))
        {
            Destroy(transform.gameObject);
            gameCon.EggDestroyed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If Egg hits a plane
        if(collision.tag == "Plane")
        {
            // Get Plane Script
            PlaneBehavior plane = collision.GetComponent<PlaneBehavior>();
            plane.Hit();

            Destroy();
        }
    }

    public void Destroy()
    {
        // Delete the Egg
        Destroy(gameObject);

        // Update the Controller
        gameCon.EggDestroyed();
    }
}
