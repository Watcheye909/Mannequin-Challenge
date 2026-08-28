using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    // --THIS ONLY OPENS THE UPGRADE MENU, SLOT ASSIGNMENT HAPPENS IN SLOTBUTTONS--

    public bool isOpen;

    [Header("References")]
    public GameObject upgradeMenu;
    public GameMaster gm;
    public PlayerCamera cam;
    //public PowerUps power;

    /*
    [Header("Slot Keybindings")]
    public KeyCode slot1Key;
    public KeyCode slot2Key;
    public KeyCode slot3Key;
    */


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        cam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        //power = GameObject.Find("PowerUps").GetComponent<PowerUps>();
        isOpen = false;
        upgradeMenu.SetActive(isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuOpen()
    {
        isOpen = true;
        upgradeMenu.SetActive(isOpen);
        if (isOpen)
        {
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
            cam.enabled = false; // Disable the camera movement
        }
    }
    public void MenuClose()
    {
        isOpen = false;
        upgradeMenu.SetActive(isOpen);
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        cam.enabled = true; // Enable the camera movement
    }

    //player picks up upgrade
    //upgrade menu opens, player can select slot to place upgrade in
    //player selects slot, upgrade is placed in slot and is assigned to the corresponding keybind, menu closes, player can use upgrade
}
