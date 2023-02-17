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
    private LevelData currentLevel;
    
    public void SetLevelCompleteLabels(LevelPassageService levelPassageService)
    {
        currentLevel = levelPassageService.LevelData;
        
        mainLevelCompleteLabel.text = $"Level {levelPassageService.LevelData.LevelName} completed.";

        scoreLabel.text = $"Score: {levelPassageService.Score}";
        passageTimeLabel.text = $"Passage time: " +
                                $"{new TimeSpan(0,0,(int)levelPassageService.PassageTimeSec)}";
        enemyKilledLabel.text = $"Enemy killed: {levelPassageService.EnemyKilled}";
        secretsFoundLabel.text = $"Secrets found: {levelPassageService.SecretsFound}";
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

            SceneManager.LoadScene(nextLevelSceneId);
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
            
            SceneManager.LoadScene(nextCampaign.CampaignLevels[0].LevelSceneId);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
