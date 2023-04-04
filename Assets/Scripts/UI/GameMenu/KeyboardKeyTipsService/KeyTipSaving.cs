using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTipSaving : KeyboardKeyTip
{
    private LevelSaveLoadSystem levelSaveLoadSystem;

    private void Awake()
    {
        levelSaveLoadSystem = FindObjectOfType<LevelSaveLoadSystem>();
    }

    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return levelSaveLoadSystem;
    }
}
