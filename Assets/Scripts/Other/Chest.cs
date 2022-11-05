using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    private GameObject ui;
    private int drop;
    [SerializeField] BoxCollider col;
    void Start()
    {
        anim = GetComponent<Animator>();
        ui = GameObject.FindGameObjectWithTag("UI");
        drop = Random.Range(5, 15);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.Play("OpenChest");
            ui.GetComponent<UIManager>().UpdateCoins(drop);
            col.enabled = false;
        }
    }
}
