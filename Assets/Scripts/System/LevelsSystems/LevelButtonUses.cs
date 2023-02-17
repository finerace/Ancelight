using UnityEngine;
using UnityEngine.Events;

public class LevelButtonUses : MonoBehaviour,IPlayerUsesItem
{
    [SerializeField] private UnityEvent useEvent;
    [SerializeField] private bool isSingleUsedButton = true;
    private bool buttonIsActive = true;
    
    public void PlayerUse()
    {
        if(!buttonIsActive)
            return;
        
        if(useEvent != null)
            useEvent.Invoke();

        if (isSingleUsedButton)
            buttonIsActive = false;
    }
}
