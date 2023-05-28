using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public UnityEvent triggerEvent;
    [SerializeField] private bool isOneUse = false;
    private bool isUsed = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if(isUsed)
            return;

        if (isOneUse)
            isUsed = true;
        
        triggerEvent?.Invoke();
    }
    
}
