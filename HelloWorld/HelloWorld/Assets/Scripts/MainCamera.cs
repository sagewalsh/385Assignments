using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] public Transform player;

    [SerializeField] public float zoomOut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!player)
        {
            return;
        }

        transform.position = new Vector3(player.position.x, player.position.y, zoomOut);
    }
}
