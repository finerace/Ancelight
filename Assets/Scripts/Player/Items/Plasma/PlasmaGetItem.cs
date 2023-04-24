using System;
using UnityEngine;

public class PlasmaGetItem : OrdinaryPlayerItem
{
    [SerializeField] private string plasmaId = "yellow";
    [SerializeField] private int plasmaNameId;
    [SerializeField] private float plasmaCount = 0;

    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        if(IsPlasmaMax())
            return;

        player.AddPlasma(plasmaNameId,plasmaId,plasmaCount);

        DestroyItem();
        
        bool IsPlasmaMax()
        {
            var count = player.weaponsBulletsManager.PlasmaReserves[plasmaId];
            var maxCount = player.weaponsBulletsManager.PlasmaMaxReserves[plasmaId];

            return count >= maxCount;
        }
    }
}
