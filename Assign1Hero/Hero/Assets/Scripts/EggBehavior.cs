using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{

    public const float eggSpeed = 40f;

    private GameController gameCon = null;
    private CameraSupport s = null;


    // Start is called before the first frame update
    void Start()
    {
        gameCon = FindObjectOfType<GameController>();
        s = Camera.main.GetComponent<CameraSupport>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * (eggSpeed * Time.smoothDeltaTime);
        
        // If outside the game window: kill itself
        Vector3 pos = transform.position;

        if( !s.GetWorldBounds().Contains(pos))
        {
            Destroy(transform.gameObject);
            gameCon.EggDestroyed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Plane")
        {
            PlaneBehavior plane = collision.GetComponent<PlaneBehavior>();
            plane.Hit();
            Destroy(gameObject);
            gameCon.EggDestroyed();
            gameCon.EnemyDestroyed();
        }
    }
}
