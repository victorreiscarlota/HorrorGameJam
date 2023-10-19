using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [Header("Main")] [SerializeField] private NavMeshAgent agent;
    private NavMeshPath currentPath;
    
    
    private Vector3 destinationPosition;


    private float pathTimer;

    public void StartEntity()
    {
        pathTimer = 0;
        StartPath();
        GameManager.Instance.OnPauseGame.AddListener(PauseEntity);
        GameManager.Instance.OnResumeGame.AddListener(ResumeEntity);
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
    
    public void PauseEntity()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
    }

    public void ResumeEntity()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
    }

    public bool IsStopped()
    {
        return agent.isStopped;
    }

    public bool IsPathObstructed()
    {
        return agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.isPathStale;
    }

    public bool IsMoving()
    {
        return agent.velocity.sqrMagnitude > 0.1f;
    }
}

public enum EntityBehaviourState
{
    Iddle,
    Chasing,
    Searching,
    Patrolling,
    Attacking,
}