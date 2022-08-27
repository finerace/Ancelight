using UnityEngine;

public class WeaponGetItem : OrdinaryPlayerItem
{
    [Space]
    [SerializeField] private int getWeaponId;
    [SerializeField] private int bonusBullets = 0;
    [SerializeField] private Transform stand;

    // ReSharper disable Unity.PerformanceAnalysis
    protected override void PickUpItemAlgorithm(PlayerMainService playerMainService)
    {
        if(!IsWeaponNotUseless())
            return;
        
        playerMainService.UnlockWeapon(getWeaponId);
        
        AddBonusBullets();
    
        DestroyItem();
        
        void AddBonusBullets()
        {
            if(bonusBullets <= 0)
                return;

            var weaponData = playerMainService.weaponsManager.FindWeaponData(getWeaponId);
            var bulletId = weaponData.BulletsID;
            
            playerMainService.weaponsBulletsManager.AddBullets(bulletId,bonusBullets);
        }

        bool IsWeaponNotUseless()
        {
            var isGetWeaponUnlocked = playerMainService.weaponsManager.CheckIsUnlockedID(getWeaponId);

            var isBonusBulletsNotMax = true;

            // ReSharper disable once InvertIf
            if (bonusBullets > 0 && isGetWeaponUnlocked)
            {
                var bulletId = playerMainService.weaponsManager.FindWeaponData(getWeaponId).BulletsID;

                var bulletCount = playerMainService.weaponsBulletsManager.BulletsCount[bulletId];
                var bulletMaxCount = playerMainService.weaponsBulletsManager.BulletsMax[bulletId];

                isBonusBulletsNotMax = bulletCount < bulletMaxCount;
            }

            return !isGetWeaponUnlocked || isBonusBulletsNotMax;
        }
    }

    protected override void DestroyItem()
    {
        SaveWeaponStand();
        
        base.DestroyItem();
        
        void SaveWeaponStand()
        {
            if (stand != null)
                stand.parent = null;
        }
    }
}
