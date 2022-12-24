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

    [Header("Collectable Counter")]
    public int collectable;
    public Text collectableText;

    [Header("Text Boxes")]
    public GameObject merchantText;
    public GameObject cryptText;


    [Header("Player")]
    [SerializeField] private GameObject player;

    void Start()
    {
        maxHealth = GameManager.instance.maxHealth;
        health = GameManager.instance.health;
        mana = GameManager.instance.mana;
        maxMana = GameManager.instance.maxMana;
        coins = 0;
        collectable = 0;
    }

    void Update()
    {
        if(player != null)
        {
            HealthUI();
            CoinsUI();
            CollectableUI();
        }

        if (merchantText.activeInHierarchy == true && GetComponentInParent<PauseMenu>().pause == true)
        {
            merchantText.GetComponent<CanvasGroup>().alpha = 0;
        } else if (merchantText.activeInHierarchy == true && GetComponentInParent<PauseMenu>().pause == false)
        {
            merchantText.GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    void HealthUI()
    {
        health = GameManager.instance.health;
        mana = GameManager.instance.mana;
        maxHealth = GameManager.instance.maxHealth;
        maxMana = GameManager.instance.maxMana;
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

    public void buyCoins(int coinsAmmount)
    {
        coins -= coinsAmmount;
    }

    void CollectableUI()
    {
        collectableText.text = collectable.ToString();
    }
    public void UpdateCollectables(int collectableAmmount)
    {
        collectable += collectableAmmount;
    }

}
