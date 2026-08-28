using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public ParticleSystem Speedlines;
    //public ParticleSystem canDashFX;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;
    public PlayerCamera cam;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration; //how long the dash lasts for
    private float dashTime;

    public bool canDash;

    [Header("Settings")]
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool useTimer;
    public float cameraTilt;

    [Header("Cooldown")]
    public float dashCooldown; //this time can be changed in the engine to assign the time that the cooldowntimer resets to
    private float dashCooldownTimer;
    //private bool fxReady;

    [Header("Input")]
    public KeyCode dashKey;

    Vector3 forceToApply;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        Speedlines.Stop();

        //setting the dash duration
        dashTime = dashDuration;

        //fxReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.grounded) 
        {
            canDash = true;
        }

        /*
        if(fxReady && pm.grounded)
        {
            canDashFX.Stop(); // reset the vfx before playing it to prevent an animation overlap

            canDashFX.Play(); //enable vfx

            fxReady = false;
        }
        */

        
        //if(!pm.grounded && Input.GetKeyDown(dashKey))
            //Dash();

        if(Input.GetKeyDown(dashKey))
            Dash();

        if(dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        if(pm.dashing)
            DashMovement();
    }

    private void Dash()
    {
        
        
        /* [HOW THE DASH COOLDOWN WORKS]
            when this Dash function is triggered it sets the dashCooldownTimer (amount of time currently left)
            to whatever length the dashCooldown is set to and decreases back to 0. If the player 
            attepmts to trigger the dash again while dashCooldownTimer is not 0 yet, the function return
            no actions.
        */

        if(useTimer)
        {
            if(dashCooldownTimer > 0) return;
            else dashCooldownTimer = dashCooldown;
        }

        if(!useTimer)
        {
            if(!canDash) return;
            if(dashCooldownTimer > 0) return;
            else dashCooldownTimer = 0.5f;
        }

        Speedlines.Stop(); // reset the vfx before playing it to prevent an animation overlap

        Speedlines.Play(); //enable vfx

        pm.dashing = true;
        
        Transform forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        forceToApply = direction * dashForce + orientation.up * dashUpwardForce;
        

        //if the player is falling and the dash is performed, stop falling momentum
        if(disableGravity)
        {
            if(rb.velocity.y < 0)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            rb.useGravity = false;
        }

        /*
        //apply force to move player
        rb.AddForce(forceToApply, ForceMode.Impulse);

        //when the dash ends reset dash
        dashDuration -= Time.deltaTime;

        if(dashDuration <= 0)
            ResetDash();
        //Invoke(nameof(ResetDash), dashDuration);
        */
    }

    private void DashMovement()
    {
        //Transform forwardT = orientation;

        //Vector3 direction = GetDirection(forwardT);

        //Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        // [CAMERA MOVEMENT]
        if(pm.verticalInput == 1)
            cam.DoLean(cameraTilt);

        if(pm.verticalInput == -1)
            cam.DoLean(cameraTilt * -1);

        if(pm.horizontalInput == 1)
            cam.DoTilt(cameraTilt * -1);

        if(pm.horizontalInput == -1)
            cam.DoTilt(cameraTilt);

        if(pm.verticalInput == 0 && pm.horizontalInput == 0)
            cam.DoLean(cameraTilt);


        //apply force to move player

        rb.AddForce(forceToApply, ForceMode.Impulse);
        

        //when the dash ends reset dashTime
        
        dashTime -= Time.deltaTime;

        if(dashTime <= 0)
            ResetDash();

        if(!useTimer)
            canDash = false;
    }

    private void ResetDash()
    {
        if(disableGravity)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = true;
        }

            cam.DoLean(0f);
            //Speedlines.SetActive(false);
            pm.dashing = false;
            dashTime = dashDuration;

            //fxReady = true;

    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction =new Vector3();

        if(allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        
        else
            direction = forwardT.forward;
        
        if(verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
