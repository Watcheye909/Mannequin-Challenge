using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashOld : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    public PlayerMovementOld pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Settings")]
    public bool useCamForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.Q;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementOld>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(dashKey))
            Dash();

        if(dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if(dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;

        pm.dashing = true;

        Transform forwardT;

        if(useCamForward)
            forwardT = playerCam; // where you're looking
        else
            forwardT = orientation; // where you're facing (no up or down)

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if(disableGravity)
            rb.useGravity = false;
        
        Invoke(nameof(delayedForceToApply),0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if(resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;

        if(disableGravity)
            rb.useGravity = true;
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
