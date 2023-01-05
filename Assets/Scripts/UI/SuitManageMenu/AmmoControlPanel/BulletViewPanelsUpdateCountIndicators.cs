using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletViewPanelsUpdateCountIndicators : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private BulletsViewPanelCreator viewPanelCreator;

    [SerializeField] private float indicatorsSpeed = 15f;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

    private void Update()
    {
        IndicatorsUpdate();        
    }

    private void IndicatorsUpdate()
    {
        var bulletManager = playerMainService.weaponsBulletsManager;
        var bulletsIndicator = viewPanelCreator.BulletCountIndicators;

        foreach (var bulletId in bulletsIndicator.Keys)
        {
            var bulletCount = bulletManager.BulletsCount[bulletId];
            var bulletMaxCount = bulletManager.FindData(bulletId).MaxBullets;

            var bulletCountAmount = (float)bulletCount / bulletMaxCount;

            var timeStep = Time.unscaledDeltaTime * indicatorsSpeed;

            bulletsIndicator[bulletId].fillAmount =
                Mathf.Lerp(bulletsIndicator[bulletId].fillAmount, bulletCountAmount, timeStep);
        }

    }
    
}
