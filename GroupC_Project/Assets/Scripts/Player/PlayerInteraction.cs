using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController controller;
    private PlayerLocomotion locomotion;

    [Header("Interaction Settings")]
    public LayerMask interactLayer;

    [Header("Arm Settings")]
    public Transform armPosition;
    public List <ArmData> arms = new List<ArmData>();
    private int armSelection;

    [Header("Strong Arm Settings")]
    public float strongRayDistance = 1;

    [Header("Tech Arm Settings")]
    public float techRayDistance = 1;

    [Header("Attraction Arm Settings")]
    public float attractionForce = 5;
    public float attractRayDistance = 10;


    public void GetInteractionComponents()
    {
        controller = GetComponent<PlayerController>();
        locomotion = GetComponent<PlayerLocomotion>();
    }
    public void SetInteractionComponents()
    {
        armSelection = -1;
    }
    //Selección de brazo------------------------------------
    public void ArmSelection()
    {
        if (controller.changeArmInput)
        {
            armSelection++;//Suma

            if (armSelection > arms.Count - 1)//Comprueba
            {
                armSelection = -1;
            }


            if (armSelection == -1)//Brazo Normal
            {
                Debug.Log("normal");
                UpdateArmPrefab(null);
                return;
            }

            //Aplica
            Debug.Log(arms[armSelection].armType);
            UpdateArmPrefab(arms[armSelection].armPrefab);
        }
    }

    private void UpdateArmPrefab(GameObject newArm)
    {
        //Eliminar
        if (armPosition.childCount > 0)
        {
            foreach (Transform obj in armPosition)
            {
                Destroy(obj.gameObject);
            }
        }

        //Instanciar
        if (newArm != null)
        {
            Instantiate(newArm, armPosition);
        }
    }
    //------------------------------------

    //Uso de brazo------------------------------------

    public void ArmInteraction()
    {
        if (armSelection < 0)
            return;

        switch (arms[armSelection].armType) //Depende de armType. Se ha hecho por string --> "strong", "tech", "attract"
        {
            case "strong":
                UseStrongArm();
                break;
            case "tech":
                UseTechArm();
                break;
            case "attract":
                UseAttractArm();
                break;
        }
    }

    //OnlyOneShot
    private void UseStrongArm() //Changes movement
    {
        RaycastHit hit;
        float rayDistance = strongRayDistance;//Cerca

        bool hitInteractable = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rayDistance, interactLayer);
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * rayDistance);

        if (hitInteractable)
        {
            controller.isInteracting = !controller.isInteracting;

            GrabObjectInteraction grabObject = hit.collider.GetComponent<GrabObjectInteraction>();
            if (grabObject == null) return;

            locomotion.SetInteractionObject(grabObject.transform.position);
            grabObject.ObjectOffset(transform);
            grabObject.interactionActive = controller.isInteracting;

            Debug.Log("grab" + grabObject.name);
        }
    }
    private void UseTechArm()
    {
        RaycastHit hit;
        float rayDistance = techRayDistance;//Cerca

        bool hitInteractable = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rayDistance, interactLayer);
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * rayDistance);

        if (hitInteractable)
        {
            ActivateMechanismInteraction activateSystem = hit.collider.GetComponent<ActivateMechanismInteraction>();
            if (activateSystem == null) return;

            activateSystem.ActivateMechanism(true);

            Debug.Log("tech" + activateSystem.name);
        }
    }
    private void UseAttractArm() //Changes movement
    {
        RaycastHit hit;
        float rayDistance = attractRayDistance;//Lejos

        //¿Crear una zona (cono) para cubrir más rango?
        bool hitInteractable = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rayDistance, interactLayer);
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * rayDistance);

        if (hitInteractable)
        {
            controller.isInteracting = !controller.isInteracting;

            AttractObjectInteraction attractObject = hit.collider.GetComponent<AttractObjectInteraction>();
            if (attractObject == null) return;

            locomotion.SetInteractionObject(attractObject.transform.position);
            attractObject.SetAttractComponent(transform, attractionForce);
            attractObject.interactionActive = controller.isInteracting;

            Debug.Log("attractObject" + attractObject.name);
        }
    }
}
