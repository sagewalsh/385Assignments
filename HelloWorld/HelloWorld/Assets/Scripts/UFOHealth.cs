using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    private bool takenDamage = false;
    private bool newSpriteSet = false;
    public Sprite AlienCracked;
    [SerializeField] private int currHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public int scoreBonus;
    [SerializeField] public PlayerScore playerScore;

    Animator anim;

    private void Start()
    {
        currHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void Die()
    {
        SFXManager.instance.PlaySound("EnemyExplosion");
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        //playerScore.AddScore(scoreBonus);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        takenDamage = true;
    }

    private void Update()
    {
        if (takenDamage == true && newSpriteSet == false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = AlienCracked;
            anim.SetBool("alienHit", true);
            newSpriteSet = true;
        }
        if (currHealth <= 0)
        {
            Die();
        }
    }
}
