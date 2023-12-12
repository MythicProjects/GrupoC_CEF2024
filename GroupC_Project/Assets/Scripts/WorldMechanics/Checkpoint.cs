using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            SetRespawnPosition(playerStats);
        }
    }

    private void SetRespawnPosition(PlayerStats player)
    {
        if (player != null)
        {
            player.UpdatePlayerCheckpoint(transform.position);
        }
    }
}
