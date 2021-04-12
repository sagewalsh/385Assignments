using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{

    public const float eggSpeed = 40f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * (eggSpeed * Time.smoothDeltaTime);
        
        // If outside the game window: kill itself
        Vector3 pos = transform.position;
        if(pos.x > 273 || pos.x < -274 || pos.y > 100 || pos.y < -100)
        {
            Destroy(transform.gameObject);
        }
    }
}
