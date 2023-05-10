using UnityEngine;
using UnityEngine.Events;

public class LevelTriggerEnterExit : LevelTriggerBase
{

    public UnityEvent exitEvent;

    private void OnTriggerExit(Collider other)
    {
        if(!isTriggerActive || other.isTrigger || !activateLayerMask.IsLayerInMask(other.gameObject.layer))
            return;

        exitEvent?.Invoke();
        
        if (isSingleTrigger)
            isTriggerActive = true;
    }
}
