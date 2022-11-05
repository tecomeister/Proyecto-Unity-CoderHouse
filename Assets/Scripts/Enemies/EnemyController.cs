using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject target;
    [SerializeField] private Collider weaponCol;
    [SerializeField] private int health;
    [SerializeField] private GameObject ui;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private CapsuleCollider enemyCollider;
    public int drop;
    public int speed, walkSpeed, runSpeed;
    public float maxRadiusTarget, minRadiusTarget;
    private Quaternion angle;
    private float angleDegrees;
    private bool attacking;
    private Rigidbody[] rigidbodies;
    private EnemyState enemyState;
    private int waypointIndex;
    private float dist;

    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    void Start()
    {
        speed = walkSpeed;
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        SetEnabled(false);
        drop = Random.Range(0, 5);
        target = GameObject.FindWithTag("Player");
    }


    

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0)
        {
            GetComponent<EnemySFX>().Death();
            enemyCollider.enabled = false;
            SetEnabled(true);
            ui.GetComponent<UIManager>().UpdateCoins(drop);
            this.enabled = false;
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
                break;

            case EnemyState.Chase:
                speed = runSpeed;
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                anim.SetBool("walk", false);
                anim.SetBool("run", true);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                anim.SetBool("attack", false);
                break;

            case EnemyState.Attack:
                anim.SetBool("walk", false);
                anim.SetBool("run", false);
                lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                anim.SetBool("attack", true);
                attacking = true;
                break;
        }
    }
    void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        anim.enabled = !enabled;
    }
    void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        transform.LookAt(waypoints[waypointIndex].position);
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
