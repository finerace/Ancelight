using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorImprovementItem : ImprovementItem
{
    private PlayerMainService playerMainService;

    [SerializeField] private float minArmorToBuy;
    [SerializeField] private float maxArmorToBuy;
    
    [SerializeField] private float itemNewArmor;

    [SerializeField] private bool isArmorValueItsResistance;
    
    protected new void Start()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        base.Start();
    }
    
    protected override void ImprovementEffect()
    {
        if (!isArmorValueItsResistance)
        {
            playerMainService.SetMaxArmor(itemNewArmor);
            return;            
        }
        
        playerMainService.SetArmorDamageResistance(itemNewArmor);
    }

    protected override bool SpecialsBuyConditionsCheck()
    {
        if (!isArmorValueItsResistance)
            return playerMainService.MaxArmor >= minArmorToBuy && playerMainService.MaxArmor <= maxArmorToBuy;
        
        return 
                playerMainService.ArmorDamageResistance >= minArmorToBuy && 
                playerMainService.ArmorDamageResistance <= maxArmorToBuy;
    }

    protected override bool NowSellCheck()
    {
        if(!isArmorValueItsResistance)
            return playerMainService.MaxArmor >= itemNewArmor;

        return playerMainService.ArmorDamageResistance >= itemNewArmor;
    }
    
}
