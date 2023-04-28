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
    
    private bool isButtonClose;
    public bool IsButtonClose => isButtonClose;
    
    private DeviceButton useButton = new DeviceButton();
    

    private void Update()
    {
        if(!isManageActive)
            return;
        
        CheckPlayerUsesItem();
    }

    private void CheckPlayerUsesItem()
    {
        if (!playerLookService.IsCameraHit)
        {
            isButtonClose = false;
            return;
        }
            
        var useItemRayHit = playerLookService.GetCameraRayHit();

        if (useItemRayHit.collider == null)
        {
            isButtonClose = false;
            return;
        }
        
        if (!IsDistanceAllow(useItemRayHit.point))
        {
            isButtonClose = false;
            
            return;
        }

        if (useItemRayHit.collider.gameObject.TryGetComponent(out IPlayerUsesItem usesItem))
        {
            isButtonClose = true;

            if (useButton.IsGetButtonDown())
                usesItem.PlayerUse();
        }
        else
            isButtonClose = false;
        
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
