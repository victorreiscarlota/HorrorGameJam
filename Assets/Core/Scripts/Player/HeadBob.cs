using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadBob : PlayerModule
{
    [Header("Configuration")] [SerializeField]
    private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float minAmplitude;
    [SerializeField, Range(0, 0.1f)] private float maxAmplitude;
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPos = cameraTransform.localPosition;
    }

    public override void Tick()
    {
        if (!enable) return;
        UpdateAmplitude();
        CheckMotion();
        ResetPosition();
        cameraTransform.LookAt(FocusTarget());
    }

    private void UpdateAmplitude()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        
        float normalizedSpeed = Mathf.InverseLerp(ThisPlayer.playerMovement.MinSpeed, ThisPlayer.playerMovement.MaxSpeed, speed);
        
        float newAmplitude = Mathf.Lerp(minAmplitude, maxAmplitude, normalizedSpeed);

        amplitude = newAmplitude;

    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        if (speed < toggleSpeed) return;
        if (!controller.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        cameraTransform.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y,
            transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }

    private void ResetPosition()
    {
        if (cameraTransform.localPosition == startPos) return;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, startPos, 1 * Time.deltaTime);
    }
}