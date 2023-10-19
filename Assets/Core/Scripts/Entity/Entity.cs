using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [Header("Main")] [SerializeField] private NavMeshAgent agent;
    private NavMeshPath currentPath;
    public EntityState CurrentEntityState { get; private set; }
    [Header("Config")] //
    [SerializeField] private float pathUpdateFrequency;
    

    [Header("Debug")] //
    [SerializeField] private bool debugFollowTarget;

    [SerializeField] private Transform debugTarget;
    private Vector3 destinationPosition;
    

    private float pathTimer;

    public void StartEntity()
    {
        pathTimer = 0;
        StartPath();
    }

    public void UpdateEntity()
    {
        pathTimer += Time.deltaTime;

        if (pathTimer >= pathUpdateFrequency)
        {
            pathTimer = 0;
            if (debugFollowTarget)
            {
                MoveTowardPosition(debugTarget.position);
            }
        }
    }

    private void StartPath()
    {
        currentPath = new NavMeshPath();
        agent.SetPath(currentPath);
    }

    public bool MoveTowardPosition(Vector3 targetPosition)
    {
        if (agent.SetDestination(targetPosition))
        {
            destinationPosition = targetPosition;
            return true;
        }

        return false;
    }

    
}


public enum EntityBuyableConsumable
{
    None,
    PlayerPosition,
}

public enum EntityState
{
    Iddle,
    Chasing,
    Searching,
    Patrolling,
}