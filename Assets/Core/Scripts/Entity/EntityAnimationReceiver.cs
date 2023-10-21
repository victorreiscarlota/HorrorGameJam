using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationReceiver : MonoBehaviour
{
    public void OnAttackEnd()
    {
        GameManager.Instance.EndEntityAttack();
    }
}