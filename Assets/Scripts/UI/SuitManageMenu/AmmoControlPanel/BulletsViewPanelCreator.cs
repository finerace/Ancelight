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

    [SerializeField] private int bulletTextId;
    
    private readonly Dictionary<int, Image> bulletCountIndicators = new Dictionary<int, Image>();

    public IDictionary<int, Image> BulletCountIndicators => bulletCountIndicators;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

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
            
            bulletViewPanelData.panelName.text = 
                $"{CurrentLanguageData.GetText(localWeaponData.NameTextId)} {CurrentLanguageData.GetText(bulletTextId)}";
            
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
                bulletViewPanelData.bulletImage.sprite = localBulletData.Icon;
                bulletViewPanelData.weaponImage.sprite = localWeaponData.Icon;

                SetColorWithoutAlpha(bulletViewPanelData.bulletImage, localWeaponData.MainColor);
                SetColorWithoutAlpha(bulletViewPanelData.weaponImage, localWeaponData.MainColor);

                void SetColorWithoutAlpha(Image image, Color newColor)
                {
                    var resultColor = newColor;
                    resultColor.a = image.color.a;

                    var H = 1f;
                    var S = 1f;
                    var V = 1f;
                    
                    Color.RGBToHSV(resultColor, out H, out S, out V);
                    resultColor = Color.HSVToRGB(H, 0.45f, V);
                    
                    image.color = resultColor;
                }
                
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
