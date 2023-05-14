using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchancedSuitPointItem : OrdinaryPlayerItem
{
    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        player.AddImprovementPoint(1);
        
        DestroyItem();
    }
}
