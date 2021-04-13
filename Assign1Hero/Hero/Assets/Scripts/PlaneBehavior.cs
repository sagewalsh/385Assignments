using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    private int hitsByEgg = 0;
    private float energy = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        hitsByEgg++;
        if(hitsByEgg >= 4)
        {
            Destroy(gameObject);
        }
        else
        {
            ColorChange();
        }
    }

    private void ColorChange()
    {
        energy *= 0.8f;
        Color newColor = new Color(1f, 0f, 0f, energy);
        GetComponent<Renderer>().material.color = newColor;
    }
}
