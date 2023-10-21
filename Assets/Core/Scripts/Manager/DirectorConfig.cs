using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DirectorData", menuName = "ScriptableObjects/Director", order = 1)]
public class DirectorConfig : ScriptableObject
{
    [Header("EntityBehaviour")] //
    public float frequencyToUpdatePatrolAction = 2;
    public float distanceToForcePatrol = 35f;
    public int chanceToTakePatrolAction = 100;

    [Header("Chasing Behaviour")]
    public float chaseFrequencyUpdate = 0.666f;

    public float minDistanceToAttack;
    public float chaseLookAheadTime = 2;
    public float timeBeforeLosingAggro = 15;

    [Header("Iddle Behaviour")]
    public List<EntityBehaviourState> behaviours;

    public float timeBetweenIddleUpdates = 1;

    [Header("SearchBehaviour")]
    public float searchUpdateTime = 1;

    public float searchFreeRadius = 12;
    public float timeOutSearchTime = 2.5f;
    public int minAttemptsSearchOnAlert = 2;
    public int maxAttemptsSearchOnAlert = 5;


    [Header("Lookout")]
    public float maxLookoutDuration = 35;

    public float maxLookoutTime = 2.5f;
    public float minLookoutTime = 4;
    public int minLookoutDirections = 1;
    public int maxLookoutDirections = 1;
    public float lookoutTurnSpeed = 0.75f;
    public float maxAngleToCountDirection = 7;

    [Header("InterestZones")]
    public int zoneMaxWeight = 5;

    public float zoneDistanceRatio = 25;
}