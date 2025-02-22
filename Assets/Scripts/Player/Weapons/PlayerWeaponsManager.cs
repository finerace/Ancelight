using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponsManager : MonoBehaviour,IUsePlayerDevicesButtons
{
    [SerializeField] private List<WeaponData> weaponsDatas = new List<WeaponData>();
    [SerializeField] private List<int> weaponsUnlockedIDs = new List<int>();
    [SerializeField] internal PlayerWeaponsBulletsManager bulletsManager;
    public event Action<WeaponData> NewWeaponEvent;
    public int currentBulletsCount;

    [Space]
    [SerializeField] private LayerMask layerMaskRayCastMode;
    public LayerMask LayerMaskRayCastMode {get => layerMaskRayCastMode;}

    [SerializeField] private int selectedWeapon = 0;
    [SerializeField] internal WeaponData selectedWeaponData;
    [SerializeField] internal Transform shootingPoint;
    [SerializeField] protected Transform[] shotGunShootingPoints;
    [SerializeField] internal Transform meleeShootingPoint;
    [SerializeField] internal Transform weaponPoint;

    public List<WeaponData> WeaponsDatas { get { return weaponsDatas; } }
    public List<int> WeaponsUnlockedIDs { get { return weaponsUnlockedIDs; } }
    public int SelectedWeapon { get { return selectedWeapon; } }
    public BulletData selectedBulletData;

    [Space]
    [SerializeField] private int selectedWeaponID = 0;
    [SerializeField] private float damage;
    [HideInInspector] [SerializeField] private bool isRaycastShot;
    [HideInInspector] [SerializeField] private bool isMelee;
    [HideInInspector] [SerializeField] private bool OneClick;
    [HideInInspector] [SerializeField] private float attackTime;
    [HideInInspector] [SerializeField] private bool endTimeShoot;
    private WeaponData.ShootingMode shootingMode;
    private bool isAttacking = false;

    [Space]
    private GameObject weaponPrefab;
    private GameObject bulletPrefab;
    private GameObject effectsPrefab;

    [Space]
    private GameObject nowWeaponPrefab;
    private GameObject nowEffectsPrefab;
    internal bool animatorAttackAllowed = false;
    internal bool animatorWeaponChangeAllowed = false;

    [HideInInspector] public bool IsAttacking { get { return isAttacking; } }
    [HideInInspector] public int SelectedWeaponID { get { return selectedWeaponID; } }
    [HideInInspector] public bool WeaponSpecialState { get { return weaponSpecialState; } }

    [HideInInspector] [SerializeField] private bool weaponSpecialState = false; //false = right true = left fist;
    [HideInInspector] public bool isBulletsHere { get { return 
                bulletsManager.CheckBullets(selectedWeaponData.BulletsID); } }

    [HideInInspector] [SerializeField] private bool oneClickState = false; //?????? ??? 1 ??? ????
    [HideInInspector] [SerializeField] private bool adjustableAttackAllow = false;
    [HideInInspector] [SerializeField] private bool isWeaponInCooldown = false;
    [SerializeField] private float weaponChangeCooldown = 0.20f;
    [HideInInspector] [SerializeField] private float adjustableAttackStartPower = 0.2f;

     private event Action ShotEvent;
     private event Action ShotPreparingEvent;
     private event Action WeaponChangeEvent;
     private event Action WeaponChangeButtonUseEvent;

     
     private event Action abilityUseEvent;
     public event Action AbilityUseEvent
     {
         add
         {
             if (value == null)
                 throw new NullReferenceException();

             abilityUseEvent += value;
         }

         remove
         {
             if (value == null)
                 throw new NullReferenceException();

             abilityUseEvent -= value;
         }
     }

     /*private event Action abilitySpecialSoundEvent;
     public event Action AbilitySpecialSoundEvent
     {
         add
         {
             if (value == null)
                 throw new NullReferenceException();

             abilitySpecialSoundEvent += value;
         }

         remove
         {
             if (value == null)
                 throw new NullReferenceException();

             abilitySpecialSoundEvent -= value;
         }
     }*/
     
    [Space]
    private bool isThereAnyBullets;

    private bool isManageActive = true;

    private float weaponCooldownTimer;

    private int mouseWheel;
    private int fire1;
    
    private DeviceButton fireButton = new DeviceButton();
    private DeviceButton nextWeaponButton = new DeviceButton();
    private DeviceButton previousWeaponButton = new DeviceButton();
    
    private DeviceButton useAbilityButton = new DeviceButton();
    private DeviceButton nextAbilityButton = new DeviceButton();
    private DeviceButton previousAbilityButton = new DeviceButton();
    
    public void Load(PlayerMainService playerMainService)
    {
        shootingPoint = playerMainService.ShootingPoint;
        shotGunShootingPoints = playerMainService.ShotGunPoints;
        meleeShootingPoint = playerMainService.MeleeAttackPoint;
        weaponPoint = playerMainService.WeaponPoint;
    }
    
    private void Start()
    {
        ShotEvent += () => {bulletsManager.SubtractOneBullet(selectedWeaponData.BulletsID);};

        layerMaskRayCastMode = 
            GameObject.Find("LayerMasks").GetComponent<LayerMasks>().PlayerShootingLayerMask;

        SetSelectedWeapon(selectedWeaponID);
    }
    
    private void Update()
    {
        if (!isManageActive)
            return;

        if (nextWeaponButton.IsGetButton())
            mouseWheel = 1;
        else if (previousWeaponButton.IsGetButton())
            mouseWheel = -1;
        else
            mouseWheel = 0;

        if (fireButton.IsGetButton())
            fire1 = 1;
        else
            fire1 = 0;

        if (!isWeaponInCooldown && !isAttacking)
        {
            if (mouseWheel < 0)
                PreviousWeapon();

            else if (mouseWheel > 0)
                NextWeapon();
        }
    }

    private void FixedUpdate()
    {
        WeaponCooldownWork();
        void WeaponCooldownWork()
        {
            if (weaponCooldownTimer > 0)
                weaponCooldownTimer -= Time.fixedDeltaTime;
            else
                isWeaponInCooldown = false;
        }
        
        isThereAnyBullets = bulletsManager.CheckBullets(selectedWeaponData.BulletsID);

        if (OneClick && fire1 <= 0) oneClickState = false;

        if (isThereAnyBullets)
        {
            switch (shootingMode)
            {
                case WeaponData.ShootingMode.Normal:

                    if (fire1 > 0 && !isAttacking && !oneClickState && animatorAttackAllowed)
                    {
                        StartAttack();

                        if (OneClick) oneClickState = true;
                    }

                    break;

                case WeaponData.ShootingMode.Adjustable:

                    if (fire1 > 0 && animatorAttackAllowed)
                    {
                        if (!isAttacking)
                        {
                            StartAdjustableAttack();
                            
                            if(ShotPreparingEvent != null)
                                ShotPreparingEvent.Invoke();
                        }

                        isAttacking = true;

                        float adjustableAttackSmoothness = 1.3f;

                        if (adjustableAttackStartPower < 1)
                        {
                            adjustableAttackStartPower += adjustableAttackSmoothness * Time.deltaTime;
                        }
                    }
                    else if (fire1 <= 0 && adjustableAttackAllow)
                    {
                        EndAdjustableAttack();
                    }

                    break;

                case WeaponData.ShootingMode.Lazer:

                    if (fire1 > 0 && !isAttacking && !oneClickState && animatorAttackAllowed)
                    {
                        StartAttack();

                        if (OneClick) oneClickState = true;
                    }

                    break;

                case WeaponData.ShootingMode.Shotgun:

                    if (fire1 > 0 && !isAttacking && !oneClickState && animatorAttackAllowed)
                    {
                        StartAttack();

                        if (OneClick) oneClickState = true;
                    }

                    break;
            }
        }

        currentBulletsCount = bulletsManager.GetBulletsCount(selectedWeaponData.BulletsID);
    }

    private void InitializedWeaponsFields()
    {
        weaponsUnlockedIDs.Sort();

        ChangeWeaponCooldown(weaponChangeCooldown);

        selectedWeaponID = selectedWeaponData.Id;
        damage = selectedWeaponData.Damage;
        isRaycastShot = selectedWeaponData.IsRaycast;
        isMelee = selectedWeaponData.IsMelee;
        OneClick = selectedWeaponData.OneClick;
        attackTime = selectedWeaponData.RateOfFire;
        endTimeShoot = selectedWeaponData.EndTimeShoot;
        shootingMode = selectedWeaponData.ShootingMode_;
        weaponPrefab = selectedWeaponData.WeaponPrefab;
        bulletPrefab = selectedWeaponData.BulletPrefab;
        effectsPrefab = selectedWeaponData.ShootingEffects;

        if (selectedWeaponData.BulletsID != 0)
            selectedBulletData = bulletsManager.FindData(selectedWeaponData.BulletsID);
        else selectedBulletData = null;

        float destroyTime = 0.08f;
        float spawnTime = 0.15f;
        StartCoroutine(DeletePrefabs(destroyTime));
        StartCoroutine(SpawnPrefabs(spawnTime));

        IEnumerator SpawnPrefabs(float time)
        {
            int thisWeaponID = selectedWeaponData.Id;
            yield return new WaitForSeconds(time);

            if (selectedWeaponData.Id == thisWeaponID)
            {
                if (weaponPrefab != null && nowWeaponPrefab == null)
                {
                    nowWeaponPrefab = Instantiate(weaponPrefab, weaponPoint);
                    nowWeaponPrefab.transform.parent = weaponPoint;
                    nowWeaponPrefab.transform.position = weaponPoint.position;
                }

                if (effectsPrefab != null && nowEffectsPrefab == null && nowWeaponPrefab != null)
                {
                    nowEffectsPrefab = Instantiate(weaponPrefab, weaponPoint);
                    nowEffectsPrefab.transform.parent = nowWeaponPrefab.transform;
                    nowEffectsPrefab.transform.position = nowWeaponPrefab.transform.position;
                }
            }
        }

        IEnumerator DeletePrefabs(float time)
        {
            int thisWeaponID = selectedWeaponData.Id;
            yield return new WaitForSeconds(time);

            if (selectedWeaponData.Id == thisWeaponID)
            {
                if (nowWeaponPrefab != null) Destroy(nowWeaponPrefab);
                if (nowEffectsPrefab != null) Destroy(nowEffectsPrefab);
            }

        }
    }

    private void ChangeWeaponCooldown(float time)
    {
        weaponCooldownTimer = time;
        isWeaponInCooldown = true;
    }

    private void NextWeapon()
    {
        if (weaponsUnlockedIDs.Count >= 1)
        {
            selectedWeapon++;

            if (selectedWeapon >= weaponsUnlockedIDs.Count) 
                selectedWeapon = 0;

            WeaponData nowWeaponData = FindWeaponData(weaponsUnlockedIDs[selectedWeapon]);

            if (nowWeaponData.Id != selectedWeaponData.Id)
            {
                if(WeaponChangeEvent != null)
                    WeaponChangeEvent.Invoke();
            }

            selectedWeaponData = nowWeaponData;
            InitializedWeaponsFields();
            
            if (WeaponChangeButtonUseEvent != null) 
                WeaponChangeButtonUseEvent.Invoke();
        }
    }

    private void PreviousWeapon()
    {
        if (weaponsUnlockedIDs.Count >= 1)
        {
            selectedWeapon--;

            if (selectedWeapon < 0)
                selectedWeapon = weaponsUnlockedIDs.Count-1;
            
            WeaponData nowWeaponData = FindWeaponData(weaponsUnlockedIDs[selectedWeapon]);
            
            if (nowWeaponData.Id != selectedWeaponData.Id)
            {
                if(WeaponChangeEvent != null)
                    WeaponChangeEvent.Invoke();
            }
            
            selectedWeaponData = nowWeaponData;
            InitializedWeaponsFields();
            if (WeaponChangeButtonUseEvent != null) WeaponChangeButtonUseEvent.Invoke();
        }
    }

    private void SetSelectedWeapon(int id)
    {
        for (int i = 0; i < weaponsUnlockedIDs.Count; i++)
        {
            if (weaponsUnlockedIDs[i] == id)
            {
                selectedWeapon = i;
                break;
            }
        }

        if (weaponsUnlockedIDs.Count >= 1)
        {
            if (selectedWeapon >= weaponsUnlockedIDs.Count)
                selectedWeapon = 0;

            var oldWeaponDataId = 0;
            if(selectedWeaponData != null)
                oldWeaponDataId = selectedWeaponData.Id;
            
            selectedWeaponData = FindWeaponData(weaponsUnlockedIDs[selectedWeapon]);
            
            if(selectedWeaponData != null)
                if (oldWeaponDataId != selectedWeaponData.Id)
                {
                    if(WeaponChangeEvent != null)
                        WeaponChangeEvent.Invoke();
                }
            
            InitializedWeaponsFields();
            if(WeaponChangeButtonUseEvent != null) WeaponChangeButtonUseEvent.Invoke();
        }
    }

    public WeaponData FindWeaponData(int id)
    {
        for (int i = 0; i < weaponsDatas.Count; i++)
        {
            if (weaponsDatas[i].Id == id)
                return weaponsDatas[i];
        }

        throw new Exception($"?????? ??? ???? {id} ?? ???????!");
    }

    public void StartAttack()
    {
        isAttacking = true;

        StartCoroutine(StartAttack_(endTimeShoot));
    }

    private IEnumerator StartAttack_(bool endTimeShoot)
    {
        isAttacking = true;

        int nowWeaponID = selectedWeaponData.Id;

        if (!endTimeShoot)
        {
            Shot();
        }

        yield return new WaitForSeconds(attackTime);

        if (endTimeShoot && selectedWeaponData.Id == nowWeaponID)
        {
            Shot();
        }

        float fire1 = 0;

        if (fireButton.IsGetButton())
            fire1 = 1;

        if (shootingMode != WeaponData.ShootingMode.Lazer) 
            isAttacking = false;

        else if (fire1 > 0 && isThereAnyBullets)  
        {
            StartAttack();
        }
        else isAttacking = false;

        weaponSpecialState = !weaponSpecialState;
    }

    public void StartAdjustableAttack()
    {
        StartCoroutine(StartAdjustableAttack_());
    }

    private IEnumerator StartAdjustableAttack_()
    {

        adjustableAttackAllow = false;

        yield return new WaitForSeconds(attackTime);

        adjustableAttackAllow = true;
        weaponSpecialState = true;

    }

    public void EndAdjustableAttack()
    {
        if (bulletPrefab != null)
        {
            AdjustableShot();
        }

        isAttacking = false;
        weaponSpecialState = false;
        adjustableAttackAllow = false;
        adjustableAttackStartPower = 0.2f;
    }

    public void UnlockWeapon(int id)
    {
        if (!CheckIsUnlockedID(id))
        {
            weaponsUnlockedIDs.Add(id);
            weaponsUnlockedIDs.Sort();
            WeaponData newWeapon = FindWeaponData(id);

            if (newWeapon.BulletsID != 0)
                bulletsManager.UnlockBullet(newWeapon.BulletsID);

            NewWeaponEvent.Invoke(FindWeaponData(id));

            if (!isAttacking)
            {
                SetSelectedWeapon(id);
            }
            else
            {
                if (id < selectedWeaponID)
                    selectedWeapon++;
            }
            
        }
    }

    public bool CheckIsUnlockedID(int id)
    {
        foreach (var item in weaponsUnlockedIDs)
        {
            if(item == id)
                return true;
        }

        return false;
    }

    public void SubscribeShotEvent(Action action)
    {
        if (action != null)
            ShotEvent += action;
    }

    public void UnsubShotEvent(Action action)
    {
        if (action != null)
            ShotEvent -= action;
    }

    public void SubWeaponChangeUseButtonEvent(Action action)
    {
        if (action != null)
            WeaponChangeButtonUseEvent += action;
    }

    public void UnsubWeaponChangeUseButtonEvent(Action action)
    {
        if (action != null)
            WeaponChangeButtonUseEvent -= action;
    }
    
    public void SubWeaponChangeEvent(Action action)
    {
        if (action != null)
            WeaponChangeEvent += action;
    }

    public void UnsubWeaponChangeEvent(Action action)
    {
        if (action != null)
            WeaponChangeEvent -= action;
    }
    
    public void SubscribeShotPreparingEvent(Action action)
    {
        if (action != null)
            ShotPreparingEvent += action;
    }

    public void UnsubShotPreparingEvent(Action action)
    {
        if (action != null)
            ShotPreparingEvent -= action;
    }

    private void Shot()
    {
        if (bulletPrefab != null)
        {
            if (shootingMode == WeaponData.ShootingMode.Shotgun)
            {
                ShotGunShoot(shotGunShootingPoints);
                
                return;
            }

            if (!isRaycastShot && !isMelee)
            {
                OrdinaryShot(shootingPoint);
            }
            else if (isRaycastShot && !isMelee)
            {
                RaycastShot(shootingPoint);
            }
            else if (!isRaycastShot && isMelee)
            {
                OrdinaryShot(meleeShootingPoint);
            }
            else if (isRaycastShot && isMelee)
            {
                RaycastShot(meleeShootingPoint);
            }

            void OrdinaryShot(Transform shotStartPoint)
            {
                Transform currentShotPoint = shotStartPoint;

                Instantiate(bulletPrefab, currentShotPoint.position, currentShotPoint.rotation)
                    .GetComponent<Bullet>().SetDamage(damage);
            }

            void RaycastShot(Transform shotStartPoint)
            {
                Transform currentShotPoint = shotStartPoint;

                Ray shotRay = new Ray(currentShotPoint.position, currentShotPoint.forward);
                RaycastHit raycastHit;
                
                if (Physics.Raycast(shotRay, out raycastHit,300,layerMaskRayCastMode))
                {
                    var smooth = 2f;
                    Vector3 truePos = raycastHit.point - (currentShotPoint.forward / smooth);

                    Instantiate(bulletPrefab, truePos, currentShotPoint.rotation)
                    .GetComponent<Bullet>().SetDamage(damage);
                }
            }

            void ShotGunShoot(Transform[] shotStartPoints)
            {
                foreach (var shotPoint in shotStartPoints)
                {
                    Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation)
                        .GetComponent<Bullet>().SetDamage(damage/shotStartPoints.Length);
                }
                
                ShotEvent?.Invoke();
            }
            
            ShotEvent?.Invoke();

        }
    }

    private void AdjustableShot()
    {
        if (bulletPrefab != null)
        {
            Bullet nowBullet;

            nowBullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation)
                .GetComponent<Bullet>();

            if (ShotEvent != null) ShotEvent();
            nowBullet.SetDamage(damage);
            nowBullet.SetStartImpulsePower(adjustableAttackStartPower);
        }
    }

    public void SetManageActive(bool state)
    {
        isManageActive = state;
    }

    public DeviceButton[] GetUsesDevicesButtons()
    {
        var getButtons = new[]
            { fireButton, nextWeaponButton, previousWeaponButton,useAbilityButton,nextAbilityButton,previousAbilityButton};

        return getButtons;
    }
}
