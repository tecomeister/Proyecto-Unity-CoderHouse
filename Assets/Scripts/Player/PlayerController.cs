using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    CharacterController player;

    [Header("Speed")]
    public float walkSpeed = 1f;
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float slidingSpeed = 2f;
    [HideInInspector] public float speed;

    [Header("Gravity")]
    [SerializeField] float gravity = 9f;
    [SerializeField] float jumpForce;
    float fallSpeed;
    bool isOnSlope = false;
    Vector3 normalFace;
    [SerializeField] float slopeDownwardsForce;
    [SerializeField] float pushForce = 2f;
    float targetMass;

    Vector3 playerInput;
    Vector3 movePlayer;

    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject camCombat;
    [SerializeField] GameObject camExploration;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] GameObject bullet;
    CameraType cameraType;
    Vector3 camForward;
    Vector3 camRight;
    Vector3 mouseWorldPos;

    [Header("UI Elements")]
    [SerializeField] GameObject crosshair;

    Animator anim;

    bool isAiming = false;

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
    }

    void Update()
    {
        PlayerMovement();
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

        if (isAiming = true && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("ThrowSpell");
        }

        void SetCameraType(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.Exploration:

                    camExploration.GetComponent<CinemachineCameraOffset>().enabled = false;

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

                    camExploration.GetComponent<CinemachineCameraOffset>().enabled = true;

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
            }
            else
            {
                fallSpeed -= gravity * Time.deltaTime;
                movePlayer.y = fallSpeed;
                anim.SetFloat("PlayerVerticalVelocity", -player.velocity.y);
                anim.SetBool("IsGrounded", false);
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
    }

    private void Shoot()
    {
        Vector3 aimDir = (mouseWorldPos - SpawnPoint.position).normalized;

        Instantiate(bullet, SpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
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