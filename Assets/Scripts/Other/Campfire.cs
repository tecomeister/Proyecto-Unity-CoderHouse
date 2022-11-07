using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public Transform chestSpawnPoint;
    public GameObject chest;
    public GameObject fx;
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerSpell")
        {
            boxCollider.enabled = false;
            Instantiate(chest, chestSpawnPoint);
            fx.SetActive(true);
        }
    }
}
