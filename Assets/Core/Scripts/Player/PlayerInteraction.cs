using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : PlayerModule
{
    [SerializeField] private float interactionRange;
    [SerializeField] private float interationSphereRadius;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private bool debugMode;

    private Interactor currentInteractor;
    private Vector3 currentHitPoint;

    private void Start()
    {
        InputManager.Instance.InteractionTrigger.AddListener((TryInteraction));
    }
    public override void FixedTick()
    {
        base.FixedTick();
        UpdateInteractions();
    }

    private void UpdateInteractions()
    {
        if (Physics.SphereCast(Camera.main.transform.position, interationSphereRadius, Camera.main.transform.forward,
                out RaycastHit hit, interactionRange, interactionMask))
        {
            currentHitPoint = hit.point;
            if (hit.collider.TryGetComponent(out Interactor interactor))
            {
                UpdateInteractor(interactor);
            }
            else
            {
                UpdateInteractor(null);
            }
        }
        else
        {
            UpdateInteractor(null);
            currentHitPoint = Vector3.zero;
        }
    }

    private void UpdateInteractor(Interactor newInteractor)
    {
        if (newInteractor != null)
        {
        }
        else
        {
        }

        currentInteractor = newInteractor;
    }

    private void TryInteraction()
    {
        if (!CanInteract()) return;

        currentInteractor.Interact();
    }

    private bool CanInteract()
    {
        if (!currentInteractor) return false;
        
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.DrawRay(Camera.main.transform.position, currentHitPoint != Vector3.zero ? currentHitPoint : Camera.main.transform.forward * interactionRange);
            Gizmos.DrawWireSphere(currentHitPoint != Vector3.zero ? currentHitPoint : (Camera.main.transform.position + Camera.main.transform.forward * interactionRange),
                interationSphereRadius);
        }
    }

#endif
}