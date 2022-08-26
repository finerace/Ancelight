using UnityEngine;

public class WeaponGetItem : MonoBehaviour, IPlayerItem
{
    [SerializeField] private int getWeaponId;
    [SerializeField] private int bonusBullets = 0;
    [SerializeField] private Transform stand;

    [SerializeField] private bool isPickUpped = false;
    
    public void PickUp(PlayerMainService playerMainService)
    {
        if(isPickUpped)
            return;
            
        isPickUpped = true;
        
        playerMainService.UnlockWeapon(getWeaponId);
        
        AddBonusBullets();

        if (stand != null)
            stand.parent = null;
        
        Destroy(gameObject);
        
        void AddBonusBullets()
        {
            if(bonusBullets <= 0)
                return;

            var weaponData = playerMainService.weaponsManager.FindWeaponData(getWeaponId);
            var bulletId = weaponData.BulletsID;
            
            playerMainService.weaponsBulletsManager.AddBullets(bulletId,bonusBullets);
        }
    }

    
}
