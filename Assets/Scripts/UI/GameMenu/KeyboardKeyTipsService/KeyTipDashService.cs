using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipDashService : KeyboardKeyTip
{
    private PlayerDashsService playerDashService;

    private void Awake()
    {
        playerDashService = FindObjectOfType<PlayerDashsService>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerDashService;
    }
}
