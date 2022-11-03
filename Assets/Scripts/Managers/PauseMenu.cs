using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool pause;
    public GameObject menu;

    void Start()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Cancel"))
        { 
            if (pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        pause = true;
    }

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
