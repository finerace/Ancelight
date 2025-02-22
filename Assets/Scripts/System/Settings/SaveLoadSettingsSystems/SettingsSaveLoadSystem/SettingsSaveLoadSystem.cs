using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSaveLoadSystem : MonoBehaviour
{
    [SerializeField] private Slider soundsMasterVolume;
    [SerializeField] private Slider soundsMusicVolume;
    [SerializeField] private Slider soundsEffectsVolume;
    [SerializeField] private Slider soundsAmbientVolume;
    [SerializeField] private Slider soundsUiVolume;

    
    [SerializeField] private TMP_Dropdown soundsMusicQuality;
    [SerializeField] private Slider soundsMaxSoundsCount;

    [Space] 
    
    [SerializeField] private TMP_Dropdown textureQuality;
    [SerializeField] private TMP_Dropdown anisotropicTextureQuality;

    [SerializeField] private TMP_Dropdown shadowsResolutionQuality;
    [SerializeField] private Slider shadowDistance;
    [SerializeField] private Toggle softShadows;
    [SerializeField] private Toggle ssao;
    
    [SerializeField] private TMP_Dropdown msaaQuality;
    [SerializeField] private TMP_Dropdown antiAliasingQuality;

    [SerializeField] private TMP_Dropdown grassQuality;
    [SerializeField] private Slider grassDrawingDistance;
    [SerializeField] private TMP_Dropdown plantsQuality;
    [SerializeField] private TMP_Dropdown enemyPartsQuality;
    [SerializeField] private Slider timeOfDestructionOfParts;

    [SerializeField] private Slider drawDistanceCoof;
    [SerializeField] private Toggle vsync;

    [SerializeField] private Slider fieldOfView;
    [SerializeField] private TMP_Dropdown screenResolution;
    [SerializeField] private TMP_Dropdown screenFormat;
    [SerializeField] private TMP_Dropdown language;

    [Space] 
    
    [SerializeField] private InputButtonField forwardButton;
    [SerializeField] private InputButtonField backButton;
    [SerializeField] private InputButtonField leftButton;
    [SerializeField] private InputButtonField rightButton;
    [SerializeField] private InputButtonField jumpButton;
    [SerializeField] private InputButtonField crouchButton;
    [SerializeField] private InputButtonField dashButton;
    
    [SerializeField] private InputButtonField shootingButton;
    [SerializeField] private InputButtonField useProtection;
    [SerializeField] private InputButtonField nextWeaponButton;
    [SerializeField] private InputButtonField previousButton;
    [SerializeField] private InputButtonField useHookButton;
    [SerializeField] private InputButtonField useItemButton;
    [SerializeField] private InputButtonField nextAbilityButton;
    [SerializeField] private InputButtonField previousAbilityButton;
    [SerializeField] private InputButtonField useAbilityButton;
    [SerializeField] private InputButtonField useEnemyCleanerButton;

    [SerializeField] private InputButtonField openSuitManageMenuButton;
    [SerializeField] private InputButtonField saveLevelButton;
    
    [SerializeField] private Slider mouseSensitivity;

    private const string saveFileName = "SettingsSave.bob";
    private string saveFilePath;

    [Space] 
    
    [SerializeField] private SettingsData defaultSettings;

    public void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/";
    }

    public void SaveSettings(bool isFirstSave = false)
    {
        SettingsData.SettingsSoundsData soundsData;
        SettingsData.SettingsGraphicsData graphicsData;
        SettingsData.SettingsControlsData controlsData;

        (soundsData, graphicsData, controlsData) = CreateSettingsData();

        var mainSettingsData = new SettingsData(soundsData, graphicsData, controlsData);

        if (isFirstSave)
        {
            mainSettingsData = defaultSettings;   
            SaveToFile();
            LoadSettings();
            
            return;
        }

        SaveToFile();
        
        void SaveToFile()
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(saveFilePath + saveFileName);
            
            binaryFormatter.Serialize(fileStream,mainSettingsData);
            
            fileStream.Close();
        }
    }

    public void LoadSettings()
    {
        LoadInputElementsNewValue(GetSavedSettings());
    }

    public SettingsData GetSavedSettings()
    {
        var saveFileIsExists = File.Exists(saveFilePath + saveFileName);

        if (!saveFileIsExists)
        {
            SaveSettings(true);
        }

        var binaryFormatter = new BinaryFormatter();
        var saveFileStream = File.Open(saveFilePath+saveFileName,FileMode.Open);

        var loadSettingsData = (SettingsData)binaryFormatter.Deserialize(saveFileStream);
        
        saveFileStream.Close();

        return loadSettingsData;
    }

    (SettingsData.SettingsSoundsData soundsData,
            SettingsData.SettingsGraphicsData graphicsData,
            SettingsData.SettingsControlsData controlsData) CreateSettingsData()
        {
            var soundsSettingsData =
            new SettingsData.SettingsSoundsData(
                soundsMasterVolume.value,
                soundsMusicVolume.value,
                soundsEffectsVolume.value,
                soundsAmbientVolume.value,
                soundsUiVolume.value,
                soundsMusicQuality.value,
                soundsMaxSoundsCount.value);

            var graphicsSettingsData =
                new SettingsData.SettingsGraphicsData(
                    textureQuality.value,
                    anisotropicTextureQuality.value,
                    shadowsResolutionQuality.value,
                    shadowDistance.value,
                    softShadows.isOn,
                    ssao.isOn,
                    msaaQuality.value,
                    antiAliasingQuality.value,
                    grassQuality.value,
                    grassDrawingDistance.value,
                    plantsQuality.value,
                    enemyPartsQuality.value,
                    timeOfDestructionOfParts.value,
                    drawDistanceCoof.value,
                    vsync.isOn,
                    fieldOfView.value,
                    screenResolution.value,
                    screenFormat.value,
                    language.value);

            var controlsSettingsData =
                new SettingsData.SettingsControlsData(
                    forwardButton.AssignedButtonKeyCode,
                    forwardButton.AssignedButtonMouseWheelMove,
                    backButton.AssignedButtonKeyCode,
                    backButton.AssignedButtonMouseWheelMove,
                    leftButton.AssignedButtonKeyCode,
                    leftButton.AssignedButtonMouseWheelMove,
                    rightButton.AssignedButtonKeyCode,
                    rightButton.AssignedButtonMouseWheelMove,
                    jumpButton.AssignedButtonKeyCode,
                    jumpButton.AssignedButtonMouseWheelMove,
                    crouchButton.AssignedButtonKeyCode,
                    crouchButton.AssignedButtonMouseWheelMove,
                    dashButton.AssignedButtonKeyCode,
                    dashButton.AssignedButtonMouseWheelMove,
                    shootingButton.AssignedButtonKeyCode,
                    shootingButton.AssignedButtonMouseWheelMove,
                    useProtection.AssignedButtonKeyCode,
                    useProtection.AssignedButtonMouseWheelMove,
                    nextWeaponButton.AssignedButtonKeyCode,
                    nextWeaponButton.AssignedButtonMouseWheelMove,
                    previousButton.AssignedButtonKeyCode,
                    previousButton.AssignedButtonMouseWheelMove,
                    useHookButton.AssignedButtonKeyCode,
                    useHookButton.AssignedButtonMouseWheelMove,
                    useItemButton.AssignedButtonKeyCode,
                    useItemButton.AssignedButtonMouseWheelMove,
                    nextAbilityButton.AssignedButtonKeyCode,
                    nextAbilityButton.AssignedButtonMouseWheelMove,
                    previousAbilityButton.AssignedButtonKeyCode,
                    previousAbilityButton.AssignedButtonMouseWheelMove,
                    useAbilityButton.AssignedButtonKeyCode,
                    useAbilityButton.AssignedButtonMouseWheelMove,
                    useEnemyCleanerButton.AssignedButtonKeyCode,
                    useEnemyCleanerButton.AssignedButtonMouseWheelMove,
                    openSuitManageMenuButton.AssignedButtonKeyCode,
                    openSuitManageMenuButton.AssignedButtonMouseWheelMove,
                    saveLevelButton.AssignedButtonKeyCode,
                    saveLevelButton.AssignedButtonMouseWheelMove,
                    mouseSensitivity.value);
            
            return (soundsSettingsData,graphicsSettingsData,controlsSettingsData);
        }

    private void LoadInputElementsNewValue(SettingsData settingsData)
    {
        LoadSoundsSettings();

        LoadGraphicsSettings();
        
        LoadControlsSettings();
        
        void LoadSoundsSettings()
        {
            soundsMasterVolume.value = settingsData.SoundsSettingsData.MasterVolumeValue;
            soundsMusicVolume.value = settingsData.SoundsSettingsData.MusicVolumeValue;
            soundsEffectsVolume.value = settingsData.SoundsSettingsData.EffectsVolumeValue;
            soundsAmbientVolume.value = settingsData.SoundsSettingsData.AmbientVolumeValue;
            soundsUiVolume.value = settingsData.SoundsSettingsData.UiVolumeValue;
            
            soundsMusicQuality.value = settingsData.SoundsSettingsData.MusicQuality;
            soundsMaxSoundsCount.value = settingsData.SoundsSettingsData.MaxSoundsCountValue;
        }

        void LoadGraphicsSettings()
        {
            textureQuality.value = settingsData.GraphicsSettingsData.TexturesQuality;
            anisotropicTextureQuality.value = settingsData.GraphicsSettingsData.AnisotropicTexturesQuality;

            shadowsResolutionQuality.value = settingsData.GraphicsSettingsData.ShadowsResolutionQuality;
            shadowDistance.value = settingsData.GraphicsSettingsData.ShadowDistance;
            softShadows.isOn = settingsData.GraphicsSettingsData.SoftShadows;
            ssao.isOn = settingsData.GraphicsSettingsData.Ssao;
            
            msaaQuality.value = settingsData.GraphicsSettingsData.MSAAQuality;
            antiAliasingQuality.value = settingsData.GraphicsSettingsData.AntiAliasingQuality;

            grassQuality.value = settingsData.GraphicsSettingsData.GrassQuality;
            grassDrawingDistance.value = settingsData.GraphicsSettingsData.GrassDrawingDistance;

            plantsQuality.value = settingsData.GraphicsSettingsData.PlantsQuality;
            enemyPartsQuality.value = settingsData.GraphicsSettingsData.EnemyPartsQuality;
            timeOfDestructionOfParts.value = settingsData.GraphicsSettingsData.TimeOfDestructionOfParts;

            drawDistanceCoof.value = settingsData.GraphicsSettingsData.DrawingDistanceCoof;
            vsync.isOn = settingsData.GraphicsSettingsData.Vsync;

            fieldOfView.value = settingsData.GraphicsSettingsData.FieldOfView;
            screenResolution.value = settingsData.GraphicsSettingsData.ScreenResolution;
            screenFormat.value = settingsData.GraphicsSettingsData.ScreenFormat;
            language.value = settingsData.GraphicsSettingsData.Language;
        }

        void LoadControlsSettings()
        {
            if (settingsData.ControlsSettingsData.ForwardButton != KeyCode.None)
            {
                forwardButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.ForwardButton;
            }
            else
            {
                forwardButton.AssignedButtonKeyCode = KeyCode.None;
                forwardButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.ForwardButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.BackButton != KeyCode.None)
            {
                backButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.BackButton;
            }
            else
            {
                backButton.AssignedButtonKeyCode = KeyCode.None;
                backButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.BackButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.LeftButton != KeyCode.None)
            {
                leftButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.LeftButton;
            }
            else
            {
                leftButton.AssignedButtonKeyCode = KeyCode.None;
                leftButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.LeftButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.RightButton != KeyCode.None)
            {
                rightButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.RightButton;
            }
            else
            {
                rightButton.AssignedButtonKeyCode = KeyCode.None;
                rightButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.RightButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.JumpButton != KeyCode.None)
            {
                jumpButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.JumpButton;
            }
            else
            {
                jumpButton.AssignedButtonKeyCode = KeyCode.None;
                jumpButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.JumpButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.CrouchButton != KeyCode.None)
            {
                crouchButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.CrouchButton;
            }
            else
            {
                crouchButton.AssignedButtonKeyCode = KeyCode.None;
                crouchButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.CrouchButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.DashButton != KeyCode.None)
            {
                dashButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.DashButton;
            }
            else
            {
                dashButton.AssignedButtonKeyCode = KeyCode.None;
                dashButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.DashButtonMouseWheelMove;
            }

            
            if (settingsData.ControlsSettingsData.ShootingButton != KeyCode.None)
            {
                shootingButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.ShootingButton;
            }
            else
            {
                shootingButton.AssignedButtonKeyCode = KeyCode.None;
                shootingButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.ShootingButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.UseProtectionButton != KeyCode.None)
            {
                useProtection.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseProtectionButton;
            }
            else
            {
                useProtection.AssignedButtonKeyCode = KeyCode.None;
                useProtection.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseProtectionButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.NextWeaponButton != KeyCode.None)
            {
                nextWeaponButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.NextWeaponButton;
            }
            else
            {
                nextWeaponButton.AssignedButtonKeyCode = KeyCode.None;
                nextWeaponButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.NextButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.PreviousWeaponButton != KeyCode.None)
            {
                previousButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.PreviousWeaponButton;
            }
            else
            {
                previousButton.AssignedButtonKeyCode = KeyCode.None;
                previousButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.PreviousButtonMouseWheelMove;
            }

            if (settingsData.ControlsSettingsData.UseHookButton != KeyCode.None)
            {
                useHookButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseHookButton;
            }
            else
            {
                useHookButton.AssignedButtonKeyCode = KeyCode.None;
                useHookButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseHookButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.UseItemButton != KeyCode.None)
            {
                useItemButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseItemButton;
            }
            else
            {
                useItemButton.AssignedButtonKeyCode = KeyCode.None;
                useItemButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseItemButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.NextAbilityButton != KeyCode.None)
            {
                nextAbilityButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.NextAbilityButton;
            }
            else
            {
                nextAbilityButton.AssignedButtonKeyCode = KeyCode.None;
                nextAbilityButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.NextAbilityButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.PreviousAbilityButton != KeyCode.None)
            {
                previousAbilityButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.PreviousAbilityButton;
            }
            else
            {
                previousAbilityButton.AssignedButtonKeyCode = KeyCode.None;
                previousAbilityButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.PreviousAbilityButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.UseAbilityButton != KeyCode.None)
            {
                useAbilityButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseAbilityButton;
            }
            else
            {
                useAbilityButton.AssignedButtonKeyCode = KeyCode.None;
                useAbilityButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseAbilityButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.UseEnemyCleanerButton != KeyCode.None)
            {
                useEnemyCleanerButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseEnemyCleanerButton;
            }
            else
            {
                useEnemyCleanerButton.AssignedButtonKeyCode = KeyCode.None;
                useEnemyCleanerButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseEnemyCleanerButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.OpenSuitManageMenuButton != KeyCode.None)
            {
                openSuitManageMenuButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.OpenSuitManageMenuButton;
            }
            else
            {
                openSuitManageMenuButton.AssignedButtonKeyCode = KeyCode.None;
                openSuitManageMenuButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.OpenSuitManageMenuButtonMouseWheelMove;
            }
            
            if (settingsData.ControlsSettingsData.SaveLevelButton != KeyCode.None)
            {
                saveLevelButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.SaveLevelButton;
            }
            else
            {
                saveLevelButton.AssignedButtonKeyCode = KeyCode.None;
                saveLevelButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.SaveLevelButtonMouseWheelMove;
            }

            mouseSensitivity.value = settingsData.ControlsSettingsData.MouseSensitivity;
        }
    }
}

