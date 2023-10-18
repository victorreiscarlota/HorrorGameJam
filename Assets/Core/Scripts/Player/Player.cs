using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private PlayerMadness playerMadness;
    private PlayerCameraControl playerCameraControl;


    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMadness = GetComponent<PlayerMadness>();
        playerCameraControl = GetComponent<PlayerCameraControl>();
    }


    private void Update()
    {
        playerCameraControl.Tick();
        playerMovement.Tick();
    }

    private void FixedUpdate()
    {
        playerInteraction.FixedTick();
    }
}