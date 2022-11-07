using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public int maxHealth = 5;
    public int health;
    public int mana;
    public int maxMana = 100;

    void Awake()
    {
        if(GameManager.instance == null)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

        health = maxHealth;
        mana = maxMana;
    }

    public void NewMaxHealth(int healthToAdd)
    {
        instance.maxHealth += healthToAdd;
        health = maxHealth;
    }

    public void NewMaxMana(int manaToAdd)
    {
        instance.maxMana += manaToAdd;
        mana = maxMana;
    }
}
