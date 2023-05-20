using UnityEngine;
using System;

public class PlayerMainService : Health,IUsePlayerDevicesButtons
{
    [Space]
    [SerializeField] private float maxArmor;
    [SerializeField] private float armor;

    [SerializeField] private float armorDamageResistance = 2.5f;
    
    public float MaxArmor => maxArmor;
    public float Armor => armor;
    public float ArmorDamageResistance => armorDamageResistance;

    [Space]
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal PlayerWeaponsManager weaponsManager;
    [SerializeField] internal PlayerWeaponsBulletsManager weaponsBulletsManager;
    [SerializeField] internal PlayerRotation playerRotation;
    [SerializeField] internal PlayerLookService playerLook;
    [SerializeField] internal PlayerHookService hookService;
    [SerializeField] internal PlayerDashsService dashsService;
    [SerializeField] internal PlayerImmediatelyProtectionService immediatelyProtectionService;
    [SerializeField] internal PlayerUseService playerUseService;
    [SerializeField] internal PlayerCleaner playerCleaner;

    [Space] 
    
    [SerializeField] private bool godModeEnabled = false;
    
    [Space]
    
    [SerializeField] private MenuSystem menuSystem;
    
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform weaponPoint;
    [SerializeField] private Transform meleeAttackPoint;
    
    [SerializeField] private int suitImprovementPoints;

    [Space]
    
    [SerializeField] private Transform playerHands;
    
    public int SuitImprovementPoints
    {
        get => suitImprovementPoints;

        set
        {
            if (value < 0)
                throw new Exception("Improvement points cannot be less than zero!");

            suitImprovementPoints = value;
            OnImprovementPointsValueChange?.Invoke(suitImprovementPoints);
        }
    }
    
    public Transform ShootingPoint => shootingPoint;

    public Transform WeaponPoint => weaponPoint;

    public Transform MeleeAttackPoint => meleeAttackPoint;
    
    private bool isManageActive = true;

    public bool IsManageActive => isManageActive;
    
    public event Action<float> GetDamageEvent;
    public event Action<float> AddHealthEvent;
    public event Action<float> AddArmorEvent;
    public event Action<WeaponData> UnlockWeaponEvent; 
    public event Action<(int, string,float)> AddPlasmaEvent;
    public event Action<int> OnImprovementPointsValueChange; 
    
    public event Action<PlayerModules> OnSpecialModuleUnlock; 

    
    private DeviceButton openSuitManageMenuButton = new DeviceButton();
    
    private void Awake()
    {
        weaponsManager.Load(this);
    }

    private void Start()
    {
        CheckPlayerComponents();

        weaponsManager.NewWeaponEvent += 
            (WeaponData weaponData) => {UnlockWeaponEvent?.Invoke(weaponData);};
        
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }

    public void Load(PlayerMainService savedPlayerMainService)
    {
        SetHealth(savedPlayerMainService.Health_);
        armor = savedPlayerMainService.armor;

        suitImprovementPoints = savedPlayerMainService.suitImprovementPoints;
        maxArmor = savedPlayerMainService.MaxArmor;
        armorDamageResistance = savedPlayerMainService.ArmorDamageResistance;
    }
    
    private void FixedUpdate()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        CheckItems();
        
