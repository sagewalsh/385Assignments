using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoint : MonoBehaviour
{
    public float gravityScale, gravityMinRange, gravityMaxRange, planetRadius;
    public bool FinalPlanet = false;
    [SerializeField]
    [Range(0.001f, 0.1f)]
    [Tooltip("Min percent of total gravity scale that gravity will fuzz")]
    private float minGravityScaleFuzz = 0.01f;
    [SerializeField]
    [Range(0.001f, 0.1f)]
    [Tooltip("Max percent of total gravity scale that gravity will fuzz")]
    private float maxGravityScaleFuzz = 0.02f;

    private Transform thisTransform;
    private CircleCollider2D col;

    private void Start()
    {
        //Caches the gravitys position
        thisTransform = this.transform;

        //Gets the gravitys circle collider
        col = GetComponent<CircleCollider2D>();

        //Get the planet's radius
        planetRadius = this.transform.parent.GetComponent<CircleCollider2D>().radius;

        //Calculates the coliders Radius based on the planets size
        float colRadius = (gravityMaxRange + planetRadius) / thisTransform.parent.localScale.x;

        //Sets the radius of the Collider
        col.radius = colRadius;

        //Just prevents the player from getting caught in limbo between two planets. Makes one force a little bit stronger or weaker 
        //so the player continues flying to the next planet
        gravityScale *= 1f + Utilities.PossibleNegative() * Random.Range(minGravityScaleFuzz, maxGravityScaleFuzz);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //Sets the gravitational power set by the global variable
        float gravitationalPower = gravityScale;
        
        //Calculates distance between the gavity point position and the colliders position
        float dist = Vector2.Distance(col.transform.position, thisTransform.position);
        
        //Calculates where the maximum amount of gravity being applied ends. Basically where gravity starts to fall off as you
        //get away from the planet
        float min = planetRadius + gravityMinRange;

        if (dist > min)
        {
            //Reduce gravity of outer circle atmosphere
            gravitationalPower = (min + gravityMaxRange - dist) / gravityMaxRange * gravitationalPower;
        } 

        Vector3 dir = (thisTransform.position - col.transform.position) * gravitationalPower;
        
        //if collider that enters is a player, track and apply this force
        if(col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().ApplyPlanetForce(this, dir);
        }
        
        //otherwise, non-player objects should just get some force applied
        // else
        // {
        //     col.GetComponent<Rigidbody2D>().AddForce(dir);
        // }

        //if (col.CompareTag("Player"))
        //{
        //    col.transform.up = Vector3.MoveTowards(col.transform.up, -dir, gravitationalPower * Time.deltaTime + 90f);
        //}
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //stop tracking this planet once the player leaves it
        if(col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().StopTrackingForce(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gravityMinRange + planetRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gravityMaxRange + planetRadius);
    }
#endif
}
