using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement pm;
    public Rigidbody rb;

    public bool canDouble;
    public bool doubleReady;
    public float doubleJumpHeight;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pm = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pm.grounded && !pm.readyToJump)
            canDouble = true;
        if(pm.grounded)
            canDouble = false;

        if(Input.GetKeyDown(pm.jumpKey) && canDouble && !pm.grounded && !pm.wallrunning && !pm.sliding && !pm.dashing)
        {
            doubleReady = true;
            canDouble = false;
        }
    }

    void FixedUpdate()
    {
        if(doubleReady)
        Jump();
    }

    public void Jump()
    {
        //exitSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * doubleJumpHeight, ForceMode.Impulse);
        canDouble = false;
        doubleReady = false;
    }
}