[Serializable]
public class SettingsData
{
    [SerializeField] private SettingsSoundsData soundsSettingsData;

    [SerializeField] private SettingsGraphicsData graphicsSettingsData;

    [SerializeField] private SettingsControlsData controlsSettingsData;

    public SettingsSoundsData SoundsSettingsData => soundsSettingsData;

    public SettingsGraphicsData GraphicsSettingsData => graphicsSettingsData;

    public SettingsControlsData ControlsSettingsData => controlsSettingsData;

    public SettingsData(
        SettingsSoundsData soundsSettingsData, 
        SettingsGraphicsData graphicsSettingsData, 
        SettingsControlsData controlsSettingsData)
    {
        this.soundsSettingsData = soundsSettingsData;
        this.graphicsSettingsData = graphicsSettingsData;
        this.controlsSettingsData = controlsSettingsData;
    }

    [Serializable]
    public class SettingsSoundsData
    {
        [SerializeField] private float masterVolumeValue;
        [SerializeField] private float musicVolumeValue;
        [SerializeField] private float effectsVolumeValue;
        [SerializeField] private float ambientVolumeValue;
        [SerializeField] private float uiVolumeValue;
        [SerializeField] private int musicQuality;
        [SerializeField] private float maxSoundsCountValue;

        public float MasterVolumeValue => masterVolumeValue;
        public float MusicVolumeValue => musicVolumeValue;
        public float EffectsVolumeValue => effectsVolumeValue;
        public int MusicQuality => musicQuality;
        public float MaxSoundsCountValue => maxSoundsCountValue;
        public float AmbientVolumeValue => ambientVolumeValue;
        public float UiVolumeValue => uiVolumeValue;
        
