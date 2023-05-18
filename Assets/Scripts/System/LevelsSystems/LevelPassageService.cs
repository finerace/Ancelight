using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LevelPassageService : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private int secretsFound;
    [SerializeField] private int score;
    [SerializeField] private int enemyKilled;
    [SerializeField] private float passageTimeSec;
    
    [SerializeField] private MenuSystem menuSystem;

    public event Action OnSecretFind;
    public event Action<int> OnScoreAdd;

    public LevelData LevelData => levelData;

    public int SecretsFound => secretsFound;

    public int Score => score;

    public int EnemyKilled => enemyKilled;

    public float PassageTimeSec => passageTimeSec;

    private void Awake()
    {
        if (menuSystem == null)
            menuSystem = FindObjectOfType<MenuSystem>();
    }

    private void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }

    public void Load(LevelPassageService levelPassageService)
    {
        secretsFound = levelPassageService.secretsFound;
        score = levelPassageService.score;
        enemyKilled = levelPassageService.enemyKilled;
        passageTimeSec = levelPassageService.passageTimeSec;
    }
    
    public void CompleteLevel()
    {
        var levelCampaignData = levelData.LevelCampaignData;

        levelData.SetEndLevelData(score,secretsFound,AuxiliaryFunc.ConvertSecondsToTimeSpan((int)passageTimeSec));
        print($"Level {levelData.LevelNameTextId} completed!");

        UnlockNextLevel();
        void UnlockNextLevel()
        {
            var completedLevelNumInCampaign = FindLevelNumInCampaign(levelData);
            var isLevelLastInCampaign = completedLevelNumInCampaign == levelCampaignData.CampaignLevels.Length - 1;

            if (!isLevelLastInCampaign)
                levelCampaignData.CampaignLevels[completedLevelNumInCampaign + 1].Unlock();
            else if (levelCampaignData.NextCampaignData != null)
                levelCampaignData.NextCampaignData.Unlock();
        }
        
        menuSystem.OpenLocalMenu("LevelCompleteMenu");
        menuSystem.isBackActionLock = true;
        
        menuSystem.CurrentMenuData.menu.GetComponent
            <LevelCompleteMenuSpecial>().SetLevelCompleteLabels(this);

        CampaignsLevelsSaveUtility.Save(levelCampaignData);
    }

    public void AddScore(int scoreValue)
    {
        if (scoreValue < 0)
            throw new ArgumentException("Added score not be could less than a zero!");

        score += scoreValue;
        OnScoreAdd?.Invoke(score);
    }

    public void AddDiedEnemy()
    {
        enemyKilled += 1;
    }

    public void SecretFind()
    {
        secretsFound += 1;
        
        OnSecretFind?.Invoke();
    }
    
    public static int FindLevelNumInCampaign(LevelData levelData)
    {
        for (var i = 0; i < levelData.LevelCampaignData.CampaignLevels.Length; i++)
        {
            var campaignLevelData = levelData.LevelCampaignData.CampaignLevels[i];

            if (campaignLevelData.Equals(levelData))
                return i;
        }

        throw new DataException("Is a impossible exception.");
    }
    
    public void Update()
    {
        passageTimeSec += Time.deltaTime;
    }
}

[Serializable]
public class CampaignsLevelsSaveUtility
{
    [SerializeField] private CampaignSaveData[] allCampaignsSaveDatas;
    [SerializeField] private LevelSaveData[] allLevelsSaveDatas;

    public CampaignSaveData[] AllCampaignsSaveDatas => allCampaignsSaveDatas;
    public LevelSaveData[] AllLevelsSaveDatas => allLevelsSaveDatas;

    private void SetNewSaveData(CampaignSaveData[] campaignSaveDatas, LevelSaveData[] levelSaveDatas)
    {
        allCampaignsSaveDatas = campaignSaveDatas;
        allLevelsSaveDatas = levelSaveDatas;
    }
    
    [Serializable]
    public class CampaignSaveData
    {
        public string campaignName;
        public bool isCampaignUnlocked;

        public string CampaignName => campaignName;

        public bool IsCampaignUnlocked => isCampaignUnlocked;

        public CampaignSaveData(CampaignData campaignData)
        {
            campaignName = CurrentLanguageData.GetText(campaignData.CampaignNameTextId);
            isCampaignUnlocked = campaignData.IsCampaignUnlocked;
        }

        public void OverwriteCampaignData(CampaignData campaignData)
        {
            if(isCampaignUnlocked)
                campaignData.Unlock();
        }
        
    }
    
    [Serializable]
    public class LevelSaveData
    {
        [SerializeField] private int levelNameTextId;
        [SerializeField] private int levelSceneId;
        [Space]
        [SerializeField] private bool isLevelUnlocked;
        [SerializeField] private int levelMaxScore;
        [SerializeField] private int levelSecretsFounded;
        [SerializeField] private TimeSpan levelMinimumPassageTime;

        public int LevelNameTextId => levelNameTextId;

        public int LevelSceneId => levelSceneId;

        public bool IsLevelUnlocked => isLevelUnlocked;

        public int LevelMaxScore => levelMaxScore;

        public int LevelSecretsFounded => levelSecretsFounded;

        public TimeSpan LevelMinimumPassageTime => levelMinimumPassageTime;

        public LevelSaveData(LevelData levelData)
        {
            levelNameTextId = levelData.LevelNameTextId;
            levelSceneId = levelData.LevelSceneId;

            isLevelUnlocked = levelData.IsLevelUnlocked;
            levelMaxScore = levelData.LevelMaxScore;
            levelSecretsFounded = levelData.LevelSecretsFounded;
            levelMinimumPassageTime = levelData.LevelMinimumPassageTime;
        }

