using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaypointScript : MonoBehaviour
{
    //-----------------------------------------------------------------------------------------------------------------
    // Code referenced from: http://wiki.unity3d.com/index.php/Camera_Shake 
    // Unify Community Wikipedia Page: Camera Shake
    public float shakeAmount;//The amount to shake this frame.
    public float shakeDuration;//The duration this frame.

    //Readonly values...
    float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
    float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
    float startDuration;//The initial shake duration, set when ShakeCamera is called.

    bool isRunning = false; //Is the coroutine running right now?
    bool killCamera = false;
  
    //-----------------------------------------------------------------------------------------------------------------
    // Spawning boundaries
    private Bounds WayBounds;

    // Waypoint's color
    Color newColor;

    // Variables for A Waypoint's health
    private int hitsByEgg = 0;
    private float energy = 1f;
    private bool hide = false;
    public GameObject wayPointCamera = null;

    void Start()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = 0f;

        WayBounds = new Bounds();

        WayBounds.center = spawnPos;
        WayBounds.size = new Vector3(30f, 30f, 0f);

        newColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            hide = !hide;
        }
        if(hide)
        {
            Hide();
        }
        else
        {
            Show();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Egg")
        {
            Hit();
            wayPointCamera.GetComponent<WayPointCamera>().cameraOn(transform);
            // Delete the Egg
            EggBehavior egg = collision.GetComponent<EggBehavior>();
            egg.Destroy();

        }
    }

    public void Hit()
    {
        // Increases hit count by 1
        hitsByEgg++;
        wayPointCamera.SetActive(true);
        if (hitsByEgg == 1)
        {
            ShakeCamera(1.0f, 1.0f);
        }
        if (hitsByEgg == 2)
        {
            ShakeCamera(2.0f, 2.0f);
        }
        if (hitsByEgg == 3)
        {
            ShakeCamera(3.0f, 3.0f);
        }
        
        // Respawns after 4 hits
        if (hitsByEgg >= 4)
        {      
            Respawn();
        }
        else
        {
            ColorChange(); // Show Damage
        }
    }

    private void Respawn()
    {
        Hide();
        // Random Spawn location inside bounds
        Vector3 pos;
        pos.x = WayBounds.min.x + Random.value * WayBounds.size.x;
        pos.y = WayBounds.min.y + Random.value * WayBounds.size.y;
        pos.z = 0;

        // Move Waypoint
        transform.position = pos;

        // Reset life span
        hitsByEgg = 0;
        energy = 1f;
        Show();
    }

    public void Hide()
    {
        // Make Waypoint transparent
        newColor.a = 0f;
        GetComponent<Renderer>().material.color = newColor;
    }

    public void Show()
    {
        // Make Waypoint visible
        newColor.a = energy;
        GetComponent<Renderer>().material.color = newColor; 
    }

    private void ColorChange()
    {
        energy -= 0.25f;
        newColor.a = energy;
        GetComponent<Renderer>().material.color = newColor;
    }


    //-----------------------------------------------------------------------------------------------------------------
    // Code referenced from: http://wiki.unity3d.com/index.php/Camera_Shake 
    // Unify Community Wikipedia Page: Camera Shake
    public void ShakeCamera(float amount, float duration)
    {

        shakeAmount += amount;//Add to the current amount.
        startAmount = shakeAmount;//Reset the start amount, to determine percentage.
        shakeDuration += duration;//Add to the current time.
        startDuration = shakeDuration;//Reset the start time.

        if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
    }
    IEnumerator Shake()
    {
        isRunning = true;
        //Get Original Pos and rot
        Vector3 defaultPos = transform.position;
        //Quaternion defaultRot = transform.position;
        while (shakeDuration > 0.04f)
        {
            //defaultPos.z = 0;
            Vector3 rotationAmount = defaultPos + Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation
            rotationAmount.z = 0;//Don't change the Z; it looks funny.     

            transform.position = rotationAmount;
            shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).

            shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
            shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.

            transform.rotation = Quaternion.Euler(rotationAmount);//Set the local rotation the be the rotation amount.

            transform.rotation = Quaternion.identity;

            yield return null;
        }
        wayPointCamera.GetComponent<WayPointCamera>().cameraOff();
        transform.rotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
        isRunning = false;
    }
    //-----------------------------------------------------------------------------------------------------------------
}
