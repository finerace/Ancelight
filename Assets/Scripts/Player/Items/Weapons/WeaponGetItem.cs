using UnityEngine;

public class WeaponGetItem : MonoBehaviour, IPlayerItem
{
    [SerializeField] private int getWeaponId;
    
    public void PickUp(PlayerMainService playerMainService)
    {
        playerMainService.AddWeapon(getWeaponId);
        
        Destroy(gameObject);
    }
}
