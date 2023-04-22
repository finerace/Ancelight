using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementSelectBuyService : MonoBehaviour
{
    [SerializeField] public static ImprovementSelectBuyService instance; 
    [HideInInspector] public PlayerMainService playerMainService;
    [SerializeField] private SuitManageMenuIndicatorsSetService suitIndicators;
    
    [SerializeField] private ImprovementItem selectedItem;
    
    [Space] 
    
    [SerializeField] private TMP_Text improvementPointsLabel;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemLevelLabel;

    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private TMP_Text itemDescLabel;
    [SerializeField] private TMP_Text itemEffectLabel;
    [SerializeField] private TMP_Text itemCostLabel;
    
    [Space] 
    
    private AudioPoolService audioPoolService;
    
    [SerializeField] private AudioCastData onSellSound;
    [SerializeField] private AudioCastData onSellDefeatSound;
    
    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        instance = this;
    }

    private void Start()
    {
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
    }

    private void OnEnable()
    {
        ClearItemFields();
        UpdateImprovementPointsLabel();
    }

    public void SelectImprovementItem(ImprovementItem item)
    {
        selectedItem = item;

        itemIcon.sprite = item.ItemIcon;
        itemLevelLabel.text = item.ItemDecorationLevel;
        
        itemNameLabel.text = CurrentLanguageData.GetText(item.ItemNameTextId);
        itemDescLabel.text = CurrentLanguageData.GetText(item.ItemDescTextId);
        itemEffectLabel.text = CurrentLanguageData.GetText(item.ItemEffectTextId);
        itemCostLabel.text = item.ImprovementPointCost.ToString();
    }

    public void BuySelectedItem()
    {
        if (selectedItem == null || !selectedItem.IsSellPossible())
        {
            audioPoolService.CastAudio(onSellDefeatSound);    
            return;
        }
        
        selectedItem.Buy();
        
        ClearItemFields();
        UpdateImprovementPointsLabel();
        
        audioPoolService.CastAudio(onSellSound);
        suitIndicators.UpdateIndicators();
    }
    
    private void UpdateImprovementPointsLabel()
    {
        improvementPointsLabel.text = $"{playerMainService.SuitImprovementPoints}";
    }

    private void ClearItemFields()
    {
        selectedItem = null;

        itemIcon.sprite = null;
        itemLevelLabel.text = String.Empty;
        
        itemNameLabel.text = String.Empty;
        itemDescLabel.text = String.Empty;
        itemEffectLabel.text = String.Empty;
        itemCostLabel.text = String.Empty;
    }
    
    public int SuitImprovementPoints
    {
        get => playerMainService.SuitImprovementPoints;

        set => playerMainService.SuitImprovementPoints = value;
    }
}
