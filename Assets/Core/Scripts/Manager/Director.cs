using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Micosmo;
using Micosmo.SensorToolkit;

public class Director : MonoBehaviour
{
    [Header("Director")]
    [SerializeField]
    private DirectorConfig directorConfigData;

    [SerializeField] private bool debugMode;

    public EntityBehaviourState CurrentEntityBehaviourState { get; set; }
    public EntityBehaviourState LastEntityBehaviourState { get; set; }
    [SerializeField] public Player player;
    [SerializeField] private Entity entity;
    [SerializeField] private InterestPoint[] interestPoints;
    [SerializeField] public EntityState CurrentEntityState;
    private List<EntityBehaviourState> weightedBehaviour;
    InterestPoint currentInterestPoint;
    float patrollingActionTimer;
    float searchTimer;
    private float iddleTimer;

    //Patrol
    private Vector3 currentSearchPosition;

    //Search
    private float remaingSearchAttempts;
    private float searchTimeoutTimer;
    private float timeSinceLastSawPlayer;
    private bool isSearchInCooldown;
    private bool searchTimeOut;
    private Vector3 startingSearchPosition;


    [Header("Detection")]
    [SerializeField]
    private LOSSensor fovComponent;

    private float targetTimer;
    private float chaseTimer;
    private Vector3 playerLastTrace;
    private bool isTargetVisible;

    private RangeSensor rangeSensor;

    public void StartDirector()
    {
        entity.StartEntity();
        CurrentEntityBehaviourState = EntityBehaviourState.Iddle;

        searchTimeOut = false;
        fovComponent.OnDetected.AddListener(ReceivedPlayerTrace);
        fovComponent.OnLostDetection.AddListener(LostPlayerTrace);
    }

    public void HandleDirector()
    {
        StateMachine();
        UpdateWeights();
        if (isTargetVisible) timeSinceLastSawPlayer = 0;
        else timeSinceLastSawPlayer += Time.deltaTime;
    }


    #region Detection

    private void ReceivedPlayerTrace(GameObject player, Sensor sensor)
    {
        if (!GameManager.Instance.isDirectorActive) return;
        isTargetVisible = true;
        playerLastTrace = player.transform.position;
        EnterState(EntityBehaviourState.Chasing);
    }

    private void LostPlayerTrace(GameObject player, Sensor sensor)
    {
        if (!GameManager.Instance.isDirectorActive) return;
        isTargetVisible = false;
        playerLastTrace = player.transform.position;
    }

    void ChaseDetectionHandle()
    {
        if (!isTargetVisible)
        {
            targetTimer += Time.deltaTime;

            if (targetTimer >= directorConfigData.timeBeforeLosingAggro)
            {
                targetTimer = 0;
                EnterState(EntityBehaviourState.Searching);
            }
        }
        else
        {
            targetTimer = 0;
        }
    }

    #endregion

    #region StateMachines

    private void StateMachine()
    {
        switch (CurrentEntityBehaviourState)
        {
            case EntityBehaviourState.Iddle:
                Iddle();
                break;
            case EntityBehaviourState.Chasing:
                Chasing();
                break;
            case EntityBehaviourState.Searching:
                Searching();
                break;
            case EntityBehaviourState.Patrolling:
                Patrolling();
                break;
            case EntityBehaviourState.Attacking:
                break;
        }
    }

    private void EnterState(EntityBehaviourState newState)
    {
        ExitState();
        switch (newState)
        {
            case EntityBehaviourState.Iddle:
                entity.MoveTowardPosition(entity.transform.position);
                break;
            case EntityBehaviourState.Chasing:
                entity.MoveTowardPosition(playerLastTrace);
                break;
            case EntityBehaviourState.Searching:
                remaingSearchAttempts = Random.Range(directorConfigData.minAttemptsSearchOnAlert, directorConfigData.maxAttemptsSearchOnAlert);
                startingSearchPosition = LastEntityBehaviourState == EntityBehaviourState.Chasing ? playerLastTrace : entity.transform.position;
                SearchRandomPointInCloseArea();
                break;
            case EntityBehaviourState.Patrolling:
                break;
            case EntityBehaviourState.Attacking:
                entity.PauseEntity();
                entity.PlayAnimation("Attack");
                break;
        }

        CurrentEntityBehaviourState = newState;
    }


