using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class OrdinaryButtonEffects : MonoBehaviour , ButtonEffects
{

    [SerializeField] private Color buttonColor = Color.white;
    [SerializeField] public float effectsIntensity = 1f;
    private Color startButtonColor;
    [Space]
    [SerializeField] private Image buttonBodyEmissionEffect;
    [SerializeField] private Image buttonTriangleEmissionEffect;
    [SerializeField] private Image buttonLinkEffect;
    [SerializeField] private bool isUnscaledDeltaTimeButtonShadersAnimationOn = true;
    private static readonly int UnscaledTimeReferenceID = Shader.PropertyToID("_unscaledTime");
    private static readonly int MainColorReferenceID = Shader.PropertyToID("_MainColor");

    private void Update()
    {
        if(isUnscaledDeltaTimeButtonShadersAnimationOn)
        {
            UnscaledDeltaTimeButtonEffectsAnimation();
        }

        DynamicEffectsIntensity();
    }

    
    
    public void SetButtonColor()
    {
#if UNITY_EDITOR
        string newAssetFolderPath = "Assets/UI/DefaultButtonColors";
        string newAssetFolderName = $"Button_{gameObject.name}";

        if (!AssetDatabase.IsValidFolder($"{newAssetFolderPath}/{newAssetFolderName}"))
        {
            AssetDatabase.CreateFolder(newAssetFolderPath, newAssetFolderName);
        }
        
        string assetPath =
        $"{newAssetFolderPath}/{newAssetFolderName}/{nameof(buttonBodyEmissionEffect)}.mat";
        SetNewImageMaterialColor(buttonBodyEmissionEffect, buttonColor, assetPath);

        assetPath =
        $"{newAssetFolderPath}/{newAssetFolderName}/{nameof(buttonTriangleEmissionEffect)}.mat";
        SetNewImageMaterialColor(buttonTriangleEmissionEffect, buttonColor, assetPath);

        assetPath =
        $"{newAssetFolderPath}/{newAssetFolderName}/{nameof(buttonLinkEffect)}.mat";
        SetNewImageMaterialColor(buttonLinkEffect, buttonColor, assetPath);
        

        void SetNewImageMaterialColor(Image image,Color color,string materialAssetCreatePath)
        {
            
            var newColor = color * effectsIntensity;
            
            var newMaterial = new Material(image.material);

            newMaterial.SetColor(MainColorReferenceID, newColor);

            AssetDatabase.CreateAsset(newMaterial, materialAssetCreatePath);

            image.material = newMaterial;
            
        }
#endif
    }

    private void UnscaledDeltaTimeButtonEffectsAnimation()
    {
        if(buttonBodyEmissionEffect != null)
            SetMaterialUnscaledDeltaTime(buttonBodyEmissionEffect.material);
        
        if(buttonTriangleEmissionEffect != null)
            SetMaterialUnscaledDeltaTime(buttonTriangleEmissionEffect.material);
        
        if(buttonLinkEffect != null)
            SetMaterialUnscaledDeltaTime(buttonLinkEffect.material);

        void SetMaterialUnscaledDeltaTime(Material mat)
        {
            var effectSetTime = mat.GetFloat(UnscaledTimeReferenceID) + Time.unscaledDeltaTime;

            mat.SetFloat(UnscaledTimeReferenceID, effectSetTime);
        }
    }

    private void DynamicEffectsIntensity()
    {
        
        ImageMaterialSetNewIntensity(buttonBodyEmissionEffect);
        ImageMaterialSetNewIntensity(buttonTriangleEmissionEffect);
        ImageMaterialSetNewIntensity(buttonLinkEffect);

        void ImageMaterialSetNewIntensity(Image item)
        {
            if(item == null)
                return;
            
            var newColor = buttonColor * effectsIntensity;

            var newMaterial = new Material(item.material);

            newMaterial.SetColor(MainColorReferenceID, newColor);

            item.material = newMaterial;
        }

    }

    public float GetIntensity()
    {
        return effectsIntensity;
    }

    public void SetIntensity(float intensity)
    {
        if (intensity < 0)
        {
            throw new System.ArgumentOutOfRangeException
                ("Button intensity cannot be less than zero!");
        }

        effectsIntensity = intensity;
    }
}
