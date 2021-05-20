using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossiblePM : MonoBehaviour
{
    public float moveSpeed;
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

    [SerializeField] private float secondsCount;
    bool inPlanetGrav;

    public float maxOxygenSeconds;

    public OxygenBar bar;

    private Dictionary<GP, Vector3> trackedForces;
    public GameObject controller;
    public GP currentPlanet;

    private void Start()
    {
        /*----------------------------------------------------------------------
        //Gets the Players Rigidbody Collider
        ------------------------------------------------------------------------*/
        body = GetComponent<Rigidbody2D>();
        
        /*----------------------------------------------------------------------
        //Just caches the players transform to be used in the rest of the code
        ------------------------------------------------------------------------*/
        thisTransform = this.transform;
        
        /*------------------------------------------------------------------------------------------------------------------------------------
        //This is just a dictionary of all of the forces being applied on the Player
        //to then calculate which direction is "UP" for the player. This helps for the scenario if you have three planets close together and 
        //you jump in the middle and three gravities are pulling on the player. This allows the player to always be rotating to the larger force
        --------------------------------------------------------------------------------------------------------------------------------------*/
        trackedForces = new Dictionary<GP, Vector3>();

        inPlanetGrav = true;
        secondsCount = 0;
        bar.SetMaxOxygen(maxOxygenSeconds);
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

        if (secondsCount > maxOxygenSeconds)
        {
            Die();
        }
    }

    private void Jump()
    {
        // //jump direction is only up when its the first jump.
        // //otherwise we just whatever direction we jumped in before
        // //for any jumps after the first

        // if (currJumps == 0)
        // {
        //     jumpDir = thisTransform.up;
        // }

        // /*------------------------------------------------------
        // //Addes Force to the player in the up direction
        // -------------------------------------------------------*/

        // body.AddForce(jumpDir * jumpPower, ForceMode2D.Impulse);
        // currJumps++;
        Transform gravityEdge = currentPlanet.GetComponent<CircleCollider2D>().transform;
        Vector2 position = thisTransform.position;
        if(currJumps == 0)
        {
            position = new Vector2(gravityEdge.position.x/2, gravityEdge.position.y/2);
        }
        if(currJumps == 1)
        {
            position = new Vector2(gravityEdge.position.x, gravityEdge.position.y);
        }
        body.MovePosition(position);
    }

    private void FixedUpdate()
    {
        /*--------------------------------------------------------------------------------------------------
         Players movement
        ---------------------------------------------------------------------------------------------------*/

        body.AddForce(thisTransform.right * horizontal * moveSpeed);
        if(moveSpeed < 0 || horizontal < 0)
        {
            render.sprite = left;
        }
        else
        {
            render.sprite = right;
        }
        
        /*-----------------------------------------------------------------------------------------------
        //update the player's up direction based on all external forces
        ------------------------------------------------------------------------------------------------*/
        Vector3 up = CalculateUp();
        thisTransform.up = Vector3.MoveTowards(thisTransform.up, -up, upAdjustmentSpeed * up.magnitude);

        // handle the players oxygen
        if (trackedForces.Count == 0)
        {
            inPlanetGrav = false;
            secondsCount += Time.deltaTime;
            bar.SetOxygen(maxOxygenSeconds - secondsCount);

        }
        else if (!inPlanetGrav) { 
            inPlanetGrav = true;
            bar.SetOxygen(maxOxygenSeconds);
            secondsCount = 0; 
        }


   }

  /*-----------------------------------------------------------------------------------------------
  //track a force from a gravity source, so we can later calculate what the up direction is
  //based on the summation of all the currently applied external forces
  ------------------------------------------------------------------------------------------------*/
    public void ApplyPlanetForce(GP gravitySource, Vector3 force)
    {
        body.AddForce(force);
        trackedForces[gravitySource] = force;
    }

    /*-----------------------------------------------------------------------------------------------
    //Removes the weaker force as the player leaves one planet
    ------------------------------------------------------------------------------------------------*/

    public void StopTrackingForce(GP gravitySource)
    {
        trackedForces.Remove(gravitySource);
    }
    
    /*-----------------------------------------------------------------------------------------------
    //add all tracked external forces to determine where "up" is from the players position
    ------------------------------------------------------------------------------------------------*/

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
            //Just sets the drag on the player
            body.drag = onPlanetDrag;

            currentPlanet = obj.GetComponent<GP>();

            //Findes the distance between the planets that the player is jumping between
            float distance = Mathf.Abs(currentPlanet.planetRadius - Vector2.Distance(thisTransform.position, obj.transform.position));
            
            
            if(distance < 1f)
            {
                isGrounded = distance < 0.1f;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.CompareTag("Planet"))
        {
            currJumps = 0;
            body.drag = onPlanetDrag;
        }
    }

    private void Die()
    {
        // Destroy(gameObject);
        // controller.GetComponent<GameConScript>().Restart();
    }
}
