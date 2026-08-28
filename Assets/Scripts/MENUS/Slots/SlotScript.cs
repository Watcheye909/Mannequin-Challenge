using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotScript : MonoBehaviour
{
    public GameMaster GM;
    public AbilityManager AM;
    public bool empty = true;
    public slotState state = slotState.None;
    public Animator ani;
    //public KeyCode SlotKey;

    public enum slotState
    {
        None,
        dash,
        peachFloat,
        doubleJump,
        highJump,
        sprintBoost,
        groundPound,
        slowMotion,
        grappleHook,
        platform,
        speedRing
    }

    void Start()
    {
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        AM = GameObject.Find("GameMaster").GetComponent<AbilityManager>();
    }

/*
    void Update()
    {
        if(state != slotState.dash)
        {
            GM.gotDash = false;
        }

        if(state != slotState.peachFloat)
        {
            GM.gotFloat = false;
        }
    }
*/

    public void SlotCheck()
    {
       if(state != slotState.dash)
        {
            AM.gotDash = false;
        }

        if(state != slotState.peachFloat)
        {
            AM.gotFloat = false;
        } 
    }


    public void AssignState(slotState newState)
    {
        state = newState;
        empty = newState == slotState.None;
    }

    public void ClearSlot()
    {
        AssignState(slotState.None);
        empty = true;
    }
}
