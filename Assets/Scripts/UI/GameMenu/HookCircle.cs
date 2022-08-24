using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookCircle : MonoBehaviour
{

    [SerializeField] private PlayerHookService hookService;
    [SerializeField] private Image hookStrengthCircle;
    [SerializeField] private bool isVanish = false;
    [Space]
    [SerializeField] private Image[] allCircleImages = new Image[0];

    [SerializeField] private Image[] allColorChangeAllowCircleImages;
    
    private float[] allCircleImagesStartTransparency;
    [SerializeField] private float colorChangeSpeed = 15f;
    private float currentCircleTransparency = 1f;

    private Color startIndicatorColor;
    
    private bool circleIsActive =>
        hookService.IsHookStrengthRegenerationActive || 
        hookService.IsHookUsed || hookService.IsAfterUseTimerActive;

    private void Start()
    {
        SetStartColor();
        
        CopyImagesMaterials();
        
        if (!isVanish)
            return;
        
        SetAllCircleImagesStartAlpha();
        
        void SetAllCircleImagesStartAlpha()
        {
            allCircleImagesStartTransparency = new float[allCircleImages.Length];

            for (int i = 0; i < allCircleImages.Length; i++)
            {
                var item = allCircleImages[i];

                allCircleImagesStartTransparency[i] = item.color.a;

            }

        }

        void SetStartColor()
        {
            startIndicatorColor = allColorChangeAllowCircleImages[0].material.GetColor(MainColorShadeId);
        }

        void CopyImagesMaterials()
        {
            foreach (var localImage in allColorChangeAllowCircleImages)
            {
                var newMat = localImage.material;

                localImage.material = new Material(newMat);
            }
        }
    }

    private bool hookManageIsBlocked = false;
    
    private static readonly int MainColorShadeId = Shader.PropertyToID("_MainColor");

    private void Update()
    {
        if (!isVanish)
        {
            if(circleIsActive)
                UpdateHookCircle();

            return;
        }

        if (circleIsActive)
        {
            UpdateHookCircle();

            SmoothnesSetImagesTransparency(1);
        }
        else
        {
            SmoothnesSetImagesTransparency(0);
        }    
    }

    private void UpdateHookCircle()
    {
        float realFillAmount = hookService.HookCurrentStrength / hookService.HookMaxStrength;
        
        UpdateFillAmount();
        
        UpdateColor();
        
        void UpdateFillAmount()
        {
            float timeStep = 15f * Time.deltaTime;
            realFillAmount = hookService.HookCurrentStrength / hookService.HookMaxStrength;
            float smoothneess = 0.25f * (realFillAmount - 0.5f);

            float nowFillAmount = hookStrengthCircle.fillAmount;
            nowFillAmount = Mathf.Lerp(nowFillAmount, realFillAmount - smoothneess, timeStep);

            hookStrengthCircle.fillAmount = nowFillAmount;
        }

        void UpdateColor()
        {
            bool isStrengthTooSmall = realFillAmount < hookService.MinStrengthAmountToUse;

            var targetColor = !isStrengthTooSmall ? startIndicatorColor : new Color(0.85f, 0f, 0.02f, 1f);

            SetCircleColorSmoothness(allColorChangeAllowCircleImages,targetColor);

            void SetCircleColorSmoothness(Image[] images,Color targetColor)
            {
                var newColor = images[0].material.GetColor(MainColorShadeId);

                var timeStep = Time.deltaTime * colorChangeSpeed;
                newColor = Color.Lerp(newColor, targetColor, timeStep);                
                
                foreach (var localImage in images)
                {
                    localImage.material.SetColor(MainColorShadeId,newColor);
                }
            }
        }
    }

    private void SmoothnesSetImagesTransparency(float transparencyAmount)
    {
        const float circleTransparencyChangeSpeed = 5f;

        float timeStep = Time.deltaTime * circleTransparencyChangeSpeed;

        currentCircleTransparency = 
            Mathf.Lerp(currentCircleTransparency, transparencyAmount, timeStep);

        SetImagesTransparency(currentCircleTransparency);
    }

    private void SetImagesTransparency(float transparencyAmount)
    {
        for (int i = 0; i < allCircleImages.Length; i++)
        {
            var item = allCircleImages[i];

            var newItemColor = item.color;

            var newItemTransparency = 
                allCircleImagesStartTransparency[i] * transparencyAmount;

            newItemColor.a = newItemTransparency;

            allCircleImages[i].color = newItemColor;
        }
    }

}
