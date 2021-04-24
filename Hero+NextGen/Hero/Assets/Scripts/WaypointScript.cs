using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    // Spawning boundaries
    private Bounds WayBounds;

    // Waypoint's color
    Color newColor;

    // Variables for A Waypoint's health
    private int hitsByEgg = 0;
    private float energy = 1f;

    private bool hide = false;
    
    void Start()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = 0f;

        WayBounds = new Bounds();

        WayBounds.center = spawnPos;
        WayBounds.size = new Vector3(30f, 30f, 0f);

        newColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            hide = !hide;
        }
        if(hide)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Egg")
        {
            Hit();

            // Delete the Egg
            EggBehavior egg = collision.GetComponent<EggBehavior>();
            egg.Destroy();
        }
    }

    public void Hit()
    {
        // Increases hit count by 1
        hitsByEgg++;

        // Respawns after 4 hits
        if(hitsByEgg >= 4)
        {      
            Respawn();  
        }
        else
        {
            ColorChange(); // Show Damage
        }
    }

    private void Respawn()
    {
        Hide();

        // Random Spawn location inside bounds
        Vector3 pos;
        pos.x = WayBounds.min.x + Random.value * WayBounds.size.x;
        pos.y = WayBounds.min.y + Random.value * WayBounds.size.y;
        pos.z = 0;

        // Move Waypoint
        transform.position = pos;

        // Reset life span
        hitsByEgg = 0;
        energy = 1f;
        Show();
    }

    public void Hide()
    {
        // Make Waypoint transparent
        newColor.a = 0f;
        GetComponent<Renderer>().material.color = newColor;
    }

    public void Show()
    {
        // Make Waypoint visible
        newColor.a = energy;
        GetComponent<Renderer>().material.color = newColor; 
    }

    private void ColorChange()
    {
        energy -= 0.25f;
        newColor.a = energy;
        GetComponent<Renderer>().material.color = newColor;
    }
}
