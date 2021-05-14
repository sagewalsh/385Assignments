using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GravityPoint : MonoBehaviour
{
    public float gravityScale, planetRadius, gravityMinRange, gravityMaxRange;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D obj)
    {
        float gravitationalPower = gravityScale;
        float dist = Vector2.Distance(obj.transform.position, transform.position);
        if(dist > (planetRadius + gravityMinRange))
        {
            float min = planetRadius + gravityMinRange;

            //Reduce gravity of outer circle atmosphere
            gravitationalPower = (((min + gravityMaxRange) - dist) / gravityMaxRange) * gravitationalPower;
        } 

        Vector3 dir = (transform.position - obj.transform.position) * gravityScale;
        obj.GetComponent<Rigidbody2D>().AddForce(dir);
        if(obj.CompareTag("Player"))
        {
            obj.transform.up = Vector3.MoveTowards(obj.transform.up, -dir, gravityScale * Time.deltaTime + 90f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gravityMinRange + planetRadius);
        Gizmos.DrawWireSphere(transform.position, gravityMaxRange + planetRadius);
    }
}