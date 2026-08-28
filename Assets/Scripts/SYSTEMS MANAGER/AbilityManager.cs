using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    /*
        This script serves the purpose of keeping track of ability values
        and feeding the information to the GameMaster script
    */

    [Header("References")]
    public GameMaster GM;
    public UpgradeMenu UM;
    public GameObject player;
    public PlayerHealth Health;
    public PlayerMovement PM;
    public WallRunning wallScript;
    public PlayerDash dashScript;
    public Hover floatScript;
    public DoubleJump doubleJumpScript;


    [Header("Slot Settings")]
    GameObject Slot1;
    GameObject Slot2;
    GameObject Slot3;
    SlotScript SlotScript1;
    SlotScript SlotScript2;
    SlotScript SlotScript3;
    public SlotButtons slotScript;

    [Header("Keycode Related")]
    public KeyCode dashSlotKey = KeyCode.None;
    public KeyCode floatSlotKey = KeyCode.None;


    [Header("Player Stats")]
    public float playerHealth;
    public float playerSpeed;
    public float playerJumpHeight;

    [Header("Player Base")]
    //THIS VALUE SHOULD NEVER CHANGE
    public float baseHealth;
    public float baseSpeed;
    //float baseSprint;
    public float baseWallrun; 
    public float baseJumpHeight;

    [Header("Stat Boost Amount (IGNORE)")]
    //non of these are being used cause of how I handle stats now (go to PowerUps.cs)
    public float newHealth;
    public float newSpeed;
    public float newJumpHeight;
    //add more based on the stat upgrades added to the game


    [Header("Dash Stats")]
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;

    [Header("Ability Checks")]
    public bool gotDash;
    public bool gotFloat;
    public bool gotDoubleJump;
    public bool gotHighJump;
    public bool gotSprintBoost;

    [Header("Game UI References")]
    public SlotAnimation slotAni1;
    public SlotAnimation slotAni2;
    public SlotAnimation slotAni3;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        UM = GameObject.Find("Menu").GetComponent<UpgradeMenu>();

        Health = player.GetComponent<PlayerHealth>();
        


        Slot1 = GameObject.Find("SlotButton(1)");
        Slot2 = GameObject.Find("SlotButton(2)");
        Slot3 = GameObject.Find("SlotButton(3)");

        SlotScript1 = Slot1.GetComponent<SlotScript>();
        SlotScript2 = Slot2.GetComponent<SlotScript>();
        SlotScript3 = Slot3.GetComponent<SlotScript>();



        baseHealth = Health.health;
        baseSpeed = PM.sprintSpeed;
        //float baseSprint;
        baseWallrun = PM.wallRunSpeed; 
        baseJumpHeight = PM.jumpForce;

        
        playerSpeed = baseSpeed;
        playerJumpHeight = baseJumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerSearch()
    {
        //GameUI Slot Related
        slotAni1 = GameObject.Find("Slot 1").GetComponent<SlotAnimation>();
        slotAni2 = GameObject.Find("Slot 2").GetComponent<SlotAnimation>();
        slotAni3 = GameObject.Find("Slot 3").GetComponent<SlotAnimation>();

        AssignSlotAnimation(slotAni1);
        AssignSlotAnimation(slotAni2);
        AssignSlotAnimation(slotAni3);

        if(GM == null)
            GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        player = GameObject.Find("Player");
        PM = player.GetComponent<PlayerMovement>();
        UM.cam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();

        //playerSpeed = PM.sprintSpeed;
        //playerJumpHeight = PM.jumpForce;

        //Ability References
        dashScript = player.GetComponent<PlayerDash>();
        floatScript = player.GetComponent<Hover>();
        
        doubleJumpScript = player.GetComponent<DoubleJump>();
        
        
        //SLOT ABILITY CHECK
        SlotCheck();

        //Dash
        if(gotDash)
        {
            dashScript.enabled = true;
            
            // Restore assigned slot key if it was set
            if(dashSlotKey != KeyCode.None)
            {
                dashScript.dashKey = dashSlotKey;
            }
        }
        else
        {
            dashScript.enabled = false;
        }


        //Float
        if(gotFloat)
        {
            floatScript.enabled = true;
            
            // Restore assigned slot key if it was set
            if(floatSlotKey != KeyCode.None)
            {
                floatScript.floatKey = floatSlotKey;
            }
        }
        else
        {
            floatScript.enabled = false;
        }





        //OTHER ABILITY
        if(gotDoubleJump)
        {
            doubleJumpScript.enabled = true;
        }
        else
        {
            doubleJumpScript.enabled = false;
        }


        if(gotHighJump)
        {
            PM.storeJumpForce = playerJumpHeight;
            PM.jumpForce = playerJumpHeight;

            

            //PM.storeJumpForce += newJumpHeight;
            //PM.jumpForce += newJumpHeight;
        }
        if(gotSprintBoost)
        {
            PM.sprintSpeed = playerSpeed;
        }


    }

    public void SlotCheck()
    {

        //Checks if any slot has dash before making gotDash = false
        if(SlotScript1.state != SlotScript.slotState.dash && SlotScript2.state != SlotScript.slotState.dash && SlotScript3.state != SlotScript.slotState.dash)
        {
            dashSlotKey = KeyCode.None;
            gotDash = false;
        }

        if(SlotScript1.state != SlotScript.slotState.peachFloat && SlotScript2.state != SlotScript.slotState.peachFloat && SlotScript3.state != SlotScript.slotState.peachFloat)
        {
            floatSlotKey = KeyCode.None;
            gotFloat = false;
        } 
    }

    private void AssignSlotAnimation(SlotAnimation slotAni)
    {
        if (slotAni == null)
            return;

        if (slotAni.isSlot1)
            slotAni.currentSlot = SlotScript1;
        else if (slotAni.isSlot2)
            slotAni.currentSlot = SlotScript2;
        else if (slotAni.isSlot3)
            slotAni.currentSlot = SlotScript3;
        else
            slotAni.currentSlot = null;
    }
}
