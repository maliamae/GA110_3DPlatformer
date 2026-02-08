using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Fields")]
    public CharacterController controller; //plug in character controller component that is attached to same object this script is attached to
    public CinemachineCamera playerCam; //plug in third person free look camera
    public Animator animator; //plug in animator component that is attached to same object this script is attached to

    [Header("Jump")]
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    [Header("Movement Speed")]
    public float walkSpeed;
    public float dashSpeed;
    public float dashCoolDown = 2f;

    [Header("Camera Alignment")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity; //used as a ref in smoothly turning character model to face same direction as camera

    private Vector2 move; //used to store input direction
    private Vector3 targetDir; //converts and stores input direction into a Vector3 format 
    private Vector3 velocity; //used to apply gravity and jump force on y axis
    private bool isDashing = false; //used to limit dash frequency
    //private bool isDead = false; //originally intended to disable movements when player dies, but with the new input system I can just use the .Dsiable() function instead
    private bool cursorOn = true; //used to keep track of cursor visibility

    private void OnEnable()
    {
        GetComponent<PlayerInput>().currentActionMap.Enable();
    }
    private void Update()
    {
        Move();

        //apply gravity on y axis if player is not grounded (jumped):
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        animator.SetBool("isGrounded", controller.isGrounded); //update animator isGrounded parameter
    }

    private void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(0f, playerCam.transform.eulerAngles.y, 0f); //strictly matches player rotation with camera rotation with no smooth angle adjustment
        /*
        //player can walk backwards towards camera, cannot ever see front of character; smooth angle adjustment to align with camera rotation
        float camAlignAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, playerCam.transform.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, camAlignAngle, 0f);
        */
    }

    private void OnTriggerEnter(Collider other) //testing collision with water boundary mechanics
    {
        if (other.gameObject.tag == "Water")
        {
            OnWaterEnter(); //disables player input action map
        }
    }

    private void OnMove(InputValue inputValue) //called when action map recieves Move inputs
    {
        move = inputValue.Get<Vector2>();
    }

    private void OnJump() //called when action map recieves Jump input
    {
        Jump();
    }

    private void OnDash() //called when action map recieves Dash input
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void OnLockCursor() //called when action map recieves LockCursor input
    {
        DisableCursor();
    }

    private void Move()
    {
        targetDir = new Vector3(move.x, 0f, move.y).normalized;
        float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y; //angle needed to be applied to match camera direction
        
        //walking backwards turns character around; smooth angle adjustment to face same direction as camera; occurs while player is moving and when they are idle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        if (targetDir.magnitude >= 0.1f) //if player is giving inputs for x and z direction...
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //calculates the direction the player should move in, aligning with the player camera, in Vector3 format
            controller.Move(moveDir.normalized * walkSpeed * Time.deltaTime); //moves player in that direction at desired walking speed

            animator.SetFloat("moveSpeed", targetDir.magnitude); //updates animator moveSpeed parameter
        }
        else
        {
            animator.SetFloat("moveSpeed", targetDir.magnitude); //updates animator moveSpeed parameter
        }
        
    }

    private void Jump()
    {
        if (controller.isGrounded) //if the player is grounded, apply set jumpforce to the y axis of the player 
        {
            //velocity.y += gravity * Time.deltaTime;
            velocity.y = jumpForce;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float initialSpeed = walkSpeed; //keeps track of initial walk speed
        walkSpeed = dashSpeed;
        yield return new WaitForSeconds(0.25f); //dash speed is applied for a duration of 0.25 seconds
        walkSpeed = 0.1f; //reduces speed to almost nothing for a split second to make dash movement feel more snappy
        yield return new WaitForSeconds(0.1f);
        walkSpeed = initialSpeed; //returns original walking speed
        yield return new WaitForSeconds(dashCoolDown); //stops player from dashing again for set seconds
        isDashing = false;
    }

    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; //locks cursor to center of screen and turns off visibility
        cursorOn = false;
    }

    private void OnWaterEnter()
    {
        GetComponent<PlayerInput>().currentActionMap.Disable(); //stops taking and applying player inputs (movement)
        transform.position = Vector3.zero; //returns player to current temperary respawn point
    }
}
