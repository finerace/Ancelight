using System;
using UnityEngine;

public class PlayerAmmoItem : OrdinaryPlayerItem
{
    [Space] 
    
    [SerializeField] private int bulletId;
    [SerializeField] private int bulletCount;

    private void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }

    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        var bulletsManager = player.weaponsBulletsManager;

        var isBulletsFull = bulletsManager.GetBulletsCount(bulletId) >= bulletsManager.FindData(bulletId).MaxBullets;

        if (!isBulletsFull)
        {
            player.AddBullets(bulletId, bulletCount);
            
            LevelSaveData.mainLevelSaveData.RemoveFromSaveData(this);
            DestroyItem();
        }
    }
}
