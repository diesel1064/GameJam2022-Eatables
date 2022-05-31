using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float startingTime;
    public UnityEvent timesUp;
    
    private static float _startingTime;
    private static float _timeRemaining;
    private bool _triggeredEvent;
    private bool _startTimer;

    public static float GetStartingTime() => _startingTime;
    public static float GetTimeRemaining() => _timeRemaining;

    private void Start()
    {
        _startingTime = startingTime;
        _timeRemaining = _startingTime;
        _triggeredEvent = false;
    }

    private void Update()
    {
        if (_startTimer)
            UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }
        else if (!_triggeredEvent)
        {
            timesUp.Invoke();
            _triggeredEvent = true;
        }
    }

    public void StartTimer()
    {
        _startTimer = true;
    }
}
