using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Hover : MonoBehaviour
{
    //PEACH FLOAT ABILITY just renamed cause float is already a variable
    [Header("References")]
    public Rigidbody rb;
    public PlayerMovement pm;
    
    
    [Header("Hovering")]
    public bool floating;
    public float floatUpwardForce; //not needed rn
    public float floatDuration; //how long the hover lasts for
    public float floatTime;

    public bool canFloat;

    [Header("Settings")]
    //public bool allowAllDirections = true;
    //public bool disableGravity = false;
    //public bool useTimer;
    //public float cameraTilt;

    [Header("Cooldown")]
    public float floatCooldown; //this time can be changed in the engine to assign the time that the cooldowntimer resets to
    public float floatCoolTime;
    //private bool fxReady;

    [Header("Input")]
    public KeyCode floatKey;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        canFloat = true;

        //floatCoolTime = floatCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(floating && floatTime > 0)
            floatTime -= Time.deltaTime;
        
        
        if(floatCoolTime > 0)
            floatCoolTime -= Time.deltaTime;

        
        
        if(floatCoolTime <= 0 && !canFloat && !floating)
            canFloat = true;


        if(Input.GetKeyDown(floatKey))
        {
            if(canFloat && !pm.grounded)
            {
                LevitateCheck();
            }
        }

        if(Input.GetKeyUp(floatKey) && floating)
            ResetLevitate();

        if(floating)
            Levitate();
    }

    void LevitateCheck()
    {
        if(floatCoolTime > 0) return;
        if(pm.grounded) return;

        if(!floating)
            floatTime = floatDuration;
        
        floating = true;

        canFloat= false;

    }

    //NOT SURE IF THIS THING WILL BE NECESSARY SO I'M lEAVING IT AS A PLACEHOLDER
    void Levitate()
    {
        if(rb.velocity.y <= 0)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.mass = 1;
        }

        if(rb.velocity.y > 0 && rb.velocity.y < 2)
        {
            //rb.mass = 0.15f;
            //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //rb.AddForce(transform.up * floatUpwardForce, ForceMode.Impulse);
        }
        
        rb.useGravity = false;
        
        if(floatTime <= 0)
            ResetLevitate();
    }

    private void ResetLevitate()
    {
        floating = false;
        
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;

        //cam.DoLean(0f);
        //Speedlines.SetActive(false);
        //pm.dashing = false;
        
        //floatTime = floatDuration;
        floatCoolTime = floatCooldown;
        //fxReady = true;

    }
}
