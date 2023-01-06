using UnityEngine;
using System;

public class PlayerMainService : Health,IUsePlayerDevicesButtons
{
    [Space]
    [SerializeField] private float maxArmor;
    [SerializeField] private float armor;

    public float MaxArmor => maxArmor;
    public float Armor => armor;

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
    
    [SerializeField] private MenuSystem menuSystem;
    
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform weaponPoint;
    [SerializeField] private Transform meleeAttackPoint;

    public Transform ShootingPoint => shootingPoint;

    public Transform WeaponPoint => weaponPoint;

    public Transform MeleeAttackPoint => meleeAttackPoint;
    
    private bool isManageActive = true;

    public bool IsManageActive => isManageActive;
    
    public event Action<float> GetDamageEvent;
    public event Action<float> AddHealthEvent;
    public event Action<float> AddArmorEvent;
    public event Action<WeaponData> UnlockWeaponEvent; 
    public event Action<(string,float)> AddPlasmaEvent;

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

    public void AddPlasma(string id, float count)
    {
        weaponsBulletsManager.AddPlasma(id,count);

        AddPlasmaEvent?.Invoke((id,count));

    }
    
    public override void GetDamage(float damage,Transform source)
    {
        if(armor >= damage)
        {
            armor -= damage/2f;
            damage /= 2.5f;
        } 
        else if(armor < damage && armor > 0)
        {
            damage -= armor / 2.5f;
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
    
    public override void AddHealth(float health)
    {
        base.AddHealth(health);
        
        AddHealthEvent?.Invoke(health);
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

    }

    private void CheckPlayerComponents()
    {
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
}