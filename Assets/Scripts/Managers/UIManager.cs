using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image HealthOrb;
    public Text healthText;
    public float maxHealth;
    public float health;
    public Image ManaOrb;
    public Text manaText;
    public float maxMana;
    public float mana;
    public GameObject player;

    void Start()
    {
        maxHealth = player.GetComponent<PlayerHealth>().maxHealth;
        health = player.GetComponent<PlayerHealth>().health;
        mana = player.GetComponent<PlayerHealth>().mana;
        maxMana = player.GetComponent<PlayerHealth>().maxMana;
    }

    void Update()
    {
        if(player != null)
        {
            HealthUI();
        }

    }

    void HealthUI()
    {
        health = player.GetComponent<PlayerHealth>().health;
        mana = player.GetComponent<PlayerHealth>().mana;
        healthText.text = health + "/" + maxHealth;
        manaText.text = mana + "/" + maxMana;
        HealthOrb.fillAmount = health / maxHealth;
        ManaOrb.fillAmount = mana / maxMana;
    }
}
