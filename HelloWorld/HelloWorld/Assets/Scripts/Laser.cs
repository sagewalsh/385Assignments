using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float dieTime;

    void Start()
    {
        dieTime = Time.time + 2.0f;
    }

    void Update()
    {
        if(Time.time > dieTime)
        {
            Die();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<UFOHealth>().TakeDamage(1);
            Die();
        }
    }



    public void Die()
    {
        Destroy(this.gameObject);
    }
}
