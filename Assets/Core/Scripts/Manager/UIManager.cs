using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject hudParent;
    [SerializeField] private GameObject inGameMenus;
    [SerializeField] private GameObject inGameSettings;


    //Crosshair
    [Header("Crosshair")] [SerializeField] private Transform crosshairTransform;

    [SerializeField, Range(1, 1.5f)] private float crosshairInteractionSize;

    public void ChangeHUDState(bool state)
    {
        hudParent.SetActive(state);
    }

    public void ChangeMenusState(bool menuState, bool settingsState)
    {
        inGameMenus.SetActive(menuState);
        inGameSettings.SetActive(settingsState);
    }

    public void OpenSettingsMenu()
    {
        ChangeMenusState(false, true);
    }

    public void CloseSettingsMenu()
    {
        ChangeMenusState(true, false);
    }

    public void UpdateCrosshairScale(bool hasInteractor)
    {
        if (hasInteractor)
        {
            crosshairTransform.localScale = Vector3.one * crosshairInteractionSize;
        }
        else
        {
            crosshairTransform.localScale = Vector3.one;
        }
    }
}