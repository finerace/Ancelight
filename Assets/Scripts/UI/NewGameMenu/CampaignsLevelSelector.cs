using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignsLevelSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameLabel;
    [SerializeField] private TextMeshProUGUI campaignNameLabel;
    
    [SerializeField] private TextMeshProUGUI levelPassageTimeLabel;
    [SerializeField] private TextMeshProUGUI levelEnemyCountLabel;
    
    [SerializeField] private TextMeshProUGUI levelMaxScoreLabel;
    [SerializeField] private TextMeshProUGUI levelSecretsFoundLabel;
    [SerializeField] private TextMeshProUGUI levelMinimumPassageTimeLabel;

    [Space]
    
    [SerializeField] private int levelPassageTimeTextId;
    [SerializeField] private int levelEnemyCountTextId;
    [SerializeField] private int levelSecretCountTextId;
                             
    [SerializeField] private int levelMaxScoreTextId;
    [SerializeField] private int levelSecretsFoundTextId;
    [SerializeField] private int levelMinimumPassageTimeTextId;

    
    private LevelData selectedLevelData;
    private CampaignData selectedCampaignData;
    
    public void SelectNewLevel(CampaignData levelCampaignData, LevelData levelData)
    {
        selectedCampaignData = levelCampaignData;
        selectedLevelData = levelData;
        
        SetLabels();
        void SetLabels()
        {
            levelNameLabel.text = CurrentLanguageData.GetText(levelData.LevelNameTextId);
            campaignNameLabel.text = CurrentLanguageData.GetText(levelCampaignData.CampaignNameTextId);

            levelPassageTimeLabel.text = $"{CurrentLanguageData.GetText(levelPassageTimeTextId)} {AuxiliaryFunc.ConvertNumCharacters(levelData.LevelPassageTimeHours)}:" +
                                         $"{AuxiliaryFunc.ConvertNumCharacters(levelData.LevelPassageTimeMinutes)}:00";
            
            levelEnemyCountLabel.text = $"{CurrentLanguageData.GetText(levelEnemyCountTextId)} " + levelData.LevelEnemyCount.ToString();

            levelMaxScoreLabel.text = $"{CurrentLanguageData.GetText(levelMaxScoreTextId)} " + levelData.LevelMaxScore.ToString();
            levelSecretsFoundLabel.text = $"{CurrentLanguageData.GetText(levelSecretsFoundTextId)} {levelData.LevelSecretsFounded}/{levelData.LevelMaxSecretsCountInLevel}";
            levelMinimumPassageTimeLabel.text = $"{CurrentLanguageData.GetText(levelMinimumPassageTimeTextId)} " + levelData.LevelMinimumPassageTime.ToString();
        }
    }
    
    public void StartNewGame()
    {
        if(selectedLevelData == null)
            return;
        SceneManager.LoadScene(selectedLevelData.LevelSceneId);
    }
    
}
