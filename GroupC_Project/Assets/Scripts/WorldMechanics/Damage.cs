using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float triggerDamage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            DoDamageToPlayer(playerStats);
        }
    }

    private void DoDamageToPlayer(PlayerStats player)
    {
        if (player != null)
        {
            player.UpdatePlayerHealth(-triggerDamage);
        }
    }
}
