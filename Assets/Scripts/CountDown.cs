using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Counts down from startTime in custom time steps(timeScaler) and do actions on hitting 0
/// </summary>
public class CountDown : MonoBehaviour {

    public int startTime = 3;
    private float startTimeStemp;
    public Text CountDownText;
    public float timeScaler = 1;

    public UnityEvent action;

	void Start () {
        startTimeStemp = Time.time;
    }
	
	void Update () {
        int time = Mathf.RoundToInt(startTime + (startTimeStemp - Time.time) * timeScaler);
        if(time < 0)
        {
            if (action != null) action.Invoke();
            return;
        }
        CountDownText.text = time.ToString();
	}
}
