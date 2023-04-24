using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesSelectLoadDeleteService : MonoBehaviour
{
    [SerializeField] private SaveUnitData selectedSaveUnitData;
    
    [SerializeField] private TextMeshProUGUI saveNameLabel;
    [SerializeField] private TextMeshProUGUI saveLevelNameLabel;
    
    [SerializeField] private TextMeshProUGUI savePassageTimeLabel;
    [SerializeField] private TextMeshProUGUI saveScoreLabel;
    [SerializeField] private TextMeshProUGUI saveEnemyKilledLabel;
    [SerializeField] private TextMeshProUGUI saveSecretsFoundLabel;
    [SerializeField] private TextMeshProUGUI saveDateLabel;
    
    
    [SerializeField] private Image saveScreenshot;

    private const int saveNotSelectedNameTextId = 102;
    private string savesPath;
    
    [Space] 
    
    [SerializeField] private LevelSaveLoadSystem loadSystem;
    [SerializeField] private SavesFoundSpawnService savesFoundSpawnService;
    private MenuSystem menuSystem;
    
    [Space]
    
    [SerializeField] private int savePassageTimeTextId;
    [SerializeField] private int saveScoreTextId;
    [SerializeField] private int saveEnemyKilledTextId;
    [SerializeField] private int saveSecretsFoundTextId;
    [SerializeField] private int saveDateTextId;
    
    [Space]
    
    [SerializeField] private int splashWindowTextId1;
    [SerializeField] private int splashWindowTextId2;

    
    
    private void Awake()
    {
        savesPath = $"{Application.persistentDataPath}/Saves";
        saveScreenshot.color = Color.clear;

        menuSystem = FindObjectOfType<MenuSystem>();
    }

    public void SelectSave(SaveUnitData saveUnit)
    {
        selectedSaveUnitData = saveUnit;

        var levelSaveData = new LevelSaveData();
        var levelSaveDataJson = loadSystem.GetJsonLevelSaveData(saveUnit.SaveName);
        
        JsonUtility.FromJsonOverwrite(levelSaveDataJson, levelSaveData);

        var levelPassageService = new LevelPassageService();
        JsonUtility.FromJsonOverwrite(levelSaveData.SavedLevelPassageService,levelPassageService);
        
        saveNameLabel.text = selectedSaveUnitData.SaveName;
        saveLevelNameLabel.text = CurrentLanguageData.GetText(levelSaveData.LevelNameTextId);

        savePassageTimeLabel.text = $"{CurrentLanguageData.GetText(savePassageTimeTextId)} " +
                                    $"{AuxiliaryFunc.ConvertSecondsToTimeSpan((int)levelPassageService.PassageTimeSec)}";
        saveScoreLabel.text = $"{CurrentLanguageData.GetText(saveScoreTextId)} {levelPassageService.Score}";
        saveEnemyKilledLabel.text = $"{CurrentLanguageData.GetText(saveEnemyKilledTextId)} {levelPassageService.EnemyKilled}";
        saveSecretsFoundLabel.text = $"{CurrentLanguageData.GetText(saveSecretsFoundTextId)} {levelPassageService.SecretsFound}";
        
        saveDateLabel.text = $"{CurrentLanguageData.GetText(saveDateTextId)} \n"  + selectedSaveUnitData.SaveDateTimeLabel.text;
        
        
        SetScreenshot();
        void SetScreenshot()
        {
            var saveScreenshotPath = $"{savesPath}/{saveUnit.SaveName}_screenshot.png";
            if (!File.Exists(saveScreenshotPath))
                return;

            var screenshotBytes = File.ReadAllBytes(saveScreenshotPath);


            var screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RG16, false);
            screenshotTexture.LoadImage(screenshotBytes);

            var screenshotRect = new Rect(0, 0, Screen.width, Screen.height);
            var screenshotPivot = Vector2.zero;

            var screenshotSprite = Sprite.Create(screenshotTexture, screenshotRect, screenshotPivot);

            saveScreenshot.sprite = screenshotSprite;
            saveScreenshot.color = Color.white;
        }
    }

    public void LoadSelectedSave()
    {
        if(selectedSaveUnitData == null)
            return;
        
        loadSystem.StartLoadLevel(selectedSaveUnitData.SaveName);
    }

    private void DeleteSelectedSave()
    {
        if(selectedSaveUnitData == null)
            return;
        
        loadSystem.DeleteSave(selectedSaveUnitData.SaveName);
        File.Delete($"{savesPath}/{selectedSaveUnitData.SaveName}_screenshot.png");
        
        saveNameLabel.text = CurrentLanguageData.GetText(saveNotSelectedNameTextId);
        saveLevelNameLabel.text = String.Empty;

        savePassageTimeLabel.text = String.Empty;
        saveScoreLabel.text = String.Empty;
        saveEnemyKilledLabel.text = String.Empty;
        saveSecretsFoundLabel.text = String.Empty;
        saveDateLabel.text = String.Empty;

        saveScreenshot.sprite = null;
        
        savesFoundSpawnService.ReloadSaves();

        saveScreenshot.color = Color.clear;
    }

    public void CallSplashWindowDeleteSave()
    {
        var splashWindowText = 
            $"{CurrentLanguageData.GetText(splashWindowTextId1)} \"{selectedSaveUnitData.SaveName}\" {CurrentLanguageData.GetText(splashWindowTextId2)}";
        
        menuSystem.CallSplashWindow(splashWindowText,DeleteSelectedSave);
    }

}
