using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Global.inMenu && !Global.isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Global.inMenu && Global.isPaused)
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Global.isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        Global.isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
    }
}
