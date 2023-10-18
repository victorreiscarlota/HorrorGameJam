using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform NormalPointer;
    [SerializeField] private Transform InvertedPointer;
    
    private float currentNormalRotation = 0.0f;
    private float currentInvertedRotation = 0.0f;
    private float targetRotation = 360.0f;

    [HideInInspector] public UnityEvent<bool> OnClockChange;
    public bool normalClockEnabled;
    public bool invertedClockEnabled;

    private void Start()
    {
        OnClockChange = new UnityEvent<bool>();
    }

    public void UpdateNormalClock(float duration)
    {
        float rotationAmount = (targetRotation / duration) * Time.deltaTime;
        currentNormalRotation += rotationAmount;

        if (currentNormalRotation >= targetRotation)
        {
            normalClockEnabled = false;
            invertedClockEnabled = true;
            currentNormalRotation = 0;
            OnClockChange?.Invoke(true);
        }


        NormalPointer.Rotate(new Vector3(0, rotationAmount, 0));
    }

    public void UpdateInvertedClock(float duration)
    {
        float rotationAmount = (targetRotation / duration) * Time.deltaTime;
        currentInvertedRotation += rotationAmount;

        if (currentInvertedRotation >= targetRotation)
        {
            invertedClockEnabled = false;
            normalClockEnabled = true;
            OnClockChange?.Invoke(false);
            currentInvertedRotation = 0;
        }
        
        InvertedPointer.Rotate(new Vector3(0, -rotationAmount, 0));
    }
}