    private void ExitState()
    {
        switch (CurrentEntityBehaviourState)
        {
            case EntityBehaviourState.Iddle:
                break;
            case EntityBehaviourState.Chasing:
                entity.MoveTowardPosition(playerLastTrace);
                break;
            case EntityBehaviourState.Searching:
                StopCoroutine(SearchLookout());
                break;
            case EntityBehaviourState.Patrolling:
                currentInterestPoint = null;
                break;
            case EntityBehaviourState.Attacking:
                entity.ResumeEntity();
                break;
        }

        LastEntityBehaviourState = CurrentEntityBehaviourState;
    }

    #endregion

    private void Iddle()
    {
        iddleTimer += Time.deltaTime;

        if (iddleTimer >= directorConfigData.timeBetweenIddleUpdates)
        {
            if (Vector3.Distance(entity.transform.position, player.transform.position) >= directorConfigData.distanceToForcePatrol)
            {
                EnterState(EntityBehaviourState.Patrolling);
            }

            iddleTimer = 0;
            int randomState = Random.Range(0, directorConfigData.behaviours.Count - 1);

            if (directorConfigData.behaviours[randomState] != CurrentEntityBehaviourState)
            {
                EnterState(directorConfigData.behaviours[randomState]);
            }
        }
    }

    private void Chasing()
    {
        chaseTimer += Time.deltaTime;

        if (chaseTimer >= directorConfigData.chaseFrequencyUpdate)
        {
            if (Vector3.Distance(entity.transform.position, player.transform.position) < directorConfigData.minDistanceToAttack)
            {
                EnterState(EntityBehaviourState.Attacking);
            }

            chaseTimer = 0;
            playerLastTrace = player.transform.position;
            entity.MoveTowardPosition(playerLastTrace + player.playerMovement.Velocity.normalized * directorConfigData.chaseLookAheadTime);
        }


        ChaseDetectionHandle();
    }

    public void OnAttackEnd()
    {
        EnterState(EntityBehaviourState.Iddle);
    }

    private void Searching()
    {
        SearchArea();
    }

    private void Patrolling()
    {
        if (!entity.IsMoving())
        {
            patrollingActionTimer += Time.deltaTime;
            if (patrollingActionTimer >= directorConfigData.frequencyToUpdatePatrolAction)
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
            patrollingActionTimer = 0;
            if (currentInterestPoint != null)
            {
                if (Vector3.Distance(currentInterestPoint.transform.position, entity.transform.position) < 3f)
                {
                    EnterState(EntityBehaviourState.Searching);
                }
            }
        }
    }

    #region Chase

    #endregion

    #region Search

    private void SearchArea()
    {
        searchTimer += Time.deltaTime;
        if (remaingSearchAttempts <= 0 && !isSearchInCooldown)
        {
            EnterState(EntityBehaviourState.Iddle);
        }

        if (searchTimer >= directorConfigData.searchUpdateTime)
        {
            searchTimer = 0;
            if (searchTimeOut || (!isSearchInCooldown && (Vector3.Distance(entity.transform.position, currentSearchPosition) <= 0.65f || entity.IsPathObstructed())))
            {
                searchTimeOut = false;
                StartCoroutine(SearchLookout());
            }
        }

        if (!isSearchInCooldown && !entity.IsMoving())
        {
            searchTimeoutTimer += Time.deltaTime;

            if (searchTimeoutTimer >= directorConfigData.timeOutSearchTime)
            {
                searchTimeoutTimer = 0;
                searchTimeOut = true;
            }
        }
        else
        {
            searchTimeoutTimer = 0;
        }
    }

    private void SearchRandomPointInCloseArea()
    {
        Vector3 randomPoint = GetRandomPointInCloseArea();
        currentSearchPosition = randomPoint;
        entity.MoveTowardPosition(randomPoint);
    }


