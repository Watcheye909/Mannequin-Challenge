using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerAudio : MonoBehaviour
{

    public GameObject player;
    public PlayerMovement pm;
    public AudioSource footsteps;
    public AudioSource sprintsteps;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pm = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        //Footstep audio control
        if (!pm.moving || !pm.grounded )
        {
            footsteps.enabled = false;
            sprintsteps.enabled = false;
        }
        
        if (pm.grounded && pm.moving && pm.moveSpeed == pm.walkSpeed)
        {
            footsteps.enabled = true;
            sprintsteps.enabled = false;
        }
        if (pm.grounded && pm.moving && pm.moveSpeed == pm.sprintSpeed)
        {
            sprintsteps.enabled = true;
            footsteps.enabled = false;
        }
    }
}
