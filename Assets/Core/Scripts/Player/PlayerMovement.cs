using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{
    [Header("Main")]
    [SerializeField] private CharacterController charController;

    [SerializeField] private GameObject eyeLevelObject;

    [Header("Speed")]
    [SerializeField] private float playerBaseSpeed;

    [SerializeField] private float playerAcceleration;
    [SerializeField] private float playerDeacceleration;
    [SerializeField, Range(1f, 2f)] private float playerRunMultiplier;
    [SerializeField, Range(0.1f, 1f)] private float playerCrouchMultiplier;

    [Header("Crouch")]
    [SerializeField] private float normalHeight = 1.75f;

    [SerializeField] private float crouchHeight;
    [SerializeField] private float playerCrouchCharacterHeight;
    [SerializeField] private float playerCrouchCenter;
    [SerializeField] private float changeHeightSpeed;
    private float playerBaseCharacterHeight;
    private float playerBaseCenter;
    private float currentHeight;
    private float currentCharHeight;
    private float currentCenter;


    [Header("FOV")]
    [SerializeField] private float minFOV;

    [SerializeField] private float maxFOV;
    public Vector3 Velocity { get; private set; }
    public Vector2 horizontalVelocity;
    public float currentSpeed;
    public float MinSpeed { get; private set; }
    public float MaxSpeed { get; private set; }

    public override void StartComponent()
    {
        base.StartComponent();
        currentHeight = eyeLevelObject.transform.localPosition.y;
        currentCharHeight = charController.height ;
        currentCenter = charController.center.y ;
        MinSpeed = playerBaseSpeed * playerCrouchMultiplier;
        MaxSpeed = playerBaseSpeed * playerRunMultiplier;
        playerBaseCharacterHeight = charController.height;
        playerBaseCenter = charController.center.y;
    }

    public override void Tick()
    {
        base.Tick();
        HandleMovement();
        HandleHeight();
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
        Velocity = new Vector3(movDirection.x, -5f, movDirection.z);
        charController.Move(Velocity * Time.deltaTime * currentSpeed);
    }

    private void HandleHeight()
    {
        float targetHeight = InputManager.Instance.CrouchFlag ? crouchHeight : normalHeight;
        float targetCenter = InputManager.Instance.CrouchFlag ? playerCrouchCenter : playerBaseCenter;
        float targetCharHeight = InputManager.Instance.CrouchFlag ? playerCrouchCharacterHeight : playerBaseCharacterHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, changeHeightSpeed * Time.deltaTime);
        currentCenter = Mathf.Lerp(currentCenter, targetCenter, changeHeightSpeed * Time.deltaTime);
        currentCharHeight = Mathf.Lerp(currentCharHeight, targetCharHeight, changeHeightSpeed * Time.deltaTime);

        eyeLevelObject.transform.localPosition = new Vector3(0, currentHeight, 0);
        charController.height = currentCharHeight;
        charController.center = new Vector3(0, currentCenter, 0);
    }

    private void UpdateSpeed()
    {
        float targetSpeed = horizontalVelocity.magnitude > 0 ? playerBaseSpeed : 0;

        if (InputManager.Instance.CrouchFlag)
        {
            targetSpeed *= playerCrouchMultiplier;
        }
        else if (InputManager.Instance.RunFlag && IsMovingFoward() && !InputManager.Instance.CrouchFlag)
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

        ThisPlayer.virtualCamera.m_Lens.FieldOfView = newFOV;
    }
}