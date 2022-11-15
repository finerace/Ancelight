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

    [SerializeField] private TMP_Dropdown soundsMusicQuality;
    [SerializeField] private Slider soundsMaxSoundsCount;

    [Space] 
    
    [SerializeField] private TMP_Dropdown textureQuality;
    [SerializeField] private TMP_Dropdown anisotropicTextureQuality;

    [SerializeField] private TMP_Dropdown shadowsResolutionQuality;
    [SerializeField] private Slider shadowDistance;

    [SerializeField] private TMP_Dropdown msaaQuality;
    [SerializeField] private TMP_Dropdown antiAliasingQuality;

    [SerializeField] private TMP_Dropdown grassQuality;
    [SerializeField] private Slider grassDrawingDistance;
    [SerializeField] private TMP_Dropdown plantsQuality;
    [SerializeField] private TMP_Dropdown enemyPartsQuality;
    [SerializeField] private Slider timeOfDestructionOfParts;

    [SerializeField] private Slider drawDistanceCoof;
    [SerializeField] private Toggle vsync;

    [Space] 
    
    [SerializeField] private InputButtonField forwardButton;
    [SerializeField] private InputButtonField backButton;
    [SerializeField] private InputButtonField leftButton;
    [SerializeField] private InputButtonField rightButton;
    [SerializeField] private InputButtonField jumpButton;
    [SerializeField] private InputButtonField crouchButton;
    [SerializeField] private InputButtonField dashButton;
    
    [SerializeField] private InputButtonField shootingButton;
    [SerializeField] private InputButtonField useHookButton;
    [SerializeField] private InputButtonField nextWeaponButton;
    [SerializeField] private InputButtonField previousButton;

    private const string saveFileName = "SettingsSabe.bob";
    private string saveFilePath;

    public void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/";
    }

    public void SaveSettings()
    {
        SettingsData.SettingsSoundsData soundsData;
        SettingsData.SettingsGraphicsData graphicsData;
        SettingsData.SettingsControlsData controlsData;

        (soundsData, graphicsData, controlsData) = CreateSettingsData();

        var mainSettingsData = new SettingsData(soundsData, graphicsData, controlsData);
        
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
        SetInputElementsNewValue(GetSavedSettings());
    }

    public SettingsData GetSavedSettings()
    {
        var saveFileIsExists = File.Exists(saveFilePath + saveFileName);

        if (!saveFileIsExists)
        {
            SaveSettings();
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
                soundsMusicQuality.value,
                soundsMaxSoundsCount.value);

            var graphicsSettingsData =
                new SettingsData.SettingsGraphicsData(
                    textureQuality.value,
                    anisotropicTextureQuality.value,
                    shadowsResolutionQuality.value,
                    shadowDistance.value,
                    msaaQuality.value,
                    antiAliasingQuality.value,
                    grassQuality.value,
                    grassDrawingDistance.value,
                    plantsQuality.value,
                    enemyPartsQuality.value,
                    timeOfDestructionOfParts.value,
                    drawDistanceCoof.value,
                    vsync.isOn);

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
                useHookButton.AssignedButtonKeyCode,
                useHookButton.AssignedButtonMouseWheelMove,
                nextWeaponButton.AssignedButtonKeyCode,
                nextWeaponButton.AssignedButtonMouseWheelMove,
                previousButton.AssignedButtonKeyCode,
                previousButton.AssignedButtonMouseWheelMove);
            
            return (soundsSettingsData,graphicsSettingsData,controlsSettingsData);
        }

    private void SetInputElementsNewValue(SettingsData settingsData)
    {
        SetSoundsSettings();

        SetGraphicsSettings();
        
        SetControlsSettings();
        
        void SetSoundsSettings()
        {
            soundsMasterVolume.value = settingsData.SoundsSettingsData.MasterVolumeValue;
            soundsMusicVolume.value = settingsData.SoundsSettingsData.MusicVolumeValue;
            soundsEffectsVolume.value = settingsData.SoundsSettingsData.EffectsVolumeValue;

            soundsMusicQuality.value = settingsData.SoundsSettingsData.MusicQuality;
            soundsMaxSoundsCount.value = settingsData.SoundsSettingsData.MaxSoundsCountValue;
        }

        void SetGraphicsSettings()
        {
            textureQuality.value = settingsData.GraphicsSettingsData.TexturesQuality;
            anisotropicTextureQuality.value = settingsData.GraphicsSettingsData.AnisotropicTexturesQuality;

            shadowsResolutionQuality.value = settingsData.GraphicsSettingsData.ShadowsResolutionQuality;
            shadowDistance.value = settingsData.GraphicsSettingsData.ShadowDistance;

            msaaQuality.value = settingsData.GraphicsSettingsData.MsaAquality;
            antiAliasingQuality.value = settingsData.GraphicsSettingsData.AntiAliasingQuality;

            grassQuality.value = settingsData.GraphicsSettingsData.GrassQuality;
            grassDrawingDistance.value = settingsData.GraphicsSettingsData.GrassDrawingDistance;

            plantsQuality.value = settingsData.GraphicsSettingsData.PlantsQuality;
            enemyPartsQuality.value = settingsData.GraphicsSettingsData.EnemyPartsQuality;
            timeOfDestructionOfParts.value = settingsData.GraphicsSettingsData.TimeOfDestructionOfParts;

            drawDistanceCoof.value = settingsData.GraphicsSettingsData.DrawingDistanceCoof;
            vsync.isOn = settingsData.GraphicsSettingsData.Vsync;
        }

        void SetControlsSettings()
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

            if (settingsData.ControlsSettingsData.UseHookButton != KeyCode.None)
            {
                useHookButton.AssignedButtonKeyCode = settingsData.ControlsSettingsData.UseHookButton;
            }
            else
            {
                useHookButton.AssignedButtonKeyCode = KeyCode.None;
                useHookButton.AssignedButtonMouseWheelMove = settingsData.ControlsSettingsData.UseHookButtonMouseWheelMove;
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

        }
    }
    
}

