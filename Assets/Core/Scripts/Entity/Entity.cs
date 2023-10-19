using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [Header("Main")] [SerializeField] private NavMeshAgent agent;
    private NavMeshPath currentPath;
    public EntityBehaviourState CurrentEntityBehaviourState { get; set; }

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

    public void SetEntityEnabled(bool value)
    {
        agent.isStopped = value;
    }
}

public enum EntityBehaviourState
{
    Iddle,
    Chasing,
    Searching,
    Patrolling,
}