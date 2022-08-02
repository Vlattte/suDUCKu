using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    private float timerValue;
    public TextMeshProUGUI textObj;

    void Update()
    {
        timerValue += Time.deltaTime;
        DisplayTime();
    }

    void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(timerValue / 60);
        float seconds = Mathf.FloorToInt(timerValue % 60);

        textObj.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public float ManageTimerValue
    {
        get { return timerValue; }
        set { timerValue = value; }
    }
}
