using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MannequinGameMaster : MonoBehaviour
{
    public Timer timer;
    public GameObject LiveMannequin;
    public GroundChaser GC;
    
    public GameObject mannequinSquad;

    public int stageSelect;


    public int currentRound;
    public bool nextRoundReady;
    public bool roundFailed;

    public bool randomize;
    public int randomIndex;

    public bool playerDied;

    [Header("KeyCode")]
    public KeyCode returnKey;

    // Start is called before the first frame update
    void Start()
    {
        //search and assign variables
        timer = GameObject.Find("PLAYER UI").GetComponent<Timer>();
        
        //GC = GameObject.Find("LIVING MANNEQUIN").GetComponent<GroundChaser>();
        randomIndex = UnityEngine.Random.Range(0, 3);
        
        
        //RandomizeMannequinCheck();

        //randomize = true;

    }

    // Update is called once per frame
    void Update()
    {
        // 1. Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // 2. Check if the scene name matches your target scene
        if (currentScene.name != "Intro")
            RandomizeMannequinCheck();


        if(GC != null && GC.health <= 0)
        {
            nextRoundReady = true;
        }


        if(timer.currentTime <= 0)
            roundFailed = true;

        //spirit chases the player at high speeds if the timer runs out
        if(roundFailed)
        {
            GC.angry = true;
            LoseRound();
        }

        if(playerDied)
        {
            currentRound = 0;
            GC.angry = false;
            playerDied = false;
        }
        


        if (Input.GetKey(returnKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        
    }


    void NextRoundSwitch()
    {
        randomIndex = UnityEngine.Random.Range(0, 3);
        RandomizeMannequinCheck();
    }

    void LoseRound()
    {
        if(GC.attacking)
        {
            playerDied = true;
        }
    }

    void RandomizeMannequinCheck()
    {
        //int randomIndex = UnityEngine.Random.Range(0, 5);
        switch(randomIndex)
        {
            case 0:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 1");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                Debug.Log("is 0");
                break;
            case 1:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 2");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                Debug.Log("is 1");
                break;
            case 2:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 3");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                Debug.Log("is 2");
                break;
            case 3:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 4");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                Debug.Log("is 3");
                break;
        }

        if(LiveMannequin != null)
            GC.enabled = true;

        //randomize = false;
    }
}
