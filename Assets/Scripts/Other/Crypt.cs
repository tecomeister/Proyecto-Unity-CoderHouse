using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crypt : MonoBehaviour
{
    private GameObject ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("UI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ui.GetComponent<UIManager>().collectable == 3)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Main Menu");
            GameManager.instance.GetComponent<SoundManager>().ResumeMusic();
            ui.GetComponent<UIManager>().cryptText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && ui.GetComponent<UIManager>().collectable != 3)
        {
            ui.GetComponent<UIManager>().cryptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (ui.GetComponent<UIManager>().cryptText.activeInHierarchy)
        {
            ui.GetComponent<UIManager>().cryptText.SetActive(false);
        }
    }
}
