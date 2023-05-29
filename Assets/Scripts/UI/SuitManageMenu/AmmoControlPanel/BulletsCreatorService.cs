using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCreatorService : MonoBehaviour
{

    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private TMP_InputField bulletCountInputField;

    [SerializeField] private Image bulletIcon;
    [SerializeField] private Sprite defaultBulletIcon;
    [SerializeField] private TextMeshProUGUI bulletsCurrentMaxIndicator;
    
    [SerializeField] private TextMeshProUGUI yellowPlasmaCreateCostText;
    [SerializeField] private TextMeshProUGUI redPlasmaCreateCostText;
    [SerializeField] private TextMeshProUGUI bluePlasmaCreateCostText;

    [SerializeField] private int selectedBulletId;
    
    private static readonly int TargetTexturePropId = Shader.PropertyToID("_TargetTexture2");

    [Space]
    
    private AudioPoolService audioPoolService;
    [SerializeField] private AudioCastData onUnsuccessfulBulletCreate;
    
    private void Start()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        bulletCountInputField.onValueChanged.AddListener(BulletsCostUpdate);
        
        audioPoolService = AudioPoolService.audioPoolServiceInstance;

        selectedBulletId = 0;
        bulletIcon.material.SetTexture(TargetTexturePropId,defaultBulletIcon.texture);
    }

    private void OnEnable()
    {
        if(selectedBulletId == 0)
            return;
            
        InitializeNewSelectedBullet(selectedBulletId);
        
        BulletsCurrentMaxIndicatorUpdate();
    }

    public void InitializeNewSelectedBullet(int bulletId)
    {
        if(bulletId <= 0)
            return;
        
        selectedBulletId = bulletId;
        
        var selectedBulletData = playerMainService.weaponsBulletsManager.FindData(bulletId);
        bulletIcon.material.SetTexture(TargetTexturePropId,selectedBulletData.Icon.texture);
        
        BulletsCurrentMaxIndicatorUpdate();
        
        BulletsCostUpdate(bulletCountInputField.text);
    }

    private void BulletsCostUpdate(string value)
    {   
        if(selectedBulletId <= 0)
            return;
        
        var selectedBulletData = playerMainService.weaponsBulletsManager.FindData(selectedBulletId);

        var createBulletsCount = 0;
        
        if(value != string.Empty)
            createBulletsCount = Convert.ToInt32(value);
        
        var yellowPlasmaCreateCost = selectedBulletData.YellowPlasmaCreateCost * createBulletsCount;
        var redPlasmaCreateCost = selectedBulletData.RedPlasmaCreateCost * createBulletsCount;
        var bluePlasmaCreateCost = selectedBulletData.BluePlasmaCreateCost * createBulletsCount;
        
        yellowPlasmaCreateCostText.text = $"{yellowPlasmaCreateCost}Y";
        redPlasmaCreateCostText.text = $"{redPlasmaCreateCost}R";
        bluePlasmaCreateCostText.text = $"{bluePlasmaCreateCost}B";
    }

    private void BulletsCurrentMaxIndicatorUpdate()
    {
        var bulletId = selectedBulletId;
        
        var bulletManager = playerMainService.weaponsBulletsManager;
        
        var selectedBulletsCount = bulletManager.BulletsCount[bulletId];
        var selectedBulletMax = bulletManager.BulletsMax[bulletId];    
        
        bulletsCurrentMaxIndicator.text = $"{selectedBulletsCount}/{selectedBulletMax}";
    }

    public void CreateBullets()
    {
        if (selectedBulletId <= 0)
        {
            audioPoolService.CastAudio(onUnsuccessfulBulletCreate);
            
            return;
        }

        var selectedBulletData = playerMainService.weaponsBulletsManager.FindData(selectedBulletId);
        var bulletManager = playerMainService.weaponsBulletsManager;
        
        var bulletCountToCreate = bulletCountInputField.text;
        
        var createBulletsCount = 0;
        
        if(bulletCountToCreate != string.Empty)
            createBulletsCount = Convert.ToInt32(bulletCountToCreate);

        if (createBulletsCount == 0)
        {
            audioPoolService.CastAudio(onUnsuccessfulBulletCreate);
            return;
        }

        var selectedBulletCount = bulletManager.BulletsCount[selectedBulletId];

        var createBulletsToMany = selectedBulletCount + createBulletsCount > selectedBulletData.MaxBullets;

        string resultBulletCountInputFieldText;
        
        var oldCreateBulletCount = createBulletsCount;
        
        if (createBulletsToMany)
        {
            createBulletsCount = selectedBulletData.MaxBullets - selectedBulletCount;
            
            var remainingBullets = selectedBulletCount + oldCreateBulletCount - selectedBulletData.MaxBullets;
            
            resultBulletCountInputFieldText = remainingBullets + "";
        }
        else
        {
            resultBulletCountInputFieldText = 0 + "";
        }

        createBulletsCount = CheckToCreateBulletsCostOnPlasmaCounts();
        
        if(createBulletsCount == 0)
        {
            audioPoolService.CastAudio(onUnsuccessfulBulletCreate);
            return;
        }

        if (createBulletsCount < oldCreateBulletCount)
        {
            resultBulletCountInputFieldText = (oldCreateBulletCount - createBulletsCount) + "";
        }
        
        var yellowPlasmaCreateCost = selectedBulletData.YellowPlasmaCreateCost * createBulletsCount;
        var redPlasmaCreateCost = selectedBulletData.RedPlasmaCreateCost * createBulletsCount;
        var bluePlasmaCreateCost = selectedBulletData.BluePlasmaCreateCost * createBulletsCount;
        
        bulletManager.SubtractPlasma("yellow",yellowPlasmaCreateCost);
        bulletManager.SubtractPlasma("red",redPlasmaCreateCost);
        bulletManager.SubtractPlasma("blue",bluePlasmaCreateCost);
        
        bulletManager.AddBullets(selectedBulletId,createBulletsCount);

        bulletCountInputField.text = resultBulletCountInputFieldText;
        
        BulletsCurrentMaxIndicatorUpdate();

        int CheckPlasmaCost(string plasmaId, int bulletsCount, float bulletCreateCost)
        {
            if (bulletCreateCost == 0)
                return bulletsCount;
            
            var allBulletsCreateCost = bulletsCount * bulletCreateCost;

            var plasmaReserves = bulletManager.PlasmaReserves[plasmaId];
            
            if (plasmaReserves - allBulletsCreateCost < 0)
            {
                var allowBulletsCount = 0;
                
                allowBulletsCount = (int)(plasmaReserves / bulletCreateCost);
                
                return allowBulletsCount;
            }
            
            return bulletsCount;
        }

        int CheckToCreateBulletsCostOnPlasmaCounts()
        {
            var maxYellowCreateBulletsCount =
                CheckPlasmaCost("yellow", createBulletsCount, selectedBulletData.YellowPlasmaCreateCost);
        
            var maxRedCreateBulletsCount =
                CheckPlasmaCost("red", createBulletsCount, selectedBulletData.RedPlasmaCreateCost);

            var maxBlueCreateBulletsCount =
                CheckPlasmaCost("blue", createBulletsCount, selectedBulletData.BluePlasmaCreateCost);

            if (maxYellowCreateBulletsCount <= maxRedCreateBulletsCount &&
                maxYellowCreateBulletsCount <= maxBlueCreateBulletsCount)
            {
                return maxYellowCreateBulletsCount;
            }
            
            if (maxRedCreateBulletsCount <= maxYellowCreateBulletsCount &&
                     maxRedCreateBulletsCount <= maxBlueCreateBulletsCount)
            {
                return maxRedCreateBulletsCount;
            }
            
            return maxBlueCreateBulletsCount;
            
        }
        
    }
    
}
