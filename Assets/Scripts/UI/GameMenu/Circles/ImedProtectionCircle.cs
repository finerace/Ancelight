using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImedProtectionCircle : MonoBehaviour
{
    [SerializeField] private PlayerImmediatelyProtectionService immedProtectionService;

    [SerializeField] private Image icon;
    [SerializeField] private Image dynamicCircle;
    private bool isProtectionExist = true;
    
    private void Awake()
    {
        immedProtectionService = FindObjectOfType<PlayerImmediatelyProtectionService>();

        if (!immedProtectionService.IsProtectionExist)
        {
            transform.SetChildsActive(false);
            isProtectionExist = false;

            immedProtectionService.OnUnlock += OnUnlock;
            
            void OnUnlock()
            {
                transform.SetChildsActive(true);
                isProtectionExist = true;
            }
        }
            
    }

    private void Update()
    {
        if(!isProtectionExist)
            return;
            
        UpdateHookCircle();
    }

    private void UpdateHookCircle()
    {
        var realFillAmount = 1 - (immedProtectionService.CooldownTimer / immedProtectionService.CooldownTime);
        
        UpdateFillAmount();
        SetIconTransparency();
        
        void UpdateFillAmount()
        {
            var smoothneess = 0.25f * (realFillAmount - 0.5f);

            realFillAmount = realFillAmount - smoothneess;
            
            dynamicCircle.fillAmount = realFillAmount;
        }

        void SetIconTransparency()
        {
            var iconTransparencyChangeSpeed = 30f;
            var timeStep = Time.deltaTime * iconTransparencyChangeSpeed;

            var targetTransparency = 1f;
            
            if (realFillAmount < 0.85f)
                targetTransparency = 0.2f;

            
            var targetColor = icon.color;
            targetColor.a = targetTransparency;
            
            var newColor = Color.Lerp(icon.color, targetColor,timeStep);
            icon.color = newColor;

        }
        
    }

}
