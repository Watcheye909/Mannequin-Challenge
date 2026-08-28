using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectZone : MonoBehaviour
{
    public GroundChaser GC;
    // Start is called before the first frame update
    void Start()
    {
        GC = GetComponent<GroundChaser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            GC.playerInSightRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            GC.playerInSightRange = false;
        }
    }
}
