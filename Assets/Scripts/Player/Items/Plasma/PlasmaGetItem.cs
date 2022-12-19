using System;
using UnityEngine;

public class PlasmaGetItem : OrdinaryPlayerItem
{
    [SerializeField] private string plasmaId = "yellow";
    [SerializeField] private float plasmaCount = 0;

    private void Awake()
    {
        ToSaveData.mainSaveData.AddToSaveData(this);
    }

    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        if(IsPlasmaMax())
            return;

        player.AddPlasma(plasmaId,plasmaCount);

        ToSaveData.mainSaveData.RemoveFromSaveData(this);
        
        DestroyItem();
        
        bool IsPlasmaMax()
        {
            var count = player.weaponsBulletsManager.PlasmaReserves[plasmaId];
            var maxCount = player.weaponsBulletsManager.PlasmaMaxReserves[plasmaId];

            return count >= maxCount;
        }
    }
}
