using UnityEngine;

public class Enemy: MonoBehaviour
{
    public int health;
    public int walkSpeed;
    public int runSpeed;
    void Awake()
    {
        EnemyController orco = new EnemyController();
        health = orco.health;
        walkSpeed = orco.walkSpeed;
        runSpeed = orco.runSpeed; 
        Debug.Log("Vida del Orco = " + orco.health);
        Debug.Log("Velocidad al caminar del Orco = " + orco.walkSpeed);
        Debug.Log("Velocidad al correr del Orco = " + orco.runSpeed);
    }
}
