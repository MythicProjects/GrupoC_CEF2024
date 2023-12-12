using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerLocomotion locomotion;
    PlayerInteraction playerInteraction;


    [Header("Movement Flags")]
    public bool stopPlayer;
    public bool isGrounded;
    public bool isWallCollision;
    public bool isOnPlatform;

    [Header("Interaction Flags")]
    public bool isInteracting;

    [Header("Input Controller")]
    public float hAxisInput, vAxisInput, axisInputAmount;
    public bool jumpInput;
    public bool useArmInput;
    public bool changeArmInput;


    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        locomotion = GetComponent<PlayerLocomotion>();
        playerInteraction = GetComponent<PlayerInteraction>();

        playerStats.GetStatsComponents();
        locomotion.GetLocomotionComponents();
        playerInteraction.GetInteractionComponents();

    }
    private void Start()
    {
        playerStats.SetPlayerStats();
        locomotion.SetLocomotionComponents();
        playerInteraction.SetInteractionComponents();
    }
    private void Update()
    {
        float delta = Time.deltaTime;

        InputController();

        //Interaction
        if (changeArmInput && !isInteracting) playerInteraction.ArmSelection();
        if (useArmInput) playerInteraction.ArmInteraction();

        //Movement
        if (stopPlayer) return;
        locomotion.HandleJump();
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        //Movement
        if (stopPlayer) return;

        //Detect collisions
        locomotion.HandleGroundCollision(delta);
        locomotion.HandleWallsCollision();

        //Apply Gravity
        locomotion.HandleGravity(delta);

        //Apply Movement
        if (!isInteracting) locomotion.HandleMovement(delta);
        else locomotion.HandleGrabMovement(delta);

        //Apply Rotation
        locomotion.HandleRotation(delta);

    }


    //Input Controller
    public void InputController()
    {
        GetMovementInput();
        GetActionInputs();
    }
    private void GetMovementInput()
    {
        hAxisInput = Input.GetAxisRaw("Horizontal");
        vAxisInput = Input.GetAxisRaw("Vertical");
        axisInputAmount = Mathf.Clamp01(Mathf.Abs(hAxisInput) + Mathf.Abs(vAxisInput));
    }
    private void GetActionInputs()
    {
        jumpInput = Input.GetButtonDown("Jump");

        changeArmInput = Input.GetKeyDown(KeyCode.Tab);
        useArmInput = Input.GetKeyDown(KeyCode.E);
        //if (useArmInput) isInteracting = !isInteracting;// ----> Aplicado al detectar un objeto con el que interactuar
    }
}
