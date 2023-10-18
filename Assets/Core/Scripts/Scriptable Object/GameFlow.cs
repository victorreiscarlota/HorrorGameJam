
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFlowData", menuName = "ScriptableObjects/GameFlow", order = 1)]
public class GameFlow : ScriptableObject
{

    public MadnessLevelData NoMadnessLevel;
    public MadnessLevelData LowMadnessLevel;
    public MadnessLevelData MediumMadnessLevel;
    public MadnessLevelData HighMadnessLevel;
}

