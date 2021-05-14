using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoint : MonoBehaviour
{
    public float gravityScale, planetRadius, gravityMinRange, gravityMaxRange;
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
        thisTransform = this.transform;
        col = GetComponent<CircleCollider2D>();
        float colRadius = (gravityMaxRange + planetRadius) / thisTransform.parent.localScale.x;
        col.radius = colRadius;

        //apply gravity fuzzing
        gravityScale *= 1f + Utilities.PossibleNegative() * Random.Range(minGravityScaleFuzz, maxGravityScaleFuzz);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        float gravitationalPower = gravityScale;
        float dist = Vector2.Distance(col.transform.position, thisTransform.position);
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
        else
        {
            col.GetComponent<Rigidbody2D>().AddForce(dir);
        }

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
