using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenUpBehavior : MonoBehaviour
{
        public float speed = 20f;

        // 90 Degrees in 2 seconds
        public float heroRotateSpeed = 90f / 2f;

        public bool followMousePos = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M))
        {
            followMousePos = !followMousePos;
        }

        Vector3 pos = transform.position;

        if(followMousePos)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                pos += ((speed * Time.smoothDeltaTime) * transform.up);
            }

            if (Input.GetKey(KeyCode.S))
            {
                pos -= ((speed * Time.smoothDeltaTime) * transform.up);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(transform.forward, -heroRotateSpeed * Time.smoothDeltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(transform.forward, heroRotateSpeed * Time.smoothDeltaTime);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject e = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.rotation = transform.rotation;
        }

        transform.position = pos;
    }
}
