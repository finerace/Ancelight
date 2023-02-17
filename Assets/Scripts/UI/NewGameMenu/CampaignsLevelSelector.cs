using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignsLevelSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameLabel;
    [SerializeField] private TextMeshProUGUI campaignNameLabel;
    
    [SerializeField] private TextMeshProUGUI levelPassageTimeLabel;
    [SerializeField] private TextMeshProUGUI levelEnemyCountLabel;
    [SerializeField] private TextMeshProUGUI levelSecretCountLabel;
    
    [SerializeField] private TextMeshProUGUI levelMaxScoreLabel;
    [SerializeField] private TextMeshProUGUI levelSecretsFoundLabel;
    [SerializeField] private TextMeshProUGUI levelMinimumPassageTimeLabel;
    
    private LevelData selectedLevelData;
    private CampaignData selectedCampaignData;
    
    public void SelectNewLevel(CampaignData levelCampaignData, LevelData levelData)
    {
        selectedCampaignData = levelCampaignData;
        selectedLevelData = levelData;
        
        SetLabels();
        void SetLabels()
        {
            levelNameLabel.text = levelData.LevelName;
            campaignNameLabel.text = levelCampaignData.CampaignName;

            levelPassageTimeLabel.text = $"Passage time: {AuxiliaryFunc.ConvertNumCharacters(levelData.LevelPassageTimeHours)}:" +
                                         $"{AuxiliaryFunc.ConvertNumCharacters(levelData.LevelPassageTimeMinutes)}:00";
            
            levelEnemyCountLabel.text = "Enemy count: " + levelData.LevelEnemyCount.ToString();
            levelSecretCountLabel.text = "Secrets count: " + levelData.LevelSecretsCount.ToString();

            levelMaxScoreLabel.text = "Max score: " + levelData.LevelMaxScore.ToString();
            levelSecretsFoundLabel.text = $"Founded secrets: {levelData.LevelSecretsFounded}/{levelData.LevelSecretsCount}";
            levelMinimumPassageTimeLabel.text = "Minimum passage time: " + levelData.LevelMinimumPassageTime.ToString();
        }
    }
    
    public void StartNewGame()
    {
        if(selectedLevelData == null)
            return;
        SceneManager.LoadScene(selectedLevelData.LevelSceneId);
    }
    
}
