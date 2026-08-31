using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinAudio : MonoBehaviour
{
    GroundChaser GC;
    public AudioSource buzzing;

    // Start is called before the first frame update
    void Start()
    {
        GC = GetComponent<GroundChaser>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(GC.angry)
            buzzing.enabled = true;
        else
            buzzing.enabled = false;
    }
}
