using System;
using UnityEngine;

public class ItemAmmo : OrdinaryPlayerItem
{
    [Space] 
    
    [SerializeField] private int bulletId;
    [SerializeField] private int bulletCount;

    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        var bulletsManager = player.weaponsBulletsManager;

        var isBulletsFull = bulletsManager.GetBulletsCount(bulletId) >= bulletsManager.FindData(bulletId).MaxBullets;

        if (!isBulletsFull)
        {
            player.AddBullets(bulletId, bulletCount);
            DestroyItem();
        }
    }
}