        public SettingsSoundsData(float masterVolumeValue, float musicVolumeValue, float effectsVolumeValue, float ambientVolumeValue, float uiVolumeValue, int musicQuality, float maxSoundsCountValue)
        {
            this.masterVolumeValue = masterVolumeValue;
            this.musicVolumeValue = musicVolumeValue;
            this.effectsVolumeValue = effectsVolumeValue;
            this.ambientVolumeValue = ambientVolumeValue;
            this.uiVolumeValue = uiVolumeValue;
            this.musicQuality = musicQuality;
            this.maxSoundsCountValue = maxSoundsCountValue;
        }
    }
    [Serializable]
    public class SettingsGraphicsData
    {
        [SerializeField] private int texturesQuality;
        [SerializeField] private int anisotropicTexturesQuality;

        [SerializeField] private int shadowsResolutionQuality;
        [SerializeField] private float shadowDistance;
        [SerializeField] private bool softShadows;
        [SerializeField] private bool ssao;

        [SerializeField] private int msaaQuality;
        [SerializeField] private int antiAliasingQuality;

        [SerializeField] private int grassQuality;
        [SerializeField] private float grassDrawingDistance;

        [SerializeField] private int plantsQuality;
        [SerializeField] private int enemyPartsQuality;
        [SerializeField] private float timeOfDestructionOfParts;

