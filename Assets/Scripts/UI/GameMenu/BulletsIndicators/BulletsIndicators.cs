using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsIndicators : MonoBehaviour
{

    [SerializeField] private PlayerWeaponsBulletsManager bulletsManager;
    [SerializeField] private PlayerWeaponsManager weaponsManager;
    [SerializeField] private Dictionary<int, OneBulletIndicator> indicators = new Dictionary<int, OneBulletIndicator>();
    [SerializeField] private Dictionary<int, Transform> indicatorsT = new Dictionary<int, Transform>();
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private float indicatorHeight;
    [SerializeField] private float useScaleSpeed = 5f;
    [SerializeField] private float useScaleMultiply = 2f;
    private Vector3 startIndicatorsScale;

    private bool isIndicatorsCreate = false;

    public bool IsIndicatorsCreate {get {return isIndicatorsCreate;}}

    private void Start()
    {
        SetReferences();

        InstantiateIndicators();

        weaponsManager.SubscribeShotEvent(SetUseScale);

        void SetUseScale()
        {
            int currentBulletID = weaponsManager.FindWeaponData
                (weaponsManager.SelectedWeaponID).BulletsID;
            
            if (currentBulletID != 0)
                indicatorsT[currentBulletID].localScale = startIndicatorsScale * useScaleMultiply;
        }
    }

    private void Update()
    {
        SetIndicators();
    }

    private void InstantiateIndicators()
    {
        SetReferences();

        if(CheckForIndicators())
        {
            GameObject[] destroyIndicators = new GameObject[indicatorsT.Count];

            for (int i = 0; i < indicatorsT.Count; i++)
            {
                destroyIndicators[i] = indicatorsT[i].gameObject;
            }

            for (int i = 0; i < destroyIndicators.Length; i++)
            {
                Destroy(indicatorsT[i].gameObject);
            }
        }

        for (int i = 0; i < bulletsManager.BulletDatas.Length; i++)
        {
            Vector3 newPos = Vector3.right * (indicatorHeight * i);

            GameObject newIndicator = Instantiate(indicatorPrefab, startPoint);
            newIndicator.transform.localPosition = newPos;
            OneBulletIndicator newBulletIndicator = newIndicator.GetComponent<OneBulletIndicator>();
            newBulletIndicator.SetNewSprite(bulletsManager.BulletDatas[i].Icon);
            Image newImage = newBulletIndicator.MainImage;
            Material newMaterial = new Material (newImage.material);
            newImage.material = newMaterial;

            foreach (var item in weaponsManager.WeaponsDatas)
            {
                if (item.BulletsID == bulletsManager.BulletDatas[i].Id)
                {
                    newImage.material.color = item.MainColor * 2.5f;
                    break;
                }
            }

            indicators.Add(bulletsManager.BulletDatas[i].Id, newBulletIndicator);
            indicatorsT.Add(bulletsManager.BulletDatas[i].Id, newImage.transform);

            startIndicatorsScale = indicatorsT[bulletsManager.BulletDatas[i].Id].localScale;
        }

        isIndicatorsCreate = true;
    }

    public bool CheckForIndicators()
    {
        return indicatorsT.Count > 0;
    }

    private void SetIndicators()
    {
        for (int i = 0; i < bulletsManager.BulletDatas.Length; i++)
        {
            var item = bulletsManager.BulletDatas[i];

            if(bulletsManager.IsIdUnlocked(item.Id))
            {
                if(!indicators[item.Id].gameObject.activeSelf)
                    indicators[item.Id].gameObject.SetActive(true);

                float fillAmount = (float)bulletsManager.GetBulletsCount(item.Id)
                    / bulletsManager.BulletsMax[item.Id];

                indicators[item.Id].MainImage.fillAmount = fillAmount;

                if(indicatorsT[item.Id].gameObject.activeSelf && 
                    indicatorsT[item.Id].localScale != startIndicatorsScale)
                {
                    indicatorsT[item.Id].localScale = Vector3.Lerp(indicatorsT[item.Id].localScale,
                        startIndicatorsScale, Time.deltaTime * useScaleSpeed);
                }
            }
            else if (indicators[item.Id].gameObject.activeSelf)
                    indicators[item.Id].gameObject.SetActive(false);
        }
    }

    private void SetReferences()
    {
        if (bulletsManager == null || weaponsManager == null)
        {
            PlayerMainService player = GameObject.Find("Player").GetComponent<PlayerMainService>();

            if (player == null)
                throw new MissingReferenceException("Player service not find!");

            weaponsManager = player.weaponsManager;
            bulletsManager = player.weaponsManager.bulletsManager;
        }
    }

}
