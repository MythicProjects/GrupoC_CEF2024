using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerController controller;

    public bool killPlayer; //Delete///////////////////////////

    public float actualPlayerHealth;
    public float startPlayerHealth = 100;

    private Vector3 lastCheckpointPosition;

    public void GetStatsComponents()
    {
        controller = GetComponent<PlayerController>();
    }
    public void SetPlayerStats()
    {
        actualPlayerHealth = startPlayerHealth;
        UpdatePlayerCheckpoint(transform.position + Vector3.up);
    }

    private void Update()//Delete///////////////////////
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdatePlayerHealth(-1000);
        }
    }
    public void UpdatePlayerHealth(float value)
    {
        actualPlayerHealth += value;

        if (actualPlayerHealth <= 0)
        {
            controller.stopPlayer = true;
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        //Al estar moviendo con RB no sirve solo transform.position
        transform.position = lastCheckpointPosition;

        actualPlayerHealth = startPlayerHealth;

        Invoke(nameof(ResetStopPlayer), 1);
    }
    public void UpdatePlayerCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpointPosition = newCheckpoint + Vector3.up;
    }

    private void ResetStopPlayer()
    {
        controller.stopPlayer = false;

        killPlayer = false;
    }
}
