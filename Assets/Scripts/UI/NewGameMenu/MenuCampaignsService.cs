using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCampaignsService : MonoBehaviour
{
    [SerializeField] private CampaignData[] gameCampaigns;
    [SerializeField] private GameObject campaignLevelUnitPrefab;
    [Space]
    [SerializeField] private CampaignsLevelSelector campaignsLevelService;
    [SerializeField] private ScrollObjectService scrollObjectService;
    [SerializeField] private TextMeshProUGUI campaignsLevelsPanelLabel;
    [SerializeField] private GameObject backToCampaignUnitsButton;
    
    private GameObject[] spawnedCampaignsUnits;
    private float campaignUnitsScrollDistance;
    
    private GameObject[] spawnedLevelUnits = new GameObject[0];
    private const int unitsDistance = 110;
    private static readonly int BrightnessShaderId = Shader.PropertyToID("_Brightness");

    const float notUnlockedButtonMatBrightness = 0.70f;
    const float notUnlockedButtonDecLineBrightness = 0.55f;

    private const string campaignsUnitsOnPanelLabelText = "Select Campaign";
    private const string levelsUnitsOnPanelLabelText = "Select Level";
    
    private void Awake()
    {
        CampaignsLevelsSaveUtility.Load(gameCampaigns[0]);
        
        SpawnCampaignsUnits();
    }

    private void SpawnCampaignsUnits()
    {
        var resultUnitsScrollDistance = 0f;
        const int minScrollDistance = 900;
        
        var spawnedCampaignUnitsList = new List<GameObject>();
        
        foreach (var campaignData in gameCampaigns)
        {
            if(campaignData.CampaignLevels.Length == 0)
                return;

            var unitSpawnObjT = scrollObjectService.ScrollObjectT;

            var spawnedUnitData = 
                Instantiate(campaignLevelUnitPrefab,unitSpawnObjT).GetComponent<CampaignLevelUnitData>();
            
            spawnedUnitData.transform.localPosition += new Vector3(0, -(resultUnitsScrollDistance), 0);
            spawnedUnitData.SetNewCampaignData(campaignData);
            spawnedUnitData.NameLabel.text = spawnedUnitData.CampaignData.CampaignName;
            
            if (!campaignData.IsCampaignUnlocked)
            {
                SetNewLockedCampaign();
                void SetNewLockedCampaign()
                {
                    spawnedUnitData.ButtonBackground.material = new Material(spawnedUnitData.ButtonBackground.material);
                    spawnedUnitData.ButtonBackground.material.SetFloat(BrightnessShaderId,
                        notUnlockedButtonMatBrightness);

                    spawnedUnitData.ButtonDecorationLine.material =
                        new Material(spawnedUnitData.ButtonDecorationLine.material);
                    spawnedUnitData.ButtonDecorationLine.material.SetFloat(BrightnessShaderId,
                        notUnlockedButtonDecLineBrightness);

                    spawnedCampaignUnitsList.Add(spawnedUnitData.gameObject);
                    resultUnitsScrollDistance += unitsDistance;
                }
                
                continue;
            }

            SetUnlockedCampaign();
            void SetUnlockedCampaign()
            {
                var spawnedUnitButton = spawnedUnitData.GetComponent<ButtonMainService>();
                spawnedUnitButton.onClickAction.AddListener(
                    (() => OpenCampaignLevelsList(spawnedUnitData.CampaignData)));

                spawnedCampaignUnitsList.Add(spawnedUnitData.gameObject);
                resultUnitsScrollDistance += unitsDistance;
            }
        }

        spawnedCampaignsUnits = spawnedCampaignUnitsList.ToArray();
        
        SetNewScrollDistance();
        void SetNewScrollDistance()
        {
            if (resultUnitsScrollDistance < minScrollDistance)
                resultUnitsScrollDistance = 0;
            else
                resultUnitsScrollDistance -= minScrollDistance;

            scrollObjectService.ReloadScrollSystem(resultUnitsScrollDistance);
            campaignUnitsScrollDistance = resultUnitsScrollDistance;

        }
    }

    private void OpenCampaignLevelsList(CampaignData campaignData)
    {
        SetObjectsActive(spawnedCampaignsUnits,false);
        campaignsLevelsPanelLabel.text = levelsUnitsOnPanelLabelText;
        
        var resultUnitsScrollDistance = 0f;
        const int minScrollDistance = 900;
        
        var spawnedLevelsUnitsList = new List<GameObject>();
        
        foreach (var levelData in campaignData.CampaignLevels)
        {
            var unitSpawnObjT = scrollObjectService.ScrollObjectT;

            var spawnedUnitData = 
                Instantiate(campaignLevelUnitPrefab,unitSpawnObjT).GetComponent<CampaignLevelUnitData>();
            
            spawnedUnitData.transform.localPosition += new Vector3(0, -(resultUnitsScrollDistance), 0);
            spawnedUnitData.SetNewCampaignData(campaignData);
            spawnedUnitData.SetNewLevelData(levelData);
            spawnedUnitData.NameLabel.text = spawnedUnitData.LevelData.LevelName;
            
            if (!levelData.IsLevelUnlocked)
            {
                SetNewLockedLevel();
                void SetNewLockedLevel()
                {
                    spawnedUnitData.ButtonBackground.material = new Material(spawnedUnitData.ButtonBackground.material);
                    spawnedUnitData.ButtonBackground.material.SetFloat(BrightnessShaderId,
                        notUnlockedButtonMatBrightness);

                    spawnedUnitData.ButtonDecorationLine.material =
                        new Material(spawnedUnitData.ButtonDecorationLine.material);
                    spawnedUnitData.ButtonDecorationLine.material.SetFloat(BrightnessShaderId,
                        notUnlockedButtonDecLineBrightness);

                    spawnedLevelsUnitsList.Add(spawnedUnitData.gameObject);
                    resultUnitsScrollDistance += unitsDistance;
                }
                
                continue;
            }

            SetUnlockedCampaign();
            void SetUnlockedCampaign()
            {
                var spawnedUnitButton = spawnedUnitData.GetComponent<ButtonMainService>();
                spawnedUnitButton.onClickAction.AddListener(
                    (() =>
                    {
                        campaignsLevelService.SelectNewLevel(spawnedUnitData.CampaignData,spawnedUnitData.LevelData);
                    }
                        ));

                spawnedLevelsUnitsList.Add(spawnedUnitData.gameObject);
                resultUnitsScrollDistance += unitsDistance;
            }
        }

        spawnedLevelUnits = spawnedLevelsUnitsList.ToArray();
        
        backToCampaignUnitsButton.SetActive(true);
        
        SetNewScrollDistance();
        void SetNewScrollDistance()
        {
            if (resultUnitsScrollDistance < minScrollDistance)
                resultUnitsScrollDistance = 0;
            else
                resultUnitsScrollDistance -= minScrollDistance;

            scrollObjectService.ReloadScrollSystem(resultUnitsScrollDistance);
        }
    }

    public void BackToCampaignUnits()
    {
        foreach (var levelUnit in spawnedLevelUnits)
            if (levelUnit != null)
                Destroy(levelUnit);
        
        backToCampaignUnitsButton.SetActive(false);
        SetObjectsActive(spawnedCampaignsUnits,true);

        campaignsLevelsPanelLabel.text = campaignsUnitsOnPanelLabelText;
        scrollObjectService.ReloadScrollSystem(campaignUnitsScrollDistance);
    }
    
    private void SetObjectsActive(GameObject[] gameObjects, bool activeState)
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(activeState);
        }
    }
}