        [SerializeField] private float drawingDistanceCoof;
        [SerializeField] private bool vsync;

        [SerializeField] private float fieldOfView;
        [SerializeField] private int screenResolution;
        [SerializeField] private int screenFormat;
        [SerializeField] private int language;
        
        public int TexturesQuality => texturesQuality;

        public int AnisotropicTexturesQuality => anisotropicTexturesQuality;

        public int ShadowsResolutionQuality => shadowsResolutionQuality;

        public float ShadowDistance => shadowDistance;
        
        public bool SoftShadows => softShadows;

        public bool Ssao => ssao;

        public int MSAAQuality => msaaQuality;

        public int AntiAliasingQuality => antiAliasingQuality;

        public int GrassQuality => grassQuality;

        public float GrassDrawingDistance => grassDrawingDistance;

        public int PlantsQuality => plantsQuality;

        public int EnemyPartsQuality => enemyPartsQuality;

        public float TimeOfDestructionOfParts => timeOfDestructionOfParts;

        public float DrawingDistanceCoof => drawingDistanceCoof;

        public bool Vsync => vsync;

        public float FieldOfView => fieldOfView;

        public int ScreenResolution => screenResolution;

        public int ScreenFormat => screenFormat;

        public int Language => language;

        public SettingsGraphicsData(int texturesQuality, int anisotropicTexturesQuality, int shadowsResolutionQuality, float shadowDistance, bool softShadows, bool ssao, int msaaQuality, int antiAliasingQuality, int grassQuality, float grassDrawingDistance, int plantsQuality, int enemyPartsQuality, float timeOfDestructionOfParts, float drawingDistanceCoof, bool vsync, float fieldOfView, int screenResolution, int screenFormat, int language)
        {
            this.texturesQuality = texturesQuality;
            this.anisotropicTexturesQuality = anisotropicTexturesQuality;
            this.shadowsResolutionQuality = shadowsResolutionQuality;
            this.shadowDistance = shadowDistance;
            this.softShadows = softShadows;
            this.ssao = ssao;
            this.msaaQuality = msaaQuality;
            this.antiAliasingQuality = antiAliasingQuality;
            this.grassQuality = grassQuality;
            this.grassDrawingDistance = grassDrawingDistance;
            this.plantsQuality = plantsQuality;
            this.enemyPartsQuality = enemyPartsQuality;
            this.timeOfDestructionOfParts = timeOfDestructionOfParts;
            this.drawingDistanceCoof = drawingDistanceCoof;
            this.vsync = vsync;
            this.fieldOfView = fieldOfView;
            this.screenResolution = screenResolution;
            this.screenFormat = screenFormat;
            this.language = language;
        }
    }
    [Serializable]
    public class SettingsControlsData
    {
        [SerializeField] private KeyCode forwardButton;
        [SerializeField] private bool forwardButtonMouseWheelMove;

