using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediatelyProtectionImprovementItem : ImprovementItem
{
    private PlayerImmediatelyProtectionService playerProtection;
    
    [SerializeField] private float minDamageToBuy; 
    [SerializeField] private float maxDamageToBuy;

    [SerializeField] private float toSetDamage;
    [SerializeField] private float toSetRadius;

    protected new void Awake()
    {
        base.Awake();
        
        playerProtection = FindObjectOfType<PlayerImmediatelyProtectionService>();
    }
    
    protected override void ImprovementEffect()
    {
        playerProtection.SetNewDamage(toSetDamage);
        playerProtection.SetNewDamageRadius(toSetRadius);
    }

    protected override bool SpecialsBuyConditionsCheck()
    {
        return playerProtection.ExplosionDamage >= minDamageToBuy && playerProtection.ExplosionDamage <= maxDamageToBuy
            && playerProtection.IsProtectionExist;
    }

    protected override bool NowSellCheck()
    {
        return playerProtection.ExplosionDamage >= toSetDamage;
    }
}
