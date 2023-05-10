using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class LevelTriggerBase : LevelTrigger
{
    [SerializeField] protected bool isSingleTrigger = true;

    [Space]

    public UnityEvent triggerEvent;

    private void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }
    
    protected override void ActivateTrigger()
    {
        triggerEvent.Invoke();
        
        if (isSingleTrigger)
        {
            isTriggerActive = false;
        }
    }
}

public abstract class LevelTrigger : MonoBehaviour
{
    [SerializeField] protected int triggerId;
    public int TriggerId => triggerId;
    
    public bool isTriggerActive = true;

    public LayerMask activateLayerMask = 1 << 3;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(!isTriggerActive || other.isTrigger)
            return;
        
        if (activateLayerMask.IsLayerInMask(other.gameObject.layer))
        {
            ActivateTrigger();
        }
    }
    
    protected abstract void ActivateTrigger();
}
