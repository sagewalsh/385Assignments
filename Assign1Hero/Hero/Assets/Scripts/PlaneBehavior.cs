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



    void Start()
    {
        gameCon = FindObjectOfType<GameController>();
    }


    void Update(){}


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
