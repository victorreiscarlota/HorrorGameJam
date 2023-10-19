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


    [SerializeField] private Player player;
    [SerializeField] private Entity entity;
    [SerializeField] private GameFlow gameFlow;

    [SerializeField] private Clock centerClock;
    [SerializeField] private bool isClockActive;

    [Header("Director")] //
    [SerializeField]
    private Director directorData;

    [SerializeField] private float currentEntityPoints;
    private float pointsTimer;

    [Header("VFX")] [SerializeField] private CinemachineImpulseSource cameraShakeSource;

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
        centerClock.OnClockChange = new UnityEvent();
        centerClock.OnClockChange.AddListener(ClockUpdate);
        currentEntityPoints = 0;
        pointsTimer = 0;
        entity.StartEntity();
    }

    private void Update()
    {
        if (CurrentGameState != GameState.Running) return;

        TimeManager();
        HandleDirector();
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
        CurrentGameState = GameState.Stopped;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.Instance.ChangeHUDState(false);
        UIManager.Instance.ChangeMenusState(true);
    }

    public void UnpauseGame()
    {
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
        if (!isClockActive) return;

        if (centerClock.normalClockEnabled) centerClock.UpdateNormalClock();
    }

    private void ClockUpdate()
    {
        Debug.Log("ClockChange");
        cameraShakeSource.GenerateImpulseWithForce(0.65f);
    }

    #endregion

    #region Director

    private void HandleDirector()
    {
        pointsTimer += Time.deltaTime;

        if (pointsTimer >= 1)
        {
            pointsTimer = 0;
            currentEntityPoints += directorData.pointsPerSecond;

            currentEntityPoints = Mathf.Clamp(currentEntityPoints, 0, directorData.maxEntityPoints);
        }

        entity.UpdateEntity();
    }

    #endregion

    private void EntityShop(EntityBuyableConsumable consumableToBuy)
    {
        switch (consumableToBuy)
        {
            case EntityBuyableConsumable.PlayerPosition:
                entity.MoveTowardPosition(player.transform.position);
                break;
        }
    }
}


public enum GameState
{
    Running,
    Stopped,
}