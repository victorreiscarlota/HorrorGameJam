using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class Clock : Interactor
{
    [Header("Interaction")] //
    [SerializeField]
    private float interactionCooldown;

    [SerializeField] private float maxDuration;
    [SerializeField] private float interactionDurationIncrease;
    [SerializeField] private Transform NormalPointer;
    [SerializeField] private Transform InvertedPointer;
    [SerializeField] private float timeToMakeATurn;


    private bool isInCooldown;
    [SerializeField] private float remaningDuration;
    private float currentNormalRotation = 0.0f;
    private float targetRotation = 360.0f;

    [HideInInspector] public UnityEvent OnDurationEnd;
    [HideInInspector] public UnityEvent OnNewDuration;

    public void StartClock()
    {
        OnDurationEnd = new UnityEvent();
        OnNewDuration = new UnityEvent();
    }

    public override void Interact()
    {
        base.Interact();

        if (isInCooldown) return;
        remaningDuration += interactionDurationIncrease;
        remaningDuration = Mathf.Clamp(remaningDuration, 0, maxDuration);
        StartCoroutine(InteractionCooldown());
    }

    IEnumerator InteractionCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(interactionCooldown);
        isInCooldown = false;
    }

    public void UpdateClock()
    {
        if (!GameManager.Instance.IsClockActive)
        {
            if (remaningDuration > 0) OnNewDuration?.Invoke();
        }

        remaningDuration -= Time.deltaTime;
        remaningDuration = Mathf.Clamp(remaningDuration, 0, maxDuration);
        if (remaningDuration <= 0 && GameManager.Instance.IsClockActive)
        {
            OnDurationEnd?.Invoke();
        }

        if (remaningDuration <= 0) return;
        
        float rotationAmount = (targetRotation / timeToMakeATurn) * Time.deltaTime;
        currentNormalRotation += rotationAmount;

        if (currentNormalRotation >= targetRotation)
        {
            currentNormalRotation = 0;
        }


        NormalPointer.Rotate(new Vector3(0, rotationAmount, 0));
        InvertedPointer.Rotate(new Vector3(0, -rotationAmount, 0));
    }
}