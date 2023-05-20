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
    [SerializeField] private int itemNameTextId;
    [SerializeField] private int itemDescTextId;
    [SerializeField] private int itemEffectTextId;
    [SerializeField] private string itemDecorationLevel;
    
    public Sprite ItemIcon => itemIcon;

    public int ItemNameTextId => itemNameTextId;

    public int ItemDescTextId => itemDescTextId;

    public int ItemEffectTextId => itemEffectTextId;

    public string ItemDecorationLevel => itemDecorationLevel;

    [Space] 
    
    [SerializeField] private Image buyIndicator;
    [SerializeField] private Image selectIndicator;

    [SerializeField] private Color noBuySelectColor;
    [SerializeField] private Color buySelectColor;
    [SerializeField] private Color idleColor;
    
    [SerializeField] private Color buyColor;
    
    private bool mouseInItem;

    [Space] 
    
    private AudioPoolService audioPoolService;
    [SerializeField] private AudioCastData onMouseEnter;
    [SerializeField] private AudioCastData onMouseClickComplete;
    [SerializeField] private AudioCastData onMouseClickDefeat;

    protected void Awake()
    {
        Init();
        void Init()
        {
            if (improvementSelectBuyService == null)
                improvementSelectBuyService = FindObjectOfType<ImprovementSelectBuyService>();
        }
    }

    protected void Start()
    {
        improvementSelectBuyService = ImprovementSelectBuyService.instance;
        
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
        
        if(NowSellCheck())
            ActivateBuyIndicator();
    }

    protected abstract void ImprovementEffect();

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
        
        ImprovementEffect();
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
        if (!SpecialsBuyConditionsCheck())
        {
            audioPoolService.CastAudio(onMouseClickDefeat);
            return;
        }
        
        audioPoolService.CastAudio(onMouseClickComplete);
        improvementSelectBuyService.SelectImprovementItem(this);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SpecialsBuyConditionsCheck())
            SetSelectIndicator(buySelectColor);
        else
            SetSelectIndicator(noBuySelectColor);

        audioPoolService.CastAudio(onMouseEnter);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        SetSelectIndicator(idleColor);
    }
}
