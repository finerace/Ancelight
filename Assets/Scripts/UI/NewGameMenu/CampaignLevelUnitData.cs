using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignLevelUnitData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Image buttonDecorationLine;
    [SerializeField] private CampaignData campaignData;
    [SerializeField] private LevelData levelData;

    public void SetNewCampaignData(CampaignData campaignData)
    {
        this.campaignData = campaignData;
    }
    
    public void SetNewLevelData(LevelData levelData)
    {
        this.levelData = levelData;
    }
    
    public TextMeshProUGUI NameLabel => nameLabel;

    public Image ButtonBackground => buttonBackground;

    public CampaignData CampaignData => campaignData;

    public LevelData LevelData => levelData;

    public Image ButtonDecorationLine => buttonDecorationLine;
}
