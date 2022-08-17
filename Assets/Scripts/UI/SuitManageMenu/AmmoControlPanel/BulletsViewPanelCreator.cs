using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BulletsViewPanelCreator : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private BulletsCreatorService bulletsCreatorService;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject bulletViewPanelPrefab;
    [SerializeField] private UIObjectsScrollingService viewPanelsScrollService; 
    
    private PlayerWeaponsManager playerWeaponsManager;
    private PlayerWeaponsBulletsManager playerWeaponsBulletsManager;
    private static readonly int TargetTexture2 = Shader.PropertyToID("_TargetTexture2");
    private static readonly int MainColor = Shader.PropertyToID("_MainColor");

    private readonly Dictionary<int, Image> bulletCountIndicators = new Dictionary<int, Image>();

    public IDictionary<int, Image> BulletCountIndicators => bulletCountIndicators;

    private void Start()
    {
        CreateNewViewPanels();
    }

    private void CreateNewViewPanels()
    {
        var playerWeaponManager = playerMainService.weaponsManager;
        var playerBulletManager = playerMainService.weaponsBulletsManager;

        bulletCountIndicators.Clear();
        
        foreach (var localWeaponID in playerWeaponManager.WeaponsUnlockedIDs)
        {
            var localWeaponData = playerWeaponManager.FindWeaponData(localWeaponID);
            
            if(localWeaponData.BulletsID == 0)
                continue;
                        
            var localBulletData = playerBulletManager.FindData(localWeaponData.BulletsID);

            var isLocalBulletUnlocked = playerBulletManager.IsIdUnlocked(localWeaponData.BulletsID);
            
            if(!isLocalBulletUnlocked)
                continue;

            
            var bulletViewPanelData = Instantiate(bulletViewPanelPrefab, spawnPoint)
                    .GetComponent<BulletViewPanelData>();

            var buttonService = bulletViewPanelData.gameObject.GetComponent<ButtonMainService>();

            UnityAction updateBulletsCreator = () => 
                bulletsCreatorService.InitializeNewSelectedBullet(localBulletData.Id);
            
            buttonService.onClickAction.AddListener(updateBulletsCreator);
            
            
            viewPanelsScrollService.AddScrollingObject(bulletViewPanelData.transform);
            
            bulletViewPanelData.bulletImage.material.SetTexture(TargetTexture2, localBulletData.Icon.texture);
            bulletViewPanelData.weaponImage.material.SetTexture(TargetTexture2,localWeaponData.Icon.texture);
            
            bulletViewPanelData.bulletImage.material.SetColor(MainColor, localWeaponData.MainColor);
            bulletViewPanelData.weaponImage.material.SetColor(MainColor, localWeaponData.MainColor);
            
            bulletViewPanelData.bulletImage.material = new Material(bulletViewPanelData.bulletImage.material);
            bulletViewPanelData.weaponImage.material = new Material(bulletViewPanelData.weaponImage.material);
            
            bulletViewPanelData.panelName.text = $"{localWeaponData.Name} Bullet";
            
            
            bulletCountIndicators.Add(localBulletData.Id,bulletViewPanelData.bulletCountIndicator);
        }
        
        
        
    }
    
}
