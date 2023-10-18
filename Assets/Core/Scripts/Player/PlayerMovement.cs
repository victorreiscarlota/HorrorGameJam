using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private float playerBaseSpeed;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float playerDeacceleration;
    [SerializeField, Range(1f, 2f)] private float playerRunMultiplier;


    private Vector3 velocity;

    public override void Tick()
    {
        base.Tick();
        HandleMovement();
    }


    private void HandleMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //cameraForward.y = 0;
        //cameraRight.y = 0;

        Vector3 movDirection = InputManager.Instance.InputDirection.y * cameraForward;
        movDirection += InputManager.Instance.InputDirection.x * cameraRight;

        movDirection.Normalize();
        movDirection.y = 0;

        velocity = new Vector3(movDirection.x, -5f, movDirection.z);
        charController.Move(velocity * Time.deltaTime * GetSpeed());
    }

    private float GetSpeed()
    {
        float targetSpeed = playerBaseSpeed;
        
        if (InputManager.Instance.RunFlag)
        {
            targetSpeed *= playerRunMultiplier;
        }

        return targetSpeed;
    }
}