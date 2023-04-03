using System;
using UnityEngine;

public class KeyTipPlayerMovement : KeyboardKeyTip
{
    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerMovement;
    }
    
}
