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
    [HideInInspector]public float speed;

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
    CameraType cameraType;
    Vector3 camForward;
    Vector3 camRight;

    public enum CameraType
    {
        Exploration,
        Combat
    }

    void Start()
    {
        player = GetComponent<CharacterController>();
        speed = walkSpeed;
    }

    void FixedUpdate()
    {

        PlayerMovement();
    }

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        playerInput = new Vector3(horizontalInput, 0, verticalInput);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        if (Input.GetButton("Fire2")) SetCameraType(CameraType.Combat);
        else SetCameraType(CameraType.Exploration);

        if (Input.GetButton("Sprint")) speed = runSpeed;
        else speed = walkSpeed;
    }

    void SetCameraType(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.Exploration:

                camExploration.SetActive(true);
                camCombat.SetActive(false);

                GetCamDirection();

                movePlayer = playerInput.x * camRight + playerInput.z * camForward;
                movePlayer *= speed;

                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(movePlayer), rotationSpeed * Time.deltaTime);

                Gravity();
                Jump();

                player.Move(movePlayer * Time.deltaTime);
                break;

            case CameraType.Combat:
                camExploration.SetActive(false);
                camCombat.SetActive(true);

                GetCamDirection();

                movePlayer = playerInput.x * camRight + playerInput.z * camForward;
                movePlayer *= speed;

                Quaternion lookRotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

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
        }else
        {
            fallSpeed -= gravity * Time.deltaTime;
            movePlayer.y = fallSpeed;
        }

        Slide();
    }

    void Jump()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallSpeed = jumpForce;
            movePlayer.y = fallSpeed;
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
