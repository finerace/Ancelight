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

    [SerializeField] private float hightScaleCoof = 10f;
    [SerializeField] private float toMinScaleDistance = 15f;
    
    [SerializeField] private float maxPanelScale = 1;
    [SerializeField] private float minPanelScale = 0.1f;
    
    [Space] 
    
    [SerializeField] private Image panelColorImage;
    [SerializeField] private Image healthIndicator;
    
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private TextMeshProUGUI panelDescription;
    [SerializeField] private float descriptionOnlyHealthTextScale = 20;
    private float oldDescriptionTextScale = 0;
    
    private float toInformedObjectDistance = 0;
    
    private void Awake()
    {
        playerLookService = FindObjectOfType<PlayerLookService>();
        searchRayPoint = playerLookService.mainCamera.transform;
        
        CopyColorImageMaterial();

        oldDescriptionTextScale = panelDescription.fontSize;
    }

    private void Update()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        
        if (SearchInformationObjects(out var informationData))
        {
            if(!panelT.gameObject.activeSelf)
                panelT.gameObject.SetActive(true);

            SetNewInformationData(informationData);
            
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

        var informedObjectScaledHight = data.InformedObjectHight + (toInformedObjectDistance / hightScaleCoof);
        var informedObjectPosHightScaled = informedObjectPos + (Vector3.up * informedObjectScaledHight);

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
    
    private void SetNewInformationData(AdditionalInformationData data)
    {
        SetNewTexts();
        
        SetNewColor();

        SetNewHealthIndicator();
        
        void SetNewTexts()
        {
            panelName.text = data.InformationName;
            
            var isDescriptionUseless = data.InformationDescription == String.Empty;

            if (!isDescriptionUseless)
            {
                panelDescription.text = data.InformationDescription;

                if (panelDescription.fontSize != oldDescriptionTextScale) {
                    panelDescription.fontSize = oldDescriptionTextScale;
                }                

                return;
            }

            if (data.InformedObjectHealth == null) 
                return;

            var health = data.InformedObjectHealth.Health_.ClampToTwoRemainingCharacters();
            var maxHealth = data.InformedObjectHealth.MaxHealth_.ClampToTwoRemainingCharacters();
            
            if (panelDescription.fontSize != descriptionOnlyHealthTextScale) {
                panelDescription.fontSize = descriptionOnlyHealthTextScale;
            }                

            panelDescription.text = $"{health}/{maxHealth}";
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
