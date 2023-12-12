using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractObjectInteraction : InteractionController
{
    Rigidbody rb;
    [Header("Attract Origin")]
    public Transform attractOrigin;
    public float attractOffset = 1;
    private float attractionForce;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void Update()
    {
        base.Update();

        if (interactionActive)
        {
            AttractObjectToPlayer();
        }
    }
    private void AttractObjectToPlayer()
    {
        Vector3 attractPosition = attractOrigin.position + Vector3.up + attractOrigin.forward * attractOffset;
        float distanceToPlayer = Vector3.Distance(transform.position, attractOrigin.position);

        Vector3 forceDirection = attractPosition - transform.position;
        forceDirection.Normalize();

        rb.AddForce(forceDirection * attractionForce * distanceToPlayer, ForceMode.Acceleration);

    }
    public void SetAttractComponent(Transform newAttractOrigin, float newAttractionForce)
    {
        attractionForce = newAttractionForce;
        attractOrigin = newAttractOrigin;
    }
}
