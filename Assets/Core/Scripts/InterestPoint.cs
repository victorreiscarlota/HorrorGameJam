using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    public float areaRadius => transform.localScale.magnitude;
    public float weight;
    public float distanceFromPlayer;
}