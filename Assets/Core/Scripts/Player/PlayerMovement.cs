using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{
    [Header("Main")] [SerializeField] private CharacterController charController;
    [SerializeField] private float playerBaseSpeed;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float playerDeacceleration;
    [SerializeField, Range(1f, 2f)] private float playerRunMultiplier;

    [Header("FOV")] [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;

    private Vector3 velocity;
    private Vector2 horizontalVelocity;
    private float currentSpeed;
    public float MinSpeed{get ; private set;}
    public float MaxSpeed{get ; private set;}

    private void Start()
    {
        MinSpeed = playerBaseSpeed;
        MaxSpeed = playerBaseSpeed * playerRunMultiplier;
    }

    public override void Tick()
    {
        base.Tick();
        HandleMovement();
        FovUpdateBasedOnSpeed();
        UpdateSpeed();
    }


    private void HandleMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 movDirection = InputManager.Instance.InputDirection.y * cameraForward;
        movDirection += InputManager.Instance.InputDirection.x * cameraRight;

        movDirection.Normalize();
        movDirection.y = 0;
        horizontalVelocity = movDirection;
        velocity = new Vector3(movDirection.x, -5f, movDirection.z);
        charController.Move(velocity * Time.deltaTime * currentSpeed);
    }

    private void UpdateSpeed()
    {
        float targetSpeed = horizontalVelocity.magnitude > 0 ? playerBaseSpeed : 0;
        
        if (InputManager.Instance.RunFlag && IsMovingFoward())
        {
            targetSpeed *= playerRunMultiplier;
        }

        targetSpeed *= GameManager.Instance.CurrentMadnessLevelData.PlayerSpeedMultiplier;
        
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, (horizontalVelocity.magnitude > 0 ? playerAcceleration : playerDeacceleration) * Time.deltaTime);
    }

    private bool IsMovingFoward()
    {
        return InputManager.Instance.InputDirection.y > 0 && InputManager.Instance.InputDirection.x == 0;
    }

    private void FovUpdateBasedOnSpeed()
    {
        float normalizedSpeed = Mathf.InverseLerp(MinSpeed, MaxSpeed, currentSpeed);
        
        float newFOV = Mathf.Lerp(minFOV, maxFOV, normalizedSpeed);
        
        Camera.main.fieldOfView = newFOV;
    }
}