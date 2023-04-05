using UnityEngine;

public class DashImprovementItem : ImprovementItem
{
    private PlayerDashsService playerDashsService;
    
    [SerializeField] private int minDashToBuy;
    [SerializeField] private int itemDashBuy;
    
    protected new void Start()
    {
        playerDashsService = FindObjectOfType<PlayerDashsService>();
        
        base.Start();
    }
    
    protected override void ImprovementEffect()
    {
        playerDashsService.SetNewDashCount(itemDashBuy);
    }

    protected override bool SpecialsBuyConditionsCheck()
    {
        return playerDashsService.DashsCount == minDashToBuy && playerDashsService.IsDashServiceExist;
    }

    protected override bool NowSellCheck()
    {
        return playerDashsService.DashsCount >= itemDashBuy;
    }
}