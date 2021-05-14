using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OGPlayer : MonoBehaviour
{
    private Rigidbody2D body;
    public float moveSpeed, jumpPower;
    private bool isGrounded;
    private float horizontal;
    public SpriteRenderer render;
    public Sprite left, right;
    [SerializeField] int MaxJumps;
    private int currJumps;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Left and Right arrows and A and D on a keyboard
        horizontal = Input.GetAxisRaw("Horizontal"); 

        //Space bar on keyboard
        //Forgot to add the "isGrounded" part that the video says
        //Would that mess up the double jump?
        if(Input.GetButtonDown("Jump") && currJumps < MaxJumps)
        {
            body.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            currJumps++;
        }  
    }

    void FixedUpdate()
    {
        body.AddForce(transform.right * horizontal * moveSpeed);
        if(moveSpeed < 0 || horizontal < 0)
        {
            render.sprite = left;
        }
        else
        {
            render.sprite = right;
        }
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if(obj.CompareTag("Planet"))
        {
            body.gravityScale = 0;
            body.drag = 1f;

            float distance = Mathf.Abs(obj.GetComponent<GravityPoint>().planetRadius - Vector2.Distance(transform.position, obj.transform.position));
            if(distance < 1f)
            {
                isGrounded = distance < 0.1f;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        //If you're falling of a planet
        if(obj.CompareTag("Planet"))
        {
            body.drag = 0.2f;
            
            body.gravityScale = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.collider.CompareTag("Planet"))
        {
            Debug.Log("Hit Planet");

            currJumps = 0;
        }
    }
}