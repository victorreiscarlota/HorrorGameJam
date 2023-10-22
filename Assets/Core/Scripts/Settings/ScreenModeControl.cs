using System;
using UnityEngine;

public class ScreenModeControl : MonoBehaviour
{
    public void SetFullScreen()
    {
        Screen.fullScreen = true;
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    public void SetWindowed()
    {
        Screen.fullScreen = false;
    }

    public void SetWindowedFullScreen()
    {
        Screen.fullScreen = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    
    public void OnDropdownValueChanged(int choice)
    {
        switch(choice)
        {
            case 0: 
                SetFullScreen();
                break;
            case 1: 
                SetWindowed();
                break;
            case 2: 
                SetWindowedFullScreen();
                break;
        }
    }
}