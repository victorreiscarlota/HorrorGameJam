using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Director director;
    [SerializeField] private GameFlow gameFlow;

    [Header("Clock")] //
    [SerializeField] private Clock centerClock;

    public bool IsClockActive;

    [Header("VFX")]
    [SerializeField] private CinemachineImpulseSource cameraShakeSource;

    //Getter and Setters
    public GameState CurrentGameState { get; private set; }
    public MadnessLevelData CurrentMadnessLevelData { get; private set; }
    [HideInInspector] public UnityEvent OnPauseGame;
    [HideInInspector] public UnityEvent OnResumeGame;
    [SerializeField] public bool isDirectorActive;

    #region Unity Functions

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        UnpauseGame();
        OnPauseGame = new UnityEvent();
        OnResumeGame = new UnityEvent();

        InputManager.Instance.EscapeTrigger.AddListener(ChangeGameState);
        CurrentMadnessLevelData = gameFlow.NoMadnessLevel;
        IsClockActive = true;
        centerClock.StartClock();
        centerClock.OnDurationEnd.AddListener(OnClockDurationEnd);
        centerClock.OnNewDuration.AddListener(OnNewClockDuration);
        director.StartDirector();
    }

    private void Update()
    {
        if (CurrentGameState != GameState.Running) return;

        TimeManager();
        if (isDirectorActive) director.HandleDirector();
    }

    private void FixedUpdate()
    {
        if (CurrentGameState != GameState.Running) return;
    }

    #endregion

    #region GameStates

    private void ChangeGameState()
    {
        if (CurrentGameState == GameState.Running)
            PauseGame();
        else
            UnpauseGame();
    }

    public void PauseGame()
    {
        OnPauseGame?.Invoke();
        CurrentGameState = GameState.Stopped;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.Instance.ChangeHUDState(false);
        UIManager.Instance.ChangeMenusState(true);
    }

    public void UnpauseGame()
    {
        OnResumeGame?.Invoke();
        CurrentGameState = GameState.Running;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.Instance.ChangeHUDState(true);
        UIManager.Instance.ChangeMenusState(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
        centerClock.UpdateClock();
    }

    private void OnClockDurationEnd()
    {
        Debug.Log("TimeEnd");
        cameraShakeSource.GenerateImpulseWithForce(0.65f);
        director.UpdateEntityState(EntityState.Agressive);
        IsClockActive = false;
    }

    private void OnNewClockDuration()
    {
        Debug.Log("TimeStart");
        director.UpdateEntityState(EntityState.Normal);
        IsClockActive = true;
    }

    #endregion
}


public enum GameState
{
    Running,
    Stopped,
}