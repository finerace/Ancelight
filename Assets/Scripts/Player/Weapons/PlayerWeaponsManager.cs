using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerWeaponsManager : MonoBehaviour
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
    [SerializeField] internal Transform meleeShootingPoint;
    [SerializeField] internal Transform weaponPoint;

    public List<WeaponData> WeaponsDatas { get { return weaponsDatas; } }
    public List<int> WeaponsUnlockedIDs { get { return weaponsUnlockedIDs; } }
    public int SelectedWeapon { get { return selectedWeapon; } }
    public BulletData selectedBulletData;

    [Space]
    [SerializeField] private int selectedWeaponID = 0;
    [SerializeField] private float damage;
    private bool isRaycastShot;
    private bool isMelee;
    private bool OneClick;
    private float attackTime;
    private bool endTimeShoot;
    [SerializeField] private WeaponData.ShootingMode shootingMode;
    [SerializeField] private bool isAttacking = false;

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

    private bool weaponSpecialState = false; //false = right true = left fist;
    [HideInInspector] public bool isBulletsHere { get { return 
                bulletsManager.CheckBullets(selectedWeaponData.BulletsID); } }

    private bool oneClickState = false; //?????? ??? 1 ??? ????
    private bool adjustableAttackAllow = false;
    private bool isWeaponInCooldown = false;
    [SerializeField] private float weaponChangeCooldown = 0.20f;
    private float adjustableAttackStartPower = 0.2f;

    private event Action ShotEvent;
    private event Action WeaponChange;

    [Space]
    [HideInInspector] public UnityEvent extraAbilityUseEvent;
    [HideInInspector] public UnityEvent extraAbilityChangeEvent;
    [SerializeField] private List<ExtraAbilityData> abilityDatas = new List<ExtraAbilityData>();
    public List<ExtraAbilityData> AbilityDatas { get { return abilityDatas; } }

    [SerializeField] private List<int> abilityUnlockedIDs = new List<int>(32);
    [SerializeField] private List<int> abilityInDelayIDs = new List<int>(16);
    [SerializeField] private ExtraAbilityData selectedAbilityData;
    private ExtraAbility selectedAbility;
    public ExtraAbility SelectedAbility { get { return selectedAbility; } }

    public ExtraAbilityData SelectedAbilityData { get { return selectedAbilityData; } }

    private int selectedAbilityNum;
    private GameObject nowAbilityPrefab;

    private bool extraAbilityOneClickState = false;
    [SerializeField] private bool isThereAnyBullets;

    private bool isManageActive = true;

    private void Start()
    {
        abilityUnlockedIDs.Sort();
        ShotEvent += () => {bulletsManager.SubtractOneBullet(selectedWeaponData.BulletsID);};

        layerMaskRayCastMode = 
            GameObject.Find("LayerMasks").GetComponent<LayerMasks>().PlayerShootingLayerMask;

        SetSelectedWeapon(0);
        SetSelectedAbility(0);
    }
    
    private void Update()
    {
        if (!isManageActive)
            return;

        float mouseWheel = Axes.MouseWheel;
        float fire1 = Axes.Fire1;
        bool useAbility = Input.GetKeyDown(KeyCode.Q);

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

                case WeaponData.ShootingMode.Particle:

                    if (fire1 > 0 && !isAttacking && !oneClickState && animatorAttackAllowed)
                    {
                        StartAttack();

                        if (OneClick) oneClickState = true;
                    }

                    break;
            }
        }

        currentBulletsCount = bulletsManager.GetBulletsCount(selectedWeaponData.BulletsID);

        if (AbilityDelayTest(selectedAbilityData.Id))
        {
            if (useAbility && !extraAbilityOneClickState)
            {
                extraAbilityUseEvent.Invoke();
                AbilityDelay();

                extraAbilityOneClickState = true;
            }
            else if (!useAbility) extraAbilityOneClickState = false;
        }

        if (!selectedAbility.isAttack)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                NextAbility();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                PreviousAbility();
            }
        }
        
        if (!isWeaponInCooldown && !IsAttacking)
        {
            if (mouseWheel < 0)
                PreviousWeapon();

            else if (mouseWheel > 0)
                NextWeapon();
        }
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

        //int nowWeaponID = selectedWeaponID;
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
        StartCoroutine(weaponCooldown(time));
        IEnumerator weaponCooldown(float time)
        {
            isWeaponInCooldown = true;
            yield return new WaitForSeconds(time);
            isWeaponInCooldown = false;
        }
        
    }

    private void NextWeapon()
    {
        if (weaponsUnlockedIDs.Count >= 1)
        {
            selectedWeapon++;

            if (selectedWeapon >= weaponsUnlockedIDs.Count) 
                selectedWeapon = 0;

            WeaponData nowWeaponData = FindWeaponData(weaponsUnlockedIDs[selectedWeapon]);

            selectedWeaponData = nowWeaponData;
            InitializedWeaponsFields();
            if (WeaponChange != null) WeaponChange.Invoke();
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

            selectedWeaponData = nowWeaponData;
            InitializedWeaponsFields();
            if (WeaponChange != null) WeaponChange.Invoke();
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

            if (selectedAbilityNum <= 0)
                selectedAbilityNum = weaponsUnlockedIDs.Count-1;

            selectedWeaponData =
                FindWeaponData(weaponsUnlockedIDs[selectedWeapon]);
            InitializedWeaponsFields();
            if(WeaponChange != null) WeaponChange.Invoke();
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

    private void InitializedAbilitysFields() //????????????? ????? ??? ????? ???. ????????????
    {
        abilityUnlockedIDs.Sort();
        Destroy(nowAbilityPrefab);
        nowAbilityPrefab = Instantiate(selectedAbilityData.AbilityPrefab, transform);
        selectedAbility = nowAbilityPrefab.GetComponent<ExtraAbility>();

        extraAbilityChangeEvent.Invoke();
    }

    private void NextAbility()
    {
        if (abilityUnlockedIDs.Count >= 1)
        {
            selectedAbilityNum++;

            if (selectedAbilityNum >= abilityUnlockedIDs.Count)
                selectedAbilityNum = 0;

            selectedAbilityData = FindAbilityID(abilityUnlockedIDs[selectedAbilityNum]);
            InitializedAbilitysFields();
        }
    }

    private void PreviousAbility()
    {
        if (abilityUnlockedIDs.Count >= 1)
        {
            --selectedAbilityNum;

            if (selectedAbilityNum < 0)
                selectedAbilityNum = abilityUnlockedIDs.Count - 1;

            selectedAbilityData = FindAbilityID(abilityUnlockedIDs[selectedAbilityNum]);
            InitializedAbilitysFields();
        }
    }

    private void SetSelectedAbility(int num)
    {
        if (abilityUnlockedIDs.Count >= 1)
        {
            selectedAbilityNum = num;

            if (selectedAbilityNum < 0)
                selectedAbilityNum = abilityUnlockedIDs.Count - 1;

            if (selectedAbilityNum >= abilityUnlockedIDs.Count)
                selectedAbilityNum = 0;

            selectedAbilityData = FindAbilityID(abilityUnlockedIDs[selectedAbilityNum]);
            InitializedAbilitysFields();
        }
    }

    public ExtraAbilityData FindAbilityID(int id)
    {
        for (int i = 0; i < abilityDatas.Count; i++)
        {
            if (abilityDatas[i].Id == id)
                return abilityDatas[i];
        }

        throw new Exception($"??????????? ??? ???? {id} ?? ???????!");
    }

    private void AbilityDelay()
    {
        abilityInDelayIDs.Add(selectedAbilityData.Id);
        int abilityDeleteId = selectedAbilityData.Id;
        StartCoroutine(DelayTimer(selectedAbilityData.DelayTime));

        IEnumerator DelayTimer(float time)
        {
            yield return new WaitForSeconds(time);
            abilityInDelayIDs.Remove(abilityDeleteId);
        }
    }

    public bool AbilityDelayTest(int id)
    {
        foreach (var item in abilityInDelayIDs)
            if (id == item)
                return false;
        return true;
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

        float fire1 = Axes.Fire1;

        if (shootingMode != WeaponData.ShootingMode.Lazer) 
            isAttacking = false;

        else if (fire1 > 0 && isThereAnyBullets)  
        {
            //????? ?????????? ??????? ?? ??? ???? ??? ?????
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

    public void SubWeaponChangeEvent(Action action)
    {
        if (action != null)
            WeaponChange += action;
    }

    public void UnsubWeaponChangeEvent(Action action)
    {
        if (action != null)
            WeaponChange -= action;
    }

    private void Shot()
    {
        if (bulletPrefab != null)
        {
            ShotEvent?.Invoke();

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
                
                if (Physics.Raycast(shotRay, out raycastHit,layerMaskRayCastMode))
                {
                    float smoothness = 4f;
                    Vector3 truePos = raycastHit.point - (currentShotPoint.forward / smoothness);

                    Instantiate(bulletPrefab, truePos, currentShotPoint.rotation)
                    .GetComponent<Bullet>().SetDamage(damage);
                }
            }

        }
        else if(shootingMode == WeaponData.ShootingMode.Particle)
        {
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
            nowBullet.SetStartImpulsPower(adjustableAttackStartPower);
        }
    }

    public void SetManageActive(bool state)
    {
        isManageActive = state;
    }

}