using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementOld : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    public float wallRunSpeed;
    public float slideSpeed;
    public float BHopSpeed;

    public float groundMulti;

    public float groundDrag;

    [Header("MovementState Stuff")]
    public float desiredSpeed;
    private float lastDesiredSpeed;
    private MovementState lastState;
    private bool keepMomentum;


    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Crouching")]
    public bool isCrouching = false;
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Player collision Checks")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsRoof;
    public bool grounded;
    public bool nearRoof;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking, 
        sprinting,
        wallrunning,
        dashing,
        crouching,
        sliding,
        air
    }

    public bool dashing;
    public bool wallrunning;

    public bool sliding;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when ready to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && isCrouching == false)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        //start crouch
        if (Input.GetKeyDown(crouchKey) && grounded && isCrouching == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            isCrouching = true;

        }

        //crouching check for roof
        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            desiredSpeed = crouchSpeed;

        }
        



        //stop crouching
        if (Input.GetKeyDown(jumpKey) && isCrouching == true && nearRoof == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }

        if (Input.GetKeyDown(sprintKey) && isCrouching == true && nearRoof == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }

        if (Input.GetKeyUp(crouchKey) && isCrouching == true && nearRoof == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }

    }

    private void StateHandler()
    {

        //mode - dashing
        if(dashing)
        {
            state = MovementState.dashing;
            desiredSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        //mode - sliding
        else if (sliding)
        {
            state = MovementState.sliding;
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredSpeed = slideSpeed;

            //MAYBE CHANGE FOR BETTER MOVEMENT
            //else
                //desiredSpeed = sprintSpeed;
        }

        //mode - wallrunning
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredSpeed = wallRunSpeed;
        }

        //mode - crouching
        else if (Input.GetKey(crouchKey) && grounded  && isCrouching|| nearRoof && grounded)
        {
            state = MovementState.crouching;
            desiredSpeed = crouchSpeed;
        }

        //mode - sprinting
        else if (grounded && Input.GetKey(sprintKey) && isCrouching == false && sliding == false)
        {
            state = MovementState.sprinting;
            desiredSpeed = sprintSpeed;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        //mode - walking
        else if (grounded && isCrouching == false && sliding == false)
        {
            state = MovementState.walking;
            desiredSpeed = walkSpeed;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        //mode - air
        else
        {
            state = MovementState.air;
        }

        // check if desiredSpeed has changed drastically
        if(Mathf.Abs(desiredSpeed - lastDesiredSpeed) > 1.5f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredSpeed != lastDesiredSpeed;
        if(lastState == MovementState.dashing) keepMomentum = true;

        if(desiredMoveSpeedHasChanged)
        {
            if(keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredSpeed;
            }
        }

        lastDesiredSpeed = desiredSpeed;
        lastState = state;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredSpeed;
    }


    
    // Update is called once per frame
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        //roof check
        nearRoof = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, whatIsRoof);
        if (nearRoof && grounded)
            isCrouching = true;
            


        MyInput();
        SpeedControl();
        StateHandler();

        //drag control
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        //Scene Management

        if (Input.GetKey(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            GM.lastCheckPointPos = GM.startCheckPointPos;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        */
        

        //if (Input.GetKey(KeyCode.Escape))
            //Application.Quit();
    }

    private void FixedUpdate()
    {
        //MyInput();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(state == MovementState.dashing) return;
        
        // calculate movement direction to orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope movement
        if (OnSlope() && !exitSlope)
        {
            rb.AddForce(GetSlopeDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * groundMulti, ForceMode.Force);

        // in the air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn off gravity on slopes
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        /*slope speed control
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        */
        //limiting speed grounded or in air
        //else
        //{
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit the velocity
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        //}
    }

    public void Jump()
    {
        exitSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
