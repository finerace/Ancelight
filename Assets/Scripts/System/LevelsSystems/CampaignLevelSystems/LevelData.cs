using System;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data",menuName = "LevelData",order = 51)]
[Serializable]
public class LevelData : ScriptableObject
{
    [SerializeField] private CampaignData levelCampaignData;
    [SerializeField] private int levelNameTextId;
    [SerializeField] private int levelSceneId;
    [Space] 
    [SerializeField] private int levelPassageTimeHours;
    [SerializeField] private int levelPassageTimeMinutes;
    [Space]
    [SerializeField] private int levelEnemyCount;
    [SerializeField] private int levelSecretsCount;
    [Space] 
    [SerializeField] private bool isLevelUnlocked;
    [SerializeField] private int levelMaxScore;
    [SerializeField] private int levelSecretsFounded;
    [SerializeField] private TimeSpan levelMinimumPassageTime;

    public CampaignData LevelCampaignData => levelCampaignData;

    public int LevelNameTextId => levelNameTextId;

    public int LevelSceneId => levelSceneId;

    public int LevelPassageTimeHours => levelPassageTimeHours;

    public int LevelPassageTimeMinutes => levelPassageTimeMinutes;

    public int LevelEnemyCount => levelEnemyCount;

    public int LevelSecretsCount => levelSecretsCount;
    
    public bool IsLevelUnlocked => isLevelUnlocked;
    
    public int LevelMaxScore => levelMaxScore;
    
    public int LevelSecretsFounded => levelSecretsFounded;
    
    public TimeSpan LevelMinimumPassageTime => levelMinimumPassageTime;

    public void SetEndLevelData(int endLevelScore, int endLevelSecretsFound, TimeSpan endLevelPassageTime)
    {
        var isValuesTrue = endLevelScore >= 0 && endLevelSecretsFound >= 0;

        if (!isValuesTrue)
            throw new DataException("Data is less of zero!");
        
        if(endLevelScore > levelMaxScore)
            levelMaxScore = endLevelScore;
        
        if(endLevelSecretsFound > levelSecretsFounded)
            levelSecretsFounded = endLevelSecretsFound;
        
        if(endLevelPassageTime.CompareTo(levelMinimumPassageTime) < 0 || levelMinimumPassageTime == TimeSpan.Zero)
            levelMinimumPassageTime = endLevelPassageTime;
    }

    public void Unlock()
    {
        isLevelUnlocked = true;
    }
    
}