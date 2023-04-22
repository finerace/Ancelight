using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenuSpecial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainLevelCompleteLabel;
    
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI passageTimeLabel;
    [SerializeField] private TextMeshProUGUI enemyKilledLabel;
    [SerializeField] private TextMeshProUGUI secretsFoundLabel;

    [Space] 
    [SerializeField] private int mainLevelTextId1;
    [SerializeField] private int mainLevelTextId2;
    
    [SerializeField] private int scoreTextId;
    
    [SerializeField] private int passageTimeTextId;
    [SerializeField] private int enemyKilledTextId;
    [SerializeField] private int secretsFoundTextId;
    
    private LevelData currentLevel;
    
    private LevelSaveLoadSystem levelSaveLoadSystem;
    private PlayerMainService playerMainService;
    
    private void Awake()
    {
        levelSaveLoadSystem = FindObjectOfType<LevelSaveLoadSystem>();
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

    public void SetLevelCompleteLabels(LevelPassageService levelPassageService)
    {
        currentLevel = levelPassageService.LevelData;
        
        mainLevelCompleteLabel.text 
            = $"{CurrentLanguageData.GetText(mainLevelTextId1)} " +
              $"\n{CurrentLanguageData.GetText(levelPassageService.LevelData.LevelNameTextId)} " +
              $"\n{CurrentLanguageData.GetText(mainLevelTextId2)}";

        scoreLabel.text = $"{CurrentLanguageData.GetText(scoreTextId)} {levelPassageService.Score}";
        passageTimeLabel.text = $"{CurrentLanguageData.GetText(passageTimeTextId)} " +
                                $"{new TimeSpan(0,0,(int)levelPassageService.PassageTimeSec)}";
        enemyKilledLabel.text = $"{CurrentLanguageData.GetText(enemyKilledTextId)} {levelPassageService.EnemyKilled}";
        secretsFoundLabel.text = $"{CurrentLanguageData.GetText(secretsFoundTextId)} {levelPassageService.SecretsFound}";
    }

    public void LoadNextLevel()
    {
        var inCampaignLevelNum = LevelPassageService.FindLevelNumInCampaign(currentLevel);
        var levelCampaign = currentLevel.LevelCampaignData;
        var levelCampaignCount = levelCampaign.CampaignLevels.Length;

        var isLevelLast = inCampaignLevelNum == levelCampaignCount - 1;

        if (!isLevelLast)
        {
            var nextLevelSceneId = levelCampaign.CampaignLevels[inCampaignLevelNum + 1].LevelSceneId;
            
            levelSaveLoadSystem.LoadNextLevel(
                nextLevelSceneId,
                playerMainService,
                playerMainService.weaponsManager,
                playerMainService.weaponsBulletsManager);
        }
        else
        {
            var nextCampaign = levelCampaign.NextCampaignData;

            if (nextCampaign == null)
            {
                SceneManager.LoadScene(0);
                print("Game completed!!11! 0_0");
                return;    
            }
            
            levelSaveLoadSystem.LoadNextLevel(
                nextCampaign.CampaignLevels[0].LevelSceneId,
                playerMainService,
                playerMainService.weaponsManager,
                playerMainService.weaponsBulletsManager);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
