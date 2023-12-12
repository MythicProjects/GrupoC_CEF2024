using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Arm Data", menuName = "Arm/New Arm")]
public class ArmData : ScriptableObject
{
    [Header("Arm Type")]
    public string armType;

    [Header("Arm Object")]
    public GameObject armPrefab;
}
