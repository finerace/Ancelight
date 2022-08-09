using System.Collections;
using System.Collections.Generic;
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
            
            Color newColor = color * effectsIntensity;
            
            Material newMaterial = new Material(image.material);

            newMaterial.SetColor("_MainColor", newColor);

            AssetDatabase.CreateAsset(newMaterial, materialAssetCreatePath);

            image.material = newMaterial;
            
        }
    }

    private void UnscaledDeltaTimeButtonEffectsAnimation()
    {

        SetMaterialUnscaledDeltaTime(buttonBodyEmissionEffect.material);
        SetMaterialUnscaledDeltaTime(buttonTriangleEmissionEffect.material);
        SetMaterialUnscaledDeltaTime(buttonLinkEffect.material);

        void SetMaterialUnscaledDeltaTime(Material mat)
        {
            float effectSetTime = mat.GetFloat("_UnscaledTime") + Time.unscaledDeltaTime;

            mat.SetFloat("_UnscaledTime", effectSetTime);
        }
    }

    private void DynamicEffectsIntensity()
    {
        
        ImageMaterialSetNewIntensity(buttonBodyEmissionEffect);
        ImageMaterialSetNewIntensity(buttonTriangleEmissionEffect);
        ImageMaterialSetNewIntensity(buttonLinkEffect);

        void ImageMaterialSetNewIntensity(Image item)
        {
            Color newColor = buttonColor * effectsIntensity;

            Material newMaterial = new Material(item.material);

            newMaterial.SetColor("_MainColor", newColor);

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
