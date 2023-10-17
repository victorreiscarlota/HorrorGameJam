using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{
    [SerializeField] private float playerBaseSpeed;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float playerDeacceleration;
    [SerializeField,Range(1f,2f)] private float playerRunMultiplier;

    public override void Tick()
    {
        base.Tick();
    }
}
