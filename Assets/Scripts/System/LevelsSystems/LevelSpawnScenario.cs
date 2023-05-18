using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class LevelSpawnScenario : MonoBehaviour
{
    [SerializeField]private bool isScenarioActive;
    private bool isScenarioEnded;
    
    [SerializeField,Range(0,240)] private int repeatCounts = 0;
    
    [Space] 
    
    [SerializeField] private SpawnScenarioUnit[] spawnScenario;
    
    [Space] 
    
    [SerializeField] private UnityEvent onKillAllEnemiesEvent;

    private int toDestroyObjectsCount;
    [HideInInspector] [SerializeField] private int nowDestroyedObjectsCount;

    private bool isScenarioLoaded = false;
    [HideInInspector] [SerializeField] private float timeFromActivate = 0;
    
    [HideInInspector] [SerializeField] private int mainLoopCount;
    [HideInInspector] [SerializeField] private float mainScenarioUnitWaitTime;
    
    [HideInInspector] [SerializeField] private int mainCurrentScenarioUnitLoopCount;
    [HideInInspector] [SerializeField] private bool isMainCurrentScenarioUnitCompleted;
    
    [HideInInspector] [SerializeField] private int mainCurrentScenarioRepeatCounts;
    [HideInInspector] [SerializeField] private int mainRepeatScenarioUnitCurrentRepeatCounts;
    [HideInInspector] [SerializeField] private int mainRepeatScenarioUnitLoopCount;
    [HideInInspector] [SerializeField] private float mainRepeatScenarioUnitWaitTime;
    [HideInInspector] [SerializeField] private float mainRepeatWaitCooldownTime;
    [HideInInspector] [SerializeField] private bool isMainRepeatScenarioUnitCompleted;

    [HideInInspector] [SerializeField] private float mainToNextScenarioWaitTime;
    
    [HideInInspector] [SerializeField] private bool isMainCurrentScenarioUnitCompletedRepeating;

    [Space] 
    
    [SerializeField] private GameObject teleportationEffectPrefab;
    
    public bool IsScenarioActive => isScenarioActive;

    public bool IsScenarioEnded => isScenarioEnded;

    public int RepeatCounts => repeatCounts;

    public SpawnScenarioUnit[] SpawnScenario => spawnScenario;

    public UnityEvent OnKillAllEnemiesEvent => onKillAllEnemiesEvent;

    public int ToDestroyObjectsCount => toDestroyObjectsCount;

    public int NowDestroyedObjectsCount => nowDestroyedObjectsCount;

    public bool IsScenarioLoaded => isScenarioLoaded;

    public float TimeFromActivate => timeFromActivate;

    public int MainLoopCount => mainLoopCount;

    public float MainScenarioUnitWaitTime => mainScenarioUnitWaitTime;

    public int MainCurrentScenarioUnitLoopCount => mainCurrentScenarioUnitLoopCount;

    public bool IsMainCurrentScenarioUnitCompleted => isMainCurrentScenarioUnitCompleted;

    public int MainCurrentScenarioRepeatCounts => mainCurrentScenarioRepeatCounts;

    public int MainRepeatScenarioUnitCurrentRepeatCounts => mainRepeatScenarioUnitCurrentRepeatCounts;

    public int MainRepeatScenarioUnitLoopCount => mainRepeatScenarioUnitLoopCount;

    public float MainRepeatScenarioUnitWaitTime => mainRepeatScenarioUnitWaitTime;

    public float MainRepeatWaitCooldownTime => mainRepeatWaitCooldownTime;

    public bool IsMainRepeatScenarioUnitCompleted => isMainRepeatScenarioUnitCompleted;

    public float MainToNextScenarioWaitTime => mainToNextScenarioWaitTime;

    public bool IsMainCurrentScenarioUnitCompletedRepeating => isMainCurrentScenarioUnitCompletedRepeating;

    [SerializeField] private int spawnerId;

    public int SpawnerId => spawnerId;
    
    public void ActivateSpawnScenario()
    {
        if(isScenarioActive)
            return;

        isScenarioActive = true;
        
        StartCoroutine(StartScenario());
    }

    private void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
        
        toDestroyObjectsCount = GetMustBeDestroyObjectsCount();
    }
    
    private void Update()
    {
        if (isScenarioActive)
            timeFromActivate += Time.deltaTime;
    }

    private IEnumerator StartScenario()
    {
        var isFirstLoadLoopActivation = isScenarioLoaded;
        
        for (int i = 0; i < spawnScenario.Length; i++)
        {
            if (isFirstLoadLoopActivation)
                i = mainLoopCount;
            
            var scenarioUnit = spawnScenario[i];
            mainLoopCount = i;
            
            if(!isMainCurrentScenarioUnitCompleted)
                StartCoroutine(ActivateScenarioUnit(scenarioUnit,
                    true,
                    isFirstLoadLoopActivation));
            
            while (true)
            {
                if(isMainCurrentScenarioUnitCompleted)
                    break;

                isFirstLoadLoopActivation = false;
                yield return null;
            }

            mainCurrentScenarioUnitLoopCount = 0;
            
            var scenarioUnitCanBeRepeat =
                scenarioUnit.RepeatCounts > 0;

            if (scenarioUnitCanBeRepeat)
            { 
                if(!isMainCurrentScenarioUnitCompletedRepeating)
                    StartCoroutine( StartRepeatScenarioUnit(scenarioUnit,true,isFirstLoadLoopActivation) );
                
                while (true)
                {
                    if(isMainCurrentScenarioUnitCompletedRepeating)
                        break;

                    isFirstLoadLoopActivation = false;
                    yield return null;
                }
            }
            
            scenarioUnit.OnCompleteScenarioUnitEvent?.Invoke();

            mainRepeatScenarioUnitLoopCount = 0;

            var waitTime = timeFromActivate + scenarioUnit.ToNextScenarioUnitTime;

            if (isFirstLoadLoopActivation)
                waitTime = mainToNextScenarioWaitTime;

            mainToNextScenarioWaitTime = waitTime;

            while (true)
            {
                if(timeFromActivate >= waitTime)
                    break;

                isFirstLoadLoopActivation = false;
                
                yield return null;

            }
            
            isMainCurrentScenarioUnitCompleted = false;
            isMainCurrentScenarioUnitCompletedRepeating = false;
            
            if(isFirstLoadLoopActivation)
                isFirstLoadLoopActivation = false;

            var isLastSpawnScenarioUnitActivation = !(i+1 < spawnScenario.Length);
            var scenarioCanBeRepeated = repeatCounts > mainCurrentScenarioRepeatCounts;

            if (isLastSpawnScenarioUnitActivation && scenarioCanBeRepeated)
            {
                mainCurrentScenarioRepeatCounts++;
                i = -1;
            }
            
        }

        isScenarioActive = false;
    }

    private IEnumerator ActivateScenarioUnit(SpawnScenarioUnit scenarioUnit,bool isMainScenarioUnit = false,bool isLoadedScenarioUnit = false)
    {
        var toSpawnObject = scenarioUnit.SpawnObject;

        var spawnCooldown = scenarioUnit.ObjectsSpawnCooldown;

        var isFirstLoadLoopSpawn = isLoadedScenarioUnit;

        var isMainRepeatActivation = !isMainScenarioUnit;
        
        for (var i = 0; i < scenarioUnit.SpawnPoints.Length; i++)
        {
            if (isMainRepeatActivation && isFirstLoadLoopSpawn)
                i = mainRepeatScenarioUnitLoopCount;
            else if (isFirstLoadLoopSpawn)
                i = mainCurrentScenarioUnitLoopCount;
            
            if(isMainScenarioUnit)
                mainCurrentScenarioUnitLoopCount = i;
            else
                mainRepeatScenarioUnitLoopCount = i;
            
            var point = scenarioUnit.SpawnPoints[i];
            float waitTime = 0;

            if (!isFirstLoadLoopSpawn)
                waitTime = timeFromActivate + spawnCooldown;
            else if (!isMainRepeatActivation)
                waitTime = mainScenarioUnitWaitTime;
            else
                waitTime = mainRepeatScenarioUnitWaitTime;

            if (isMainScenarioUnit)
                mainScenarioUnitWaitTime = waitTime;
            else
                mainRepeatScenarioUnitWaitTime = waitTime;
        
            if (!isFirstLoadLoopSpawn && timeFromActivate < waitTime)
            {
                var spawnedObject =
                    SpawnObject(toSpawnObject, point.position,point.rotation,!scenarioUnit.IsQuietSpawn);
                
                if (scenarioUnit.WayPoint != null)
                    SetWayPoint(spawnedObject);

                if (scenarioUnit.IsMustBeDestroy)
                    RegistrationDieEvent(spawnedObject);
                
            }

            var isLastSpawn = (1 + i < scenarioUnit.SpawnPoints.Length) == false;
            
            if(isFirstLoadLoopSpawn && isMainScenarioUnit)
                mainCurrentScenarioUnitLoopCount = i+1;
            else if (isFirstLoadLoopSpawn)
                mainRepeatScenarioUnitLoopCount = i+1;
            
            while (true)
            {
                if(isLastSpawn)
                    break;
                    
                if (timeFromActivate >= waitTime)
                    break;

                yield return null;
            }

            if (isFirstLoadLoopSpawn)
                isFirstLoadLoopSpawn = false;
        }

        if(isMainScenarioUnit)
            isMainCurrentScenarioUnitCompleted = true;

        if (isMainRepeatActivation)
            isMainRepeatScenarioUnitCompleted = true;
        
        isLoadedScenarioUnit = false;
        
        void RegistrationDieEvent(GameObject spawnedObject)
        {
            if (!spawnedObject.TryGetComponent(out Health objectHealth))
                throw new InvalidCastException("This object not must be destroyed!");
            
            objectHealth.OnDie += RegistrationOneDestroy;
            objectHealth.OnDie += CheckDestroyCountHasAllKillEvent;
            
            void RegistrationOneDestroy()
            {
                nowDestroyedObjectsCount++;
            }

            void CheckDestroyCountHasAllKillEvent()
            {
                var isAllTargetObjectsDestroy = 
                    toDestroyObjectsCount == nowDestroyedObjectsCount;
                
                if(isAllTargetObjectsDestroy && onKillAllEnemiesEvent != null)
                    onKillAllEnemiesEvent.Invoke();
            }
        }

        void SetWayPoint(GameObject botObj)
        {
            if (botObj.TryGetComponent(out DefaultBot bot))
            {
                const int wayTime = 30;
                
                bot.SetWayPoint(scenarioUnit.WayPoint.position,wayTime);
            }
        }
    }
    
    private IEnumerator StartRepeatScenarioUnit(SpawnScenarioUnit scenarioUnit,bool isWaitRepeat = false,bool isLoadRepeat = false)
    {
        var isFirstLoadRepeat = isLoadRepeat;
        
        for (int i = 0; i < scenarioUnit.RepeatCounts; i++)
        {
            if (isFirstLoadRepeat)
                i = mainRepeatScenarioUnitCurrentRepeatCounts;

            mainRepeatScenarioUnitCurrentRepeatCounts = i;
            
            var waitCooldown = timeFromActivate + scenarioUnit.RepeatCooldown;

            if (isFirstLoadRepeat)
                waitCooldown = mainRepeatWaitCooldownTime;

            mainRepeatWaitCooldownTime = waitCooldown;
            
            while (true)
            {
                if(timeFromActivate >= waitCooldown)
                    break;

                isFirstLoadRepeat = false;
                
                yield return null;
            }
            
            StartCoroutine(ActivateScenarioUnit(scenarioUnit,false,isFirstLoadRepeat));

            while (true)
            {
                if(isMainRepeatScenarioUnitCompleted)
                    break;

                yield return null;
            }

            isMainRepeatScenarioUnitCompleted = false;
            isFirstLoadRepeat = false;
        }

        if (isWaitRepeat)
            isMainCurrentScenarioUnitCompletedRepeating = true;
    }
    
    private GameObject SpawnObject(GameObject obj, Vector3 pos,Quaternion rotation,bool withEffects)
    {
        var spawnedObject = Instantiate(obj, pos, rotation);

        if (withEffects)
        {
            var teleportationEffect = Instantiate(teleportationEffectPrefab, pos, rotation);
            Destroy(teleportationEffect, 1.25f);
        }

        return spawnedObject;
    }

    private int GetMustBeDestroyObjectsCount()
    {
        var resultCount = 0;
        
        foreach (var scenarioUnit in spawnScenario)
        {
            var localResult = 0;
            
            if (scenarioUnit.IsMustBeDestroy)
                localResult++;
            
            localResult *= (scenarioUnit.RepeatCounts+1);
            localResult *= scenarioUnit.SpawnPoints.Length;

            resultCount += localResult;
        }

        resultCount *= (repeatCounts + 1);

        return resultCount;
    }

    public void LoadSpawnScenario(LevelSpawnScenario savedScenario)
    {
        isScenarioActive = savedScenario.isScenarioActive;
        repeatCounts = savedScenario.repeatCounts;
        toDestroyObjectsCount = savedScenario.toDestroyObjectsCount;
        nowDestroyedObjectsCount = savedScenario.nowDestroyedObjectsCount;
        timeFromActivate = savedScenario.timeFromActivate;
        mainLoopCount = savedScenario.mainLoopCount;
        mainScenarioUnitWaitTime = savedScenario.mainScenarioUnitWaitTime;
        mainCurrentScenarioUnitLoopCount = savedScenario.MainCurrentScenarioUnitLoopCount;
        isMainCurrentScenarioUnitCompleted = savedScenario.isMainCurrentScenarioUnitCompleted;
        mainCurrentScenarioRepeatCounts = savedScenario.mainCurrentScenarioRepeatCounts;
        mainRepeatScenarioUnitCurrentRepeatCounts = savedScenario.mainRepeatScenarioUnitCurrentRepeatCounts;
        mainRepeatScenarioUnitLoopCount = savedScenario.MainRepeatScenarioUnitLoopCount;
        mainRepeatScenarioUnitWaitTime = savedScenario.mainRepeatScenarioUnitWaitTime;
        mainRepeatWaitCooldownTime = savedScenario.mainRepeatWaitCooldownTime;
        isMainRepeatScenarioUnitCompleted = savedScenario.isMainRepeatScenarioUnitCompleted;
        mainToNextScenarioWaitTime = savedScenario.mainToNextScenarioWaitTime;
        isMainCurrentScenarioUnitCompletedRepeating = savedScenario.isMainCurrentScenarioUnitCompletedRepeating;
        
        if(isScenarioActive)
            isScenarioLoaded = true;
        
        if(isScenarioActive)
            StartCoroutine(StartScenario());
    }

    public void SetNewID(int newId)
    {
        spawnerId = newId;
    }
}

