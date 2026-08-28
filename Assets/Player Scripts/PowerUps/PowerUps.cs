using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum AbilityType
    {
        None,
        Dash,
        peachFloat,
        DoubleJump,
        HighJump,
        SprintBoost
    }

    public AbilityType abilityType;

    /*
    Upgrade process:
    1. Player picks up an upgrade item (not in this script, but you would have some sort of prompt to have the player choose which slot to put the upgrade in).
    2. The player presses the corresponding key (Q, F, or V) to select the upgrade slot they want to use.
    3. The upgrade is applied to the player ().


    HOW TO CREATE AN UPGRADE:
    1. make a bool to assign what ability the powerup gives you
    
    2. add a reference variable to the ability itself. EXAMPLE - having the variable PDash to reference the script on the player
    (This part is necessary for all abilities that aren't just stat changes. all the abilities are already apart of the player 
    just disabled until the power up is picked up)

    3. make a new function for the ability. EXAMPLE - GainDash that enables the ability script and get rid of this game object from the scene
    
    4. Set the ability in the other methods involving states and keybinds (as well as putting it in the random ability selection)
    (YOU MUST ADD VARIABLES IN THE FOLLOWING SCRIPTS - GameMaster, ObjectInteract, SlotButtons, and SlotScript)


    ---BASICALLY JUST COPY EVERYTHING THE DASH DOES---
    */ 

    [Header("Settings")]
    public bool randomize;

    [Header("Slot Abilities")]
    public bool dashUpgrade;
    public bool floatUpgrade;
    
    [Header("Upgraded Moves")]
    public bool doubleJumpUpgrade;

    [Header("Stat Upgrades")]
    public bool highjumpUpgrade;
    public bool sprintUpgrade;


    [Header("Stat Upgrade Values")]
    public float newJumpValue;
    public float newSprintValue;

    [Header("TextMesh")]
    public TextMeshPro description;
    public bool showText;
    //public String currentTime;

    [Header("References")]
    public GameObject manager;
    public GameMaster GM;
    public AbilityManager AM;
    public GameObject Player;
    public Transform playerPos;
    public PlayerMovement pm;
    public PlayerDash PDash;
    public Hover PFloat;

    public DoubleJump doublej;

    [Header("Slot Assignment")]
    public KeyCode assignedSlotKey = KeyCode.None;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameMaster");
        GM = manager.GetComponent<GameMaster>();
        AM = manager.GetComponent<AbilityManager>();

        Player = GameObject.Find("Player");
        pm = Player.GetComponent<PlayerMovement>();
        PDash = Player.GetComponent<PlayerDash>();
        PFloat = Player.GetComponent<Hover>();
        
        doublej = Player.GetComponent<DoubleJump>();

        if(randomize)
            RandomizeUpgrade();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = Player.transform;
        description.transform.LookAt(playerPos);
        description.transform.forward = Camera.main.transform.forward;

        if(!showText)
        {
            description.text = ("");
        }

        else if (dashUpgrade)
        {
            description.text = ("DASH ABILITY (USES SLOT)");
        }
        else if (floatUpgrade)
        {
            description.text = ("FLOAT ABILITY (USES SLOT)");
        }
        else if (doubleJumpUpgrade)
        {
            description.text = ("DOUBLE JUMP");
        }
        else if (highjumpUpgrade)
        {
            description.text = ("JUMP STAT UPGRADE");
        }
        else if (sprintUpgrade)
        {
            description.text = ("SPRINT STAT UPGRADE");
        }

        GetAbilityType();
    }

    private void RandomizeUpgrade()
    {
        dashUpgrade = false;
        floatUpgrade = false;

        doubleJumpUpgrade = false;
        
        highjumpUpgrade = false;
        sprintUpgrade = false;

        int randomIndex = UnityEngine.Random.Range(0, 5);

        switch (randomIndex)
        {
            case 0:
                if(AM.gotDash)
                {
                    RandomizeUpgrade();
                }
                else
                {
                    dashUpgrade = true;
                }
                break;
            case 1:
                if(AM.gotFloat)
                {
                    RandomizeUpgrade();
                }
                else
                {
                    floatUpgrade = true;
                }
                break;
            case 2:
                if(AM.gotDoubleJump)
                {
                    RandomizeUpgrade();
                }
                else
                {    
                    doubleJumpUpgrade = true;
                }
                break;
            case 3:
                highjumpUpgrade = true;
                break;
            case 4:
                sprintUpgrade = true;
                break;
        }
    }

    //[ABILTIES]
    public void gainDash()
    {
        if (assignedSlotKey != KeyCode.None && PDash != null)
        {
            PDash.dashKey = assignedSlotKey;
            // Store in GameMaster so it persists after player death
            if (AM != null)
            {
                AM.dashSlotKey = assignedSlotKey;
            }
        }

        /*
        if (PDash != null)
        {
            PDash.enabled = true;
        }
        */

        if (AM != null)
        {
            AM.gotDash = true;
            AM.playerSearch();
        }
        

        this.gameObject.SetActive(false);
    }

    public void gainFloat()
    {
        if (assignedSlotKey != KeyCode.None && PFloat != null)
        {
            PFloat.floatKey = assignedSlotKey;
            // Store in GameMaster so it persists after player death
            if (AM != null)
            {
                AM.floatSlotKey = assignedSlotKey;
            }
        }

        /*
        if (PFloat != null)
        {
            PFloat.enabled = true;
        }
        */

        if (AM != null)
        {
            AM.gotFloat = true;
            AM.playerSearch();
        }
        
        this.gameObject.SetActive(false);
    }

    public void gainDoubleJump()
    {
        if (doublej != null)
        {
            doublej.enabled = true;
        }

        if (AM != null)
        {
            AM.gotDoubleJump = true;
            AM.playerSearch();
        }

        this.gameObject.SetActive(false);
    }



    //[STAT UPGRADES]
    public void gainHighJump()
    {
        AM.playerJumpHeight += newJumpValue;
        
        //old stuff
        //pm.storeJumpForce += newJumpValue;
        //pm.jumpForce = pm.storeJumpForce;

        if (AM != null)
        {
            AM.gotHighJump = true;
            AM.playerSearch();
        }

        this.gameObject.SetActive(false);
    }

    public void gainSpeedBoost()
    {
        AM.playerSpeed += newSprintValue;

        if (AM != null)
        {
            AM.gotSprintBoost = true;
            AM.playerSearch();
        }

        this.gameObject.SetActive(false);
    }





    public void AssignSlotKey(KeyCode key)
    {
        assignedSlotKey = key;

        if (dashUpgrade && PDash != null)
        {
            PDash.dashKey = assignedSlotKey;
            // Store in GameMaster so it persists after player death
            if (AM != null)
            {
                AM.dashSlotKey = key;
            }
        }

        if(floatUpgrade && PFloat != null)
        {
            PFloat.floatKey = assignedSlotKey;
            if(AM != null)
            {
                AM.floatSlotKey = key;
            }
        }
    }

    public AbilityType GetAbilityType()
    {

        if (abilityType != AbilityType.None)
        {
            return abilityType;
        }
        if (dashUpgrade)
        {
            return abilityType = AbilityType.Dash;
        }
        if (floatUpgrade)
        {
            return abilityType = AbilityType.peachFloat;
        }
        if (doubleJumpUpgrade)
        {
            return abilityType = AbilityType.DoubleJump;
        }
        if (highjumpUpgrade)
        {
            return abilityType = AbilityType.HighJump;
        }
        if (sprintUpgrade)
        {
            return abilityType = AbilityType.SprintBoost;
        }

        return AbilityType.None;
    }

    public void ApplyPendingUpgrade()
    {
        switch (GetAbilityType())
        {
            case AbilityType.Dash:
                gainDash();
                break;
            case AbilityType.peachFloat:
                gainFloat();
                break;
            case AbilityType.DoubleJump:
                gainDoubleJump();
                break;
            case AbilityType.HighJump:
                gainHighJump();
                break;
            case AbilityType.SprintBoost:
                gainSpeedBoost();
                break;
            default:
                Debug.LogWarning("PowerUps: No pending upgrade to apply.");
                break;
        }
    }

    

}
