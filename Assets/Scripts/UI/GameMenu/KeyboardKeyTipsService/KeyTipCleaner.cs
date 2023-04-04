using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipCleaner : KeyboardKeyTip
{
    private PlayerCleaner playerCleaner;

    private void Awake()
    {
        playerCleaner = FindObjectOfType<PlayerCleaner>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerCleaner;
    }
}