[Serializable]
public class SpawnScenarioUnit
{
    [SerializeField,Range(0,240)] private float toNextScenarioUnitTime;
    [SerializeField] private bool isMustBeDestroy;
    
    [Space]

    [SerializeField] private GameObject spawnObject;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField,Range(0.025f,240)] private float objectsSpawnCooldown = 1;
    [SerializeField] private Transform wayPoint;

    [Space]
    
    [SerializeField,Range(0,240)] private int repeatCounts;
    [SerializeField,Range(0,240)] private float repeatCooldown = 1;
    
    [Space]
    
    [SerializeField] private bool isQuietSpawn;

    [SerializeField] private UnityEvent onCompleteScenarioUnitEvent;

    public UnityEvent OnCompleteScenarioUnitEvent => onCompleteScenarioUnitEvent;

    public float ToNextScenarioUnitTime => toNextScenarioUnitTime;

    public bool IsMustBeDestroy => isMustBeDestroy;

    public GameObject SpawnObject => spawnObject;

    public Transform[] SpawnPoints => spawnPoints;

    public float ObjectsSpawnCooldown => objectsSpawnCooldown;

    public Transform WayPoint => wayPoint;

    public int RepeatCounts => repeatCounts;

    public float RepeatCooldown => repeatCooldown;
    
    public bool IsQuietSpawn => isQuietSpawn;
}