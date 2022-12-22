using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Collider weaponCol;
    private Animator anim;
    private GameObject target;
    private Rigidbody rb;
    private GameObject ui;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private int walkSpeed;
    [SerializeField] private int runSpeed;
    public int drop;
    private bool attacking;
    private int speed;
    private EnemyState enemyState;

    [Header("Patroll")]
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private float dist;

    [Header("Chase Area")]
    [SerializeField] private float maxRadiusTarget;
    [SerializeField] private float minRadiusTarget;


    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        ui = GameObject.FindGameObjectWithTag("UI");
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;
        drop = Random.Range(0, 5);
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GetComponent<EnemySFX>().Death();
            ui.GetComponent<UIManager>().UpdateCoins(drop);
            anim.SetTrigger("dead");
            GetComponent<EnemySFX>().Death();
            rb.isKinematic = true;
            this.enabled = false;
            GameManager.instance.GetComponent<SoundManager>().ResumeMusic();
        }

        if (target != null && health > 0)
        {
            EnemyBehaviour();
        }
    }

    private void EnemyBehaviour()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > maxRadiusTarget)
        {
            EnemyStates(EnemyState.Patrol);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > minRadiusTarget && !attacking)
        {
            EnemyStates(EnemyState.Chase);
        }
        else
        {
            EnemyStates(EnemyState.Attack);
        }
    }

    private void EnemyStates(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                speed = walkSpeed;
                dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
                if (dist < 1f)
                {
                    IncreaseIndex();
                }
                anim.SetBool("walk", true);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                Quaternion lookRotation = Quaternion.LookRotation(waypoints[waypointIndex].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.05f);
                break;

            case EnemyState.Chase:
                speed = runSpeed;
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                anim.SetBool("walk", false);
                anim.SetBool("run", true);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                anim.SetBool("attack", false);
                if(GameManager.instance.GetComponent<SoundManager>().battleSource.volume == 0f)
                {
                    GameManager.instance.GetComponent<SoundManager>().PlayBattleMusic();
                }
                break;

            case EnemyState.Attack:
                anim.SetBool("walk", false);
                anim.SetBool("run", false);
                lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
                anim.SetBool("attack", true);
                attacking = true;
                if (GameManager.instance.GetComponent<SoundManager>().battleSource.volume == 0f)
                {
                    GameManager.instance.GetComponent<SoundManager>().PlayBattleMusic();
                }
                break;
        }
    }
    void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    public void Damage(int damageAmmount)
    {
        if (health > 0)
        {
            health -= damageAmmount;
            anim.SetTrigger("hit");
        }
    }

    private void StopAttack()
    {
        anim.SetBool("attack", false);
        attacking = false;
    }

    private void ColliderWeapon()
    {
        weaponCol.enabled = !weaponCol.enabled;
    }

}
