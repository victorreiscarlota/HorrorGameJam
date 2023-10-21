using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputControls playerInput;
    public Vector2 MouseDelta { get; private set; }
    public Vector2 InputDirection { get; private set; }
    public bool RunFlag { get; private set; }
    public bool CrouchFlag { get; private set; }
    [HideInInspector] public UnityEvent EscapeTrigger;
    [HideInInspector] public UnityEvent InteractionTrigger;

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Update()
    {
        Tick();
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new InputControls();
            InteractionTrigger = new UnityEvent();
            EscapeTrigger = new UnityEvent();

            playerInput.Main.Interaction.performed += ctx => InteractionTrigger?.Invoke();

            playerInput.Main.Run.started += ctx => RunFlag = true;
            playerInput.Main.Run.canceled += ctx => RunFlag = false;
            
            playerInput.Main.Crouch.started += ctx => CrouchFlag = true;
            playerInput.Main.Crouch.canceled += ctx => CrouchFlag = false;
            
            playerInput.Main.Escape.performed += ctx => EscapeTrigger?.Invoke();
        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Tick()
    {
        MouseDelta = playerInput.Main.MouseDelta.ReadValue<Vector2>();
        InputDirection = playerInput.Main.InputDirection.ReadValue<Vector2>();
    }
}