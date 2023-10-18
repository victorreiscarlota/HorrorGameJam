using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MadnessLevelData", menuName = "ScriptableObjects/MadnessFlow", order = 1)]
public class MadnessLevelData : ScriptableObject
{
    public float InvertedWorldDuration;
    public float NormalWorldDuration;
    public float PlayerSpeedMultiplier;
}
