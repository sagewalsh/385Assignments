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

    [SerializeField] ParticleSystem hover;

    [SerializeField] private float secondsCount;
    public bool inPlanetGrav;

    public float maxOxygenSeconds;

    public OxygenBar bar;

    private ParticleSystem jump;

    private Dictionary<GravityPoint, Vector3> trackedForces;
    public GameConScript controller;

    private Animator anim;

    private PlayerHealth health; 

    private void Start()
    {
        health = GetComponent<PlayerHealth>();

        /*----------------------------------------------------------------------
        //Gets the Players Rigidbody Collider
        ------------------------------------------------------------------------*/
        body = GetComponent<Rigidbody2D>();

        jump = GetComponentInChildren<ParticleSystem>();
        
        /*----------------------------------------------------------------------
        //Just caches the players transform to be used in the rest of the code
        ------------------------------------------------------------------------*/
        thisTransform = this.transform;
        
        /*------------------------------------------------------------------------------------------------------------------------------------
        //This is just a dictionary of all of the forces being applied on the Player
        //to then calculate which direction is "UP" for the player. This helps for the scenario if you have three planets close together and 
        //you jump in the middle and three gravities are pulling on the player. This allows the player to always be rotating to the larger force
        --------------------------------------------------------------------------------------------------------------------------------------*/
        trackedForces = new Dictionary<GravityPoint, Vector3>();

        inPlanetGrav = true;
        secondsCount = 0;
        bar.SetMaxOxygen(maxOxygenSeconds);

        anim = GetComponent<Animator>();

        anim.SetInteger("Jumping", 0);
        
    }

    private void Update()
    {
        if(PauseMenu.isPaused)
        {
            return;
        }

        //Left and Right arrows and A and D on a keyboard
        horizontal = Input.GetAxisRaw("Horizontal"); 

        //Space bar on keyboard
        //Forgot to add the "isGrounded" part that the video says
        //Would that mess up the double jump?
        if((Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up")) && currJumps < MaxJumps)
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
        //jump direction is only up when its the first jump.
        //otherwise we just whatever direction we jumped in before
        //for any jumps after the first


        anim.SetInteger("Jumping", 1);

        if (currJumps == 0)
        {
            SFXManager.instance.PlaySound("PlayerJump");
            jumpDir = thisTransform.up;
            hover.Stop();
        }

        else if (currJumps == 1)
        {
            jump.Play();
        }

        /*------------------------------------------------------
        //Adds Force to the player in the up direction
        -------------------------------------------------------*/
        SFXManager.instance.PlaySound("PlayerJump");
        body.AddForce(jumpDir * jumpPower, ForceMode2D.Impulse);
        currJumps++;
    }

    private void FixedUpdate()
    {

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
        else if (!inPlanetGrav)
        {
            inPlanetGrav = true;
            bar.SetOxygen(maxOxygenSeconds);
            secondsCount = 0;
        }


        /*--------------------------------------------------------------------------------------------------
         Players movement
        ---------------------------------------------------------------------------------------------------*/
        if (inPlanetGrav)
        {
            body.AddForce(thisTransform.right * horizontal * moveSpeed);
        }
        

        if(moveSpeed < 0 || horizontal < 0)
        {
            render.sprite = left;
        }
        else
        {
            render.sprite = right;
        }
        
        


   }

  /*-----------------------------------------------------------------------------------------------
  //track a force from a gravity source, so we can later calculate what the up direction is
  //based on the summation of all the currently applied external forces
  ------------------------------------------------------------------------------------------------*/
    public void ApplyPlanetForce(GravityPoint gravitySource, Vector3 force)
    {
        body.AddForce(force);
        trackedForces[gravitySource] = force;
    }

    /*-----------------------------------------------------------------------------------------------
    //Removes the weaker force as the player leaves one planet
    ------------------------------------------------------------------------------------------------*/

    public void StopTrackingForce(GravityPoint gravitySource)
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
        if(obj.CompareTag("Planet") || obj.CompareTag("Finish"))
        {
            //Just sets the drag on the player
            body.drag = onPlanetDrag;

            //Findes the distance between the planets that the player is jumping between
            float distance = Mathf.Abs(obj.GetComponent<GravityPoint>().planetRadius - Vector2.Distance(thisTransform.position, obj.transform.position));
            
            
            if(distance < 1f)
            {
                
                isGrounded = distance < 0.1f;
            }
        }

        // if(obj.CompareTag("Gravity"))
        // {
        //     body.gravityScale = 0;
        // }
    }


    ////TODO: likely not needed
    //private void OnTriggerExit2D(Collider2D obj)
    //{
    //   //If you're falling of a planet
    //   if (obj.CompareTag("Gravity"))
    //   {
    //       body.drag = 0.2f;

    //       body.gravityScale = 1;
    //   }
    //}

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.CompareTag("Planet") || obj.collider.CompareTag("Finish"))
        {
            hover.Play();
            anim.SetInteger("Jumping", 0);
            currJumps = 0;
            body.drag = onPlanetDrag;

            if(obj.collider.CompareTag("Finish"))
            {
                controller.EndGame();
            }
        }
    }

    public void Die()
    {
        SFXManager.instance.PlaySound("PlayerDie");
        controller.RestartAfterDelay(3f);
        Destroy(this.gameObject);
    }
}
