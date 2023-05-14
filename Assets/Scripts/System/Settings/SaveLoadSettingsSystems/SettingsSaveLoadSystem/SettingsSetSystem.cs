using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

public class SettingsSetSystem : MonoBehaviour
{
    [SerializeField] private SettingsSaveLoadSystem settingsSaveLoadSystem;
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private LevelSaveLoadSystem levelSaveLoadSystem;
    
    [Space]
    
    [SerializeField] private UniversalRenderPipelineAsset urpAsset;
    [SerializeField] private UniversalRendererData urpData;
    
    [Space]
    
    [SerializeField] private bool isSetSystemInMainMenu = false;

    [Space]
    
    [SerializeField] private AudioMixer mainAudioMixer;
    private AudioPoolService audioPoolService;

    private const string masterMixerName = "Master";
    private const string musicMixerName = "Music";
    private const string effectsMixerName = "Effects";
    private const string ambientMixerName = "Ambient";
    private const string uiMixerName = "UI";

    [Space] 
    
    [SerializeField] private SettingsLevelGrassData grassOnLevelData;

    public static int enemyPartsQuality = 0;
    public static float enemyPartsDestroyTime = 90;

    [Space] 
    
    [SerializeField] private bool mainMenuMode;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        levelSaveLoadSystem = FindObjectOfType<LevelSaveLoadSystem>();
        
        settingsSaveLoadSystem.LoadSettings();
        audioPoolService = FindObjectOfType<AudioPoolService>();
        
