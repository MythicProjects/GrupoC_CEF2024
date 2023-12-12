using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Rigidbody rb; //No kinematico

    PlayerController controller;

    [Header("Camera Settings")]
    private Transform cameraObject;

    [Header("Horizontal Movement Settings")]
    [SerializeField] float movementSpeed = 6;
    private float actualSpeed;
    [SerializeField] float rotationSpeed = 20;
    private Vector3 moveDirection;
    public Transform platform;

    [Header("Interaction Movement Settings")]
    [SerializeField] float grabObjectSpeed = 6;
    private Vector3 interactionDirection;


    [Header("Vertical Movement Settings")]
    public float jumpForce = 8;
    public float fallingSpeed = -30;
    private float verticalSpeed;

    [Header("Ground Collision Settings")]
    public LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.2f;

    [Header("Step Collision Settings")]
    [SerializeField] float stepRayDistance = 1.2f;
    [SerializeField] float maxStepHeight = 0.3f;
    [SerializeField] float stepUpSpeed = 10;

    [Header("Wall Collision Settings")]
    [SerializeField] float wallRayDistance = 0.6f;
    [SerializeField] float wallRaysAngle = 0.2f;

    public void GetLocomotionComponents()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();

    }
    public void SetLocomotionComponents()
    {
        cameraObject = Camera.main.transform;
    }
    public void HandleMovement(float delta)
    {
        //Set Inputs
        float xAxis = controller.hAxisInput;
        float zAxis = controller.vAxisInput;

        //Handle direction
        moveDirection = cameraObject.forward * zAxis + cameraObject.right * xAxis;
        moveDirection.Normalize();
        moveDirection.y = 0;

        //Speed
        float inputValue = controller.axisInputAmount;
        actualSpeed = movementSpeed * inputValue;

        if (controller.isWallCollision) actualSpeed = 0;

        //Y velocity
        Vector3 verticalVelocity = Vector3.up * verticalSpeed;
        //X and Z velocity
        Vector3 horizontalVelocity = moveDirection * actualSpeed;

        //All velocity
        Vector3 movementVelocity = horizontalVelocity + verticalVelocity;

        if (controller.isOnPlatform)//Las plataformas en Kinematic impiden el uso de rb.position
        {
            transform.position += movementVelocity * delta;
            return;
        }

        rb.position += movementVelocity * delta;
    }
    public void HandleRotation(float delta)
    {
        Vector3 targetDir;

        if (!controller.isInteracting)
        {
            targetDir = moveDirection;
            if (targetDir == Vector3.zero) targetDir = transform.forward;
        }
        else
        {
            targetDir = interactionDirection;
            targetDir.y = 0;
        }
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * delta);

        if (controller.isOnPlatform)//Las plataformas en Kinematic impiden el uso de rb.rotation
        {
            transform.rotation = targetRotation;
            return;
        }
        rb.rotation = targetRotation;
    }
    public void SetInteractionObject(Vector3 newInteractionObject) 
    {
        interactionDirection = newInteractionObject - transform.position;
    }
    public void HandleGrabMovement(float delta)
    {
        float xAxis = controller.hAxisInput;
        float zAxis = controller.vAxisInput;

        moveDirection = cameraObject.forward * zAxis + cameraObject.right * xAxis;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float inputValue = controller.axisInputAmount;
        actualSpeed = grabObjectSpeed * inputValue;

        Vector3 verticalVelocity = Vector3.up * verticalSpeed;
        Vector3 horizontalVelocity = moveDirection * actualSpeed;

        Vector3 movementVelocity = horizontalVelocity + verticalVelocity;

        rb.position += movementVelocity * delta;
    }

    public void HandleGravity(float delta)
    {

        if (controller.isGrounded && verticalSpeed < 0.1f)
        {
            verticalSpeed = 0;
        }
        else if (!controller.isGrounded)
        {
            verticalSpeed += fallingSpeed * delta;
        }

        verticalSpeed = Mathf.Clamp(verticalSpeed, fallingSpeed, jumpForce);
    }

    public void HandleJump()
    {
        if (controller.jumpInput)
        {
            verticalSpeed = jumpForce;
            return;
        }
        if (controller.jumpInput && controller.isGrounded)
        {
            verticalSpeed = jumpForce;
        }
    }
    public void HandleGroundCollision(float delta)
    {
        Vector3 groundPos = transform.position;
        Vector3 centerPos = transform.position + Vector3.up;

        bool detectGround = Physics.CheckSphere(groundPos, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);


        controller.isGrounded = detectGround;

        if (detectGround)
        {
            RaycastHit hit;
            bool hitGround;

            Vector3 rayOrigin;
            if (controller.axisInputAmount > 0.1f)
            {
                rayOrigin = centerPos + transform.forward * 0.3f;
            }
            else
            {
                rayOrigin = centerPos;
            }

            hitGround = Physics.Raycast(rayOrigin, -Vector3.up, out hit, stepRayDistance, groundLayer);
            Debug.DrawRay(rayOrigin, -Vector3.up * stepRayDistance, Color.blue, 0, false);


            if (hitGround && verticalSpeed < 0.1f)
            {
                Vector3 targetPosition = transform.position;
                targetPosition.y = hit.point.y;

                float stepHeightDifference = hit.point.y - transform.position.y;

                if (Mathf.Abs(stepHeightDifference) < maxStepHeight)
                {
                    rb.position = Vector3.Slerp(transform.position, targetPosition, delta * stepUpSpeed);
                }
            }
        }
    }
    public void HandleWallsCollision()
    {
        Vector3 groundPos = transform.position;
        if (controller.isGrounded) groundPos = transform.position + Vector3.up * 0.5f;
        Vector3 centerPos = transform.position + Vector3.up;

        Vector3 forward = transform.forward * wallRayDistance;
        Vector3 forwardLeft = transform.forward * wallRayDistance - transform.right * wallRaysAngle;
        Vector3 forwardRight = transform.forward * wallRayDistance + transform.right * wallRaysAngle;

        Vector3[] rayDirection = { forward, forwardLeft, forwardRight };

        bool wallRaycast = false;

        for (int i = 0; i < rayDirection.Length; i++)
        {
            bool center = Physics.Raycast(centerPos, rayDirection[i], wallRayDistance, groundLayer);
            bool ground = Physics.Raycast(groundPos, rayDirection[i], wallRayDistance, groundLayer);

            Debug.DrawRay(groundPos, rayDirection[i] * wallRayDistance, Color.green, 0, false);
            Debug.DrawRay(centerPos, rayDirection[i] * wallRayDistance, Color.green, 0, false);

            if (center || ground)
            {
                wallRaycast = true;
                Debug.DrawRay(groundPos, rayDirection[i] * wallRayDistance, Color.red, 0, false);
                Debug.DrawRay(centerPos, rayDirection[i] * wallRayDistance, Color.red, 0, false);
            }
        }

        if (wallRaycast)
        {
            controller.isWallCollision = true;
        }
        else
        {
            controller.isWallCollision = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (controller == null) Gizmos.color = transparentGreen;
        else
        {
            if (controller.isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;
        }

        Vector3 spherePosition = transform.position;
        Gizmos.DrawSphere(spherePosition, groundCheckRadius);
    }

}
