using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainCircleUI : MonoBehaviour
{
    
    [SerializeField] private PlayerWeaponsManager weaponsManager;
    [SerializeField] private PlayerMainService playerManager;
    [SerializeField] private Image nowWeaponBullet;
    [SerializeField] private Image healthCircle;
    [SerializeField] private Sprite noWeaponSprite;
    [SerializeField] private Transform healthIconT;
    private Vector3 startHealthIconScale;

    public Image HealthCircle { get { return healthCircle; } }

    [SerializeField] private Image armorCircle;
    public Image ArmorCircle { get { return armorCircle; } }

    [SerializeField] private TextMeshProUGUI bulletsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [Space]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform[] weaponPoints = new Transform[5];
    private int selectedWeapon;

    private List<Transform> iconTrans = new List<Transform>();
    private List<Material> iconMaterial = new List<Material>();
    [SerializeField] private Sprite nullIcon;
    [SerializeField] private float iconSpeed = 5f;

    [Space]
    [SerializeField] HPstatus HealthStatus = HPstatus.Normal;
    [SerializeField] private float targetBrightness = 1f;
    [SerializeField] private float flashingTime = 1f;
    private float startBrightness;
    [SerializeField] private float nowBrightness;
    private float flashingTimer = 0;

    private void Start()
    {
        selectedWeapon = weaponsManager.SelectedWeapon;

        weaponsManager.WeaponsUnlockedIDs.Sort();

        RestartIcons();

        startBrightness = healthCircle.material.GetFloat("_Brightness");
        nowBrightness = startBrightness;

        healthCircle.material = new Material(healthCircle.material);

        startHealthIconScale = healthIconT.localScale;

        weaponsManager.newWeaponEvent += GetWeapon;

        InstantIconsSort();

    }

    private void Update()
    {
        if (weaponsManager.selectedBulletData != null)
            nowWeaponBullet.sprite = weaponsManager.selectedBulletData.Icon;
        else nowWeaponBullet.sprite = noWeaponSprite;

        selectedWeapon = weaponsManager.SelectedWeapon;

        LerpIconsSort(iconSpeed);

        //-------------------------------------------------------------

        float tempSpeed = 500f * Time.deltaTime;
        nowBrightness = Mathf.Lerp(nowBrightness,startBrightness * targetBrightness, Time.deltaTime * tempSpeed);
        healthCircle.material.SetFloat("_Brightness", nowBrightness);


        float healthIconLerpSpeed = 5f * Time.deltaTime;
        healthIconT.localScale = Vector3.Lerp(healthIconT.localScale,startHealthIconScale, healthIconLerpSpeed);

        if(flashingTimer > 0)
            flashingTimer -= Time.deltaTime;

        if(flashingTimer <= 0)
        {
            float mutiplyer;
            
            switch (HealthStatus)
            {
                case HPstatus.Normal:
                    healthCircle.material.SetInt("_Disturbance", 0);
                    targetBrightness = 1f;

                    healthIconT.localScale = startHealthIconScale * 1.1f;

                    flashingTimer = flashingTime * 1.25f;
                    break;

                case HPstatus.Low:
                    mutiplyer = 0.75f;
                    targetBrightness = mutiplyer;
                    nowBrightness = startBrightness;
                    healthCircle.material.SetInt("_Disturbance", 0);

                    healthIconT.localScale = startHealthIconScale * 1.2f;

                    flashingTimer = flashingTime;
                    break;

                case HPstatus.VeryLow:
                    mutiplyer = 0.25f;
                    targetBrightness = mutiplyer;
                    nowBrightness = startBrightness;
                    healthCircle.material.SetInt("_Disturbance", 1);

                    healthIconT.localScale = startHealthIconScale * 1.3f;

                    flashingTimer = flashingTime / 2f;
                    break;
            }
        }

        if (healthCircle.fillAmount > 0.5f)
            HealthStatus = HPstatus.Normal;
        else if (healthCircle.fillAmount <= 0.5f && healthCircle.fillAmount >= 0.3f)
            HealthStatus = HPstatus.Low;
        else
            HealthStatus = HPstatus.VeryLow;

        UpdateHealthUI();
        UpdateArmorUI();

        SetTexts();
        UpdateColors();
    }

    private void SetTexts()
    {
        bulletsText.text = $"{weaponsManager.currentBulletsCount}";
        healthText.text = $"{(int)playerManager.Health_}";
        armorText.text = $"{(int)playerManager.Armor}";
    }

    private void UpdateHealthUI()
    {
        float timeStep = 10f * Time.deltaTime;
        float realFillAmount = playerManager.Health_ / playerManager.MaxHealth_;
        float smoothneess = 0.26f * (realFillAmount-0.5f);

        float nowFillAmount = HealthCircle.fillAmount;
        nowFillAmount = Mathf.Lerp(nowFillAmount, realFillAmount - smoothneess, timeStep);

        HealthCircle.fillAmount = nowFillAmount;
    }

    private void UpdateArmorUI()
    {
        float timeStep = 10f * Time.deltaTime;
        float realFillAmount = playerManager.Armor / playerManager.MaxArmor;
        float smoothneess = 0.26f * (realFillAmount - 0.5f);

        float nowFillAmount = ArmorCircle.fillAmount;
        nowFillAmount = Mathf.Lerp(nowFillAmount, realFillAmount - smoothneess, timeStep);

        ArmorCircle.fillAmount = nowFillAmount;
    }

    private void InstantIconsSort()
    {
        for (int i = 0; i < iconTrans.Count; i++)
        {
            var item = iconTrans[i];

            int steps;
            bool isPositive;

            (steps, isPositive) = StepsToSelectedWeapon(i);

            if (isPositive)
            {
                if (steps >= 2)
                {
                    item.position = weaponPoints[4].position;
                    item.localScale = weaponPoints[4].localScale;
                }
                if (steps == 1)
                {
                    item.position = weaponPoints[3].position;
                    item.localScale = weaponPoints[3].localScale;
                }
            }
            else
            {
                if (steps >= 2)
                {
                    item.position = weaponPoints[0].position;
                    item.localScale = weaponPoints[0].localScale;
                }
                if (steps == 1)
                {
                    item.position = weaponPoints[1].position;
                    item.localScale = weaponPoints[1].localScale;
                }
            }

            if (steps == 0)
            {
                item.position = weaponPoints[2].position;
                item.localScale = weaponPoints[2].localScale;
            }

        }

        (int, bool) StepsToSelectedWeapon(int weaponNum)
        {
            int resultWay = 0;
            bool resultCountBool = true;

            int resultIndex1 = selectedWeapon - weaponNum;
            int resultIndex2 = selectedWeapon - iconTrans.Count - weaponNum;

            if (resultIndex2 <= -iconTrans.Count)
            {
                resultIndex2 = Mathf.Abs(selectedWeapon + iconTrans.Count) - weaponNum;
                resultCountBool = false;
            }

            int way1 = Mathf.Abs(resultIndex1);
            int way2 = Mathf.Abs(resultIndex2);

            if (way1 <= way2)
            {
                resultWay = way1;

                checkCountPosWay1();
            }
            else if (way2 < way1)
            {
                resultWay = way2;
            }

            void checkCountPosWay1()
            {
                if (resultIndex1 <= 0)
                    resultCountBool = true;
                else if (resultIndex1 > 0)
                    resultCountBool = false;
            }

            return (resultWay, resultCountBool);

        }

    }

    private void LerpIconsSort(float speed)
    {
        float resultSpeed = speed * Time.deltaTime;

        for (int i = 0; i < iconTrans.Count; i++)
        {
            var item = iconTrans[i];

            int steps;
            bool isPositive;

            (steps, isPositive) = StepsToSelectedWeapon(i);

            if (isPositive)
            {
                if (steps >= 2)
                {
                    LerpIconToPos(weaponPoints[4]);
                }
                else if (steps == 1)
                {
                    LerpIconToPos(weaponPoints[3]);
                }
            }
            else
            {
                if (steps >= 2)
                {
                    LerpIconToPos(weaponPoints[0]);
                }
                if (steps == 1)
                {
                    LerpIconToPos(weaponPoints[1]);
                }
            }

            if (steps == 0)
            {
                LerpIconToPos(weaponPoints[2]);
            }


            void LerpIconToPos(Transform point)
            {
                item.position =
                            Vector3.Lerp(item.position, point.position, resultSpeed);
                item.localScale =
                    Vector3.Lerp(item.localScale, point.localScale, resultSpeed);
            }
        }

        (int, bool) StepsToSelectedWeapon(int weaponNum)
        {
            
            int resultWay = 0;
            bool resultCountBool = true;

            int resultIndex1 = selectedWeapon - weaponNum;
            int resultIndex2 = selectedWeapon - iconTrans.Count - weaponNum;

            if (resultIndex2 <= -iconTrans.Count)
            {
                resultIndex2 = Mathf.Abs(selectedWeapon + iconTrans.Count) - weaponNum;
                resultCountBool = false;
            }

            int way1 = Mathf.Abs(resultIndex1);
            int way2 = Mathf.Abs(resultIndex2);

            if (way1 <= way2)
            {
                resultWay = way1;

                checkCountPosWay1();
            }
            else if (way2 < way1)
            {
                resultWay = way2;
            }

            void checkCountPosWay1()
            {
                if (resultIndex1 <= 0)
                    resultCountBool = true;
                else if (resultIndex1 > 0)
                    resultCountBool = false;
            }

            return (resultWay, resultCountBool);

        }

    }

    public void GetWeapon(WeaponData weaponData)
    {
        RestartIcons();
        InstantIconsSort();
    }

    void RestartIcons()
    {
        foreach (var item in iconTrans)
        {
            Destroy(item.gameObject);
        }

        iconMaterial.Clear();
        iconTrans.Clear();

        foreach (var item in weaponsManager.WeaponsUnlockedIDs)
        {
            
            WeaponData nowWeaponData = weaponsManager.FindWeaponID(item);

            Image newIcon = Instantiate(iconPrefab, weaponPoints[3])
                .GetComponent<Image>();

            Material newMaterial = new Material(newIcon.material);
            newIcon.material = newMaterial;
            newMaterial.SetColor("_MainColor", nowWeaponData.MainColor);

            newIcon.sprite = nowWeaponData.Icon;

            newIcon.gameObject.name = $"{item}";

            Transform _iconTransform = newIcon.transform;

            iconTrans.Add(_iconTransform);
            iconMaterial.Add(newMaterial);
            //newIconTransform.parent = gameObject.transform;

            if (newIcon.sprite == null)
                newIcon.sprite = nullIcon;
        }
    }

    private void UpdateColors()
    {

        for (int i = 0; i < iconMaterial.Count; i++)
        {
            WeaponData data = weaponsManager.FindWeaponID(weaponsManager.WeaponsUnlockedIDs[i]);

            bool bulletHere = weaponsManager.bulletsManager.CheckBullets(data.BulletsID);

            if (bulletHere)
                iconMaterial[i].SetColor("_MainColor", data.MainColor);
            else iconMaterial[i].SetColor("_MainColor", Color.black);

        }

    }

    private enum HPstatus
    {
        Normal,
        Low,
        VeryLow
    }

}
