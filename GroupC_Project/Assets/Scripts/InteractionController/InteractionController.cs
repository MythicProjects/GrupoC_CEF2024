using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    [Header("Interaction Active")]
    public bool interactionActive;

    protected virtual void Start() { }
    protected virtual void Update() { }

}
