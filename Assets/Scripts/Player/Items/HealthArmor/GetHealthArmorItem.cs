using System;
using UnityEngine;

public class GetHealthArmorItem : OrdinaryPlayerItem
{

    [SerializeField] private float getHealth;
    [SerializeField] private float getArmor;
    private bool isValuesInvalid = false;
    protected new void Start()
    {
        base.Start();
        
        CheckValuesForInvalid();
    }

    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        if(isValuesInvalid)
            return;

        var isHealthGetNotUseless = IsHealthGetNotUseless();
        var isArmorGetNotUseless = IsArmorNotUseless();
        
        var isItemUseless = !isHealthGetNotUseless && !isArmorGetNotUseless;

        if(isItemUseless)
            return;
        
        if(isHealthGetNotUseless)
            player.AddHealth(getHealth);
        
        if(isArmorGetNotUseless)
            player.AddArmor(getArmor);
        
        DestroyItem();
        
        bool IsHealthGetNotUseless()
        {
            var health = player.Health_;
            var maxHealth = player.MaxHealth_;

            var isHealthZero = getHealth <= 0;
            
            return health < maxHealth && !isHealthZero;
        }
        
        bool IsArmorNotUseless()
        {
            var armor = player.Armor;
            var maxArmor = player.MaxArmor;

            var isArmorZero = getArmor <= 0;
            
            return armor < maxArmor && !isArmorZero;
        }

    }
    
    private void CheckValuesForInvalid()
    {
        isValuesInvalid = getHealth < 0 || getArmor < 0;

        if (isValuesInvalid)
            throw new ArgumentException("Health or Armor values invalid!");
    }
}
