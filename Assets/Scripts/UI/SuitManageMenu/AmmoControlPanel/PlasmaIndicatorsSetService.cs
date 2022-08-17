using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlasmaIndicatorsSetService : MonoBehaviour
{

    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private float fillSpeed = 25f;
    
    [Space] 
    
    [SerializeField] private Image yellowPlasmaEffect;
    [SerializeField] private Image yellowPlasmaLinesEffect;

    [SerializeField] private TextMeshProUGUI yellowPlasmaPercentageIndicator;
    [SerializeField] private TextMeshProUGUI yellowPlasmaCurrentMaxIndicator;
    
    [Space] 
    
    [SerializeField] private Image redPlasmaEffect;
    [SerializeField] private Image redPlasmaLinesEffect;

    [SerializeField] private TextMeshProUGUI redPlasmaPercentageIndicator;
    [SerializeField] private TextMeshProUGUI redPlasmaCurrentMaxIndicator;

    [Space] 
    
    [SerializeField] private Image bluePlasmaEffect;
    [SerializeField] private Image bluePlasmaLinesEffect;

    [SerializeField] private TextMeshProUGUI bluePlasmaPercentageIndicator;
    [SerializeField] private TextMeshProUGUI bluePlasmaCurrentMaxIndicator;

    private void Update()
    {
        UpdatePlasmaIndicators();
    }

    public void UpdatePlasmaIndicators()
    {
        UpdatePlasmaIndicator("yellow",yellowPlasmaEffect,yellowPlasmaLinesEffect,
            yellowPlasmaPercentageIndicator,yellowPlasmaCurrentMaxIndicator);
        
        UpdatePlasmaIndicator("red",redPlasmaEffect,redPlasmaLinesEffect,
            redPlasmaPercentageIndicator,redPlasmaCurrentMaxIndicator);

        UpdatePlasmaIndicator("blue",bluePlasmaEffect,bluePlasmaLinesEffect,
            bluePlasmaPercentageIndicator,bluePlasmaCurrentMaxIndicator);

        void UpdatePlasmaIndicator(string plasmaId, Image plasmaEffect, Image plasmaLinesEffect,
            TMP_Text percentageIndicator, TMP_Text currentMaxIndicator)
        {
            var bulletManager = playerMainService.weaponsBulletsManager;
            
            var plasmaCount = bulletManager.PlasmaReserves[plasmaId];
            var plasmaMaxCount = bulletManager.PlasmaMaxReserves[plasmaId];

            var plasmaAmount = plasmaCount / plasmaMaxCount;

            var timeStep = Time.unscaledDeltaTime * fillSpeed;
            
            plasmaEffect.fillAmount = Mathf.Lerp(plasmaEffect.fillAmount,plasmaAmount,timeStep) ;
            plasmaLinesEffect.fillAmount = Mathf.Lerp(plasmaLinesEffect.fillAmount,plasmaAmount,timeStep);

            percentageIndicator.text = (plasmaAmount.ClampToTwoRemainingCharacters() * 100f) + "%";

            currentMaxIndicator.text = 
                $"{plasmaCount.ClampToTwoRemainingCharacters()}/{plasmaMaxCount.ClampToTwoRemainingCharacters()}";
        }
    }
    
}
