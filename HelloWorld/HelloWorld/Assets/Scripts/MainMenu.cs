using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("FinalSceneWithoutTutorial");
    }

    public void PlayGameTutorial()
    {
        SceneManager.LoadScene("FinalSceneWithTutorial");
    }

    public void ControlScene()
    {
        SceneManager.LoadScene("Controls");
    }
}
