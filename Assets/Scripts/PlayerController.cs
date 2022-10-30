using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    CharacterController player;

    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float runSpeed = 2f;
    float speed;

    [SerializeField] float gravity = 9f;
    [SerializeField] float jumpForce;
    float fallSpeed;

    Vector3 playerInput;
    Vector3 movePlayer;

    [SerializeField] Camera mainCamera;
    Vector3 camForward;
    Vector3 camRight;

    void Start()
    {
        player = GetComponent<CharacterController>();
        speed = walkSpeed;
    }

    void Update()
    {
        
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

        GetCamDirection();

        if (Input.GetButton("Sprint"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * speed;

        player.transform.LookAt(player.transform.position + movePlayer);

        Gravity();
        PlayerControlls();

        player.Move(movePlayer * Time.deltaTime);
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
        
    }

    void PlayerControlls()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallSpeed = jumpForce;
            movePlayer.y = fallSpeed;
        }
    }

    
}