        public void OverwriteLevelData(LevelData levelData)
        {
            levelData.SetEndLevelData(levelMaxScore, levelSecretsFounded, levelMinimumPassageTime);
            
            if(isLevelUnlocked)
                levelData.Unlock();
        }
    }

    private static (CampaignData[] campaignDatas, LevelData[] levelDatas) GetAllCampaignsAndLevelsData(CampaignData campaignData)
    {
        CampaignData GetFirstCampaignData(CampaignData campaignData)
        {
            if (campaignData.PreviousCampaignData == null)
                return campaignData;
            return GetFirstCampaignData(campaignData.PreviousCampaignData);
        }
        var firstCampaign = GetFirstCampaignData(campaignData);

        var resultAllCampaigns = new List<CampaignData>();
        var resultAllLevels = new List<LevelData>();

        void SetAllCampaignsAndLevels(CampaignData firstCampaignData)
        {
            resultAllCampaigns.Add(firstCampaignData);

            foreach (var levelData in firstCampaignData.CampaignLevels)
            {
                resultAllLevels.Add(levelData);
            }
                
            if(firstCampaignData.NextCampaignData == null)
                return;
                
            SetAllCampaignsAndLevels(firstCampaignData.NextCampaignData);
        }
        SetAllCampaignsAndLevels(firstCampaign);
            
        return (resultAllCampaigns.ToArray(),resultAllLevels.ToArray());
    }

    public static void Save(CampaignData campaignData)
    {
        CampaignData[] allCampaigns;
        LevelData[] allLevels;

        (allCampaigns, allLevels) = GetAllCampaignsAndLevelsData(campaignData);

        (CampaignSaveData[] campaignSaveData, LevelSaveData[] levelSaveData) 
            GetToSaveVersionsOfCampaignsAndLevels(CampaignData[] campaignDatas,LevelData[] levelDatas)
        {
            var resultCampaignSaveDataList = new List<CampaignSaveData>();
            var resultLevelSaveDataList = new List<LevelSaveData>();

            foreach (var campaignData in campaignDatas)
            {
                var newCampaignSaveData = new CampaignSaveData(campaignData);
                resultCampaignSaveDataList.Add(newCampaignSaveData);
            }
            foreach (var levelData in levelDatas)
            {
                var newLevelSaveData = new LevelSaveData(levelData);
                resultLevelSaveDataList.Add(newLevelSaveData);
            }
            
            return (resultCampaignSaveDataList.ToArray(),resultLevelSaveDataList.ToArray());
        }

        (CampaignSaveData[] campaignSaveDatas, LevelSaveData[] levelSaveDatas) =
            GetToSaveVersionsOfCampaignsAndLevels(allCampaigns, allLevels);
        
        var mainSave = new CampaignsLevelsSaveUtility();
        mainSave.SetNewSaveData(campaignSaveDatas,levelSaveDatas);

        var savePath = Application.persistentDataPath;
        var toFilePath = $"{savePath}/GameProgress.vds";
        
        var binaryFormatter = 
            new BinaryFormatter();
        var fileStream = 
            File.Create(toFilePath);
            
        binaryFormatter.Serialize(fileStream,mainSave);
        
        fileStream.Close();
    }

    public static void Load(CampaignData campaignData)
    {
        CampaignData[] allCampaigns;
        LevelData[] allLevels;

        (allCampaigns, allLevels) = GetAllCampaignsAndLevelsData(campaignData);

        CampaignsLevelsSaveUtility GetProgressSaveFile(out bool isLoadSuccessful)
        {
            var toFilePath = $"{Application.persistentDataPath}/GameProgress.vds";
            var saveFileIsExists = File.Exists(toFilePath);

            if (!saveFileIsExists)
            {
                isLoadSuccessful = false;
                return null;
            }

            var binaryFormatter = 
                new BinaryFormatter();
            var saveFileStream = 
                File.Open(toFilePath,FileMode.Open);

            var jsonLevelData = 
                (CampaignsLevelsSaveUtility)binaryFormatter.Deserialize(saveFileStream);
        
            saveFileStream.Close();

            isLoadSuccessful = true;
            return jsonLevelData;
        }
        var mainSave = GetProgressSaveFile(out bool isLoadSaveFileSuccessful );

        if(!isLoadSaveFileSuccessful)
            return;

        void LoadCampaigns(CampaignData[] campaignDatas, CampaignSaveData[] campaignSaveDatas)
        {
            foreach (var campaignData in campaignDatas)
            {
                foreach (var campaignSaveData in campaignSaveDatas)
                {
                    campaignSaveData.OverwriteCampaignData(campaignData);
                }
            }    
        }
        
        void LoadLevels(LevelData[] levelDatas,LevelSaveData[] levelSaveDatas)
        {
            foreach (var levelData in levelDatas)
            {
                foreach (var levelSaveData in levelSaveDatas)
                {
                    var isLevelsEqual = levelData.LevelNameTextId == levelSaveData.LevelNameTextId ||
                                        levelData.LevelSceneId == levelSaveData.LevelSceneId;
                    if(isLevelsEqual)
                        levelSaveData.OverwriteLevelData(levelData);
                }    
            }
        }

        LoadCampaigns(allCampaigns,mainSave.allCampaignsSaveDatas);
        LoadLevels(allLevels,mainSave.allLevelsSaveDatas);

    }
}