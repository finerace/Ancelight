using System;
using System.Data;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

public class SettingsSetSystem : MonoBehaviour
{
    [SerializeField] private SettingsSaveLoadSystem settingsSaveLoadSystem;
    [SerializeField] private UniversalRenderPipelineAsset urpAsset;

    [Space] 
    
    [SerializeField] private SettingsLevelGrassData grassOnLevelData;

    public static int enemyPartsQuality = 0;
    public static float enemyPartsDestroyTime = 90;
    
    public void SetNewSettings()
    {
        var settingsData = settingsSaveLoadSystem.GetSavedSettings();

        SetBuildInSettings();
        
        SetUrpAssetSettings();
        
        SetOtherSettings();
        
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
        }

        void SetOtherSettings()
        {
            SetGrassSettings();
            
            SetEnemyPartsSettings();
            
            void SetGrassSettings()
            {
                var levelGrassData = grassOnLevelData;
                
                if(levelGrassData == null)
                    levelGrassData = FindObjectOfType<SettingsLevelGrassData>();

                if (levelGrassData == null)
                    throw new DataException("GRASS DATA MISSING! *_*");

                foreach (var grassMaterial in levelGrassData.LevelUsedGrassMaterials)
                {
                    grassMaterial.SetFloat("_MaxDist",settingsData.GraphicsSettingsData.GrassDrawingDistance);
                    grassMaterial.SetFloat("_MinDist",settingsData.GraphicsSettingsData.GrassDrawingDistance/4);
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
        }
    }

}
