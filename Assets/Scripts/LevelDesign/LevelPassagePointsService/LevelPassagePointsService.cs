using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelPassagePointsService : MonoBehaviour
{
    public static LevelPassagePointsService instance;
    [SerializeField] private LevelPassagePointsZone[] passagePointsZones;
    
    [Space]
    
    [SerializeField] private LevelPassagePoint currentPassagePoint;
    [SerializeField] private LevelPassagePointsZone currentPassageZone;
    [SerializeField] private bool currentZoneIsNonLinear;

    [SerializeField] private int currentZoneId;
    [SerializeField] private int currentPointId;
    [SerializeField] private int currentNonLinearZoneId = -1;

    [HideInInspector] [SerializeField] private List<int> blockedNonLinearZoneZoneId;
    [HideInInspector] [SerializeField] private List<int> blockedNonLinearZoneNonLinearZoneId;

    public LevelPassagePoint CurrentPassagePoint => currentPassagePoint;
    public LevelPassagePointsZone CurrentPassageZone => currentPassageZone;
    public bool CurrentZoneIsNonLinear => currentZoneIsNonLinear;

    private bool isLoad = false;

    public void Load(LevelPassagePointsService levelPassagePointsService)
    {
        isLoad = true;
        
        SetSavesData();
        void SetSavesData()
        {
            currentZoneId = levelPassagePointsService.currentZoneId;
            currentPointId = levelPassagePointsService.currentPointId;
            currentNonLinearZoneId = levelPassagePointsService.currentNonLinearZoneId;

            blockedNonLinearZoneZoneId = levelPassagePointsService.blockedNonLinearZoneZoneId;
            blockedNonLinearZoneNonLinearZoneId = levelPassagePointsService.blockedNonLinearZoneNonLinearZoneId;

            currentZoneIsNonLinear = levelPassagePointsService.currentZoneIsNonLinear;
        }

        LoadCurrentZonesAndPoints();
        void LoadCurrentZonesAndPoints()
        {
            currentPassageZone = passagePointsZones[currentZoneId];

            if (currentPointId < 0)
            {
                currentPassagePoint = null;
                return;
            }

            if (currentNonLinearZoneId >= 0)
            {
                currentPassagePoint = currentPassageZone.NonLinearPassageZones[currentNonLinearZoneId]
                    .PassagePoints[currentPointId];
            }
            else
                currentPassagePoint = currentPassageZone.PassagePoints[currentPointId];
        }
        
        BlockedSavedBlockZones();
        void BlockedSavedBlockZones()
        {
            for (var i = 0; i < blockedNonLinearZoneZoneId.Count; i++)
            {
                var zoneId = blockedNonLinearZoneZoneId[i];
                var nonLinearZoneId = blockedNonLinearZoneNonLinearZoneId[i];

                passagePointsZones[zoneId].NonLinearPassageZones[nonLinearZoneId].zoneIsBlocked = true;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
        
        InitZonesAndPoints();

        if (isLoad)
            return;
        
        SetStartZoneAndPoint();
        void SetStartZoneAndPoint()
        {
            if (!passagePointsZones[0].NonLinearPassage)
            {
                currentPassagePoint = passagePointsZones[0].PassagePoints[0];
                currentPassageZone = passagePointsZones[0];
            }
            else
            {
                currentPassageZone = passagePointsZones[0];
                currentZoneIsNonLinear = true;
            }
        }
    }

    private void PlayerGoingToPoint(int zoneId,int pointId,int nonLinearZoneId = -1)
    {
        var goingZone = passagePointsZones[zoneId];

        currentZoneId = zoneId;
        
        if (nonLinearZoneId < 0)
        {
            currentNonLinearZoneId = -1;

            var isLastPoint = goingZone.PassagePoints.Length-1 == pointId;

            if (!isLastPoint)
            {
                currentPassagePoint = goingZone.PassagePoints[pointId + 1];
                currentPointId = pointId + 1;
                return;
            }

            var isLastZone = passagePointsZones.Length-1 == zoneId;
            if(isLastZone)
                return;

            GoToNextZone();
            
            return;
        }

        currentZoneIsNonLinear = true;
        
        var goingNoLinearZone = passagePointsZones[zoneId].NonLinearPassageZones[nonLinearZoneId];
        currentNonLinearZoneId = nonLinearZoneId;
    
        var isFirstPoint = pointId == 0;
        if (isFirstPoint)
        {
            currentPassagePoint = null;
            currentPointId = -1;
            return;
        }
        
        var isLastNonLinearPoint = goingNoLinearZone.PassagePoints.Length-1 == pointId;

        if (!isLastNonLinearPoint)
        {
            currentPassagePoint = goingNoLinearZone.PassagePoints[pointId+1];
            currentPointId = pointId + 1;
        }
        else
        {
            goingNoLinearZone.zoneIsBlocked = true;
            
            blockedNonLinearZoneZoneId.Add(zoneId);
            blockedNonLinearZoneNonLinearZoneId.Add(nonLinearZoneId);
            
            print((zoneId,nonLinearZoneId));
            
            foreach (var nonLinearPassageZone in goingZone.NonLinearPassageZones)
            {
                if (!nonLinearPassageZone.zoneIsBlocked)
                {
                    currentPassagePoint = null;
                    currentPointId = -1;
                    return;
                }
            }

            var isLastZone = passagePointsZones.Length-1 == zoneId;
            currentPassageZone.zoneIsBlocked = true;
            
            if(isLastZone)
                return;
            
            GoToNextZone();
        }

        void GoToNextZone()
        {
            currentPassageZone = passagePointsZones[zoneId + 1];
            currentZoneId = zoneId + 1;
            
            if (!currentPassageZone.NonLinearPassage)
            {
                currentPassagePoint = currentPassageZone.PassagePoints[0];
                currentPointId = 0;
                currentZoneIsNonLinear = false;
                currentNonLinearZoneId = -1;
            }
            else
            {
                var isLastZone = passagePointsZones.Length - 1 == zoneId;

                if (currentPassageZone.zoneIsBlocked && !isLastZone)
                {
                    zoneId++;
                    GoToNextZone();
                    return;
                }
                    
                currentPassagePoint = null;
                currentPointId = -1;
                currentZoneId = zoneId+1;
                currentZoneIsNonLinear = true;
                currentNonLinearZoneId = -1;
            }
        }
    }
    
    private void InitZonesAndPoints()
    {
        for (int i = 0; i < passagePointsZones.Length; i++)
        {
            var zoneItem = passagePointsZones[i];
            zoneItem.SetNewId(i);
            
            if (!zoneItem.NonLinearPassage)
            {
                for (int j = 0; j < zoneItem.PassagePoints.Length; j++)
                {
                    var pointItem = zoneItem.PassagePoints[j];
                    pointItem.SetPointId(j);
                }
            }
            else
            {
                for (int j = 0; j < zoneItem.NonLinearPassageZones.Length; j++)
                {
                    var nonLinearZoneItem = zoneItem.NonLinearPassageZones[j];
                    nonLinearZoneItem.SetNewId(j);
                    
                    for (int k = 0; k < nonLinearZoneItem.PassagePoints.Length; k++)
                    {
                        var nonLinearPointItem = nonLinearZoneItem.PassagePoints[k];
                        nonLinearPointItem.SetPointId(k);
                    }

                    if (nonLinearZoneItem.NonLinearPassage)
                        throw new Exception("In non-linear zone can not be created second non-linear zone!");
                }
            }
            
            zoneItem.Init();
            zoneItem.OnPlayerGoing += PlayerGoingToPoint;
        }
    }

    [Serializable]
    public class LevelPassagePointsZone
    {
        [SerializeField] private LevelPassagePoint[] passagePoints;
        public LevelPassagePoint[] PassagePoints => passagePoints;
        
        private int id;
        public event Action<int, int, int> OnPlayerGoing;
        
        public void SetNewId(int newZoneId)
        {
            id = newZoneId;
        }

        public void Init()
        {
            if (!nonLinearPassage)
            {
                foreach (var passagePoint in passagePoints)
                {
                    passagePoint.OnPlayerInPoint += PlayerGoingToPointLinear;
                }
                
                return;
            }

            foreach (var nonLinearPassageZone in nonLinearPassageZones)
            {
                nonLinearPassageZone.Init();

                nonLinearPassageZone.OnPlayerGoing += PlayerGoingToPointNonLinear;
            }
            
        }

        private void PlayerGoingToPointLinear(int pointId)
        {
            if(zoneIsBlocked)
                return;

            OnPlayerGoing?.Invoke(id,pointId,-1);
        }
        
        private void PlayerGoingToPointNonLinear(int zoneId,int pointId,int none = -1)
        {
            if(zoneIsBlocked)
                return;
            
            OnPlayerGoing?.Invoke(id,pointId,zoneId);
        }
        public bool zoneIsBlocked;
        
        [Space] 
        [SerializeField] private bool nonLinearPassage;
        [SerializeField] private LevelPassagePointsZone[] nonLinearPassageZones;
        
        public bool NonLinearPassage => nonLinearPassage;
        public LevelPassagePointsZone[] NonLinearPassageZones => nonLinearPassageZones;

    }
    
}
