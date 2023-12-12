using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlatformSystem : MechanismObject
{
    [Header("Platform Time")]
    public float activateMechanismTime = 1;
    public float resetMechanismTime = 1;
    private float mechanismTimer;

    public Transform platform;
    private Quaternion targetRotation;
    private Quaternion startRotation;

    protected override void Start()
    {
        base.Start();
        startRotation = platform.rotation;
    }

    protected override void MechanismBehaviour()
    {
        mechanismTimer += Time.deltaTime;

        if(mechanismTimer > activateMechanismTime && mechanismTimer < resetMechanismTime)
        {
            targetRotation = Quaternion.LookRotation(Vector3.down);
        }
        else if (mechanismTimer > resetMechanismTime)
        {
            targetRotation = Quaternion.Slerp(platform.rotation, startRotation, Time.deltaTime * 20);

            Debug.Log(transform.rotation == targetRotation);

            if(transform.rotation == targetRotation)
            {
                mechanismTimer = 0;
                activateMechanism = false;
            }
        }

        platform.rotation = targetRotation;
    }
    
}
