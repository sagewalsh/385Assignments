using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] float MaxHealth;

    private float currHealth;

    public HealthBar bar;

    [SerializeField] float InvincTime;

    private float invincTimeStamp;

    private SpriteRenderer sr;

    [SerializeField] PlayerMovement movement;

    private Color c;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        currHealth = MaxHealth;
        bar.SetHealth(currHealth);
        invincTimeStamp = 0;
        sr = GetComponent<SpriteRenderer>();
        c = sr.color;
    }

    private void FixedUpdate()
    {
        if (Time.time < invincTimeStamp)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = c;
        }
    }

    public void TakeDamage(float health)
    {
        currHealth -= health;

        if (currHealth <= 0)
        {
            movement.Die();
        }

        bar.SetHealth(currHealth);
    }

    public void Heal(float health)
    {
        currHealth += health;

        if (currHealth > MaxHealth)
        {
            currHealth = MaxHealth;
        }

        bar.SetHealth(currHealth);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time > invincTimeStamp && collision.gameObject.tag == "Enemy")
        {
            TakeDamage(collision.GetComponent<UFOControl>().damage);
            invincTimeStamp = Time.time + InvincTime;
        }
    }
}
