using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField] private int currHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public int scoreBonus;
    [SerializeField] public PlayerScore playerScore;

    private void Start()
    {
        currHealth = maxHealth;
    }

    public void Die()
    {
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        playerScore.AddScore(scoreBonus);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
    }

    private void Update()
    {
        if (currHealth <= 0)
        {
            Die();
        }
    }
}
