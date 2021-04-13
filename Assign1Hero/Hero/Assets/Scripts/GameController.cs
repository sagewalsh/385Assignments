using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int maxPlanes = 10;
    private int numberOfPlanes = 0;
    private int planesDestroyed = 0;
    private int numberOfEggs = 0;

    public Text enemyText = null;
    public Text eggText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyText.text = "ENEMY: Count(" + numberOfPlanes +
                         ") Destroyed(" + planesDestroyed + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
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

        enemyText.text = "ENEMY: Count(" + numberOfPlanes +
                         ") Destroyed(" + planesDestroyed + ")";

        eggText.text = "EGG: OnScreen(" + numberOfEggs + ")";
    }

    public void EnemyDestroyed()
    {
        numberOfPlanes--;
        planesDestroyed++;
    }

    public void EggCreated()
    {
        numberOfEggs++;
    }

    public void EggDestroyed()
    {
        numberOfEggs--;
    }
}
