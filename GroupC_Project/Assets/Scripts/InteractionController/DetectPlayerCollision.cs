using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerCollision : MonoBehaviour
{
    ActivateMechanismInteraction interaction;
    private void Start()
    {
        interaction = GetComponent<ActivateMechanismInteraction>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if(collision.collider.tag == "Player")
        {
            interaction.ActivateMechanism(false);
        }
    }
}
