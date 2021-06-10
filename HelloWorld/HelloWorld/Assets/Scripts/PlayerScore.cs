using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int currScore;

    [SerializeField] public UIManager UI;

    [SerializeField] public OxygenBar oBar;

    [SerializeField] public PlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
        currScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UI.score = currScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Coin pickup
        if (collision.tag == "Coin")
        {
            currScore += collision.GetComponent<Coin>().value;
            oBar.AddOxygen(collision.GetComponent<Coin>().oxygen);
            health.Heal(collision.GetComponent<Coin>().healAmount);
            SFXManager.instance.PlaySound("CoinPickup");
            Destroy(collision.gameObject);
        }
    }

    public void AddScore(int score)
    {
        currScore += score;
    }
}
