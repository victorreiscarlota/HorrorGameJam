using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }



    [SerializeField] private GameFlow gameFlow;
    [SerializeField] private Clock centerClock;
    [SerializeField] private bool isClockActive;

    //Getter and Setters
    public GameState CurrentGameState { get; private set; }
    public MadnessLevelData CurrentMadnessLevelData { get; private set; }

    
    
    #region Unity Functions
    private void Awake()
    {
        if (!Instance) Instance = this;
    }
    
    private void Start()
    {
        UnpauseGame();
        InputManager.Instance.EscapeTrigger.AddListener(ChangeGameState);
        CurrentMadnessLevelData = gameFlow.NoMadnessLevel;
        centerClock.normalClockEnabled = true;
        centerClock.OnClockChange.AddListener(ClockUpdate);
    }

    private void Update()
    {
        if (CurrentGameState != GameState.Running) return;
        
        TimeManager();
    }

    #endregion

    #region GameStates

    

    
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

    #endregion

    #region TimeAndMadness
    public void UpdateCurrentMadness(MadnessLevel newMadnessLevel)
    {
        switch (newMadnessLevel)
        {
            case MadnessLevel.NoMadness:
                CurrentMadnessLevelData = gameFlow.NoMadnessLevel;
                break;
            case MadnessLevel.LowMadness:
                CurrentMadnessLevelData = gameFlow.LowMadnessLevel;
                break;
            case MadnessLevel.MediumMadness:
                CurrentMadnessLevelData = gameFlow.MediumMadnessLevel;
                break;
            case MadnessLevel.HighMadness:
                CurrentMadnessLevelData = gameFlow.HighMadnessLevel;
                break;
        }
    }

    private void TimeManager()
    {
        if (!isClockActive) return;

        if (centerClock.normalClockEnabled) centerClock.UpdateNormalClock(CurrentMadnessLevelData.NormalWorldDuration);
        else if (centerClock.invertedClockEnabled) centerClock.UpdateInvertedClock(CurrentMadnessLevelData.InvertedWorldDuration);
        
    }

    private void ClockUpdate(bool isNormal)
    {
        Debug.Log("ClockChange");
    }
    
    #endregion
}


public enum GameState
{
    Running,
    Stopped,
}