        [SerializeField] private KeyCode backButton;
        [SerializeField] private bool backButtonMouseWheelMove;

        [SerializeField] private KeyCode leftButton;
        [SerializeField] private bool leftButtonMouseWheelMove;

        [SerializeField] private KeyCode rightButton;
        [SerializeField] private bool rightButtonMouseWheelMove;

        [SerializeField] private KeyCode jumpButton;
        [SerializeField] private bool jumpButtonMouseWheelMove;

        [SerializeField] private KeyCode crouchButton;
        [SerializeField] private bool crouchButtonMouseWheelMove;

        [SerializeField] private KeyCode dashButton;
        [SerializeField] private bool dashButtonMouseWheelMove;
        
        [SerializeField] private KeyCode shootingButton;
        [SerializeField] private bool shootingButtonMouseWheelMove;

        [SerializeField] private KeyCode useProtectionButton;
        [SerializeField] private bool useProtectionButtonMouseWheelMove;

        [SerializeField] private KeyCode nextWeaponButton;
        [SerializeField] private bool nextButtonMouseWheelMove;

        [SerializeField] private KeyCode previousWeaponButton;
        [SerializeField] private bool previousButtonMouseWheelMove;

        [SerializeField] private KeyCode useHookButton;
        [SerializeField] private bool useHookButtonMouseWheelMove;

