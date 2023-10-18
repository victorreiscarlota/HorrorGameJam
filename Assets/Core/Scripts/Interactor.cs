using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
 public virtual void Interact()
 {
  Debug.Log($"Interacted with {this}");
 }
}
