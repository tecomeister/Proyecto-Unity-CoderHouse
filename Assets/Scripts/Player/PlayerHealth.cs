using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth = 5;
    [SerializeField] int mana;
    [SerializeField] int maxMana = 100;

    bool invincible = false;
    [SerializeField] float invincibilityTime = 1f;
    [SerializeField] float stopTime = 0.5f;

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmmount)
    {
        if(!invincible && health > 0)
        {
            health -= damageAmmount;
            StartCoroutine(InvencibilityFrames());
            StartCoroutine(StopPlayer());
        }
    }

    IEnumerator InvencibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    IEnumerator StopPlayer()
    {
        GetComponent<PlayerController>().speed = 0;
        yield return new WaitForSeconds(stopTime);
        GetComponent<PlayerController>().speed = GetComponent<PlayerController>().walkSpeed;
    }
}