    IEnumerator SearchLookout()
    {
        int lookoutDirections = Random.Range(directorConfigData.minLookoutDirections, directorConfigData.maxLookoutDirections);
        entity.MoveTowardPosition(entity.transform.position);
        entity.PauseEntity();

        isSearchInCooldown = true;
        Quaternion[] directionsToLook = new Quaternion[lookoutDirections];
        float[] directionDurations = new float[lookoutDirections];

        for (int i = 0; i < directionsToLook.Length; i++)
        {
            float duration = Random.Range(directorConfigData.minLookoutTime, directorConfigData.maxLookoutTime);
            Vector2 randomDirectionInsideCircle = Random.insideUnitCircle;
            Vector3 lookDirection = new Vector3(randomDirectionInsideCircle.x, 0, randomDirectionInsideCircle.y);
            directionsToLook[i] = Quaternion.LookRotation(lookDirection);
            directionDurations[i] = duration;
        }

        int directionIndex = 0;
        float t = 0;
        float lookingInDirectionTimer = 0;
        while (t < directorConfigData.maxLookoutDuration)
        {
            yield return null;


            if (Quaternion.Angle(entity.transform.rotation, directionsToLook[directionIndex]) < directorConfigData.maxAngleToCountDirection)
            {
                lookingInDirectionTimer += Time.deltaTime;

                if (lookingInDirectionTimer >= directionDurations[directionIndex])
                {
                    directionIndex++;
                    lookingInDirectionTimer = 0;
                }
            }

            if (directionIndex >= lookoutDirections)
            {
                break;
            }

            entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, directionsToLook[directionIndex], directorConfigData.lookoutTurnSpeed * Time.deltaTime);

            t += Time.deltaTime;
        }


        if (!entity.IsPathObstructed()) remaingSearchAttempts--;
        isSearchInCooldown = false;
        entity.ResumeEntity();
        SearchRandomPointInCloseArea();
    }

    #endregion

    #region InterestPoints

    private void UpdateWeights()
    {
        foreach (InterestPoint point in interestPoints)
        {
            float distanceFromCenter = Vector3.Distance(point.transform.position, player.transform.position);

            if (distanceFromCenter <= point.areaRadius)
            {
                float weight = (float)directorConfigData.zoneMaxWeight;
                point.weight = weight;
            }
            else
            {
                Vector3 circlePoint = GetClosestPointFromCircleRadius(point);
                point.distanceFromPlayer = Vector3.Distance(player.transform.position, circlePoint);
                float weight = directorConfigData.zoneMaxWeight / ((point.distanceFromPlayer / directorConfigData.zoneDistanceRatio) + 1);
                point.weight = weight;
            }
        }
    }

    private Vector3 GetClosestPointFromCircleRadius(InterestPoint point)
    {
        Vector3 playerDirection = player.transform.position - point.transform.position;
        playerDirection.Normalize();
        return point.transform.position + playerDirection * point.areaRadius;
    }

    private void GoToAnInterestPoint()
    {
        InterestPoint newPoint = SelectInterestPoint();
        currentSearchPosition = newPoint.transform.position;
        entity.MoveTowardPosition(currentSearchPosition);
        currentInterestPoint = newPoint;
    }

    private InterestPoint SelectInterestPoint()
    {
        List<InterestPoint> weightedOptions = new List<InterestPoint>();

        foreach (InterestPoint point in interestPoints)
        {
            for (int i = 0; i < Mathf.FloorToInt(point.weight); i++)
            {
                weightedOptions.Add(point);
            }
        }

        int randomIndex = Random.Range(0, weightedOptions.Count);

        return weightedOptions[randomIndex];
    }

    #endregion


    public void UpdateEntityState(EntityState newState)
    {
        CurrentEntityState = newState;
    }


    #region Functions

    private Vector3 GetRandomPointInCloseArea()
    {
        Vector2 circle = Random.insideUnitCircle * directorConfigData.searchFreeRadius;
        return startingSearchPosition + new Vector3(circle.x, 0, circle.y);
    }

    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        Handles.color = Color.red;
        Handles.Label(entity.transform.position + Vector3.up * 1.65f, CurrentEntityBehaviourState.ToString());
        Handles.color = Color.blue;
        Handles.Label(entity.transform.position + Vector3.up * 1.5f, LastEntityBehaviourState.ToString());

        foreach (InterestPoint point in interestPoints)
        {
            if (currentInterestPoint == point) Handles.color = Color.red;
            else Handles.color = Color.green;
            Handles.CircleHandleCap(0, point.transform.position, Quaternion.LookRotation(Vector3.up), point.areaRadius, EventType.Repaint);
        }
    }
#endif
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