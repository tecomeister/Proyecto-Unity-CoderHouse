using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float stopTime = 0.5f;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject cameraPlayer;
    private Animator anim;
    private CharacterController characterController;
    private Rigidbody[] rigidbodies;
    private bool invincible = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(false);
    }

    void Start()
    {
        GameManager.instance.health = GameManager.instance.maxHealth;
        GameManager.instance.mana = GameManager.instance.maxMana;
    }

    void Update()
    {

        if (GameManager.instance.health <= 0)
        {
            GetComponent<PlayerSFX>().Death();
            deathMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            cameraPlayer.SetActive(false);
            EnableRagdoll(true);
        }
    }

    void EnableRagdoll(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }
        ui.GetComponent<PauseMenu>().enabled = !enabled;
        anim.enabled = !enabled;
        characterController.enabled = !enabled;
        GetComponent<PlayerController>().enabled = !enabled;

        this.enabled = !enabled;
    }

    public void TakeDamage(int damageAmmount)
    {
        if(!invincible && GameManager.instance.health > 0)
        {
            GameManager.instance.health -= damageAmmount;
            anim.SetTrigger("GotHit");
            StartCoroutine(InvencibilityFrames());
            StartCoroutine(StopPlayer());
        }
    }

    IEnumerator InvencibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    IEnumerator StopPlayer()
    {
        GetComponent<PlayerController>().speed = 0;
        yield return new WaitForSeconds(stopTime);
        GetComponent<PlayerController>().speed = GetComponent<PlayerController>().walkSpeed;
    }
}
