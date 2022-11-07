using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public int damage = 1;
    [SerializeField] private GameObject fx;

    Rigidbody rb;
    [SerializeField] float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(fx, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyController>().Damage(damage);
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Breakable"))
        {
            Instantiate(fx, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<BreakableObjects>().Damage(damage);
            Destroy(gameObject);
        }else if (collision.gameObject.CompareTag("InvisWalls"))
        {
            Destroy(gameObject);
        }
        else
        {
            Instantiate(fx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
