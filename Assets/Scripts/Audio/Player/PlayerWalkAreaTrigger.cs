using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerWalkAreaTrigger : MonoBehaviour
{
    [SerializeField] private PlayerSoundsService.PlayerWalkZone walkZone;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerSoundsService playerSoundsService))
        {
            playerSoundsService.SetNewWalkZone(walkZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerSoundsService playerSoundsService))
        {
            playerSoundsService.SetNewWalkZone(playerSoundsService.DefaultWalkZone);
        }
    }
}