        [SerializeField] private KeyCode useItemButton;
        [SerializeField] private bool useItemButtonMouseWheelMove;

        [SerializeField] private KeyCode nextAbilityButton;
        [SerializeField] private bool nextAbilityButtonMouseWheelMove;

        [SerializeField] private KeyCode previousAbilityButton;
        [SerializeField] private bool previousAbilityButtonMouseWheelMove;

        [SerializeField] private KeyCode useAbilityButton;
        [SerializeField] private bool useAbilityButtonMouseWheelMove;

        [SerializeField] private KeyCode useEnemyCleanerButton;
        [SerializeField] private bool useEnemyCleanerButtonMouseWheelMove;

        [SerializeField] private KeyCode openSuitManageMenuButton;
        [SerializeField] private bool openSuitManageMenuButtonMouseWheelMove;

        [SerializeField] private KeyCode saveLevelButton;
        [SerializeField] private bool saveLevelButtonMouseWheelMove;

        [SerializeField] private float mouseSensitivity;

        public KeyCode ForwardButton => forwardButton;

        public bool ForwardButtonMouseWheelMove => forwardButtonMouseWheelMove;

        public KeyCode BackButton => backButton;

        public bool BackButtonMouseWheelMove => backButtonMouseWheelMove;

        public KeyCode LeftButton => leftButton;

