using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncepad : MonoBehaviour
{
    public GameObject Player;
    public PlayerMovement PM;
    public Rigidbody rb;
    public float strength;

    [Header("Value Reset")]
    public float mainJumpForce;
    public float mainLowJumpMulti;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PM = Player.GetComponent<PlayerMovement>();
        rb = Player.GetComponent<Rigidbody>();
        mainJumpForce = Player.GetComponent<PlayerMovement>().jumpForce;
        mainLowJumpMulti = Player.GetComponent<PlayerMovement>().lowJumpMulti;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Debug.Log("Bouncepad Triggered");
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Player.GetComponent<PlayerMovement>().useVarJump = false;
            rb.AddForce(transform.up * strength, ForceMode.Impulse);

            /*
            //RESIZE THE PLAYER TO NEUTRAL
            //RESET THE VERTICAL VELOCITY TO 0
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            //CHANGE THE LOW JUMP MULTIPLIER SO THAT THE PLAYER GETS THE FULL JUMP FORCE
            Player.GetComponent<PlayerMovement>().lowJumpMulti = 1f;
            //APPLY THE JUMP FORCE

            Debug.Log("Bouncepad Triggered");
            PM.jumpForce = strength;
            Player.GetComponent<PlayerMovement>().Jump();
            */
        }

    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            PM.jumpForce = mainJumpForce;
            Player.GetComponent<PlayerMovement>().lowJumpMulti = mainLowJumpMulti;
        }

    }
}
