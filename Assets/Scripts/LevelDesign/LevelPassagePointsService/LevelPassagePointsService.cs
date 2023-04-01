using System;
using UnityEngine;

public class LevelPassagePointsService : MonoBehaviour
{
    public static LevelPassagePointsService instance;
    [SerializeField] private LevelPassagePointsZone[] passagePointsZones;
    
    [Space]
    
    [SerializeField] private LevelPassagePoint currentPassagePoint;
    private LevelPassagePointsZone currentPassageZone;
    private bool currentZoneIsNonLinear;
    
    public LevelPassagePoint CurrentPassagePoint => currentPassagePoint;
    public LevelPassagePointsZone CurrentPassageZone => currentPassageZone;
    public bool CurrentZoneIsNonLinear => currentZoneIsNonLinear;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitZonesAndPoints();
        
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
        print($"{zoneId} {pointId} {nonLinearZoneId}");
        var goingZone = passagePointsZones[zoneId];

        if (nonLinearZoneId < 0)
        {
            var isLastPoint = goingZone.PassagePoints.Length-1 == pointId;

            if (!isLastPoint)
            {
                currentPassagePoint = goingZone.PassagePoints[pointId + 1];
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

        var isFirstPoint = pointId == 0;
        if (isFirstPoint)
        {
            currentPassagePoint = null;
            return;
        }
        
        var isLastNonLinearPoint = goingNoLinearZone.PassagePoints.Length-1 == pointId;

        if (!isLastNonLinearPoint)
        {
            currentPassagePoint = goingNoLinearZone.PassagePoints[pointId+1];
        }
        else
        {
            goingNoLinearZone.zoneIsBlocked = true;
            
            foreach (var nonLinearPassageZone in goingZone.NonLinearPassageZones)
            {
                if (!nonLinearPassageZone.zoneIsBlocked)
                {
                    currentPassagePoint = null;
                    return;
                }
            }

            var isLastZone = passagePointsZones.Length-1 == zoneId;
            if(isLastZone)
                return;
            
            GoToNextZone();
        }

        void GoToNextZone()
        {
            currentPassageZone = passagePointsZones[zoneId + 1];

            if (!currentPassageZone.NonLinearPassage)
            {
                currentPassagePoint = currentPassageZone.PassagePoints[0];
                currentZoneIsNonLinear = false;
            }
            else
            {
                currentPassagePoint = null;
                currentZoneIsNonLinear = true;
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
