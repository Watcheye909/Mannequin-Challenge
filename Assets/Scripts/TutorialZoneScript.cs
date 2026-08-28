using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoneScript : MonoBehaviour
{
    bool activeTuto;

    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    // Start is called before the first frame update
    void Start()
    {
        activeTuto = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTuto == false)
        {
            Destroy(text1, 1f);
            Destroy(text2, 1f);
            Destroy(text3, 0f);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 3)
        {
            activeTuto = true;
        }

    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            activeTuto = false;
        }
    }
}