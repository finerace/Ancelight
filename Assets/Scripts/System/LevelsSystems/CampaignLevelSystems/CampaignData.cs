using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Campaign Data",menuName = "CampaignData",order = 51)]
[Serializable]
public class CampaignData : ScriptableObject
{
    [SerializeField] private int campaignNameTextId;
    [SerializeField] private CampaignData previousCampaignData;
    [SerializeField] private CampaignData nextCampaignData;
    [SerializeField] private LevelData[] campaignLevels;
    [SerializeField] private bool isCampaignUnlocked;

    public int CampaignNameTextId => campaignNameTextId;

    public CampaignData PreviousCampaignData => previousCampaignData;

    public CampaignData NextCampaignData => nextCampaignData;
    
    public LevelData[] CampaignLevels => campaignLevels;

    public bool IsCampaignUnlocked => isCampaignUnlocked;

    public void Unlock()
    {
        isCampaignUnlocked = true;
    }

    public void SetNewNextPreviousCampaignData(CampaignData nextCampaignData,CampaignData previousCampaignData)
    {
        this.nextCampaignData = nextCampaignData;
        this.previousCampaignData = previousCampaignData;
    }

    public void SetNewCampaignLevelsData(LevelData[] levelDatas)
    {
        campaignLevels = levelDatas;
    }
    
}