        SetNewSettings();
    }

    public void SetNewSettings()
    {
        var settingsData = settingsSaveLoadSystem.GetSavedSettings();
        
        DelayedUpdateVolume();
        
        async void DelayedUpdateVolume()
        {
            await Task.Delay(100);
            SetSoundSettings();
        }

        SetBuildInSettings();
        
        SetUrpAssetSettings();
        
        if(!mainMenuMode)
            SetControlsSettings();
        
        SetOtherSettings();

        void SetSoundSettings()
        {
            var soundSettings = settingsData.SoundsSettingsData;
            audioPoolService.SetNewMaxAudioSourcesCount((int)soundSettings.MaxSoundsCountValue);

            mainAudioMixer.SetFloat(masterMixerName,ConvertSliderValueToMixerValue(soundSettings.MasterVolumeValue));
            mainAudioMixer.SetFloat(musicMixerName, ConvertSliderValueToMixerValue(soundSettings.MusicVolumeValue));
            mainAudioMixer.SetFloat(effectsMixerName, ConvertSliderValueToMixerValue(soundSettings.EffectsVolumeValue));
            mainAudioMixer.SetFloat(ambientMixerName, ConvertSliderValueToMixerValue(soundSettings.AmbientVolumeValue));
            mainAudioMixer.SetFloat(uiMixerName, ConvertSliderValueToMixerValue(soundSettings.UiVolumeValue));
            
            float ConvertSliderValueToMixerValue(float sliderValue)
            {
                var resultValue = sliderValue;
                resultValue /= 100;
                
                if (resultValue == 0)
                    return -80;
                
                resultValue = Mathf.Log10(resultValue) * 30;
                resultValue += 10;
                
                return resultValue;
            }
        }
        
        void SetBuildInSettings()
        {
            SetTexturesQuality();
            
            SetAnisotropicTextures();
            
            SetDrawDistanceCoof();
            
            SetVsync();
            
            void SetTexturesQuality()
            {
                switch (settingsData.GraphicsSettingsData.TexturesQuality)
                {
                    case 0:
                    {
                        QualitySettings.masterTextureLimit = 3;
                        break;
                    }
                    
                    case 1:
                    {
                        QualitySettings.masterTextureLimit = 2;
                        break;
                    }
                    
                    case 2:
                    {
                        QualitySettings.masterTextureLimit = 1;
                        break;
                    }
                    
                    case 3:
                    {
                        QualitySettings.masterTextureLimit = 0;
                        break;
                    }
                }
                
            }

            void SetAnisotropicTextures()
            {
                QualitySettings.anisotropicFiltering =
                    settingsData.GraphicsSettingsData.AnisotropicTexturesQuality switch
                    {
                        0 => AnisotropicFiltering.Disable,
                        1 => AnisotropicFiltering.Enable,
                        2 => AnisotropicFiltering.ForceEnable,
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }

            void SetDrawDistanceCoof()
            {
                QualitySettings.lodBias = settingsData.GraphicsSettingsData.DrawingDistanceCoof;
            }

            void SetVsync()
            {
                QualitySettings.vSyncCount = !settingsData.GraphicsSettingsData.Vsync ? 0 : 1;
            }
        }

        void SetUrpAssetSettings()
        {
            SetAntiAliasingSettings();

            SetShadowResolution();
            
            SetShadowDistance();
            
            SetSoftShadows();
            
            SetSsao();
            
            SetMsaaQuality();
            
            void SetAntiAliasingSettings()
            {
                var mainCameraRenderData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                        
                switch (settingsData.GraphicsSettingsData.AntiAliasingQuality)
                        {
                            case 0:
                            {
                                mainCameraRenderData.antialiasing = AntialiasingMode.None;
                                break;
                            }
                            
                            case 1:
                            {
                                mainCameraRenderData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                                break;
                            }
                            
                            case 2:
                            {
                                mainCameraRenderData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                                mainCameraRenderData.antialiasingQuality = AntialiasingQuality.Low;
                                break;
                            }
                            
                            case 3:
                            {
                                mainCameraRenderData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                                mainCameraRenderData.antialiasingQuality = AntialiasingQuality.Medium;
                                
                                break;
                            }
                            
                            case 4:
                            {
                                mainCameraRenderData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                                mainCameraRenderData.antialiasingQuality = AntialiasingQuality.High;
                                
                                break;
                            }
                        }
            }
            
            void SetShadowResolution()
            {
                switch (settingsData.GraphicsSettingsData.ShadowsResolutionQuality)
                {
                    case 0:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = false;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = false;
                    
                        break;
                    }
                
                    case 1:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = true;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                        
                        UnityGraphicsBullshit.MainLightShadowResolution = ShadowResolution._256;
                        UnityGraphicsBullshit.AdditionalLightShadowResolution = ShadowResolution._256;
                        
                        break;
                    }
                
                    case 2:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = true;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                        
                        UnityGraphicsBullshit.MainLightShadowResolution = ShadowResolution._512;
                        UnityGraphicsBullshit.AdditionalLightShadowResolution = ShadowResolution._256;
                        
                        break;
                    }
                
                    case 3:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = true;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                        
                        UnityGraphicsBullshit.MainLightShadowResolution = ShadowResolution._1024;
                        UnityGraphicsBullshit.AdditionalLightShadowResolution = ShadowResolution._512;
                        
                        break;
                    }
                
                    case 4:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = true;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                        
                        UnityGraphicsBullshit.MainLightShadowResolution = ShadowResolution._2048;
                        UnityGraphicsBullshit.AdditionalLightShadowResolution = ShadowResolution._1024;
                        
                        break;
                    }
                
                    case 5:
                    {
                        UnityGraphicsBullshit.MainLightCastShadows = true;
                        UnityGraphicsBullshit.AdditionalLightCastShadows = true;
                        
                        UnityGraphicsBullshit.MainLightShadowResolution = ShadowResolution._4096;
                        UnityGraphicsBullshit.AdditionalLightShadowResolution = ShadowResolution._2048;
                        
                        break;
                    }
                }
            }

            void SetShadowDistance()
            {
                urpAsset.shadowDistance = settingsData.GraphicsSettingsData.ShadowDistance;
            }

            void SetSoftShadows()
            {
                UnityGraphicsBullshit.SoftShadowsEnabled = settingsData.GraphicsSettingsData.SoftShadows;
            }
            
            void SetMsaaQuality()
            {
                switch (settingsData.GraphicsSettingsData.MSAAQuality)
                {
                    case 0:
                    {
                        urpAsset.msaaSampleCount = 1;
                        
                        break;
                    }
                    
                    case 1:
                    {
                        urpAsset.msaaSampleCount = 2;
                        
                        break;
                    }
                    
                    case 2:
                    {
                        urpAsset.msaaSampleCount = 4;
                        
                        break;
                    }
                    
                    case 3:
                    {
                        urpAsset.msaaSampleCount = 8;
                        
                        break;
                    }
                }
            }

            void SetSsao()
            {
                //urpData.rendererFeatures[4].SetActive(settingsData.GraphicsSettingsData.Ssao);
            }
        }

        void SetControlsSettings()
        {
            if(isSetSystemInMainMenu)
                return;
            
            var playerMovement = playerMainService.playerMovement;
            var playerWeapon = playerMainService.weaponsManager;
            var playerHook = playerMainService.hookService;
            var playerDash = playerMainService.dashsService;
            var playerProtection = playerMainService.immediatelyProtectionService;
            var playerUseItem = playerMainService.playerUseService;
            var playerEnemyCleaner = playerMainService.playerCleaner;
            
            var controlsSettings = settingsData.ControlsSettingsData;

            var playerMainServiceButtons =
                playerMainService.GetUsesDevicesButtons();
            
            playerMainServiceButtons[0].
                AssignedNewDeviceButton(controlsSettings.OpenSuitManageMenuButton,controlsSettings.OpenSuitManageMenuButtonMouseWheelMove);
            
            var movementButtons = 
                playerMovement.GetUsesDevicesButtons();
            
            movementButtons[0].
                AssignedNewDeviceButton(controlsSettings.ForwardButton,controlsSettings.ForwardButtonMouseWheelMove);
            
            movementButtons[1].
                AssignedNewDeviceButton(controlsSettings.BackButton,controlsSettings.BackButtonMouseWheelMove);

            movementButtons[2].
                AssignedNewDeviceButton(controlsSettings.LeftButton,controlsSettings.LeftButtonMouseWheelMove);

            movementButtons[3].
                AssignedNewDeviceButton(controlsSettings.RightButton,controlsSettings.RightButtonMouseWheelMove);

            movementButtons[4].
                AssignedNewDeviceButton(controlsSettings.CrouchButton,controlsSettings.CrouchButtonMouseWheelMove);

            movementButtons[5].
                AssignedNewDeviceButton(controlsSettings.JumpButton,controlsSettings.JumpButtonMouseWheelMove);

            var weaponButtons 
                = playerWeapon.GetUsesDevicesButtons();
            
            weaponButtons[0]
                .AssignedNewDeviceButton(controlsSettings.ShootingButton,controlsSettings.ShootingButtonMouseWheelMove);
            
            weaponButtons[1]
                .AssignedNewDeviceButton(controlsSettings.NextWeaponButton,controlsSettings.NextButtonMouseWheelMove);

            weaponButtons[2]
                .AssignedNewDeviceButton(controlsSettings.PreviousWeaponButton,controlsSettings.PreviousButtonMouseWheelMove);

            weaponButtons[3]
                .AssignedNewDeviceButton(controlsSettings.UseAbilityButton,controlsSettings.UseAbilityButtonMouseWheelMove);

            weaponButtons[4]
                .AssignedNewDeviceButton(controlsSettings.NextAbilityButton,controlsSettings.NextAbilityButtonMouseWheelMove);

            weaponButtons[5]
                .AssignedNewDeviceButton(controlsSettings.PreviousAbilityButton,controlsSettings.PreviousAbilityButtonMouseWheelMove);

            var hookButtons
                = playerHook.GetUsesDevicesButtons();
            
            hookButtons[0].AssignedNewDeviceButton(controlsSettings.UseHookButton,controlsSettings.UseHookButtonMouseWheelMove);

            var dashButtons
                = playerDash.GetUsesDevicesButtons();
            
            dashButtons[0].AssignedNewDeviceButton(controlsSettings.DashButton, controlsSettings.DashButtonMouseWheelMove);

            var protectionButtons
                = playerProtection.GetUsesDevicesButtons();
            
            protectionButtons[0].AssignedNewDeviceButton(controlsSettings.UseProtectionButton,controlsSettings.UseProtectionButtonMouseWheelMove);

            var playerUseButtons
                = playerUseItem.GetUsesDevicesButtons();
            
            playerUseButtons[0].AssignedNewDeviceButton(controlsSettings.UseItemButton,controlsSettings.UseItemButtonMouseWheelMove);

            var enemyCleanerButtons =
                playerEnemyCleaner.GetUsesDevicesButtons();
            
            enemyCleanerButtons[0].AssignedNewDeviceButton(controlsSettings.UseEnemyCleanerButton,controlsSettings.UseEnemyCleanerButtonMouseWheelMove);


            var levelSaveLoadButtons =
                levelSaveLoadSystem.GetUsesDevicesButtons();

            levelSaveLoadButtons[0].AssignedNewDeviceButton(controlsSettings.SaveLevelButton,
                controlsSettings.SaveLevelButtonMouseWheelMove);
            
            playerMainService.playerRotation.mouseSensivity = controlsSettings.MouseSensitivity;
        }
        
        void SetOtherSettings()
        {
            if(!mainMenuMode)
                SetGrassSettings();
            
            SetEnemyPartsSettings();

            SetFieldOfView();
            
            SetScreenResolutionAndFormat();
            
            SetLanguage();
            
            void SetGrassSettings()
            {
                if(isSetSystemInMainMenu)
                    return;
                    
                var levelGrassData = grassOnLevelData;

                if(levelGrassData == null)
                    levelGrassData = FindObjectOfType<SettingsLevelGrassData>();

                if (levelGrassData == null)
                {
                    print("grass data not found :c");
                    return;
                }
                
                foreach (var grassMaterial in levelGrassData.LevelUsedGrassMaterials)
                {
                    grassMaterial.SetFloat("_MaxDist",settingsData.GraphicsSettingsData.GrassDrawingDistance);
                    grassMaterial.SetFloat("_MinDist",settingsData.GraphicsSettingsData.GrassDrawingDistance/1.25f);
                }
                
                foreach (var grassMesh in levelGrassData.LevelUsedGrassMeshes)
                {
                    switch (settingsData.GraphicsSettingsData.GrassQuality)
                    {
                        case 0:
                        {
                            grassMesh.enabled = false;
                            break;
                        }
                        
                        case 1:
                        {
                            grassMesh.enabled = true;
                            grassMesh.shadowCastingMode = ShadowCastingMode.Off;
                            
                            break;
                        }
                        
                        case 2:
                        {
                            grassMesh.enabled = true;
                            grassMesh.shadowCastingMode = ShadowCastingMode.On;
                            break;
                        }
                    }
                }
            }

            void SetEnemyPartsSettings()
            {
                enemyPartsQuality = settingsData.GraphicsSettingsData.EnemyPartsQuality;
                enemyPartsDestroyTime = settingsData.GraphicsSettingsData.TimeOfDestructionOfParts;
            }

            void SetFieldOfView()
            {
                if (settingsData.GraphicsSettingsData.FieldOfView < 75)
                {
                    Camera.main.fieldOfView = 75;
                    
                    return;
                }

                Camera.main.fieldOfView = settingsData.GraphicsSettingsData.FieldOfView;
            }

            void SetScreenResolutionAndFormat()
            {
                var resolution = (1920,1080);
                
                var screenFormat = FullScreenMode.FullScreenWindow;

                screenFormat = settingsData.GraphicsSettingsData.ScreenFormat switch
                {
                    0 => FullScreenMode.FullScreenWindow,
                    1 => FullScreenMode.ExclusiveFullScreen,
                    2 => FullScreenMode.MaximizedWindow,
                    3 => FullScreenMode.Windowed,
                    _ => throw new IndexOutOfRangeException()
                };
                
                resolution =
                    settingsData.GraphicsSettingsData.ScreenResolution switch
                    {
                        0 => (640,480),
                        1 => (800,600),
                        2 => (1024,768),
                        3 => (1280,720),
                        4 => (1366,768),
                        5 => (1280,800),
                        6 => (1440,900),
                        7 => (1600,900),
                        8 => (1680,1050),
                        9 => (1920,1080),
                        10 => (2560,1440),
                        11 => (3840,2160),
                        12 => (5120,2880),
                        13 => (7680,4320),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                Screen.SetResolution(resolution.Item1,resolution.Item2,screenFormat);
            }
            
            void SetLanguage()
            {
                FindObjectOfType<CurrentLanguageData>().SetLanguageData(settingsData.GraphicsSettingsData.Language);
            }
        }
        
    }

}
