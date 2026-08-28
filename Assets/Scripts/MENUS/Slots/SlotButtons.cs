using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotButtons : MonoBehaviour
{
    public UpgradeMenu um;
    public GameMaster gm;
    public GameObject player;
    public PowerUps pendingPower;

    public GameObject slot1;
    public SlotScript slot1Script;

    public GameObject slot2;
    public SlotScript slot2Script;

    public GameObject slot3;
    public SlotScript slot3Script;

    [Header("Slot Keybindings")]
    public KeyCode slot1Key;
    public KeyCode slot2Key;
    public KeyCode slot3Key;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = GameObject.Find("SlotButton(1)");
        slot2 = GameObject.Find("SlotButton(2)");
        slot3 = GameObject.Find("SlotButton(3)");

        slot1Script = GameObject.Find("SlotButton(1)").GetComponent<SlotScript>();
        slot2Script = GameObject.Find("SlotButton(2)").GetComponent<SlotScript>();
        slot3Script = GameObject.Find("SlotButton(3)").GetComponent<SlotScript>();
    }

    public void SetPendingPower(PowerUps power)
    {
        pendingPower = power;
    }

    public void Slot1Select()
    {
        Debug.Log("Slot 1 Selected");
        AssignSlotToSelectedPower(slot1Key, slot1Script);
    }

    public void Slot2Select()
    {
        Debug.Log("Slot 2 Selected");
        AssignSlotToSelectedPower(slot2Key, slot2Script);
    }

    public void Slot3Select()
    {
        Debug.Log("Slot 3 Selected");
        AssignSlotToSelectedPower(slot3Key, slot3Script);
    }

    private void AssignSlotToSelectedPower(KeyCode key, SlotScript slotScript)
    {
        if (pendingPower == null)
        {
            Debug.LogWarning("SlotButtons: No PowerUps reference assigned.");
            return;
        }

        pendingPower.AssignSlotKey(key);
        slotScript.ClearSlot();
        slotScript.AssignState(ConvertPowerToSlotState(pendingPower));
        pendingPower.ApplyPendingUpgrade();
        pendingPower = null;

        if (um != null)
        {
            um.MenuClose();
        }
    }

    private SlotScript.slotState ConvertPowerToSlotState(PowerUps power)
    {
        if(power == null)
            return SlotScript.slotState.None;

        if(power.dashUpgrade)
            return SlotScript.slotState.dash;
        if(power.floatUpgrade)
            return SlotScript.slotState.peachFloat;
        if(power.doubleJumpUpgrade)
            return SlotScript.slotState.doubleJump;
        if(power.highjumpUpgrade)
            return SlotScript.slotState.highJump;
        if(power.sprintUpgrade)
            return SlotScript.slotState.sprintBoost;

        return SlotScript.slotState.None;
    }
}
