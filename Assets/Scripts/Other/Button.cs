using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Transform chestSpawnPoint;
    public GameObject chest;
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Movable")
        {
            boxCollider.enabled = false;
            Instantiate(chest, chestSpawnPoint);
            
        }
    }
}
