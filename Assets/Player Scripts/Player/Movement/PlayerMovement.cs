using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public PlayerCamera cam;
    public float cameraTilt;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float dashBoost;
    public float sprintSpeed;
    public float wallRunSpeed;
    public float slideSpeed;
    public float BHopSpeed;


    public float requiredSpeed;
    public bool playerStop;
    public bool moving;


    public float groundMulti;




    public float desiredSpeed;
    private float lastDesiredSpeed;


    public float groundDrag;


    [Header("Jumping")]
    public float jumpForce;
    public float storeJumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool jump;
    [Range(0, 1f)][SerializeField] private float CutJumpHeight = 0.5f;
    public float lowJumpMulti;
    public bool useVarJump;


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


    public float horizontalInput;
    public float verticalInput;


    Vector3 moveDirection;


    Rigidbody rb;


    public MovementState state;


    public enum MovementState
    {
        walking,
        sprinting,
        dashing,
        wallrunning,
        crouching,
        sliding,
        freeze, //Specifically used for the grapple movement but generally just works like a hitpause
        air
    }

    [Header("State Checks")]
    public bool freeze;

    public bool dashing;

    public bool wallrunning;


    public bool sliding;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        useVarJump = true;
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
            useVarJump = true;
            jump = true;
            //Jump();


            Invoke(nameof(ResetJump), jumpCooldown);


        }

        // [other option for variable jump height]
        if (rb.velocity.y > 0 && !Input.GetKey(jumpKey) && useVarJump)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
        }
        
        /*
        if (Input.GetKeyUp(jumpKey))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * CutJumpHeight, rb.velocity.z);
            }
        }
        */

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
        if(freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }


        else if(dashing)
        {
            state = MovementState.dashing;
            if(!PauseMenu.GameIsPaused)
            desiredSpeed += dashBoost;
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

        // check if player should lose momentum based on changes in speed [new system]
        if ((rb.velocity.magnitude <= moveSpeed/2 && grounded) && moveSpeed > sprintSpeed|| horizontalInput == 0 && verticalInput == 0 && grounded)
            playerStop = true;
        else
            playerStop = false;

        /* check if desiredSpeed has changed drastically [old system]
        if((rb.velocity.magnitude < requiredSpeed && grounded) || (rb.velocity.magnitude < moveSpeed/2 && grounded))
        {
            //Debug.Log("NO MOVEMENT");
            playerStop = true;
        }
        else
            playerStop = false;
        */
        


        //------------------HANDLES SPEED DECELARATION-------------------



        if(playerStop)
            moveSpeed = walkSpeed;

        else if(lastDesiredSpeed == walkSpeed && desiredSpeed == sprintSpeed)
        {
            moveSpeed = desiredSpeed;
        }

        else if(lastDesiredSpeed == sprintSpeed && desiredSpeed == walkSpeed)
        {
            moveSpeed = desiredSpeed;
        }

        else if(Mathf.Abs(desiredSpeed - lastDesiredSpeed) > 3.5f && moveSpeed != 0) 
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }

        else
        {
            moveSpeed = desiredSpeed;
        }


        lastDesiredSpeed = desiredSpeed;
    }




    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredSpeed - moveSpeed);
        float startValue = moveSpeed;

            //ENDS MOMENTUM IF PLAYER STOPS MOVING
            //if(rb.velocity.magnitude < 4)
            //{
                //moveSpeed = desiredSpeed;
                //yield return null;
            //}
        
        while(time < difference)
        {
            if(playerStop)
            {
                time = difference;
            }
            if(!playerStop)
            {
            moveSpeed = Mathf.Lerp(startValue, desiredSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
            }
        }


        moveSpeed = desiredSpeed;
    }




















   
    // Update is called once per frame
    private void Update()
    {

        //ENDS MOMENTUM IF PLAYER STOPS MOVING
        //if(rb.velocity.magnitude < 4)
            //moveSpeed = desiredSpeed;


        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
       
        if (rb.velocity.magnitude > 1 && (state == MovementState.walking || state == MovementState.sprinting))
            moving = true;
        else
            moving = false;
        //roof check
        nearRoof = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, whatIsRoof);
        if (nearRoof && grounded)
            isCrouching = true;
           
        // [CAMERA MOVEMENT]
        /*
        if(verticalInput == 1)
            cam.DoLean(cameraTilt);

        if(verticalInput == -1)
            cam.DoLean(cameraTilt * -1);

        if(horizontalInput == 1)
            cam.DoTilt(cameraTilt * -1);

        if(horizontalInput == -1)
            cam.DoTilt(cameraTilt);
        */

        MyInput();
        SpeedControl();
        StateHandler();


        //drag control
        //if(grounded && rb.velocity.y <= 0.5f && moveDirection.magnitude < 0.1f) //new method
        if(grounded && rb.velocity.y <= 0.5 && horizontalInput == 0 && verticalInput == 0) //old method
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
        /* ORIGINAL CODE FOR GROUNDDRAG
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        */

        //Scene Management


        if (Input.GetKey(KeyCode.P))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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

        if(jump)
        {
            Jump();
        }
    }


    private void MovePlayer()
    {

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
        //rb.useGravity = !OnSlope();
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

        /* [alt momentum]
        if (flatVel.magnitude > moveSpeed)
        {
            // Gradually reduce instead of snapping
            rb.velocity = Vector3.Lerp(rb.velocity, 
                new Vector3(flatVel.normalized.x * moveSpeed, rb.velocity.y, flatVel.normalized.z * moveSpeed), 
                Time.deltaTime * 5f);
        }
        */
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
        rb.drag = 0;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        jump = false;
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitSlope = false;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        
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



