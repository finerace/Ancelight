using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitInformationSelectorObjectSpawner : MonoBehaviour
{
    private SuitInformationDataBase informationDataBase;
    private SuitInformationSetUI suitInformationSetUI;
    [SerializeField] private GameObject suitInformationObject;
    [SerializeField] private float objectsSpawnDistance;
    
    [Space] 
    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ScrollObjectService scrollObjectService;
    [SerializeField] private float allScrollDistance;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        informationDataBase = FindObjectOfType<SuitInformationDataBase>();
        suitInformationSetUI = FindObjectOfType<SuitInformationSetUI>();
    }
    
    private void OnEnable()
    {
        ReSpawnSelectors();
    }

    private void ReSpawnSelectors()
    {
        DeleteOldSelectors();
        void DeleteOldSelectors()
        {
            foreach (var oldSelector in spawnedObjects.ToArray())
            {
                Destroy(oldSelector);
            }
            
            spawnedObjects.Clear();
        }
        
        var unlockedInformationsDatasId = informationDataBase.UnlockedInformationDatas;
        var spawnedSelectors = 0;
        
        foreach (var id in unlockedInformationsDatasId)
        {
            var buttonSelector = 
                Instantiate(suitInformationObject, spawnPoint).GetComponent<SuitInformationButtonSelector>();

            InitInformationSelector();
            void InitInformationSelector()
            {
                var informationName = informationDataBase.GetInformationData(id).InformationName;
                buttonSelector.SetInformation(id, informationName, suitInformationSetUI);
                spawnedObjects.Add(buttonSelector.gameObject);
            }
            
            buttonSelector.transform.localPosition = 
                new Vector3(0, -(spawnedSelectors * objectsSpawnDistance),0);
            
            spawnedSelectors++;
        }

        SetScrollDistance();
        void SetScrollDistance()
        {
            var scrollDistance = spawnedSelectors * objectsSpawnDistance;
            if (scrollDistance < allScrollDistance)
            {
                scrollObjectService.ReloadScrollSystem(0);
                return;
            }

            scrollObjectService.ReloadScrollSystem(Mathf.Abs(scrollDistance - allScrollDistance));
        }
    }

}
