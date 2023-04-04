using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipPlayerMainService : KeyboardKeyTip
{
    private PlayerMainService playerMainService;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerMainService;
    }
    
}
