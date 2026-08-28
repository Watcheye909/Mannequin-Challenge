using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    [Header("WallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float upwardForce;
    public float downForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public float wallRunEndTime; //1/3 of the wall run time

    
    
    public bool wallJumping;
    

    [Header("Input")]
    public KeyCode wallJumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool wallLeft;
    public bool wallRight;

    [Header("Camera")]
    //public bool canAcsend; NOT IN USE RN
    public int wallrunFOV;
    public float cameraTilt;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    public PlayerCamera cam;
    public PlayerMovement pm; //using other movement script
    public Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();

        /*
        if (pm.wallrunning)
            WallRunningMovement();
        */
    }
    
    private void FixedUpdate()
    {
        
        if (pm.wallrunning)
            WallRunningMovement();

        if(wallJumping)
            WallJumpAction();
        
    }
    
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //getting input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //state 1 - wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && pm.moveSpeed > pm.walkSpeed  && !exitingWall)
        {
            //start wallrunning
            if (!pm.wallrunning && pm.moveSpeed > pm.walkSpeed)
                StartWallRun();

            if (wallRunTimer > 0){
                wallRunTimer -= Time.deltaTime;
                wallRunEndTime -= Time.deltaTime;
            }

            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(wallJumpKey)) WallJump();
        }

        //exiting wall
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;
        wallRunEndTime = wallRunTimer/2;

        //if(rb.velocity.y > -15) //switch this value to -0.5 if the process below is disabled
        //{
            //Gain a vertical boost at the start of a wall run if the player presses toward the wall
            if(wallLeft && horizontalInput == -1)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                
                //upward force
                rb.AddForce(transform.up * upwardForce, ForceMode.Impulse);
            }

            if(wallRight && horizontalInput == 1)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                
                //upward force
                rb.AddForce(transform.up * upwardForce, ForceMode.Impulse);
            }
        //}

        //APPLY CAMERA EFFECTS
        cam.DoFov(wallrunFOV);
        if (wallLeft) cam.DoTilt(cameraTilt * -1);
        if (wallRight) cam.DoTilt(cameraTilt);
    }

    private void WallRunningMovement()
    {
        //rb.useGravity = false;

        if(rb.velocity.y < -0.5 && wallRunEndTime > 0){

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //upward force  set this value back to 0.1 since the stuff is processed multiple times
        //rb.AddForce(transform.up * upwardForce, ForceMode.Impulse);

        //dropdown force
        if(wallRunEndTime <= 0)
            rb.AddForce(transform.up * downForce * -1, ForceMode.Impulse);

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        rb.useGravity = true;
        
        exitingWall = true;
        exitWallTimer = exitWallTime;

        //reset camera
        cam.DoFov(cam.FOV);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        //exiting wall
        exitingWall = true;
        exitWallTimer = exitWallTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        wallJumping = true;
        Debug.Log("walljump activated");

        /*
        //wall jumping
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 horizontalNormal = new Vector3(wallNormal.x, 0f, wallNormal.z).normalized;
        Vector3 forceToApply = transform.up * wallJumpUpForce + horizontalNormal * wallJumpSideForce;

        //add the jump force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        */
    }

    void WallJumpAction()
    {
        //wall jumping
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 horizontalNormal = new Vector3(wallNormal.x, 0f, wallNormal.z).normalized;
        Vector3 forceToApply = transform.up * wallJumpUpForce + horizontalNormal * wallJumpSideForce;

        //add the jump force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        wallJumping = false;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, wallCheckDistance);
    }
   
}
