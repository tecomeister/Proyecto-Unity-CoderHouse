using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject camCombat;
    [SerializeField] GameObject camExploration;
    [SerializeField] Transform cam;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float rotationSmoothTime;
    [SerializeField] float movementSpeed;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runingSpeed = 4f;
    [SerializeField] CameraType cameraType;
    float rotationSmoothSpeed;
    float horizontalInput;
    float verticalInput;

    public enum CameraType
    {
        Exploration,
        Combat
    }

    void Start()
    {
        movementSpeed = walkSpeed;
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = runingSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }

        if (Input.GetButton("Fire2"))
        {
            SetCameraType(CameraType.Combat);
        }
        else
        {
            SetCameraType(CameraType.Exploration);
        }
    }

    void SetCameraType(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.Exploration:
                
                camExploration.SetActive(true);
                camCombat.SetActive(false);
                
                Vector3 lookdirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
                if (lookdirection.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(lookdirection.x, lookdirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothSpeed, rotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                    rb.AddForce(moveDir.normalized * movementSpeed * 5f);
                }
                break;

            case CameraType.Combat:
                
                camExploration.SetActive(false);
                camCombat.SetActive(true);

                Vector3 direction = transform.forward * verticalInput + transform.right * horizontalInput;
                rb.AddForce(direction.normalized * movementSpeed * 5f);

                Quaternion lookRotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                break;
        }
    }
}
