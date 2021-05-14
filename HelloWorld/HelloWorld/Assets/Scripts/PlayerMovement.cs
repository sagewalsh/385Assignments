using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed, jumpPower;
    public SpriteRenderer render;
    public Sprite left, right;
    [SerializeField] private int MaxJumps;
    [SerializeField] private float onPlanetDrag = 3.5f;
    [SerializeField] 
    [Tooltip("Speed the player will adjust to the new up direction, as calculated by the summation of all external forces")]
    private float upAdjustmentSpeed = 0.1f;

    private int currJumps;
    private bool isGrounded;
    private float horizontal;
    private Transform thisTransform;
    private Rigidbody2D body;
    private Vector3 jumpDir;

    private Dictionary<GravityPoint, Vector3> trackedForces;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        thisTransform = this.transform;

        trackedForces = new Dictionary<GravityPoint, Vector3>();
    }

    private void Update()
    {
        //Left and Right arrows and A and D on a keyboard
        horizontal = Input.GetAxisRaw("Horizontal"); 

        //Space bar on keyboard
        //Forgot to add the "isGrounded" part that the video says
        //Would that mess up the double jump?
        if(Input.GetButtonDown("Jump") && currJumps < MaxJumps)
        {
            Jump();
        }  
    }

    private void Jump()
    {
        //jump direction is only up when its the first jump.
        //otherwise we just whatever direction we jumped in before
        //for any jumps after the first
        if (currJumps == 0)
        {
            jumpDir = thisTransform.up;
        }

        body.AddForce(jumpDir * jumpPower, ForceMode2D.Impulse);
        currJumps++;
    }

    private void FixedUpdate()
    {
        body.AddForce(thisTransform.right * horizontal * moveSpeed);
        if(moveSpeed < 0 || horizontal < 0)
        {
            render.sprite = left;
        }
        else
        {
            render.sprite = right;
        }

        //update the player's up direction based on all external forces
        Vector3 up = CalculateUp();
        thisTransform.up = Vector3.MoveTowards(thisTransform.up, -up, upAdjustmentSpeed * up.magnitude);
    }

    //track a force from a gravity source, so we can later calculate what the up direction is
    //based on the summation of all the currently applied external forces
    public void ApplyPlanetForce(GravityPoint gravitySource, Vector3 force)
    {
        body.AddForce(force);
        trackedForces[gravitySource] = force;
    }

    public void StopTrackingForce(GravityPoint gravitySource)
    {
        trackedForces.Remove(gravitySource);
    }

    //add all tracked external forces to determine where "up" is
    private Vector3 CalculateUp()
    {
        Vector3 result = Vector3.zero;
        foreach(Vector3 dir in trackedForces.Values)
        {
            result += dir;
        }
        return result;
    }

    private void OnTriggerStay2D(Collider2D obj)
    {
        if(obj.CompareTag("Planet"))
        {
            //body.gravityScale = 0;
            body.drag = onPlanetDrag;

            float distance = Mathf.Abs(obj.GetComponent<GravityPoint>().planetRadius - Vector2.Distance(thisTransform.position, obj.transform.position));
            if(distance < 1f)
            {
                isGrounded = distance < 0.1f;
            }
        }
    }

    //TODO: likely not needed
    //VERY MUCH NEEDED IF WE ARE INCLUDING THE SUN!!
    //private void OnTriggerExit2D(Collider2D obj)
    //{
    //    //If you're falling of a planet
    //    if (obj.CompareTag("Planet"))
    //    {
    //        //body.drag = 0.2f;

    //        //body.gravityScale = 2;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.CompareTag("Planet"))
        {
            //Debug.Log("Hit Planet");

            currJumps = 0;
            body.drag = onPlanetDrag;
        }
    }
}
