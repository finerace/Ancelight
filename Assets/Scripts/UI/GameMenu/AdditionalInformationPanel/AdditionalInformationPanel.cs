using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalInformationPanel : MonoBehaviour
{
    [SerializeField] private PlayerLookService playerLookService;
    [SerializeField] private Transform searchRayPoint;
    private const int searchRayLayerMask = 1 << 17;

    [SerializeField] private Transform panelT;

    [Space] 
    
    [SerializeField] private float searchDistance = 45f;

    [SerializeField] private float toMinScaleDistance = 15f;
    
    [SerializeField] private float maxPanelScale = 1;
    [SerializeField] private float minPanelScale = 0.1f;
    
    [Space] 
    
    [SerializeField] private Image panelColorImage;
    [SerializeField] private Image healthIndicator;
    
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private TextMeshProUGUI panelDescription;

    private float toInformedObjectDistance = 0;
    
    private void Awake()
    {
        CopyColorImageMaterial();
    }

    private void Update()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation

        if (SearchInformationObjects(out var informationData))
        {
            if(!panelT.gameObject.activeSelf)
                panelT.gameObject.SetActive(true);

            SetNewAdditionalInformationPanel(informationData);
            
            SetPanelPosition(informationData);
            
            SetPanelScale(toInformedObjectDistance);
        }
        else
            panelT.gameObject.SetActive(false);
    }

    // private void SetSearchRayPoint()
    // {
    //     searchRayPoint =
    //         playerLookService.ShootingPoint;
    // }

    private void SetPanelPosition(AdditionalInformationData data)
    {
        var playerCamera = playerLookService.mainCamera;
        
        var informedObjectPos = data.InformedObjectT.position;
        
        var informedObjectPosHightScaled = informedObjectPos + (Vector3.up * data.InformedObjectHight);

        Vector2 panelPos = playerCamera.WorldToScreenPoint(informedObjectPosHightScaled);

        var xAmount = playerCamera.pixelWidth / 1920f;
        var yAmount = playerCamera.pixelHeight / 1080f;

        panelPos.x /= xAmount;
        panelPos.y /= yAmount;

        panelT.localPosition = panelPos;

    }

    private void SetPanelScale(float toInformedObjectDistance)
    {

        if (toInformedObjectDistance >= toMinScaleDistance)
        {
            SetPanelScale(minPanelScale);
            
            return;
        }

        var scaleAmount = toInformedObjectDistance / toMinScaleDistance;

        var resultScale = Mathf.Lerp(maxPanelScale, minPanelScale, scaleAmount);

        SetPanelScale(resultScale);
        
        void SetPanelScale(float scale)
        {
            panelT.localScale = Vector3.one * scale;
        }
    }
    
    private void CopyColorImageMaterial()
    {
        panelColorImage.material = new Material(panelColorImage.material);
    }

    private bool SearchInformationObjects(out AdditionalInformationData data)
    {
        Ray searchRay;

        var origin = searchRayPoint.position;
        var direction = searchRayPoint.forward;
        
        searchRay = new Ray(origin, direction);
        
        var rayIsFindInformation =
            Physics.Raycast(searchRay, out var hitData, searchDistance, searchRayLayerMask);

        if (!rayIsFindInformation)
        {
            data = null;
            return false;
        }

        var hitObject = hitData.collider.gameObject;
        data = hitObject.GetComponent<AdditionalInformationData>();

        toInformedObjectDistance = Vector3.Distance(searchRayPoint.position,hitData.point);
        
        return true;
    }
    
    private void SetNewAdditionalInformationPanel(AdditionalInformationData data)
    {
        SetNewTexts();
        
        SetNewColor();

        SetNewHealthIndicator();
        
        void SetNewTexts()
        {
            panelName.text = data.InformationName;
            panelDescription.text = data.InformationDescription;
        }

        void SetNewColor()
        {
            var mainColorShaderId = Shader.PropertyToID("_MainColor");
            panelColorImage.material.SetColor(mainColorShaderId,data.InformedObjectMainColor);
        }

        void SetNewHealthIndicator()
        {
            var dataHealth = data.InformedObjectHealth;

            if (dataHealth == null)
            {
                healthIndicator.fillAmount = 0;

                return;
            }

            var health = dataHealth.Health_;
            var maxHealth = dataHealth.MaxHealth_;

            var healthAmount = health / maxHealth;

            healthIndicator.fillAmount = healthAmount;
        }
    }
    
}
