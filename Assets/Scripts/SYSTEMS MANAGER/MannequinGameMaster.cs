using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MannequinGameMaster : MonoBehaviour
{
    private static MannequinGameMaster instance;
    public PlayerCamera playerCam;
    public Timer timer;
    public GameObject LiveMannequin;
    public GroundChaser GC;
    
    public GameObject mannequinSquad;

    public int stageSelect;


    public int playerScore;
    public bool nextRoundReady;
    public bool roundFailed;

    public bool randomize;
    public int randomIndex;

    public bool playerDied;

    [Header("Camera Settings")]
    public float shakeAmt;
    public float shakeLength;

    [Header("KeyCode")]
    public KeyCode returnKey;


    
    // Start is called before the first frame update
    void Start()
    {
        //search and assign variables
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        timer = GameObject.Find("PLAYER UI").GetComponent<Timer>();
        
        //GC = GameObject.Find("LIVING MANNEQUIN").GetComponent<GroundChaser>();
        randomIndex = UnityEngine.Random.Range(0, 3);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);
        
        
        //RandomizeMannequinCheck();

        //randomize = true;

    }

    // Update is called once per frame
    void Update()
    {
        //  =====>ROUND SETUP<======
        
        // 1. Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // 2. Check if the scene name matches your target scene
        if (currentScene.name != "Intro" && GC == null && roundFailed == false)
            RandomizeMannequinCheck();

        if(timer == null)
        ReassignVariables();


        // =====>During Round<=====

        if(timer.currentTime <= 30f)
            playerCam.DoShake(shakeAmt, shakeLength);
        
        else if(timer.currentTime <= 20f)
            playerCam.DoShake(shakeAmt*2, shakeLength);

        else if(timer.currentTime <= 10f)
            playerCam.DoShake(shakeAmt*3, shakeLength);

        //  =====>Win Condition Check<======

        if(GC != null && GC.health <= 0)
        {
            LiveMannequin.SetActive(false);
            nextRoundReady = true;
        }

        if(nextRoundReady)
        {
            NextRoundSwitch();
            return;
        }


        //  =====>Lose Condition<======

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
            playerScore = 0;
            GC.angry = false;
            playerDied = false;
        }
        

        //  =====>Intro Scene Skip<======


        if (Input.GetKey(returnKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        
    }


    void NextRoundSwitch()
    {
        if (!nextRoundReady)
            return;

        nextRoundReady = false;
        playerScore++;
        randomIndex = UnityEngine.Random.Range(0, 3);

        //randomIndex = UnityEngine.Random.Range(0, 3);
        //RandomizeMannequinCheck();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoseRound()
    {
        playerScore = 0;
        if(GC.attacking)
        {
            playerDied = true;
        }
    }


    void ReassignVariables()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        timer = GameObject.Find("PLAYER UI").GetComponent<Timer>();
    }

    void RandomizeMannequinCheck()
    {
        //int randomIndex = UnityEngine.Random.Range(0, 5);
        switch(randomIndex)
        {
            case 0:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 1");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                //Debug.Log("is 0");
                break;
            case 1:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 2");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                //Debug.Log("is 1");
                break;
            case 2:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 3");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                //Debug.Log("is 2");
                break;
            case 3:
                LiveMannequin = GameObject.Find("LIVING MANNEQUIN 4");
                GC = LiveMannequin.GetComponent<GroundChaser>();
                //Debug.Log("is 3");
                break;
        }

        if(LiveMannequin != null)
            GC.enabled = true;

        //randomize = false;
    }
}