[Serializable]
public class SettingsData
{
    private SettingsSoundsData soundsSettingsData;

    private SettingsGraphicsData graphicsSettingsData;

    private SettingsControlsData controlsSettingsData;

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
        
        private float masterVolumeValue;
        private float musicVolumeValue;
        private float effectsVolumeValue;
        private int musicQuality;
        private float maxSoundsCountValue;

        public float MasterVolumeValue => masterVolumeValue;
        public float MusicVolumeValue => musicVolumeValue;
        public float EffectsVolumeValue => effectsVolumeValue;
        public int MusicQuality => musicQuality;
        public float MaxSoundsCountValue => maxSoundsCountValue;

        public SettingsSoundsData(
            float masterVolumeValue, 
            float musicVolumeValue, 
            float effectsVolumeValue,
            int musicQuality,
            float maxSoundsCountValue)
        {
            this.masterVolumeValue = masterVolumeValue;
            this.musicVolumeValue = musicVolumeValue;
            this.effectsVolumeValue = effectsVolumeValue;
            this.musicQuality = musicQuality;
            this.maxSoundsCountValue = maxSoundsCountValue;
        }
    }
    [Serializable]
    public class SettingsGraphicsData
    {
        private int texturesQuality;
        private int anisotropicTexturesQuality;

        private int shadowsResolutionQuality;
        private float shadowDistance;

        private int MSAAquality;
        private int antiAliasingQuality;

        private int grassQuality;
        private float grassDrawingDistance;

        private int plantsQuality;
        private int enemyPartsQuality;
        private float timeOfDestructionOfParts;

        private float drawingDistanceCoof;
        private bool vsync;

        public int TexturesQuality => texturesQuality;

        public int AnisotropicTexturesQuality => anisotropicTexturesQuality;

        public int ShadowsResolutionQuality => shadowsResolutionQuality;

        public float ShadowDistance => shadowDistance;

        public int MsaAquality => MSAAquality;

        public int AntiAliasingQuality => antiAliasingQuality;

        public int GrassQuality => grassQuality;

        public float GrassDrawingDistance => grassDrawingDistance;

        public int PlantsQuality => plantsQuality;

        public int EnemyPartsQuality => enemyPartsQuality;

        public float TimeOfDestructionOfParts => timeOfDestructionOfParts;

        public float DrawingDistanceCoof => drawingDistanceCoof;

        public bool Vsync => vsync;

        public SettingsGraphicsData(
            int texturesQuality, 
            int anisotropicTexturesQuality, 
            int shadowsResolutionQuality,
            float shadowDistance,
            int msaAquality,
            int antiAliasingQuality,
            int grassQuality,
            float grassDrawingDistance,
            int plantsQuality,
            int enemyPartsQuality,
            float timeOfDestructionOfParts,
            float drawingDistanceCoof,
            bool vsync)
        {
            this.texturesQuality = texturesQuality;
            this.anisotropicTexturesQuality = anisotropicTexturesQuality;
            this.shadowsResolutionQuality = shadowsResolutionQuality;
            this.shadowDistance = shadowDistance;
            MSAAquality = msaAquality;
            this.antiAliasingQuality = antiAliasingQuality;
            this.grassQuality = grassQuality;
            this.grassDrawingDistance = grassDrawingDistance;
            this.plantsQuality = plantsQuality;
            this.enemyPartsQuality = enemyPartsQuality;
            this.timeOfDestructionOfParts = timeOfDestructionOfParts;
            this.drawingDistanceCoof = drawingDistanceCoof;
            this.vsync = vsync;
        }
    }
    [Serializable]
    public class SettingsControlsData
    {
        private KeyCode forwardButton;
        private bool forwardButtonMouseWheelMove;
        
        private KeyCode backButton;
        private bool backButtonMouseWheelMove;

        private KeyCode leftButton;
        private bool leftButtonMouseWheelMove;

        private KeyCode rightButton;
        private bool rightButtonMouseWheelMove;

        private KeyCode jumpButton;
        private bool jumpButtonMouseWheelMove;

        private KeyCode crouchButton;
        private bool crouchButtonMouseWheelMove;

        private KeyCode dashButton;
        private bool dashButtonMouseWheelMove;

        private KeyCode shootingButton;
        private bool shootingButtonMouseWheelMove;

        private KeyCode useHookButton;
        private bool useHookButtonMouseWheelMove;

        private KeyCode nextWeaponButton;
        private bool nextButtonMouseWheelMove;

        private KeyCode previousWeaponButton;
        private bool previousButtonMouseWheelMove;

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

        public KeyCode UseHookButton => useHookButton;

        public bool UseHookButtonMouseWheelMove => useHookButtonMouseWheelMove;

        public KeyCode NextWeaponButton => nextWeaponButton;

        public bool NextButtonMouseWheelMove => nextButtonMouseWheelMove;

        public KeyCode PreviousWeaponButton => previousWeaponButton;

        public bool PreviousButtonMouseWheelMove => previousButtonMouseWheelMove;

        public SettingsControlsData(KeyCode forwardButton, bool forwardButtonMouseWheelMove, KeyCode backButton, bool backButtonMouseWheelMove, KeyCode leftButton, bool leftButtonMouseWheelMove, KeyCode rightButton, bool rightButtonMouseWheelMove, KeyCode jumpButton, bool jumpButtonMouseWheelMove, KeyCode crouchButton, bool crouchButtonMouseWheelMove, KeyCode dashButton, bool dashButtonMouseWheelMove, KeyCode shootingButton, bool shootingButtonMouseWheelMove, KeyCode useHookButton, bool useHookButtonMouseWheelMove, KeyCode nextWeaponButton, bool nextButtonMouseWheelMove, KeyCode previousWeaponButton, bool previousButtonMouseWheelMove)
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
            this.useHookButton = useHookButton;
            this.useHookButtonMouseWheelMove = useHookButtonMouseWheelMove;
            this.nextWeaponButton = nextWeaponButton;
            this.nextButtonMouseWheelMove = nextButtonMouseWheelMove;
            this.previousWeaponButton = previousWeaponButton;
            this.previousButtonMouseWheelMove = previousButtonMouseWheelMove;
        }
    }
}
