using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    // Lerp Stuff
    private TimedLerp lerp = new TimedLerp(2f, 2f);
    private const float kDeltaSize = 8f; // twice current size



    // Variable for the Plane's Health
    private int hitsByEgg = 0;
    private float energy = 1f;

    // Game Controller
    private GameController gameCon = null;

    private GameController gameController;

    [SerializeField] public float speed;

    [SerializeField] public float rotateSpeed;

    private const float kScaleRate = 2f / 60f;

    private const float kRotateRate = 90f / 60f;

    private int currentTarget;

    private int mStateFrameTick = 0;

    [SerializeField] public Sprite egg;

    [SerializeField] public Sprite stunned;

    private int kSizeChangeFrames = 60;

    private int kRotateFrames = 60;

    private bool eggState;

    private bool stunState;

    // Enemy States
    public enum EnemyState
    {
        eEnlargeState,

        eShrinkState,

        ePatrolState,

        eCCWState,

        eCWState,

        eChaseState,

        eStunnedState,

        eEggState
    }

    public EnemyState mState;

    void Start()
    {

        GetComponent<SpriteRenderer>().color = Color.white;

        gameCon = FindObjectOfType<GameController>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();


        currentTarget = Random.Range(0, gameController.GetComponent<GameController>().waypoints.Length);

        foreach(GameObject w in gameController.GetComponent<GameController>().waypoints)
        {
            Debug.Log(w.gameObject.tag);
        }

        mState = EnemyState.ePatrolState;

        eggState = false;

        stunState = false;
    }

    private void UpdateFSM()
    {
        switch(mState)
        {
            case EnemyState.eEnlargeState:
                ServiceEnlargeState();
                break;

            case EnemyState.eShrinkState:
                ServiceShrinkState();
                break;

            case EnemyState.ePatrolState:
                ServicePatrolState();
                break;

            case EnemyState.eCWState:
                ServiceCWState();
                break;

            case EnemyState.eCCWState:
                ServiceCCWState();
                break;

            case EnemyState.eChaseState:
                ServiceChaseState();
                break;

            case EnemyState.eStunnedState:
                ServiceStunnedState();
                break;

            case EnemyState.eEggState:
                ServiceEggState();
                break;
        }
    }

    private void ServiceEnlargeState()
    {
        if (mStateFrameTick > kSizeChangeFrames)
        {
            mState = EnemyState.eShrinkState;
            mStateFrameTick = 0;
        }

        else
        {
            float s = transform.localScale.x;
            s += kScaleRate;
            transform.localScale = new Vector3(s, s, 1);
            mStateFrameTick++;
        }
    }

    private void ServiceCCWState()
    {
        if (GetComponent<SpriteRenderer>().color != Color.red)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (mStateFrameTick > kRotateFrames)
        {
            mState = EnemyState.eCWState;
            mStateFrameTick = 0;
        }

        else
        {
            mStateFrameTick++;
            Vector3 angles = transform.rotation.eulerAngles;
            angles.z -= kRotateRate;
            transform.rotation = Quaternion.Euler(0, 0, angles.z);
        }
        
    }

    private void ServiceCWState()
    {
        if (mStateFrameTick > kRotateFrames)
        {
            mState = EnemyState.eChaseState;
            mStateFrameTick = 0;
        }

        else
        {
            mStateFrameTick++;
            Vector3 angles = transform.rotation.eulerAngles;
            angles.z += kRotateRate;
            transform.rotation = Quaternion.Euler(0, 0, angles.z);
        }
    }

    private void ServicePatrolState()
    {

        //float step = speed * Time.deltaTime;

        //float rotStep = rotateSpeed * Time.deltaTime;

        Vector3 target = gameController.GetComponent<GameController>().waypoints[currentTarget].transform.position;

        //transform.position = Vector2.MoveTowards(transform.position, target, step);

        transform.position += (speed * Time.smoothDeltaTime) * transform.up;

        //transform.position = Vector3.MoveTowards(target.x, target.y, 0);

        // referenced https://www.codegrepper.com/code-examples/csharp/unity+2d+rotate+towards+direction
        Vector3 targetDirection = target - transform.position;

        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        PointAtPosition(target, rotateSpeed);

        checkWaypointDist();
    }

    private void ServiceChaseState()
    {
        bool found = false;
        foreach(Collider2D obj in Physics2D.OverlapCircleAll(transform.position, 40))
        {
            if (obj.tag == "Player")
            {
                found = true;

                float step = speed * Time.deltaTime;

                float rotStep = rotateSpeed * Time.deltaTime;

                Vector3 target = obj.transform.position;

                transform.position = Vector2.MoveTowards(transform.position, target, step);

                //transform.position = Vector3.MoveTowards(target.x, target.y, 0);

                // referenced https://www.codegrepper.com/code-examples/csharp/unity+2d+rotate+towards+direction
                Vector3 targetDirection = target - transform.position;

                //get the angle from current direction facing to desired target
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

                //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                Debug.Log("Chasing");
                //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
                //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                PointAtPosition(target, 100);
            }
        }

        // player outside radius
        if (!found)
        {
            mState = EnemyState.eEnlargeState;
        }
    }

    private void ServiceStunnedState()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        angles.z -= kRotateRate;
        transform.rotation = Quaternion.Euler(0, 0, angles.z);

        if (!stunState)
        {
            GetComponent<SpriteRenderer>().sprite = stunned;
            stunState = true;
        }
    }

    private void ServiceShrinkState()
    {
        if (mStateFrameTick > kSizeChangeFrames)
        {
            mState = EnemyState.ePatrolState;
            mStateFrameTick = 0;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        else
        {
            float s = transform.localScale.x;
            s -= kScaleRate;
            transform.localScale = new Vector3(s, s, 1);
            mStateFrameTick++;
        }
    }

    private void ServiceEggState()
    {
        if (!eggState)
        {
            GetComponent<SpriteRenderer>().sprite = egg;
            eggState = true;
        }
        
    }

    void FixedUpdate()
    {
        UpdateFSM();

        if (lerp.LerpIsActive())
        {
            //Debug.Log("Lerp!");
            Vector3 s = lerp.UpdateLerp();
            transform.position = s;
            // move
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // find new target waypoint
    //    if (collision.tag == "Waypoint" && collision.gameObject.GetInstanceID() == gameController.GetComponent<GameController>().waypoints[currentTarget].GetInstanceID()){
    //        bool notChosen = true;
    //        int prev = currentTarget;

    //        // random target
    //        if (gameController.isRandom)
    //        {
    //            while (notChosen)
    //            {
    //                currentTarget = Random.Range(0, gameController.GetComponent<GameController>().waypoints.Length);

    //                // make sure new target is different
    //                if (currentTarget != prev)
    //                {
    //                    notChosen = false;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (currentTarget == gameController.GetComponent<GameController>().waypoints.Length - 1)
    //            {
    //                currentTarget = 0;
    //            }
    //            else
    //            {
    //                currentTarget++;
    //            }
    //        }
    //    }
    //}

    private void checkWaypointDist()
    {
        float dist = Vector2.Distance(transform.position, gameController.GetComponent<GameController>().waypoints[currentTarget].transform.position);
        if (dist <= 25)
        {
            bool notChosen = true;
            int prev = currentTarget;

            // random target
            if (gameController.isRandom)
            {
                while (notChosen)
                {
                    currentTarget = Random.Range(0, gameController.GetComponent<GameController>().waypoints.Length);

                    // make sure new target is different
                    if (currentTarget != prev)
                    {
                        notChosen = false;
                    }
                }
            }
            else
            {
                if (currentTarget == gameController.GetComponent<GameController>().waypoints.Length - 1)
                {
                    currentTarget = 0;
                }
                else
                {
                    currentTarget++;
                }
            }
        }
    }

    public void Hit()
    {
        // Increase hit count by 1
        hitsByEgg++;

        // Destroys after 4 hits
        if(hitsByEgg >= 1)
        {
            Destroy(gameObject); // Kill itself
            gameCon.EnemyDestroyed(); // Update Controller
        }
        else
        {
            ColorChange(); // Show Damage
        }
    }

    private void ColorChange()
    {
        // Each color change depletes plane color to 80%
        // of current color
        energy *= 0.8f;
        Color newColor = new Color(1f, 0f, 0f, energy);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void StartLerp(float lerpMultiplier, Vector3 dir)
    {
        Vector3 currentLocation = transform.position;
        Vector2 finalLocation = (currentLocation + dir.normalized * 4 * lerpMultiplier);
        Debug.Log("Starting lerp from " + transform.position + " to " + finalLocation);
        lerp.SetLerpParms(lerpMultiplier, lerpMultiplier);
        lerp.BeginLerp(currentLocation, finalLocation);
    }

    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.position;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }
}


