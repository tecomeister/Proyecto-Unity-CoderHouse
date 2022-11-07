using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool pause = true;
    public GameObject menu;
    public GameObject controls;
    public GameObject playerCam;

    void Start()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (controls.activeInHierarchy == true)
        {
            Cursor.lockState = CursorLockMode.None;
            pause = true;
            playerCam.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel") && controls.activeInHierarchy == false)
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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        menu.SetActive(true);
        Time.timeScale = 0;
        pause = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void CloseControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controls.SetActive(false);
        pause = false;
        playerCam.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
