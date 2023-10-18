using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMadness : PlayerModule
{
    public MadnessLevel CurrentMadness;

    public void UpdateMadnessLevel(int modifier)
    {
        CurrentMadness += modifier;
    }
}


public enum MadnessLevel
{
    NoMadness,
    LowMadness,
    MediumMadness,
    HighMadness,
}