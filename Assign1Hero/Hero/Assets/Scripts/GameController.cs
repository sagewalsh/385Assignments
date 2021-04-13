using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int maxPlanes = 10;

    private int numberOfPlanes = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }   
        if(numberOfPlanes < maxPlanes)
        {
            CameraSupport s = Camera.main.GetComponent<CameraSupport>();


            GameObject e = Instantiate(Resources.Load("Prefabs/PlaneEnemy") as GameObject);
            Vector3 pos;
            pos.x = s.Get90Bounds().min.x + Random.value * s.Get90Bounds().size.x;
            pos.y = s.Get90Bounds().min.y + Random.value * s.Get90Bounds().size.y;
            pos.z = 0;

            e.transform.localPosition = pos;
            numberOfPlanes++;
        }
    }

    public void EnemyDestroyed()
    {
        numberOfPlanes--;
    }
}
