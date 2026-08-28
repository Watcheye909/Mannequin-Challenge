using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public KeyCode Slot1Key = KeyCode.Q; // These keys are used to let the player select which upgrade slot to use. You can change these to whatever keys you prefer.
    public KeyCode Slot2Key = KeyCode.F;
    public KeyCode Slot3Key = KeyCode.V;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Upgrade process:
        1. Player picks up an upgrade item (not in this script, but you would have some sort of prompt to have the player choose which slot to put the upgrade in).
        2. The player presses the corresponding key (Q, F, or V) to select the upgrade slot they want to use.
        3. The upgrade is applied to the player ().
        */
    }
}
