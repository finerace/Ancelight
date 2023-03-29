using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SuitManageMenuIndicatorsSetService : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMain;

    [Space]

    [SerializeField] private TextMeshProUGUI healthProcentCount;
    [SerializeField] private TextMeshProUGUI healthNow_MaxCount;
    [SerializeField] private Image healthCircle;

    [SerializeField] private TextMeshProUGUI armorPercentagesCount;
    [SerializeField] private TextMeshProUGUI armorNowMaxCount;
    [SerializeField] private Image armorCircle;

    [SerializeField] private TextMeshProUGUI commonSuitStatePercentages;

    [Space]

    [SerializeField] private TextMeshProUGUI hookFullRegenerationTime;
    [SerializeField] private TextMeshProUGUI hookActionRange;
    [SerializeField] private TextMeshProUGUI hookMaxPowerToBrake;
    
    [SerializeField] private Image hookCircle;

    [Space]

    [SerializeField] private TextMeshProUGUI dashCount;
    [SerializeField] private TextMeshProUGUI dashPower;
    [SerializeField] private TextMeshProUGUI dashOneRegenerationTime;
    [SerializeField] private Image dashCircle;

    [Space]

    [SerializeField] private TextMeshProUGUI imidProtectionRegenerationTime;
    [SerializeField] private TextMeshProUGUI imidProtectionAttackRange;
    [SerializeField] private TextMeshProUGUI imidProtectionAttackDamage;
    [SerializeField] private Image imidProtectionCircle;

    private void Awake()
    {
        playerMain = FindObjectOfType<PlayerMainService>();
    }

    private void OnEnable()
    {
        UpdateIndicators();
    }

    public void UpdateIndicators()
    {
        SetCommonSuitIndicator();
        
        SetHealthIndicators();
        
        SetArmorIndicators();
        
        SetHookIndicators();
        
        SetDashIndicators();
        
        SetImmediatelyProtectionIndicator();

        void SetCommonSuitIndicator()
        {
            var commonSuitStatePercentagesCount = 
                ((playerMain.Health_ / playerMain.MaxHealth_) + (playerMain.Armor / playerMain.MaxArmor)) / 2f * 100;
            
            var commonSuitStateAmountSmoothness = (int)(commonSuitStatePercentagesCount * 100f) / 100f;

            commonSuitStatePercentages.text = $"{commonSuitStateAmountSmoothness}%";
        }
        
        void SetHealthIndicators()
        {
            var playerHealth = playerMain.Health_;
            var playerMaxHealth = playerMain.MaxHealth_;

            var playerHealthAmount = playerHealth / playerMaxHealth;
            var playerHealthPercentages = (int)(playerHealthAmount * 100f) / 100f * 100f;
                        
            healthProcentCount.text = $"{playerHealthPercentages}%";
            healthNow_MaxCount.text = 
                $"{playerHealth.ClampToTwoRemainingCharacters()}/{playerMaxHealth.ClampToTwoRemainingCharacters()}";

            const float healthCircleReduceAmount = 0.26f;

            healthCircle.fillAmount = ReduceCircleAmount(playerHealthAmount,healthCircleReduceAmount);

        }
        
        void SetArmorIndicators()
        {
            var playerArmor = playerMain.Armor;
            var playerMaxArmor = playerMain.MaxArmor;
            
            var playerArmorAmount = playerArmor / playerMaxArmor;
            var playerArmorPercentages = (int)(playerArmorAmount * 100) / 100f * 100f;
                        
            armorPercentagesCount.text = $"{playerArmorPercentages}%";
            armorNowMaxCount.text = 
                $"{playerArmor.ClampToTwoRemainingCharacters()}/{playerMaxArmor.ClampToTwoRemainingCharacters()}";
            
            const float armorCircleReduceAmount = 0.26f;

            armorCircle.fillAmount = ReduceCircleAmount(playerArmorAmount,armorCircleReduceAmount);
        }

        void SetHookIndicators()
        {
            var playerHookStrength = playerMain.hookService.HookCurrentStrength;
            var playerMaxHookStrength = playerMain.hookService.HookMaxStrength;

            var playerHookStrengthAmount = playerHookStrength / playerMaxHookStrength;
            var playerHookFullRegenerationTime =
                playerMaxHookStrength / playerMain.hookService.HookStrengthRegenerationPerSecond;
            
            var playerHookRegenerationTimeSmoothness = (int)(playerHookFullRegenerationTime * 100) / 100f;
                        
            hookActionRange.text = $"{playerMain.hookService.HookMaxActionRange}m";
            hookFullRegenerationTime.text = $"{playerHookRegenerationTimeSmoothness}s";
            
            const float hookCircleReduceAmount = 0.26f;

            hookCircle.fillAmount = ReduceCircleAmount(playerHookStrengthAmount,hookCircleReduceAmount);
        }
        
        void SetDashIndicators()
        {
            
            var playerDashCount = playerMain.dashsService.DashsCount;

            var playerDashEnergy = playerMain.dashsService.DashCurrentEnergy;
            var playerDashMaxEnergy = playerMain.dashsService.DashMaxEnergy;

            var playerDashEnergyAmount = playerDashEnergy / playerDashMaxEnergy;
            var playerDashPower = (int)(playerMain.dashsService.DashPower * 100) / 100f;
            var playerOneDashRegenerationTime = 
                playerMain.dashsService.OneDashEnergySpend / playerMain.dashsService.DashsRegenerationSpeed;

            var playerOneDashRegenerationTimeSmoothness = (int)(playerOneDashRegenerationTime * 100) / 100f;
            
            dashCount.text = $"{playerDashCount}";
            dashPower.text = $"{playerDashPower}";
            dashOneRegenerationTime.text = $"{playerOneDashRegenerationTimeSmoothness}s";
            
            const float dashCircleReduceAmount = 0.26f;

            dashCircle.fillAmount = ReduceCircleAmount(playerDashEnergyAmount,dashCircleReduceAmount);
        }
        
        void SetImmediatelyProtectionIndicator()
        {
            var immediatelyProtectionService =
                playerMain.immediatelyProtectionService;
            
            var playerImmProtectionCooldownTime = immediatelyProtectionService.CooldownTime;
            var playerImmProtectionAttackRange = immediatelyProtectionService.ExplosionRadius;
            var playerImmProtectionAttackDamage = immediatelyProtectionService.ExplosionDamage;

            imidProtectionRegenerationTime.text = $"{playerImmProtectionCooldownTime}s";
            imidProtectionAttackDamage.text = $"{playerImmProtectionAttackDamage}";
            imidProtectionAttackRange.text = $"{playerImmProtectionAttackRange}m";
                        
            const float imedProtectionCircleReduceAmount = 0.26f;

            var imedProtectionPowerAmount = 1 -
                immediatelyProtectionService.CooldownTimer / immediatelyProtectionService.CooldownTime;
            
            imidProtectionCircle.fillAmount = 
                ReduceCircleAmount(imedProtectionPowerAmount,imedProtectionCircleReduceAmount);
        }
    }

    private float ReduceCircleAmount(float originalAmount,float reduceCount)
    {

        var substructAmount = reduceCount * (originalAmount - 0.5f);

        return originalAmount - substructAmount;

    }
}
