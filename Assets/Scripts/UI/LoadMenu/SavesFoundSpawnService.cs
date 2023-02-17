using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavesFoundSpawnService : MonoBehaviour
{
    private string savesPath;

    [Space] 
    
    [SerializeField] private Transform spawnPivot;
    [SerializeField] private int savesSpawnDistance;
    
    [Space]
    
    [SerializeField] private GameObject saveUnitPrefab;
    [SerializeField] private ScrollObjectService scrollObjectService;
    [SerializeField] private SavesSelectLoadDeleteService savesSelectLoadDeleteService;
    
    private List<SaveUnitData> spawnedSaveUnits = new List<SaveUnitData>();

    public IEnumerable<SaveUnitData> SpawnedSaveUnits => spawnedSaveUnits;

    private void Awake()
    {
        SpawnSaveUnits(FoundSavesNames());

        savesPath = $"{Application.persistentDataPath}/Saves";
    }

    private void OnEnable()
    {
        ReloadSaves();
    }

    private void SpawnSaveUnits(string[] saveNames)
    {
        var spawnedSavesCount = 0;
        
        foreach (var saveName in saveNames)
        {
            var saveDateData = File.GetLastWriteTime($"{savesPath}/{saveName}.bob");
            
            var spawnedSave = 
                Instantiate(saveUnitPrefab,spawnPivot);

            var spawnedSaveT = spawnedSave.transform;
            var spawnedSaveData = spawnedSave.GetComponent<SaveUnitData>();
            var spawnedSaveButtonService = spawnedSave.GetComponent<ButtonMainService>();

            var saveDate = 
                $"{AuxiliaryFunc.ConvertNumCharacters(saveDateData.Month)}." +
                $"{AuxiliaryFunc.ConvertNumCharacters(saveDateData.Day)}." +
                $"{saveDateData.Year}" +
                $"\n{AuxiliaryFunc.ConvertNumCharacters(saveDateData.Hour)}:" +
                $"{AuxiliaryFunc.ConvertNumCharacters(saveDateData.Minute)}";
            
            spawnedSaveT.localPosition += (new Vector3(0, -(spawnedSavesCount * savesSpawnDistance),0));
            
            spawnedSaveData.CreateSave(saveName,saveDate);
            spawnedSaveButtonService.onClickAction.AddListener
                (() => {savesSelectLoadDeleteService.SelectSave(spawnedSaveData);});
            
            spawnedSaveUnits.Add(spawnedSaveData);

            spawnedSavesCount++;
        }
        
        ReloadScrollSystem();
        
        void ReloadScrollSystem()
        {
            var minimumActivateScrollDistance = 950;
            var newScrollDistance = spawnedSavesCount * savesSpawnDistance;

            if (newScrollDistance < minimumActivateScrollDistance)
                newScrollDistance = 950;

            var resultScrollDistance = newScrollDistance - minimumActivateScrollDistance;

            scrollObjectService.ReloadScrollSystem(resultScrollDistance);
        }
    }

    public void ReloadSaves()
    {
        DestroyOldSaveUnits();
        
        SpawnSaveUnits(FoundSavesNames());
        
        void DestroyOldSaveUnits()
        {
            for (int i = 0; i < spawnedSaveUnits.Count; i++)
            {
                var saveUnitObj = spawnedSaveUnits[i].gameObject;

                DestroyImmediate(saveUnitObj);
            }
            
            spawnedSaveUnits.Clear();
        }
    }
    
    public static string[] FoundSavesNames()
    {
        var savesPath = $"{Application.persistentDataPath}/Saves";
        
        if (!Directory.Exists(savesPath))
            Directory.CreateDirectory(savesPath);
        
        var foundedFiles = 
            Directory.GetFiles(savesPath);

        var savesNamesNotSorted = new List<string>();

        foreach (var fileName in foundedFiles)
        {
            if (fileName.EndsWith(".bob"))
            {
                var resultFileName = fileName;

                resultFileName = resultFileName.Remove(0, savesPath.Length+1);
                resultFileName = resultFileName.Remove(resultFileName.Length - 4);
                
                savesNamesNotSorted.Add(resultFileName);   
            }
        }

        var saveNamesSorted = SortSavesByDate(savesNamesNotSorted.ToArray());
        
        return saveNamesSorted;

        string[] SortSavesByDate(string[] saveNames)
        {
            var saveNamesNotSorted = new List<string>();
            foreach (var saveName in saveNames)
                saveNamesNotSorted.Add(saveName);

            var saveNamesSorted = new List<string>();
            var savesCount = saveNames.Length;
            
            for (var i = 0; i < savesCount; i++)
            {
                for (int j = 0; j < saveNamesNotSorted.Count; j++)
                {
                    var isLastIteration = !(1+j < saveNamesNotSorted.Count);
                    if(isLastIteration)
                        break;

                    var currentSaveName = saveNames[j];
                    var nextSaveName = saveNames[j + 1];
                    
                    var currentSavePath = $"{savesPath}/{currentSaveName}.bob";
                    var nextSavePath = $"{savesPath}/{nextSaveName}.bob";

                    var currentSaveDate = File.GetCreationTime(currentSavePath);
                    var nextSaveDate = File.GetCreationTime(nextSavePath);

                    var compareResult = DateTime.Compare(currentSaveDate,nextSaveDate);

                    if (compareResult < 0)
                        saveNames[j + 1] = nextSaveName;
                    else if (compareResult == 0 && j + 2 < saveNamesSorted.Count)
                    {
                        saveNames[j + 2] = currentSaveName;   
                        saveNames[j + 1] = nextSaveName;
                    }
                    else
                        saveNames[j + 1] = currentSaveName;
                }
                
                saveNamesSorted.Add(saveNames[saveNames.Length-1]);
                saveNamesNotSorted.Remove(saveNames[saveNames.Length - 1]);
                saveNames = saveNamesNotSorted.ToArray();
            }

            return saveNamesSorted.ToArray();
        }
    }
}
