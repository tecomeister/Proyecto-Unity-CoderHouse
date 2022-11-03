using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;
    public int mana;
    public int maxMana = 100;

    Animator anim;

    bool invincible = false;
    [SerializeField] float invincibilityTime = 1f;
    [SerializeField] float stopTime = 0.5f;

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageAmmount)
    {
        if(!invincible && health > 0)
        {
            health -= damageAmmount;
            anim.SetTrigger("GotHit");
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
