using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuitInformationSelectorObjectSpawner : MonoBehaviour
{
    [SerializeField] private SuitInformationDataBase informationDataBase;
    [SerializeField] private GameObject suitInformationObject;
    [SerializeField] private float objectsSpawnDistance;

    [Space] 
    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ScrollObjectService scrollObjectService;

    private void Awake()
    {
        informationDataBase = FindObjectOfType<SuitInformationDataBase>();
    }

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        var unlockedInformationsDatasId = informationDataBase.UnlockedInformationDatas;
        var spawnedSelectors = 0;
        
        foreach (var id in unlockedInformationsDatasId)
        {
            var buttonSelector = 
                Instantiate(suitInformationObject, spawnPoint).GetComponent<SuitInformationButtonSelector>();

            buttonSelector.transform.localPosition = 
                new Vector3(0, -(spawnedSelectors * objectsSpawnDistance),0);
            
            buttonSelector.SetInformation(id);
            
            spawnedSelectors++;
        }
        
        
    }
    
    
    
}
