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
    private float[] allCircleImagesStartTransparency;
    [SerializeField] private float transparencyAmount = 0;
    private float currentCircleTransparency = 1f;

    private bool circleIsActive 
    { 
        get => 
            hookService.IsHookStrengthRegenerationActive || 
            hookService.IsHookUsed || hookService.IsAfterUseTimerActive; 
    }

    private void Awake()
    {
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
    }

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

        float timeStep = 15f * Time.deltaTime;
        float realFillAmount = hookService.HookCurrentStrength / hookService.HookMaxStrength;
        float smoothneess = 0.25f * (realFillAmount - 0.5f);

        float nowFillAmount = hookStrengthCircle.fillAmount;
        nowFillAmount = Mathf.Lerp(nowFillAmount, realFillAmount - smoothneess, timeStep);

        hookStrengthCircle.fillAmount = nowFillAmount;
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

            Color newItemColor = item.color;

            float newItemTransparency = 
                allCircleImagesStartTransparency[i] * transparencyAmount;

            newItemColor.a = newItemTransparency;

            allCircleImages[i].color = newItemColor;
        }

    }

}
