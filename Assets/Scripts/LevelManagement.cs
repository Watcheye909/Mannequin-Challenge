using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagement : MonoBehaviour
{
    public GameObject Manager;
    public GameMaster GM;
    public AbilityManager AM;
    
    public GameObject player;
    public PlayerMovement pm;
    public PlayerDash PDash;
    public DoubleJump doublej;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        pm = player.GetComponent<PlayerMovement>();
        PDash = player.GetComponent<PlayerDash>();
        doublej = player.GetComponent<DoubleJump>();

        Manager = GameObject.Find("player");
        GM = Manager.GetComponent<GameMaster>();
        AM = Manager.GetComponent<AbilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(AM.gotDash)
        {
            PDash.enabled = true;
        }
    }
}
