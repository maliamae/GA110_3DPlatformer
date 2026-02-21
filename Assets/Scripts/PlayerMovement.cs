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
    //public Transform spawnPoint;

    [Header("Jump")]
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    [Header("Movement Speed")]
    public float walkSpeed;
    public float climbSpeed;
    public float dashSpeed;
    public float dashCoolDown = 2f;

    [Header("Camera Alignment")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity; //used as a ref in smoothly turning character model to face same direction as camera

    private Vector2 move; //used to store input direction
    private Vector2 climb;
    private Vector3 targetDir; //converts and stores input direction into a Vector3 format 

    [SerializeField]
    private Vector3 velocity; //used to apply gravity and jump force on y axis
    [SerializeField]
    private bool isFalling = false; //used to track if character is falling for animation
    private bool isDashing = false; //used to limit dash frequency
    private bool isClimbing = false; //used to disable gravity being applied if the player is climbing

    private void OnEnable()
    {
        GetComponent<PlayerInput>().currentActionMap.Enable();
        GetComponent<PlayerInput>().currentActionMap.FindAction("Climb").Disable();
    }
    private void Update()
    {
        
        if (!isClimbing && GetComponent<PlayerInput>().currentActionMap.FindAction("Move").enabled)
        {
            Move();
        }
        else if (isClimbing && GetComponent<PlayerInput>().currentActionMap.FindAction("Climb").enabled)
        {
            Climb();
        }
        
        //Move();
        //apply gravity on y axis if player is not grounded (jumped):
        if (!controller.isGrounded && !isClimbing && GetComponent<PlayerInput>().enabled)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (isClimbing)
        {
            velocity = Vector3.zero;
        }

        //controller.Move(velocity * Time.deltaTime);
        animator.SetFloat("moveSpeed", targetDir.magnitude); //updates animator moveSpeed parameter
        isFalling = !controller.isGrounded && velocity.y < -0.1f;
        animator.SetBool("isFalling", isFalling);
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
            //OnWaterEnter(); //"kills" and respawns player
            //StartCoroutine(EnterWater());
            GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerDead);

        }
        //if the player is enters the bounds of a vine, the Move action map is disabled and the Climb action map is enabled (allows me to use the same player inputs but remapped to different directions of movement)
        if (other.gameObject.tag == "Vine")
        {
            GetComponent<PlayerInput>().currentActionMap.FindAction("Move").Disable();
            GetComponent<PlayerInput>().currentActionMap.FindAction("Climb").Enable();
            isClimbing = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //if the player exits the bounds of a vine, the Move action map is reenabled and the Climb action map is disabled
        if (other.gameObject.tag == "Vine")
        {
            isClimbing = false;
            GetComponent<PlayerInput>().currentActionMap.FindAction("Move").Enable();
            GetComponent<PlayerInput>().currentActionMap.FindAction("Climb").Disable();
        }
    }

    private void OnMove(InputValue inputValue) //called when action map recieves Move inputs
    {
        move = inputValue.Get<Vector2>();
    }

    private void OnClimb(InputValue inputValue)
    {
        climb = inputValue.Get<Vector2>();
        //Debug.Log("CLIMB: " + climb);
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
        float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y; //angle needed to be applied to face the target direction taking into account camera direction
        
        //walking backwards turns character around; smooth angle adjustment to face same direction as camera; occurs while player is moving and when they are idle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        if (targetDir.magnitude >= 0.1f) //if player is giving inputs for x and z direction...
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //calculates the direction the player should move in, aligning with the player camera, in Vector3 format
            controller.Move(moveDir.normalized * walkSpeed * Time.deltaTime); //moves player in that direction at desired walking speed
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Climb()
    {
        targetDir = new Vector3(climb.x, climb.y, 0f).normalized; //stores inputs on the X and Y axis and not the Z axis because we need upwards and side to side movement when climbing

        //raycast to get the normal of the wall/strcuture the player is climbing
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            transform.forward = Vector3.Lerp(transform.forward, -hit.normal, 5f * Time.deltaTime); //moves the player's forward to face the wall they are climbing
        }

        if (targetDir.magnitude >= 0.1f) //if player is giving inputs for x and y direction...
        {
            Vector3 moveDir = transform.right * targetDir.x + transform.up * targetDir.y; //converts movement direction into local space (so movement is aligned whether the player's left and right are along the x or z axis)
            controller.Move(moveDir.normalized * climbSpeed * Time.deltaTime); //moves player in that direction at desired walking speed (DIRECTION IS BASED ON WORLD SPACE NOT OBJECT TRANSFORM)
        }
    }

    private void Jump()
    {
        if (controller.isGrounded) //if the player is grounded, apply set jumpforce to the y axis of the player 
        {
            velocity.y = jumpForce;
            animator.SetTrigger("jump"); //updates animation for jump
        }
        if (isClimbing)
        {
            velocity = transform.forward * -jumpForce; //applies backwards force from the direction of the wall the player is currently climbing 
            controller.Move(velocity * 25f * Time.deltaTime);//(moves player far enough away from the wall to transition out of Climb and back to Move)
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
        //cursorOn = false;
    }
}
