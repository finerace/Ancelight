using UnityEngine;

public class ModulsItem : OrdinaryPlayerItem
{
    [SerializeField] private PlayerModules targetPlayerModule;
    
    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        player.UnlockModule(targetPlayerModule);
        DestroyItem();
    }
}
