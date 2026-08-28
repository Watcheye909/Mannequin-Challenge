using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class OutlineDetection : MonoBehaviour
{
    public Outline OL;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    	{
			if (collision.gameObject.layer == 3)
			{
				OL.eraseRenderer = false;
				Debug.Log("BOX GLOW");
				//GameObject.SetActive(false);
			}

    	}

		void OnTriggerExit(Collider collision)
    	{
			if (collision.gameObject.layer == 3)
			{
				OL.eraseRenderer = true;
				Debug.Log("STOP BOX GLOW");
				//GameObject.SetActive(false);
			}

    	}
}
