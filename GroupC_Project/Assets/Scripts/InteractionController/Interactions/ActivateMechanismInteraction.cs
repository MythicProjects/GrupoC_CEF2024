using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMechanismInteraction : InteractionController
{
    public MechanismObject[] otherSystems;

    public void ActivateMechanism(bool isPlayerInteraction)
    {
        if (isPlayerInteraction) interactionActive = !interactionActive; //Activado con brazo
        else interactionActive = true;//Activado con OnTrigger/OnCollision

        for (int i = 0; i < otherSystems.Length; i++)
        {
            otherSystems[i].SetMechanismBehaviour(interactionActive); //Todos los mecanismos (puertas, plataformas, corrientes de aire...)
        }
    }
}
