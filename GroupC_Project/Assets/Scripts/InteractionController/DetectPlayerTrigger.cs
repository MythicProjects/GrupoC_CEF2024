using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerTrigger : MonoBehaviour
{
    ActivateMechanismInteraction interaction;
    private void Start()
    {
        interaction = GetComponent<ActivateMechanismInteraction>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {
            interaction.ActivateMechanism(false);
        }
    }
}
