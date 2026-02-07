using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public CinemachineCamera playerCam;

    public Animator animator;
    //private float currentV;

    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 0f;

    public float gravity = -9.81f;
    public float jumpForce = 5f;

    private Vector2 move;
    private Vector3 targetDir;
    private Vector2 look;
    private Vector3 velocity;
    private bool isDashing;
    private bool isGrounded;
    private bool isDead = false;

    public float speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private bool cursorOn = true;

    private void Update()
    {
        Move();

        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        //Debug.Log(controller.isGrounded);
    }

    private void LateUpdate()
    {
        /*
        cameraRotation.x += lookSenseH * look.x;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y - lookSenseV * look.y, -lookLimitV, lookLimitV);

        playerTargetRotation.x += transform.eulerAngles.x + (lookSenseH * look.x);
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);

        //playerCam.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
        //transform.rotation = Quaternion.Euler(0f, cameraRotation.x, 0f);
        */
        transform.rotation = Quaternion.Euler(0f, playerCam.transform.eulerAngles.y, 0f);
    }

    private void OnMove(InputValue inputValue)
    {
        move = inputValue.Get<Vector2>();
        
    }

    private void OnJump(InputValue inputValue)
    {
        Jump();
    }

    private void OnDash(InputValue inputValue)
    {

    }

    private void OnLook(InputValue inputValue)
    {
        look = inputValue.Get<Vector2>();
        /*
        if (!cursorOn)
        {
            look = inputValue.Get<Vector2>();
        }
        else
        {
            look = Vector2.zero;
        }
        */
    }

    private void OnLockCursor(InputValue inputValue)
    {
        DisableCursor();
    }

    private void Move()
    {
        targetDir = new Vector3(move.x, 0f, move.y).normalized;

        if (targetDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            animator.SetFloat("moveSpeed",targetDir.magnitude);
        }
        else
        {
            animator.SetFloat("moveSpeed", targetDir.magnitude);
        }

        
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            //velocity.y += gravity * Time.deltaTime;
            velocity.y = jumpForce;
            
        }
    }


    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursorOn = false;
        //transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        //Debug.Log("click");
    }
}
