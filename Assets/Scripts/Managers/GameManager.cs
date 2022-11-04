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
            Destroy(gameObject);
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            health = maxHealth;
            mana = maxMana;
        }
    }

    public static void NewMaxHealth(int healthToAdd)
    {
        instance.maxHealth += healthToAdd;
    }

    public static void NewMaxMana(int manaToAdd)
    {
        instance.maxMana += manaToAdd;
    }
}
