using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        UnpauseGame();
        InputManager.Instance.EscapeTrigger.AddListener(ChangeGameState);
    }

    public GameState CurrentGameState { get; private set; }


    private void ChangeGameState()
    {
        if (CurrentGameState == GameState.Running)
        {
            PauseGame();
            UIManager.Instance.ChangeHUDState(false);
            UIManager.Instance.ChangeMenusState(true);
        }
        else
        {
            UnpauseGame();
            UIManager.Instance.ChangeHUDState(true);
            UIManager.Instance.ChangeMenusState(false);
        }
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Stopped;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnpauseGame()
    {
        CurrentGameState = GameState.Running;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


public enum GameState
{
    Running,
    Stopped,
}