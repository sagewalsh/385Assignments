using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Added code is sourced from https://www.youtube.com/watch?v=8eWbSN2T8TE the added code allows the enemy to fly around
//before it needs to lock on to the player

public class UFOControl : MonoBehaviour
{
    [SerializeField] 
    private float playerRange;
    [SerializeField]
    private float lineOfSight;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private bool followOnLookAway;
    [SerializeField] 
    public float damage;

    private PlayerMovement player;
    private bool enemyCanSeePlayer;
    private bool playerIsLookingAway;

    public Transform[] moveSpots;
    private int randomSpot;

    // Start is called before the first frame update
    private void Start()
    {
        //Caches the playerMovement Script
        player = FindObjectOfType<PlayerMovement>();
        randomSpot = Random.Range(0, moveSpots.Length);

    }

    // Update is called once per frame
    private void Update()
    {
        if(!player)
        {
            return;
        }

        //Always looks for players distance and sets the flag
        PlayerRangePing();


        //If the emeny isn't in sight move to waypoints
        if (!enemyCanSeePlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, movementSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f) {
                randomSpot = Random.Range(0, moveSpots.Length);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!player)
        {
            return;
        }

        if (!followOnLookAway && enemyCanSeePlayer)
        {
                Pursue();
                return;
        }
        
        CheckIfLookingAway();
        if (playerIsLookingAway && enemyCanSeePlayer)
        {
            Pursue();
        }
    }

    private void OnDrawGizmosSelected()
    {
       //Gizmos.DrawSphere(this.transform.position, lineOfSight);
    }

    private void Pursue()
    {
        //Debug.Log("Pursuing");
        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }
    private void CheckIfLookingAway()
    {
        if (player.transform.position.x < transform.position.x && player.transform.localScale.x < 0 || (player.transform.position.x > transform.position.x && player.transform.localScale.x > 0)) {
            playerIsLookingAway = true;
            return;
        }
        playerIsLookingAway = false;
    }

    private void PlayerRangePing()
    {
        playerRange = Vector3.Distance(this.transform.position, player.transform.position);
        
        if (playerRange > lineOfSight) enemyCanSeePlayer = false;
        else enemyCanSeePlayer = true;
    }
}