        public bool LeftButtonMouseWheelMove => leftButtonMouseWheelMove;

        public KeyCode RightButton => rightButton;

        public bool RightButtonMouseWheelMove => rightButtonMouseWheelMove;

        public KeyCode JumpButton => jumpButton;

        public bool JumpButtonMouseWheelMove => jumpButtonMouseWheelMove;

        public KeyCode CrouchButton => crouchButton;

        public bool CrouchButtonMouseWheelMove => crouchButtonMouseWheelMove;

        public KeyCode DashButton => dashButton;

        public bool DashButtonMouseWheelMove => dashButtonMouseWheelMove;

        public KeyCode ShootingButton => shootingButton;

        public bool ShootingButtonMouseWheelMove => shootingButtonMouseWheelMove;

        public KeyCode UseProtectionButton => useProtectionButton;

        public bool UseProtectionButtonMouseWheelMove => useProtectionButtonMouseWheelMove;

        public KeyCode NextWeaponButton => nextWeaponButton;

        public bool NextButtonMouseWheelMove => nextButtonMouseWheelMove;

        public KeyCode PreviousWeaponButton => previousWeaponButton;

        public bool PreviousButtonMouseWheelMove => previousButtonMouseWheelMove;

        public KeyCode UseHookButton => useHookButton;

        public bool UseHookButtonMouseWheelMove => useHookButtonMouseWheelMove;

        public KeyCode UseItemButton => useItemButton;

        public bool UseItemButtonMouseWheelMove => useItemButtonMouseWheelMove;

        public KeyCode NextAbilityButton => nextAbilityButton;

        public bool NextAbilityButtonMouseWheelMove => nextAbilityButtonMouseWheelMove;

        public KeyCode PreviousAbilityButton => previousAbilityButton;

        public bool PreviousAbilityButtonMouseWheelMove => previousAbilityButtonMouseWheelMove;

        public KeyCode UseAbilityButton => useAbilityButton;

        public bool UseAbilityButtonMouseWheelMove => useAbilityButtonMouseWheelMove;

        public float MouseSensitivity => mouseSensitivity;

        public KeyCode UseEnemyCleanerButton => useEnemyCleanerButton;

        public bool UseEnemyCleanerButtonMouseWheelMove => useEnemyCleanerButtonMouseWheelMove;

        public KeyCode OpenSuitManageMenuButton => openSuitManageMenuButton;

        public bool OpenSuitManageMenuButtonMouseWheelMove => openSuitManageMenuButtonMouseWheelMove;

        public KeyCode SaveLevelButton => saveLevelButton;

        public bool SaveLevelButtonMouseWheelMove => saveLevelButtonMouseWheelMove;

