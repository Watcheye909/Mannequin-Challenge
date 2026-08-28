using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotAnimation : MonoBehaviour
{
    public Animator ani;


    public bool isSlot1;
    public bool isSlot2;
    public bool isSlot3;


    public SlotScript currentSlot;
    public PlayerDash dashScript;
    public Hover floatScript;
    // Start is called before the first frame update
    void Start()
    {
        dashScript = GameObject.Find("Player").GetComponent<PlayerDash>();
        floatScript = GameObject.Find("Player").GetComponent<Hover>();
        /*
        if(isSlot1)
            currentSlot = GameObject.Find("SlotButton(1)").GetComponent<SlotScript>();
        
        else if(isSlot2)
            currentSlot = GameObject.Find("SlotButton(2)").GetComponent<SlotScript>();
        
        else
            currentSlot = GameObject.Find("SlotButton(3)").GetComponent<SlotScript>();
        */
    }

    // Update is called once per frame
    void Update()
    {

        //Set the animation bool checks when the slotState = a slot ability
        if(currentSlot.state == SlotScript.slotState.dash)
        {
            ani.SetBool("Float", false);
            
            ani.SetBool("Dash", true);
            
            //Check when the ability is being used
            if(dashScript.canDash)
            {
                ani.SetBool("DashUsed", false);
            }
            else if(!dashScript.canDash)
            {
                ani.SetBool("DashUsed", true);
            }
        }
        
        if(currentSlot.state == SlotScript.slotState.peachFloat)
        {
            ani.SetBool("Dash", false);
            
            ani.SetBool("Float", true);
            
            
            //Check when the ability is being used
            if(floatScript.canFloat)
            {
                ani.SetBool("FloatUsed", false);
            }
            else if(!floatScript.canFloat)
            {
                ani.SetBool("FloatUsed", true);
            }
            
            
            if(floatScript.floating)
            {
                ani.SetBool("Floating", true);
            }


        }
    }

    void FixedUpdate()
    {
        /*
        if(currentSlot == null)
        {
            if(isSlot1)
            currentSlot = GameObject.Find("SlotButton(1)").GetComponent<SlotScript>();
        
            else if(isSlot2)
            currentSlot = GameObject.Find("SlotButton(2)").GetComponent<SlotScript>();
        
            else
            currentSlot = GameObject.Find("SlotButton(3)").GetComponent<SlotScript>();
        }
        */
    }




    /*
    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //AM.playerSearch();
        if(isSlot1)
            currentSlot = GameObject.Find("SlotButton(1)").GetComponent<SlotScript>();
        
        else if(isSlot2)
            currentSlot = GameObject.Find("SlotButton(2)").GetComponent<SlotScript>();
        
        else
            currentSlot = GameObject.Find("SlotButton(3)").GetComponent<SlotScript>();
        Debug.Log($"Scene Loaded: {scene.name} (Build Index: {scene.buildIndex})");
        Debug.Log($"Load Mode: {mode}");
        
        // Example: Check if it's the first scene
        if (scene.buildIndex == 0)
        {
            Debug.Log("This is the first scene in the build order.");
        }
    }
    */
}
