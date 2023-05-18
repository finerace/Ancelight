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
        
        FindObjectOfType<PlayerMainService>().OnImprovementPointsValueChange += UpdateModificationPossibleState;

        StartCoroutine(WaitFrame());
        IEnumerator WaitFrame()
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            
            UpdateModificationPossibleState();
        }
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
    
}
