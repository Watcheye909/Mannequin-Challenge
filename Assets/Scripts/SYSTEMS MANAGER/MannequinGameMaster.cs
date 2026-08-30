using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinGameMaster : MonoBehaviour
{
    public Timer timer;
    public GroundChaser GC;
    
    public int mannequinSelect;


    public int currentRound;
    public bool nextRoundReady;
    public bool roundFailed;

    // Start is called before the first frame update
    void Start()
    {
        //search and assign variables
        timer = GetComponent<Timer>();
        GC = GameObject.Find("LIVING MANNEQUIN").GetComponent<GroundChaser>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GC.health <= 0)
        {
            nextRoundReady = true;
        }


        if(timer.currentTime <= 0)
            roundFailed = true;

        //spirit chases the player at high speeds if the timer runs out
        if(roundFailed)
        {
            
        }
        

        
    }


    private void randomizeMannequin()
    {

    }
}
