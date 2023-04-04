using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipSuitProtection : KeyboardKeyTip
{
    private PlayerImmediatelyProtectionService playerProtectionService;

    private void Awake()
    {
        playerProtectionService = FindObjectOfType<PlayerImmediatelyProtectionService>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerProtectionService;
    }
}
