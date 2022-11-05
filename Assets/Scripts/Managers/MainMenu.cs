using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator crossfade;
    public string nameOfScene;
    public void LoadScene(string sceneName)
    {
        nameOfScene = sceneName;

        StartCoroutine(LoadLevel());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel()
    {
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(nameOfScene);
    }
}
