using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : PlayerModule
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float sensitivity;
    [SerializeField] private float minPitchAngle;
    [SerializeField] private float maxPitchAngle;

    private Quaternion currentYaw;
    private Quaternion currentPitch;

    private void Start()
    {
        currentYaw = Quaternion.identity;
        currentPitch = Quaternion.identity;
    }

    public override void Tick()
    {
        base.Tick();
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        // Yaw
        Quaternion Yaw = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
        Quaternion Pitch = Quaternion.FromToRotation(Vector3.up, Vector3.forward);

        Quaternion yawDelta = Quaternion.SlerpUnclamped(Quaternion.identity, Yaw, InputManager.Instance.MouseDelta.x * sensitivity * Time.deltaTime); 
        currentYaw *= yawDelta;

        // Pitch
        Quaternion pitchDelta = Quaternion.SlerpUnclamped(Quaternion.identity, Pitch, -InputManager.Instance.MouseDelta.y * sensitivity * Time.deltaTime);
        
        Quaternion maxPitch = Quaternion.AngleAxis(maxPitchAngle, Vector3.right);
        Quaternion minPitch = Quaternion.AngleAxis(minPitchAngle, Vector3.right);

        Quaternion targetPitch = currentPitch * pitchDelta;

        Vector3 pitchedDir = targetPitch * Vector3.forward;

        Vector3 maxPitchedDir = maxPitch * Vector3.forward;
        Vector3 minPitchedDir = minPitch * Vector3.forward;
        
        float degsToMaxPitch = Vector3.SignedAngle(pitchedDir, maxPitchedDir, Vector3.right);
        float degsToMinPitch = Vector3.SignedAngle(pitchedDir, minPitchedDir, Vector3.right);

        if (degsToMaxPitch < 0 && degsToMinPitch > 0)
        {
            if (InputManager.Instance.MouseDelta.y > 0)
            {
                targetPitch = minPitch;
            }
            else
            {
                targetPitch = maxPitch;
            }
        }
        else if (degsToMaxPitch < 0)
        {
            targetPitch = maxPitch;
        }
        else if (degsToMinPitch > 0)
        {
            targetPitch = minPitch;
        }

        currentPitch = targetPitch;

        mainCamera.transform.rotation = currentYaw * currentPitch;
    }
}