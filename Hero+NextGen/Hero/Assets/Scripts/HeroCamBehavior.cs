using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamBehavior : MonoBehaviour
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
 
	bool isRunning = false;	//Is the coroutine running right now?
//-----------------------------------------------------------------------------------------------------------------

    public Transform hero = null;
    Vector3 pos;
    Vector3 heroPos;
    private bool followMouse = true;

    void Start()
    {
        pos = hero.transform.position;
        pos.z = -10;
        transform.position = pos;
    }

    void LateUpdate()
    {
        // Toggle between mouse and keyboard controls
        if(Input.GetKeyDown(KeyCode.M))
        {
            followMouse = !followMouse;
        }

        heroPos = hero.transform.position;
        heroPos.z = -10;
        
        if(followMouse)
        {
            // Duration: 0.5s / TimeLerp Rate: 8
            transform.position = Vector3.Lerp(pos, heroPos, 0.5f/8.0F);
        }
        else
        {
            transform.position = heroPos;
        }
        pos = transform.position;
    }


//-----------------------------------------------------------------------------------------------------------------
// Code referenced from: http://wiki.unity3d.com/index.php/Camera_Shake 
// Unify Community Wikipedia Page: Camera Shake
	public void ShakeCamera(float amount, float duration) {
 
		shakeAmount += amount;//Add to the current amount.
		startAmount = shakeAmount;//Reset the start amount, to determine percentage.
		shakeDuration += duration;//Add to the current time.
		startDuration = shakeDuration;//Reset the start time.
 
		if(!isRunning) StartCoroutine (Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
	}
	IEnumerator Shake() {
		isRunning = true;
 
		while (shakeDuration > 0.01f) {
			Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation
			rotationAmount.z = 0;//Don't change the Z; it looks funny.
 
			shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).
 
			shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
			shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.

            transform.localRotation = Quaternion.Euler (rotationAmount);//Set the local rotation the be the rotation amount.
 
			yield return null;
		}
		transform.localRotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
		isRunning = false;
	}
//-----------------------------------------------------------------------------------------------------------------
}
