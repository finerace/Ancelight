using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedMenuSpecialService : MonoBehaviour
{
    public void LoadLastSave()
    {
        var lastSaveName = SavesFoundSpawnService.FoundSavesNames()[0];
        var levelSaveLoadService = FindObjectOfType<LevelSaveLoadSystem>();
        
        levelSaveLoadService.StartLoadLevel(lastSaveName);
    }
}
