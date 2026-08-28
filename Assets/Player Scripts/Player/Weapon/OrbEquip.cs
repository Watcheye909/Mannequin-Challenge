using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbEquip : MonoBehaviour
{
    public GameObject player;
    ObjectInteract grabScript;
    public SpiritOrb spiritOrbScript;
    public GameObject spiritOrbPrefab;
    public GameObject orbObject;
    public Transform pickupTarget;

    public bool isEquipped;
    //public bool canEquip;
    public KeyCode equipKey;    
    public bool allowInvoke = true;

    [Header("Timer")]
    public float equipWaitTime;
    float equipTimer;

    public float orbCooldown;
    float orbCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        grabScript = player.GetComponent<ObjectInteract>();
        isEquipped = false;
    }

    // Update is called once per frame
    void Update()
    {


        //  [WHEN THE ORB IS NOT EQUIPPED]
        
        if(grabScript.CurrentObject == null && isEquipped == false && equipTimer <= 0)
        {
            if(orbCooldownTimer > 0)
            {
                orbCooldownTimer -= Time.deltaTime;
            }

            if(Input.GetKeyDown(equipKey) && orbCooldownTimer <= 0)
            {
                OrbSpawn();
                Debug.Log("Orb equipped!");
            }
        }

        if(grabScript.CurrentObject != orbObject && isEquipped == false)
        {
            if(Input.GetKeyDown(equipKey))
            {
                Debug.Log("You don't have the orb equipped!");
            }
        }




        //  [WHEN THE ORB IS EQUIPPED]

        //assigns the current object to the cloned iteration of the spirit orb and finds the script with a slight delay to prevent null reference errors
        if(isEquipped)
        {
            orbCooldownTimer = orbCooldown;

            if (orbObject != null)
            {
                grabScript.CurrentObject = orbObject.GetComponent<Rigidbody>();

                if(spiritOrbScript == null && allowInvoke)
                {
                    Debug.Log("Script not found, invoking OrbScriptFind...");
                    allowInvoke = false;
                    Invoke("OrbScriptFind", 0.1f);
                }
            }
            else
            {
                Debug.LogWarning("OrbEquip: orbObject was destroyed while equipped. Clearing equip state.");
                isEquipped = false;
                spiritOrbScript = null;
                allowInvoke = true;
                grabScript.CurrentObject = null;
            }
        }



        // [WHEN OTHER OBJECTS ARE EQUIPPED]

        if(grabScript.CurrentObject != orbObject && isEquipped == false)
        {
            equipTimer = equipWaitTime;
        }

        if(equipTimer > 0 && grabScript.CurrentObject == null && isEquipped == false)
        {
            equipTimer -= Time.deltaTime;
        }



        //makes sure the player can only throw the orb if they have it equipped and are holding it
        if(isEquipped && spiritOrbScript != null && grabScript.CurrentObject != null && grabScript.CurrentObject.gameObject == orbObject)
        {
            //spiritOrbScript.isThrown = false;
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                grabScript.ThrowObject();
                //spiritOrbScript.isThrown = true;
                OrbRemove();
                //Invoke("OrbRemove", 0.1f);
            }
        }
    }

    void OrbSpawn()
    {
        orbObject = Instantiate(spiritOrbPrefab, pickupTarget.position, pickupTarget.rotation);
        isEquipped = true;
    }

    void OrbScriptFind()
    {
        if (orbObject == null)
        {
            Debug.LogWarning("OrbEquip: OrbScriptFind was invoked after orbObject was destroyed.");
            return;
        }

        Debug.Log("Finding spirit orb script...");
        spiritOrbScript = orbObject.GetComponent<SpiritOrb>();
    }

    void OrbRemove()
    {
        isEquipped = false;
        grabScript.CurrentObject = null;
        orbObject = null;
        spiritOrbScript = null;
        allowInvoke = true;
    }
}
