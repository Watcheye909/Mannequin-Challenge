using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{
    public GameMaster GM;
    public bool allowInvoke = true;

    [Header("Settings")]
    public bool upgradeRequired;

    [Header("References")]
    public GameObject upgrade1;
    public GameObject upgrade2;
    public GameObject upgrade3;


    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        
        if(upgradeRequired)
            this.gameObject.SetActive(false);
    }

    void Update()
    {
        if(upgradeRequired && (upgrade1 == null || upgrade2 == null || upgrade3 == null))
            this.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            GM.lastCheckPointPos = GM.startCheckPointPos;
            Invoke("NextLevel", 0.001f);
        }
    }

    public void NextLevel()
    {
        GM.lastCheckPointPos = GM.startCheckPointPos;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //GM.playerSearch();
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
