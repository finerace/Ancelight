using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ImprovementItem : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected ImprovementSelectBuyService improvementSelectBuyService;
    [SerializeField] private int improvementPointCost;
    public int ImprovementPointCost => improvementPointCost;

    [Space] 
    
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDesc;
    [SerializeField] private string itemEffect;
    [SerializeField] private string itemDecorationLevel;

    public Sprite ItemIcon => itemIcon;

    public string ItemName => itemName;

    public string ItemDesc => itemDesc;

    public string ItemEffect => itemEffect;

    public string ItemDecorationLevel => itemDecorationLevel;

    [Space] 
    
    [SerializeField] private Image buyIndicator;
    [SerializeField] private Image selectIndicator;

    [SerializeField] private Color noBuySelectColor;
    [SerializeField] private Color buySelectColor;
    [SerializeField] private Color idleColor;
    
    [SerializeField] private Color buyColor;
    
    private bool mouseInItem;

    protected void Start()
    {
        improvementSelectBuyService = ImprovementSelectBuyService.instance;

        if(NowSellCheck())
            ActivateBuyIndicator();
    }

    protected abstract void BuyEffect();

    protected abstract bool SpecialsBuyConditionsCheck();

    protected abstract bool NowSellCheck();

    public bool IsSellPossible()
    {
        return (improvementSelectBuyService.SuitImprovementPoints - improvementPointCost) >= 0 
               && SpecialsBuyConditionsCheck();
    }

    public void Buy()
    {
        if(!IsSellPossible())
            return;

        improvementSelectBuyService.SuitImprovementPoints -= improvementPointCost;
        
        BuyEffect();
        ActivateBuyIndicator();
    }

    private void ActivateBuyIndicator()
    {
        buyIndicator.color = buyColor;
    }

    private void SetSelectIndicator(Color color)
    {
        selectIndicator.color = color;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!SpecialsBuyConditionsCheck())
            return;
            
        improvementSelectBuyService.SelectImprovementItem(this);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SpecialsBuyConditionsCheck())
            SetSelectIndicator(buySelectColor);
        else
            SetSelectIndicator(noBuySelectColor);
        
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        SetSelectIndicator(idleColor);
    }
}
