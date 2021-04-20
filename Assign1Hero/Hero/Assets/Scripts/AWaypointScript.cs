using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWaypointScript : MonoBehaviour
{
    // Spawning boundaries
    private Bounds ABounds;

    // Variables for A Waypoint's health
    private int hitsByEgg = 0;
    private float energy = 1f;
    
    void Start()
    {

        Vector3 spawnPos = transform.position;
        spawnPos.z = 0f;

        ABounds = new Bounds();

        ABounds.center = spawnPos;
        ABounds.size = new Vector3(30f, 30f, 0f);

        
        Color newColor = new Color(0f, 0f, 1f, energy);   
        GetComponent<Renderer>().material.color = newColor;

    }

    void Update()
    {}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Egg")
        {
            Hit();
        }
        if(collision.tag == "Player")
        {
            Respawn();
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
        // Make Waypoint transparent
        Color newColor = new Color (0f, 0f, 1f, 0f);
        GetComponent<Renderer>().material.color = newColor;

        // Move Waypoint
        Vector3 pos;
        pos.x = ABounds.min.x + Random.value * ABounds.size.x;
        pos.y = ABounds.min.y + Random.value * ABounds.size.y;
        pos.z = 0;

        transform.position = pos;

        // Reset life span
        hitsByEgg = 0;

        // Make Waypoint visible
        energy = 1f;
        newColor = new Color(0f, 0f, 1f, energy);   
        GetComponent<Renderer>().material.color = newColor; 
    }
    private void ColorChange()
    {
        energy *= 0.8f;
        Color newColor = new Color(0f, 0f, 1f, energy);
        GetComponent<Renderer>().material.color = newColor;
    }
}
