using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : MonoBehaviour
{
    public Player ThisPlayer { get; private set; }

    private void Start()
    {
        ThisPlayer = GetComponent<Player>();
    }

    public virtual void Tick()
    {
    }

    public virtual void FixedTick()
    {
    }
}