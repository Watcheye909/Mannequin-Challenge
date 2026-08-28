using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeTimer : MonoBehaviour
{
    [Header("Component")]
    // you put your textmeshpro object in here
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float currenttime;
    public bool countdown;

    [Header("limit settings")]
    public bool haslimit;
    public float timerlimit;


// Start is called before the first frame update
    void Start()
    {
      
    }


    // Update is called once per frame
    void Update()
    {
        
        
        currenttime = countdown ? currenttime -= Time.deltaTime : currenttime += Time.deltaTime;
        
        if(haslimit && ((countdown && currenttime <= timerlimit) || (!countdown && currenttime >= timerlimit)))
        {
            currenttime = timerlimit;
            SetTimerText();
            timerText.color = Color.red;
            enabled = false;
        }
        
        SetTimerText();
    
    }
    
    private void SetTimerText()
    {
        
     //if you wanna change the decimals edit that in the ToString " "
        timerText.text = currenttime.ToString("0.00");

    }




}


