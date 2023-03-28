using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementSelectBuyService : MonoBehaviour
{
    [SerializeField] public static ImprovementSelectBuyService instance; 
    public PlayerMainService playerMainService;

    [SerializeField] private ImprovementItem selectedItem;
    
    [Space] 
    
    [SerializeField] private TMP_Text improvementPointsLabel;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemLevelLabel;

    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private TMP_Text itemDescLabel;
    [SerializeField] private TMP_Text itemEffectLabel;
    [SerializeField] private TMP_Text itemCostLabel;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        instance = this;
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
        
        itemNameLabel.text = item.ItemName;
        itemDescLabel.text = item.ItemDesc;
        itemEffectLabel.text = item.ItemEffect;
        itemCostLabel.text = item.ImprovementPointCost.ToString();
    }

    public void BuySelectedItem()
    {
        if(selectedItem == null || !selectedItem.IsSellPossible())
            return;
        
        selectedItem.Buy();
        
        ClearItemFields();
        UpdateImprovementPointsLabel();
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
