using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInteraction playerInteraction { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PlayerMadness playerMadness { get; private set; }
    public PlayerCameraControl playerCameraControl { get; private set; }
    public HeadBob headBob { get; private set; }

    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMadness = GetComponent<PlayerMadness>();
        playerCameraControl = GetComponent<PlayerCameraControl>();
        headBob = GetComponent<HeadBob>();
    }


    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running) return;
        playerCameraControl.Tick();
        playerMovement.Tick();
        headBob.Tick();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running) return;
        playerInteraction.FixedTick();
    }
}