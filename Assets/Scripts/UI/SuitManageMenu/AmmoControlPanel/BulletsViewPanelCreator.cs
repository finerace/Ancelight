using System;
using System.Linq;
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
    
    private void OnEnable()
    {
        if(IsPlayerHasNewWeapon())
            CreateNewViewPanels();
    }

    private void CreateNewViewPanels()
    {
        var playerWeaponManager = playerMainService.weaponsManager;
        var playerBulletManager = playerMainService.weaponsBulletsManager;

        DeleteAllPreviousPanels();
        viewPanelsScrollService.RemoveAllScrollingObjects();
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
            
            viewPanelsScrollService.AddScrollingObject(bulletViewPanelData.transform);

            SetBulletSelectService();
            
            SetBulletsAmountIndicator();
            
            SetIconsAndColors();
            
            bulletViewPanelData.panelName.text = $"{localWeaponData.Name} Bullet";
            
            void SetBulletSelectService()
            {
                var buttonService = bulletViewPanelData.gameObject.GetComponent<ButtonMainService>();

                UnityAction updateBulletsCreator = () => 
                    bulletsCreatorService.InitializeNewSelectedBullet(localBulletData.Id);
            
                buttonService.onClickAction.AddListener(updateBulletsCreator);
            }
            
            void SetBulletsAmountIndicator()
            {
                var bulletAmountIndicator = bulletViewPanelData.bulletCountIndicator;

                var bulletsCount = playerBulletManager.BulletsCount[localBulletData.Id];
                var bulletMaxCount = playerBulletManager.BulletsMax[localBulletData.Id];

                var bulletsAmount = bulletsCount / bulletMaxCount;

                bulletAmountIndicator.fillAmount = bulletsAmount;
                
                bulletCountIndicators.Add(localBulletData.Id, bulletViewPanelData.bulletCountIndicator);
            }
            
            void SetIconsAndColors()
            {
                bulletViewPanelData.bulletImage.material.SetTexture(TargetTexture2, localBulletData.Icon.texture);
                bulletViewPanelData.weaponImage.material.SetTexture(TargetTexture2,localWeaponData.Icon.texture);
            
                bulletViewPanelData.bulletImage.material.SetColor(MainColor, localWeaponData.MainColor);
                bulletViewPanelData.weaponImage.material.SetColor(MainColor, localWeaponData.MainColor);
            
                bulletViewPanelData.bulletImage.material = new Material(bulletViewPanelData.bulletImage.material);
                bulletViewPanelData.weaponImage.material = new Material(bulletViewPanelData.weaponImage.material);
            }

        }

        void DeleteAllPreviousPanels()
        {
            foreach (Image item in bulletCountIndicators.Keys.Select(itemKey => bulletCountIndicators[itemKey]))
            {
                Destroy(item.transform.parent.gameObject);
            }
        }

    }

    private bool IsPlayerHasNewWeapon()
    {
        var playerBulletManager = playerMainService.weaponsBulletsManager;

        if (bulletCountIndicators.Count != playerBulletManager.UnlockedBulletsID.Count)
            return true;

        return bulletCountIndicators.Keys.Any(bulletId => !playerBulletManager.IsIdUnlocked(bulletId));
    }
    
}