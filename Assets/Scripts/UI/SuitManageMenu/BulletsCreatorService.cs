using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCreatorService : MonoBehaviour
{

    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private TMP_InputField bulletCountInputField;

    [SerializeField] private Image bulletIcon;
    [SerializeField] private TextMeshProUGUI bulletsCurrentMaxIndicator;
    
    [SerializeField] private TextMeshProUGUI yellowPlasmaCreateCostText;
    [SerializeField] private TextMeshProUGUI redPlasmaCreateCostText;
    [SerializeField] private TextMeshProUGUI bluePlasmaCreateCostText;

    [SerializeField] private int selectedBulletId;
    
    private static readonly int TargetTexturePropId = Shader.PropertyToID("_TargetTexture2");

    private void Start()
    {
        bulletCountInputField.onValueChanged.AddListener(BulletsCostUpdate);
    }


    private void OnEnable()
    {
        InitializeNewSelectedBullet(selectedBulletId);
        
        BulletsCurrentMaxIndicatorUpdate();
    }

    public void InitializeNewSelectedBullet(int bulletId)
    {
        selectedBulletId = bulletId;
        
        var selectedBulletData = playerMainService.weaponsBulletsManager.FindData(bulletId);
        
        bulletIcon.material.SetTexture(TargetTexturePropId,selectedBulletData.Icon.texture);
        
        BulletsCurrentMaxIndicatorUpdate();
        
        BulletsCostUpdate(bulletCountInputField.text);
    }

    private void BulletsCostUpdate(string value)
    {   
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
        var selectedBulletData = playerMainService.weaponsBulletsManager.FindData(selectedBulletId);
        var bulletManager = playerMainService.weaponsBulletsManager;
        
        var value = bulletCountInputField.text;
        
        var createBulletsCount = 0;
        
        if(value != string.Empty)
            createBulletsCount = Convert.ToInt32(value);
        
        if(createBulletsCount == 0)
            return;

        var selectedBulletCount = bulletManager.BulletsCount[selectedBulletId];

        var createBulletsToMany = selectedBulletCount + createBulletsCount > selectedBulletData.MaxBullets; 
        
        if (createBulletsToMany)
        {
            var oldCreateBulletCount = createBulletsCount;
            
            createBulletsCount = selectedBulletData.MaxBullets - selectedBulletCount;
            
            var remainingBullets = selectedBulletCount + oldCreateBulletCount - selectedBulletData.MaxBullets;
            
            bulletCountInputField.text = remainingBullets + "";
        }
        
        var yellowPlasmaCreateCost = selectedBulletData.YellowPlasmaCreateCost * createBulletsCount;
        var redPlasmaCreateCost = selectedBulletData.RedPlasmaCreateCost * createBulletsCount;
        var bluePlasmaCreateCost = selectedBulletData.BluePlasmaCreateCost * createBulletsCount;
        
        if(bulletManager.PlasmaReserves["yellow"] - yellowPlasmaCreateCost < 0)
            return;
        
        if(bulletManager.PlasmaReserves["red"] - redPlasmaCreateCost < 0)
            return;
        
        if(bulletManager.PlasmaReserves["blue"] - bluePlasmaCreateCost < 0)
            return;
        
        bulletManager.SubtractPlasma("yellow",yellowPlasmaCreateCost);
        bulletManager.SubtractPlasma("red",redPlasmaCreateCost);
        bulletManager.SubtractPlasma("blue",bluePlasmaCreateCost);
        
        bulletManager.AddBullets(selectedBulletId,createBulletsCount);
        
        BulletsCurrentMaxIndicatorUpdate();
    }
    
}
