using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlatformSystem : MechanismObject
{
    public float platfromSpeed;

    public Transform platform;
    public Transform pointA;
    public Transform pointB;
    private Vector3 target;

    protected override void Start()
    {
        base.Start();
        target = pointB.position;
    }

    protected override void MechanismBehaviour()
    {
        PlatformMovement();
    }

    private void PlatformMovement()
    {
        float distance = Vector3.Distance(platform.position, target);

        //if() --> Si llego al target y ese target es el punto B --> cambio al punto A
        //else if --> Viceversa
        if (distance < 0.05f && target == pointB.position)
        {
            target = pointA.position;
        }
        else if (distance < 0.05f && target == pointA.position)
        {
            target = pointB.position;
        }

        platform.position = Vector3.MoveTowards(platform.position, target, platfromSpeed * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();

            if (player.isGrounded && player.transform.position.y > platform.position.y)
            {
                player.isOnPlatform = true;
                collision.transform.SetParent(platform.transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();
            player.isOnPlatform = false;

            collision.transform.SetParent(null);
        }
    }
}
