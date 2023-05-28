using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitModificationNotification : MonoBehaviour
{
    [SerializeField] private SmoothVanishUI smoothVanishUI;
    private ImprovementItem[] allImprovementItems;
    
    private void Start()
    {
        smoothVanishUI.SetVanish(true);

        allImprovementItems = FindObjectsOfType<ImprovementItem>(true);
        
        var playerMainService = FindObjectOfType<PlayerMainService>();

        playerMainService.OnImprovementPointsValueChange += UpdateModificationPossibleState;
        playerMainService.OnSpecialModuleUnlock += UpdateModificationPossibleState;
        
        UpdateModificationPossibleState(0);
    }

    private void UpdateModificationPossibleState(int value = 0)
    {
        smoothVanishUI.SetVanish(!IsModificationPossible());
        
        bool IsModificationPossible()
        {
            foreach (var item in allImprovementItems)
            {
                if (item.IsSellPossible())
                    return true;
            }

            return false;
        }
    }
    
    private void UpdateModificationPossibleState(PlayerModules playerModule = 0)
    {
        UpdateModificationPossibleState(0);
    }
}
