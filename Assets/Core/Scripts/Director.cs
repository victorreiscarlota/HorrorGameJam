using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DirectorData", menuName = "ScriptableObjects/Director", order = 1)]
public class Director : ScriptableObject
{
    [Header("Behaviour")] //
    public float frequencyToTryUsePoints;
    
    [Header("Entity Points")]//
    public float pointsPerSecond;
    public float maxEntityPoints;

    [Header("Player Position")] //
    public float PositionWeight;
    public float PositionCost;
    public float PositionCooldown;
    
    
}
