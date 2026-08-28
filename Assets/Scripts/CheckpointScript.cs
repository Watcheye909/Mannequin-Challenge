using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{

    public GameMaster GM;
    //public LeTimer Timer;

    //public float timeActivated;
     public bool pointTouched;
    //public Transform CheckpointPosition;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //Timer = GameObject.FindGameObjectWithTag("GM").GetComponent<LeTimer>();
        pointTouched = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            pointTouched = true;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (pointTouched)
        {
            Debug.Log("Checkpoint Active");
            GM.lastCheckPointPos = gameObject.transform.position;
            GM.checkpointActivated = true;
            //GameObject.SetActive(false);
        }
    }
}
