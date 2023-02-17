using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseService : MonoBehaviour,IUsePlayerDevicesButtons
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLookService playerLookService;
    [SerializeField] private bool isManageActive = true;
    
    [Space]
    
    [SerializeField] private float allowUseDistance;

    [Space] 
    
    private DeviceButton useButton = new DeviceButton();
    

    private void Update()
    {
        if(!isManageActive)
            return;
        
        CheckPlayerUsesItem();
    }

    private void CheckPlayerUsesItem()
    {
        if (useButton.IsGetButtonDown())
        {
            if(!playerLookService.IsCameraHit)
                return;
            
            var useItemRayHit = playerLookService.GetCameraRayHit();
            
            if(!IsDistanceAllow(useItemRayHit.point))
                return;

            if (useItemRayHit.collider.gameObject.
                TryGetComponent(out IPlayerUsesItem usesItem))
            {
                usesItem.PlayerUse();
            }
        }

        bool IsDistanceAllow(Vector3 point)
        {
            return
                Vector3.Distance(point, playerMovement.Body.position) <= allowUseDistance;
        }
        
    }

    public void SetManageActive(bool state)
    {
        isManageActive = state;
    }

    public DeviceButton[] GetUsesDevicesButtons()
    {
        return new [] {useButton};
    }
}

public interface IPlayerUsesItem
{
    public void PlayerUse();
}
