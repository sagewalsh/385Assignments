using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void ControlScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
