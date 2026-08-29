using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI timerText;

    public float currentTime = 30f;
    private bool active = true;

    private void Update()
    {
        if (!active)
            return;

        currentTime -= Time.deltaTime;

        UpdateTimerUI();

        if (currentTime <= 0)
        {
            StopTimer();
        }
    }

    public void StopTimer()
    {
        active = false;
        currentTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (currentTime > 0 && currentTime < 6)
        {
            timerText.color = Color.yellow;
        }
        else if (currentTime < 1)
        {
            timerText.color = Color.red;
        }

        TimeSpan t = TimeSpan.FromSeconds(currentTime);
        timerText.text = t.ToString(@"mm\:ss");
    }
}