using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Health Orb")]
    public Image HealthOrb;
    public Text healthText;
    public float maxHealth;
    public float health;

    [Header("Mana Orb")]
    public Image ManaOrb;
    public Text manaText;
    public float maxMana;
    public float mana;

    [Header("Coin Counter")]
    public int coins;
    public Text coinsText;


    [Header("Player")]
    [SerializeField] GameObject player;

    void Start()
    {
        maxHealth = player.GetComponent<PlayerHealth>().maxHealth;
        health = player.GetComponent<PlayerHealth>().health;
        mana = player.GetComponent<PlayerHealth>().mana;
        maxMana = player.GetComponent<PlayerHealth>().maxMana;
        coins = 0;
    }

    void Update()
    {
        if(player != null)
        {
            HealthUI();
            CoinsUI();
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

    void CoinsUI()
    {
        coinsText.text = coins.ToString();
    }

    public void UpdateCoins(int coinsAmmount)
    {
        coins += coinsAmmount;
    }
}
