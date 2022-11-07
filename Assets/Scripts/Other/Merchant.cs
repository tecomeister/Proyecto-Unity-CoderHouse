using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    private GameObject ui;
    public bool canBuy = false;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("UI");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.GetComponent<UIManager>().merchantText.SetActive(true);
            canBuy = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.GetComponent<UIManager>().merchantText.SetActive(false);
            canBuy = false;
        }
    }
}
