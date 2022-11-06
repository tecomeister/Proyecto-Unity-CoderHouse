using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController player;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 playerInput;
    private Vector3 movePlayer;
    private Animator anim;
    private bool isAiming = false;

    [Header("Speed")]
    public float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float slidingSpeed = 2f;
    [HideInInspector] public float speed;

    [Header("Gravity")]
    [SerializeField] private float gravity = 9f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slopeDownwardsForce;
    [SerializeField] private float pushForce = 2f;
    private float fallSpeed;
    private bool isOnSlope = false;
    private Vector3 normalFace;
    private float targetMass;

    [Header("Actions")]
    [SerializeField] private bool canShoot = true;
    private float timeToShoot = 1.5f;
    private float timeToShootLeft;
    private float timeToRechargeMana = 1.5f;
    private float timeToRechargeManaLeft;
    private bool canShield = true;
    private float timeToShield = 8f;
    private float timeToShieldLeft;
    private bool canSlowDown = false;


    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject bullet;
    private CameraType cameraType;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 mouseWorldPos;

    [Header("Other")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject shieldSpawnPoint;
    private GameObject ui;

    public enum CameraType
    {
        Exploration,
        Combat
    }

    void Start()
    {
        player = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        speed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        ui = GameObject.FindGameObjectWithTag("UI");
    }

    void Update()
    {
        if(ui.GetComponent<PauseMenu>().pause != true)
        {
            PlayerMovement();
            ShootTimer();
            ShieldTimer();

            if(GameManager.instance.mana != GameManager.instance.maxMana)
            {
                ManaTimer();
            }
        }
    }

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        playerInput = new Vector3(horizontalInput, 0, verticalInput);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        anim.SetFloat("PlayerMoveVelocity", playerInput.magnitude * speed);

        if (Input.GetButton("Fire2"))
        {
            SetCameraType(CameraType.Combat);
            anim.SetBool("IsAiming", true);
            isAiming = true;
            crosshair.SetActive(true);

            mouseWorldPos = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPos = raycastHit.point;
            }
        }
        else
        {
            SetCameraType(CameraType.Exploration);
            anim.SetBool("IsAiming", false);
            isAiming = false;
            crosshair.SetActive(false);
        }

        if (isAiming == true && canShoot == true && Input.GetButtonDown("Fire1"))
        {
            ResetShootTimer();
            GameManager.instance.mana -= 15;
            anim.SetTrigger("ThrowSpell");
        }

        if (isAiming == true && canShield == true && Input.GetKeyDown(KeyCode.E) && player.isGrounded)
        {
            GameManager.instance.mana -= 10;
            Instantiate(shield, shieldSpawnPoint.transform.position, shieldSpawnPoint.transform.rotation);
            ResetShieldTimer();
        }
    }

    void SetCameraType(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.Exploration:

                freeLookCam.GetComponent<CinemachineCameraOffset>().enabled = false;

                GetCamDirection();

                if (Input.GetButton("Sprint")) speed = runSpeed;
                else speed = walkSpeed;

                movePlayer = playerInput.x * camRight + playerInput.z * camForward;
                movePlayer *= speed;

                if (movePlayer != Vector3.zero)
                {
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(movePlayer), rotationSpeed * Time.deltaTime);
                }

                Gravity();
                Jump();

                player.Move(movePlayer * Time.deltaTime);
                break;

            case CameraType.Combat:

                freeLookCam.GetComponent<CinemachineCameraOffset>().enabled = true;

                GetCamDirection();

                movePlayer = playerInput.x * camRight + playerInput.z * camForward;
                movePlayer *= speed;

                Quaternion lookRotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                Gravity();
                Jump();

                player.Move(movePlayer * Time.deltaTime);
                break;
        }
    }

    void ShootTimer()
    {
        timeToShootLeft -= Time.deltaTime;

        if(timeToShootLeft <= 0 && GameManager.instance.mana >= 15)
        {
            timeToShootLeft = 0;
            canShoot = true;
        }
        else if(timeToShootLeft <= 0 && GameManager.instance.mana <= 10)
        {
            timeToShootLeft = 0;
            canShoot = false;
        }
    }

    void ResetShootTimer()
    {
        timeToShootLeft = timeToShoot;
        canShoot = false;
    }

    void ShieldTimer()
    {
        timeToShieldLeft -= Time.deltaTime;

        if (timeToShieldLeft <= 0)
        {
            timeToShieldLeft = 0;
            canShield = true;
        }
    }

    void ResetManaTimer()
    {
        timeToRechargeManaLeft = timeToRechargeMana;
    }

    void ManaTimer()
    {
        timeToRechargeManaLeft -= Time.deltaTime;

        if (timeToRechargeManaLeft <= 0 && GameManager.instance.mana != GameManager.instance.maxMana)
        {
            ResetManaTimer();
            GameManager.instance.mana += 5;
        }
    }

    void ResetShieldTimer()
    {
        timeToShieldLeft = timeToShield;
        canShield = false;
    }

    void GetCamDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void Gravity()
    {
        if (player.isGrounded)
        {
            fallSpeed = -gravity * Time.deltaTime;
            movePlayer.y = fallSpeed;
            anim.SetBool("IsGrounded", true);
            Time.timeScale = 1;
            canSlowDown = false;
        } else if (!player.isGrounded && isAiming == true && canSlowDown == true)
        {
            anim.SetTrigger("Jump");
            fallSpeed -= (gravity / 2) * Time.deltaTime;
            movePlayer.y = fallSpeed;
            anim.SetFloat("PlayerVerticalVelocity", -player.velocity.y);
            anim.SetBool("IsGrounded", false);
            Time.timeScale = 0.3f;

            if(movePlayer.y <= 0.55)
            {
                fallSpeed -= gravity * Time.deltaTime;
                movePlayer.y = fallSpeed;
                anim.SetFloat("PlayerVerticalVelocity", -player.velocity.y);
                anim.SetBool("IsGrounded", false);
                Time.timeScale = 1;
                canSlowDown = false;
            }
        }
        else
        {
            fallSpeed -= gravity * Time.deltaTime;
            movePlayer.y = fallSpeed;
            anim.SetFloat("PlayerVerticalVelocity", -player.velocity.y);
            anim.SetBool("IsGrounded", false);
            Time.timeScale = 1;
        }
        Slide();
    }

    void Jump()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallSpeed = jumpForce;
            movePlayer.y = fallSpeed;
            anim.SetTrigger("Jump");
            canSlowDown = true;
        }
    }

    void Slide()
    {
        isOnSlope = Vector3.Angle(Vector3.up, normalFace) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - normalFace.y) * normalFace.x) * slidingSpeed;
            movePlayer.z += ((1f - normalFace.y) * normalFace.z) * slidingSpeed;
            movePlayer.y -= slopeDownwardsForce;
        }
    }

    private void Shoot()
    {
        Vector3 aimDir = (mouseWorldPos - spawnPoint.position).normalized;

        Instantiate(bullet, spawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        normalFace = hit.normal;

        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic || hit.moveDirection.y < -0.3) return;

        if (hit.gameObject.tag == "Movable")
        {
            targetMass = rb.mass;

            Vector3 pushForceDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            rb.velocity = pushForceDirection * pushForce / targetMass;
        }
    }
}