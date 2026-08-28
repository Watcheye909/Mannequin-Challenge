using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;
    public PlayerCamera cam;

    [Header("Camera")]
    public int slideFOV;
    public int slideJumpFOV;

    [Header("Sliding")]
    public bool startedSlide;
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public float slideMulti;
    public float sJumpHeight;

    Vector3 inputDirection;

    private Vector3 slideDir;

    //Scale variables
    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.C;
    private float horizontalInput;
    private float verticalInput;


    public bool allowInvoke = true;
    //public bool sliding;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;

        pm.storeJumpForce = pm.jumpForce;
    }



    //UPDATE



    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //starting slide condition
        if (Input.GetKeyDown(slideKey) && !pm.sliding && (horizontalInput != 0 || verticalInput != 0) && pm.moveSpeed >= 5 && pm.grounded)
        {
            startedSlide = true;
            StartSlide();
        }
    /*
        //end slide condition
        if (Input.GetKeyDown(slideKey) && pm.sliding || pm.nearRoof)
        {
            StopSlide();
        }
    */
        // SLIDE JUMP FUNCTION
        if (Input.GetKeyDown(pm.jumpKey) && pm.sliding)
        {
            SlideBoost();
            //Invoke("SlideBoost", 0.1f);
            Invoke("StopSlide", 0.2f);
        }

        //stops the slided if the speed is to low
        if(rb.velocity.magnitude < 4)
            StopSlide();

    /* OLD INPUT SYSTEM
        //starting slide condition
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && pm.moveSpeed >= 5 && pm.grounded)
            StartSlide();

        //end slide condition
        if (Input.GetKeyUp(slideKey) && pm.sliding || pm.nearRoof)
        {
            StopSlide();
        }

        // SLIDE JUMP FUNCTION
        if (Input.GetKeyDown(pm.jumpKey) && pm.sliding)
        {
            SlideBoost();
            Invoke("SlideBoost", 0.1f);
            Invoke("StopSlide", 0.2f);
        }
    */ 

    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }



    //METHODS



    private void StartSlide()
    {
        pm.sliding = true;

        if(startedSlide){
        inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;    
        startedSlide = false;
        }
        
        //slideDir = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        //changing player size for the slide
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 4f, ForceMode.Impulse);

        //adjusting jump value to allow a slide jump to be performed
        pm.jumpForce += sJumpHeight;

        //starts the slide timer
        slideTimer = maxSlideTime;

    }

    private void SlidingMovement()
    {
        if (pm.grounded)
        {
            cam.DoTilt(2f);
            cam.DoFov(slideFOV);
        }

        if(pm.grounded == false)
        {
            cam.DoTilt(0f);
            cam.DoFov(cam.FOV);
        }


        
        //Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //normal slide
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //sliding down slopes
        else
        {
            rb.AddForce(pm.GetSlopeDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        cam.DoTilt(0f);
        cam.DoFov(cam.FOV);
        pm.sliding = false;
        pm.jumpForce = pm.storeJumpForce;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    private void SlideBoost()
    {
        pm.desiredSpeed += 2f;
        cam.DoFov(slideJumpFOV);
    }
}
