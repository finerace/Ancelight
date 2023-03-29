using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class DashsIndicatorService : MonoBehaviour
{
    [SerializeField] private GameObject dashIndicatorPrefab;
    [SerializeField] private PlayerDashsService dashsService;
    [Space]
    [SerializeField] private List<DashOneIndicator> dashIndicators 
        = new List<DashOneIndicator>();

    private float currentIndicatorsTransparency = 1f;
    private float currentIndicatorsFillAmount = 1f;
    
    private event Action dashUnitReadyEvent;
    public event Action DashUnitReadyEvent
    {
        add
        {
            if (value == null)
                throw new EventSourceException("Action is null!");
            
            dashUnitReadyEvent += value;
        }

        remove
        {
            if (value == null)
                throw new EventSourceException("Action is null!");
            
            dashUnitReadyEvent -= value;
        }
    }

    private int currentReadyDashUnits;
    private int soundedReadyDashUnits;
    
    private Transform startPoint;
    
    private bool isDashServiceActive 
    { get => dashsService.DashCurrentEnergy < dashsService.DashMaxEnergy;}

    [SerializeField] private float afterActiveWorkTime = 2;
    private float afterActiveWorkTimer = 0;

    [Space] 
    
    [SerializeField] private AudioCastData onMouseEnter;
    [SerializeField] private AudioCastData onMouseClickComplete;
    [SerializeField] private AudioCastData onMouseClickDefeat;
    
    private void Awake()
    {
        startPoint = transform;
        dashsService = FindObjectOfType<PlayerDashsService>();

        ReCreateIndicators();

        dashsService.OnDashCountUpdate += ReCreateIndicators;
    }

    private void Update()
    {
        UpdateDashsIndiactors();
        
        DashUnitReadyEventWork();
    }

    private void DashUnitReadyEventWork()
    {
        if(soundedReadyDashUnits < currentReadyDashUnits)
            if(dashUnitReadyEvent != null)
                dashUnitReadyEvent.Invoke();

        soundedReadyDashUnits = currentReadyDashUnits;
    }
    
    private void ReCreateIndicators()
    {
        for (int i = 0; i < dashIndicators.Count; i++)
        {
            var item = dashIndicators[i];

            Destroy(item.gameObject);
        }
        dashIndicators.Clear();

        if (dashsService.DashsCount <= 0)
            return;

        const float firstDashIndicatorDistance = 22f;
        const float dashsIndicatorsHightDistance = 6f;

        CreateFirstDashIndicators();

        for (int i = 1; i < dashsService.DashsCount; i++)
        {

            CreateNextIndicators(dashIndicators.Count);

        }

        void CreateFirstDashIndicators()
        {
            DashOneIndicator firstIndicator1 =
                Instantiate(dashIndicatorPrefab, startPoint).GetComponent<DashOneIndicator>();

            firstIndicator1.transform.position = startPoint.position;

            firstIndicator1.transform.localPosition += 
                -Vector3.right * firstDashIndicatorDistance;

            dashIndicators.Add(firstIndicator1);


            DashOneIndicator firstIndicator2 =
                Instantiate(dashIndicatorPrefab, startPoint).GetComponent<DashOneIndicator>();

            firstIndicator2.transform.position = startPoint.position;

            firstIndicator2.transform.localPosition +=
                Vector3.right * firstDashIndicatorDistance;

            firstIndicator2.transform.localEulerAngles += new Vector3 (0, 180, 0);

            dashIndicators.Add(firstIndicator2);
        }

        void CreateNextIndicators(int count)
        {
            DashOneIndicator firstIndicator1 =
                Instantiate(dashIndicatorPrefab, startPoint).GetComponent<DashOneIndicator>();

            firstIndicator1.transform.position =
                dashIndicators[count - 2].transform.position;

            firstIndicator1.transform.localPosition +=
                -Vector3.right * dashsIndicatorsHightDistance;

            firstIndicator1.transform.localPosition +=
                -Vector3.up * dashsIndicatorsHightDistance;

            dashIndicators.Add(firstIndicator1);


            DashOneIndicator firstIndicator2 =
                Instantiate(dashIndicatorPrefab, startPoint).GetComponent<DashOneIndicator>();

            firstIndicator2.transform.position =
                dashIndicators[count - 1].transform.position;

            firstIndicator2.transform.localPosition +=
                Vector3.right * dashsIndicatorsHightDistance;

            firstIndicator2.transform.localPosition +=
                -Vector3.up * dashsIndicatorsHightDistance;

            firstIndicator2.transform.localEulerAngles += new Vector3(0, 180, 0);

            dashIndicators.Add(firstIndicator2);
        }

    }

    private void UpdateDashsIndiactors()
    {
        if(!isDashServiceActive && afterActiveWorkTimer <= 0)
        {
            SetIndicatorsTransparency(0);

            return;
        }

        float realFillAmount = currentIndicatorsFillAmount;

        int dashsCount = (dashIndicators.Count / 2);
        int targetIndicatorsID = (dashsCount - 1) - (int)(dashsCount * realFillAmount);

        if (!isDashServiceActive)
        {
            AfterActiveWorkTimer();
            SetIndicatorsEffectIntensity();
            return;
        }

        SetIndicatorFillAmount();

        SetIndicatorsEffectIntensity();

        SetIndicatorsTransparency(1);

        afterActiveWorkTimer = afterActiveWorkTime;

        void SetIndicatorFillAmount()
        {
            const float fillSpeed = 40f;
            float timeStep = fillSpeed * Time.deltaTime;
            float toLerpFillAmount = 
                dashsService.DashCurrentEnergy / dashsService.DashMaxEnergy;

            float smoothFillAmount = Mathf.Lerp(realFillAmount, toLerpFillAmount, timeStep);

            float oneDashFillAmount = 
                (dashsCount * smoothFillAmount) - ((dashsCount-1) - targetIndicatorsID);

            float fillSmoothnessCount = 0.75f * (oneDashFillAmount - 0.5f);

            float targetIndicatorsSmoothFillAmount =
                oneDashFillAmount - fillSmoothnessCount;

            for (int i = 0; i < dashIndicators.Count; i+= 2)
            {
                var item1 = dashIndicators[i];
                var item2 = dashIndicators[i+1];
                int currentIndicatorsID = i / 2;

                if(currentIndicatorsID > targetIndicatorsID)
                {
                    item1.SetFillAmount(1);
                    item2.SetFillAmount(1);

                    continue;
                }

                if(currentIndicatorsID < targetIndicatorsID)
                {
                    item1.SetFillAmount(0);
                    item2.SetFillAmount(0);

                    continue;
                }

                if(currentIndicatorsID == targetIndicatorsID)
                {
                    item1.SetFillAmount(targetIndicatorsSmoothFillAmount);
                    item2.SetFillAmount(targetIndicatorsSmoothFillAmount);

                    continue;
                }
            }

            currentIndicatorsFillAmount = smoothFillAmount;
        }

        void SetIndicatorsEffectIntensity()
        {
            const float readyDashIndicatorIntensity = 0.3f;
            const float notReadyDashIndicatorIntensity = 0.1f;
            currentReadyDashUnits = 0;
            
            for (int i = 0; i < dashIndicators.Count; i += 2)
            {
                var item1 = dashIndicators[i];
                var item2 = dashIndicators[i + 1];
                int currentIndicatorsNum = i / 2;

                if (currentIndicatorsNum > targetIndicatorsID)
                {
                    item1.SetFillZoneEffectIntensity(readyDashIndicatorIntensity);
                    item2.SetFillZoneEffectIntensity(readyDashIndicatorIntensity);

                    currentReadyDashUnits++;
                    continue;
                }

                if (currentIndicatorsNum == targetIndicatorsID)
                {
                    item1.SetFillZoneEffectIntensity(notReadyDashIndicatorIntensity);
                    item2.SetFillZoneEffectIntensity(notReadyDashIndicatorIntensity);

                    if(realFillAmount >= 0.95f)
                    {
                        item1.SetFillZoneEffectIntensity(readyDashIndicatorIntensity);
                        item2.SetFillZoneEffectIntensity(readyDashIndicatorIntensity);
                        currentReadyDashUnits++;
                    }
                }
            }
        }

        void SetIndicatorsTransparency(float transparency)
        {
            if (transparency == currentIndicatorsTransparency)
                return;

            const float transparencyChangeSpeed = 10f;
            float timeStep = Time.deltaTime * transparencyChangeSpeed;

            currentIndicatorsTransparency = 
                Mathf.Lerp(currentIndicatorsTransparency, transparency, timeStep);

            foreach (var item in dashIndicators)
            {
                item.SetTransparency(currentIndicatorsTransparency);
            }
        }

        void AfterActiveWorkTimer()
        {
            if(afterActiveWorkTimer > 0)
                afterActiveWorkTimer -= Time.deltaTime;
        }

    }

}
