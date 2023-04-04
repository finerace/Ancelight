using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipHookService : KeyboardKeyTip
{
    private PlayerHookService playerHookService;

    private void Awake()
    {
        playerHookService = FindObjectOfType<PlayerHookService>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerHookService;
    }
}
