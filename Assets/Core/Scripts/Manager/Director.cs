using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Director : MonoBehaviour
{
    [Header("Director")]
    [SerializeField]
    private DirectorConfig directorConfigData;

    [SerializeField] private Player player;
    [SerializeField] private Entity entity;
    [SerializeField] private InterestPoint[] interestPoints;
    [SerializeField] public EntityState CurrentEntityState;
    [SerializeField] private float currentEntityPoints;

    InterestPoint currentInterestPoint;
    [SerializeField] float pointsTimer;
    [SerializeField] float patrollingActionTimer;
    [SerializeField] float interestPointSearchTimer;
    [SerializeField] float interestPointTimer;
    [SerializeField] bool isPatrolling;

    private List<InterestPoint> possibleTargetPoints;

    //Patrol
    [SerializeField] private Vector3 currentPatrolPosition;

    public void StartDirector()
    {
        possibleTargetPoints = new List<InterestPoint>();
        currentEntityPoints = 0;
        pointsTimer = 0;
        entity.StartEntity();
        isPatrolling = false;
        for (int i = 0; i < interestPoints.Length; i++)
        {
            possibleTargetPoints.Add((interestPoints[i]));
        }
    }

    public void HandleDirector()
    {
        PointsManager();
        PatrolManager();
    }

    private void PointsManager()
    {
        pointsTimer += Time.deltaTime;
        if (pointsTimer >= 1)
        {
            pointsTimer = 0;
            currentEntityPoints += directorConfigData.pointsPerSecond;
            currentEntityPoints = Mathf.Clamp(currentEntityPoints, 0, directorConfigData.maxEntityPoints);
        }
    }

    private void PatrolManager()
    {
        if (!isPatrolling)
        {
            patrollingActionTimer += Time.deltaTime;
            if (patrollingActionTimer >= directorConfigData.frequencyToTakePatrolAction)
            {
                patrollingActionTimer = 0;
                int chance = Random.Range(0, 100);

                if (chance <= directorConfigData.chanceToTakePatrolAction)
                {
                    GoToAnInterestPoint();
                }
            }
        }
        else
        {
            if (IsInPatrolArea())
            {
                interestPointTimer += Time.deltaTime;
                interestPointSearchTimer += Time.deltaTime;
                if (interestPointTimer >= currentInterestPoint.searchDuration)
                {
                    interestPointTimer = 0;
                    interestPointSearchTimer = 0;
                    isPatrolling = false;
                }

                if (interestPointSearchTimer >= directorConfigData.frequencyToTakePatrolAction)
                {
                    interestPointSearchTimer = 0;
                    if (Vector3.Distance(entity.transform.position, currentPatrolPosition) <= 1.25f)
                    {
                        Vector3 randomPoint = GetRandomPointInPatrolArea();
                        currentPatrolPosition = randomPoint;
                        entity.MoveTowardPosition(randomPoint);
                    }
                }
            }
        }
    }

    private bool IsInPatrolArea()
    {
        if (Vector3.Distance(entity.transform.position, currentInterestPoint.areaTransform.position) < currentInterestPoint.searchAreaRadius) return true;

        return false;
    }

    public void UpdateEntityState(EntityState newState)
    {
        CurrentEntityState = newState;
    }

    private void GoToAnInterestPoint()
    {

        InterestPoint newPoint = SelectInterestPoint();
        if (currentInterestPoint != null) possibleTargetPoints.Add(currentInterestPoint);
        possibleTargetPoints.Remove(newPoint);
        currentPatrolPosition = newPoint.areaTransform.position;
        entity.MoveTowardPosition(currentPatrolPosition);
        currentInterestPoint = newPoint;
        interestPointTimer = 0;
        isPatrolling = true;
    }

    private Vector3 GetRandomPointInPatrolArea()
    {
        Vector2 circle = Random.insideUnitCircle * currentInterestPoint.searchAreaRadius;
        return currentInterestPoint.areaTransform.position + new Vector3(circle.x, 0, circle.y);
    }

    private InterestPoint SelectInterestPoint()
    {
        List<InterestPoint> weightedOptions = new List<InterestPoint>();

        foreach (InterestPoint point in possibleTargetPoints)
        {
            for (int i = 0; i < point.weight; i++)
            {
                weightedOptions.Add(point);
            }
        }

        int randomIndex = Random.Range(0, weightedOptions.Count);

        return weightedOptions[randomIndex];
    }
}

[System.Serializable]
public class InterestPoint
{
    public Transform areaTransform;
    public int weight;
    public float searchDuration;
    public float searchAreaRadius;
}

public enum EntityState
{
    Agressive,
    Normal,
}

public enum EntityBuyableConsumable
{
    None,
    PlayerPosition,
}