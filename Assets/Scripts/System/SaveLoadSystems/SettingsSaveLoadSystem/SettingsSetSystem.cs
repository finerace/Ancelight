using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SettingsSetSystem : MonoBehaviour
{
    [SerializeField] private SettingsSaveLoadSystem settingsSaveLoadSystem;
    [SerializeField] private UniversalRenderPipelineAsset urpAsset;
    
    public void SetNewSettings()
    {
        var settingsData = settingsSaveLoadSystem.GetSavedSettings();

        SetUrpAssetSettings();
        
        void SetUrpAssetSettings()
        {

            switch (settingsData.GraphicsSettingsData.ShadowsResolutionQuality)
            {
                case 0:
                {
                    
                    break;
                }
                
                case 1:
                {
                 
                    break;
                }
                
                case 2:
                {
                 
                    break;
                }
                
                case 3:
                {
                 
                    break;
                }
                
                case 4:
                {
                 
                    break;
                }
                
                case 5:
                {
                 
                    break;
                }
            }
            
        }
        
    }

}
