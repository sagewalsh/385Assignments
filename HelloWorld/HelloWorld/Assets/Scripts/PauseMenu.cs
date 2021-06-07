using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;

    public static bool isPaused { get; private set; } = false;

    private void Start()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(!isPaused);
        }
    }

    public void TogglePause(bool status)
    {
        isPaused = status;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuUI.SetActive(isPaused);
    }

    public void Mute()
    {
        SFXManager.instance.Mute("music");
    }

    public void Unmute()
    {
        SFXManager.instance.Unmute("music");
    }
}