        public SettingsControlsData(KeyCode forwardButton, bool forwardButtonMouseWheelMove, KeyCode backButton, bool backButtonMouseWheelMove, KeyCode leftButton, bool leftButtonMouseWheelMove, KeyCode rightButton, bool rightButtonMouseWheelMove, KeyCode jumpButton, bool jumpButtonMouseWheelMove, KeyCode crouchButton, bool crouchButtonMouseWheelMove, KeyCode dashButton, bool dashButtonMouseWheelMove, KeyCode shootingButton, bool shootingButtonMouseWheelMove, KeyCode useProtectionButton, bool useProtectionButtonMouseWheelMove, KeyCode nextWeaponButton, bool nextButtonMouseWheelMove, KeyCode previousWeaponButton, bool previousButtonMouseWheelMove, KeyCode useHookButton, bool useHookButtonMouseWheelMove, KeyCode useItemButton, bool useItemButtonMouseWheelMove, KeyCode nextAbilityButton, bool nextAbilityButtonMouseWheelMove, KeyCode previousAbilityButton, bool previousAbilityButtonMouseWheelMove, KeyCode useAbilityButton, bool useAbilityButtonMouseWheelMove, KeyCode useEnemyCleanerButton, bool useEnemyCleanerButtonMouseWheelMove, KeyCode openSuitManageMenuButton, bool openSuitManageMenuButtonMouseWheelMove, KeyCode saveLevelButton, bool saveLevelButtonMouseWheelMove, float mouseSensitivity)
        {
            this.forwardButton = forwardButton;
            this.forwardButtonMouseWheelMove = forwardButtonMouseWheelMove;
            this.backButton = backButton;
            this.backButtonMouseWheelMove = backButtonMouseWheelMove;
            this.leftButton = leftButton;
            this.leftButtonMouseWheelMove = leftButtonMouseWheelMove;
            this.rightButton = rightButton;
            this.rightButtonMouseWheelMove = rightButtonMouseWheelMove;
            this.jumpButton = jumpButton;
            this.jumpButtonMouseWheelMove = jumpButtonMouseWheelMove;
            this.crouchButton = crouchButton;
            this.crouchButtonMouseWheelMove = crouchButtonMouseWheelMove;
            this.dashButton = dashButton;
            this.dashButtonMouseWheelMove = dashButtonMouseWheelMove;
            this.shootingButton = shootingButton;
            this.shootingButtonMouseWheelMove = shootingButtonMouseWheelMove;
            this.useProtectionButton = useProtectionButton;
            this.useProtectionButtonMouseWheelMove = useProtectionButtonMouseWheelMove;
            this.nextWeaponButton = nextWeaponButton;
            this.nextButtonMouseWheelMove = nextButtonMouseWheelMove;
            this.previousWeaponButton = previousWeaponButton;
            this.previousButtonMouseWheelMove = previousButtonMouseWheelMove;
            this.useHookButton = useHookButton;
            this.useHookButtonMouseWheelMove = useHookButtonMouseWheelMove;
            this.useItemButton = useItemButton;
            this.useItemButtonMouseWheelMove = useItemButtonMouseWheelMove;
            this.nextAbilityButton = nextAbilityButton;
            this.nextAbilityButtonMouseWheelMove = nextAbilityButtonMouseWheelMove;
            this.previousAbilityButton = previousAbilityButton;
            this.previousAbilityButtonMouseWheelMove = previousAbilityButtonMouseWheelMove;
            this.useAbilityButton = useAbilityButton;
            this.useAbilityButtonMouseWheelMove = useAbilityButtonMouseWheelMove;
            this.useEnemyCleanerButton = useEnemyCleanerButton;
            this.useEnemyCleanerButtonMouseWheelMove = useEnemyCleanerButtonMouseWheelMove;
            this.openSuitManageMenuButton = openSuitManageMenuButton;
            this.openSuitManageMenuButtonMouseWheelMove = openSuitManageMenuButtonMouseWheelMove;
            this.saveLevelButton = saveLevelButton;
            this.saveLevelButtonMouseWheelMove = saveLevelButtonMouseWheelMove;
            this.mouseSensitivity = mouseSensitivity;
        }
    }
}
