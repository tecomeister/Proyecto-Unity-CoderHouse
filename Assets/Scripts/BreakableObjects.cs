using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour
{
    [SerializeField] int health;
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Damage (int damageAmmount)
    {
        if(health > 0)
        {
            health -= damageAmmount;
        }
    }
}
