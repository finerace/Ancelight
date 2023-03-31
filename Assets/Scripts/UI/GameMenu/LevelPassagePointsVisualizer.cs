using System.Collections.Generic;
using UnityEngine;

public class LevelPassagePointsVisualizer : MonoBehaviour
{
    private PlayerMainService playerMainService;
    private LevelPassagePointsService levelPassagePointsService;
    private List<Transform> pointsT = new List<Transform>();

    [SerializeField] private GameObject pointPrefab;

    private void Start()
    {
        levelPassagePointsService = LevelPassagePointsService.instance;
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

    private void Update()
    {
        UpdatePointsCount();
        void UpdatePointsCount()
        {
            var pointTransforms = GetPoints();            
            var spawnedTheoryIndex = pointTransforms.Length - pointsT.Count;

            if (spawnedTheoryIndex > 0)
            {
                for (int i = 0; i < spawnedTheoryIndex; i++)
                {
                    var newHookPoint = Instantiate(pointPrefab, transform);
                    pointsT.Add(newHookPoint.transform);
                }
            }
            if (spawnedTheoryIndex < 0)
            {
                for (int i = 0; i < -spawnedTheoryIndex; i++)
                {
                    if(i >= pointsT.Count || i < 0)
                        return;
                    
                    var toDestroyHookPoint = pointsT[i].gameObject;
                    pointsT.Remove(pointsT[i]);
                    Destroy(toDestroyHookPoint);
                }
            }
        }
        
        UpdatePointsPositionOnScreen();
        void UpdatePointsPositionOnScreen()
        {
            var points = GetPoints();
            
            for (var i = 0; i < pointsT.Count; i++)
            {
                var pointT = pointsT[i];
                
                var playerCamera = playerMainService.playerLook.mainCamera;
                var pointPos = points[i].position;
                
                Vector2 hookPointPos = playerCamera.WorldToScreenPoint(pointPos);

                var xAmount = Screen.width / 1920f;
                var yAmount = Screen.height / 1080f;
                
                hookPointPos.x /= xAmount / (playerCamera.aspect / (1920f / 1080f));
                hookPointPos.y /= yAmount;
                
                pointT.localPosition = hookPointPos;
            }
        }
    }

    Transform[] GetPoints()
    {
        var pointTransforms = new Transform[]{levelPassagePointsService.CurrentPassagePoint.pointT};
        
        if(levelPassagePointsService.CurrentZoneIsNonLinear)
        {
            var points = new List<Transform>();
            foreach (var nonLinearZone in levelPassagePointsService.CurrentPassageZone.NonLinearPassageZones)
            {
                points.Add(nonLinearZone.PassagePoints[0].pointT);
            }

            pointTransforms = points.ToArray();
        }

        return pointTransforms;
    }
    
}