        void CheckItems()
        {
            var playerT = transform;
            var playerScale = new Vector3(0.5f,1.5f,0.5f);
            const int layerMask = 1 << 16;

            var colliders =
                // ReSharper disable once Unity.PreferNonAllocApi
                Physics.OverlapBox(playerT.position, playerScale, Quaternion.identity, layerMask);

            foreach (var localCollider in colliders)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                CheckObjectHasPlayerItem(localCollider.gameObject);
            }
        }
    }

    private void Update()
    {
       if (openSuitManageMenuButton.IsGetButtonDown())
          menuSystem.OpenLocalMenu("SuitManageMenu");
    }

    private void CheckObjectHasPlayerItem(GameObject checkObject)
    {
        if (checkObject.TryGetComponent(out IPlayerItem playerItem))
        {
            playerItem.PickUp(this);
        }
    }
    
    public void UnlockWeapon(int id)
    {
        weaponsManager.UnlockWeapon(id);
    }

    public void AddBullets(int id,int count)
    {
        weaponsBulletsManager.AddBullets(id, count);
    }

    public void AddPlasma(int textId,string id, float count)
    {
        weaponsBulletsManager.AddPlasma(id,count);

        AddPlasmaEvent?.Invoke((textId,id,count));

    }
    
    public override void GetDamage(float damage,Transform source)
    {
        if(godModeEnabled)
            return;
        
        if(armor >= damage)
        {
            armor -= damage/ (armorDamageResistance / 1.25f);
            damage /= armorDamageResistance;
        } 
        else if(armor < damage && armor > 0)
        {
            damage -= armor / armorDamageResistance;
            armor = 0;
        }

        if(GetDamageEvent != null)
            GetDamageEvent.Invoke(damage);

        base.GetDamage(damage);
    }
    
    public void AddArmor(float armor)
    {
        if(this.armor + armor >= maxArmor)
            this.armor = MaxArmor;
        else if (this.armor + armor <= 0)
            throw new ArgumentException("Armor value is less of zero!");
        else
            this.armor += armor;
        
        AddArmorEvent?.Invoke(armor);
    }
    
    public void SetMaxArmor(float newMaxArmor)
    {
        if (newMaxArmor < 0)
            throw new ArgumentException("Max armor cannot be less than zero!");

        maxArmor = newMaxArmor;
    }

    public void AddImprovementPoint(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Added improvement points not be could less than a one!");

        SuitImprovementPoints += value;
    }
    
    public void SetArmorDamageResistance(float newResistanceValue)
    {
        if (newResistanceValue < 0)
            throw new ArgumentException("Armor damage resistance cannot be less than zero!");

        armorDamageResistance = newResistanceValue;
    }

    public override void AddHealth(float health)
    {
        base.AddHealth(health);
        
        AddHealthEvent?.Invoke(health);
    }

    public void UnlockModule(PlayerModules module)
    {
        switch (module)
        {
            case PlayerModules.DashService:
                dashsService.Unlock();
                break;
            case PlayerModules.HookService:
                hookService.UnlockHook();
                break;
            case PlayerModules.ProtectionService:
                immediatelyProtectionService.Unlock();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(module), module, null);
        }
        
        OnSpecialModuleUnlock?.Invoke(module);
    }
    
    public void SetManageActive(bool state)
    {
        isManageActive = state;
        
        playerMovement.SetManageActive(state);
        weaponsManager.SetManageActive(state);
        playerRotation.SetManageActive(state);
        hookService.SetManageActive(state);
        dashsService.SetManageActive(state);
        immediatelyProtectionService.SetManageActive(state);
        playerUseService.SetManageActive(state);
    }

    public override void Died()
    {
        menuSystem.OpenLocalMenu("DiedMenu");
        menuSystem.isBackActionLock = true;
    }

    private void CheckPlayerComponents()
    {
        if (menuSystem == null)
            menuSystem = FindObjectOfType<MenuSystem>();
        
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        
        if (weaponsManager == null)
            weaponsManager = GetComponent<PlayerWeaponsManager>();
        
        if (weaponsBulletsManager == null)
            weaponsBulletsManager = GetComponent<PlayerWeaponsBulletsManager>();
        
        if (playerRotation == null)
            playerRotation = GetComponent<PlayerRotation>();
        
        if (playerLook == null)
            playerRotation = GetComponent<PlayerRotation>();
    }

    public DeviceButton[] GetUsesDevicesButtons()
    {
        return new DeviceButton[] {openSuitManageMenuButton} ;
    }

    public void SetPlayerHandsActive(bool state)
    {
        if(playerHands != null)
            playerHands.gameObject.SetActive(state);
    }
}

public enum PlayerModules
{
    DashService,
    HookService,
    ProtectionService
}
