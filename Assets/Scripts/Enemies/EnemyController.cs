using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int rutina;
    [SerializeField] float cronometro;
    [SerializeField] Animator anim;
    [SerializeField] Quaternion angulo;
    [SerializeField] float grado;
    [SerializeField] GameObject target;
    [SerializeField] bool ataque;
    [SerializeField] Collider armaCollider;
    [SerializeField] bool shooter = false;
    [SerializeField] int health;
    [SerializeField] bool stun;
    void Start()
    {
 
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (target != null)
        {
           ComportamientoEnemigo();
        }
       
    }

    public void ComportamientoEnemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            anim.SetBool("run", false);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    anim.SetBool("walk", false);
                    break;

                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    anim.SetBool("walk", true);
                    break;

            }
        }
        else
        {
            

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !ataque)
            {

                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                anim.SetBool("walk", false);
                anim.SetBool("run", true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                anim.SetBool("attack", false);
            }
            else
            {
               
                anim.SetBool("walk", false);
                anim.SetBool("run", false);

                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                anim.SetBool("attack", true);
                ataque = true;

            }
            
            
            
        }
        
        
    }

    public void Damage(int damageAmmount)
    {
        if (health > 0)
        {
            health -= damageAmmount;
            anim.Play("Hit");
        }
    }

    public void FinalizarAtaque()
    {
        anim.SetBool("attack", false);
        ataque = false;
    }

    public void ColliderArma()
    {
        if (shooter == false)
        {
            armaCollider.enabled = !armaCollider.enabled;
        }
    }

}
