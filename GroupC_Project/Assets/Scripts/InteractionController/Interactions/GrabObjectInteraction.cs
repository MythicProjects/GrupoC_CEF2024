using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectInteraction : InteractionController
{
    [Header("Movement Settings")]
    public Vector3 offsetWithPlayer;
    public Transform playerTransform;


    protected override void Update()
    {
        base.Update();

        if (interactionActive)
        {
            MoveObjectWithPlayer();
        }
    }
    private  void MoveObjectWithPlayer()
    {
        transform.position = playerTransform.position + offsetWithPlayer;
    }
    public void ObjectOffset(Transform player)
    {
        playerTransform = player;
        offsetWithPlayer = transform.position - playerTransform.position;
    }

    public void ActivateObjectMovement(bool setState)
    {
        interactionActive = setState;
    }
}
