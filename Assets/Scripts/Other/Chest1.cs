using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest1 : MonoBehaviour
{
    private Animator anim;
    private GameObject ui;
    private int drop = 1;
    [SerializeField] BoxCollider col;
    void Start()
    {
        anim = GetComponent<Animator>();
        ui = GameObject.FindGameObjectWithTag("UI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.Play("OpenChest");
            ui.GetComponent<UIManager>().UpdateCollectables(drop);
            col.enabled = false;
        }
    }
}
