using UnityEngine;

public class PlasmaGetItem : MonoBehaviour,IPlayerItem
{
    [SerializeField] private string plasmaId = "yellow";
    [SerializeField] private float plasmaCount = 0;
    private bool isPickUpped = false;
    
    public void PickUp(PlayerMainService player)
    {
        if(IsPlasmaMax() || isPickUpped)
            return;

        isPickUpped = true;
        
        player.AddPlasma(plasmaId,plasmaCount);
        
        Destroy(gameObject);
        
        bool IsPlasmaMax()
        {
            var count = player.weaponsBulletsManager.PlasmaReserves[plasmaId];
            var maxCount = player.weaponsBulletsManager.PlasmaMaxReserves[plasmaId];

            return count >= maxCount;
        }
    }
}
