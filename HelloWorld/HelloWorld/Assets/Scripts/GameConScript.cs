using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // Press Q to quit game application
        if(Input.GetKey(KeyCode.Q))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }  

        //Press R to restart the scene you are in
        if(Input.GetKey(KeyCode.R))
        {
            Restart();
        }
    }

    //Call this method to restart whatever scene you are in 
    public void Restart()
    {
        string scene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
