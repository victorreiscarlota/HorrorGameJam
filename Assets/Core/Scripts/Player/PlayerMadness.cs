using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMadness : PlayerModule
{
    public MadnessLevel CurrentMadness;

    public void UpdateMadnessLevel(int modifier)
    {
        CurrentMadness += modifier;

        if (CurrentMadness < 0) CurrentMadness = 0;

        if ((int)CurrentMadness > 3) CurrentMadness = (MadnessLevel) 3;
        
        GameManager.Instance.UpdateCurrentMadness(CurrentMadness);
    }
}


public enum MadnessLevel
{
    NoMadness,
    LowMadness,
    MediumMadness,
    HighMadness,
}