using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanismObject : MonoBehaviour
{
    public bool activateMechanism;
    protected virtual void Start() { }

    protected virtual void FixedUpdate() 
    {
        if (activateMechanism)
        {
            MechanismBehaviour();
        }
    }

    public void SetMechanismBehaviour(bool setBehaviour)
    {
        activateMechanism = setBehaviour;
    }
    protected virtual void MechanismBehaviour() { }

}
