using System.Collections.Generic;
using UnityEngine;

public class LevelPassagePointsVisualizer : MonoBehaviour
{
    private PlayerMainService playerMainService;
    private LevelPassagePointsService levelPassagePointsService;
    private List<Transform> pointsT = new List<Transform>();
    private Camera playerCamera;
    private Transform playerCameraT;
    private Vector2[] cashedPointsCanvasPoses = new Vector2[16];
    
    [SerializeField] private GameObject pointPrefab;

    private void Start()
    {
        levelPassagePointsService = LevelPassagePointsService.instance;
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        playerCamera = Camera.main;
        playerCameraT = playerCamera.transform;
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
            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
            var cameraOrigin = playerCameraT.position + playerCameraT.forward * 0.1f; 
            
            for (var i = 0; i < pointsT.Count; i++)
            {
                var pointT = pointsT[i];
                var pointPos = points[i].position;

                var checkRay = new Ray(cameraOrigin, -(cameraOrigin - pointPos));

                var minDistance = 9999f;
                
                for (int j = 0; j < 4; j++)
                {
                    if (cameraPlanes[j].Raycast(checkRay, out var distance))
                    {
                        if (distance < minDistance)
                            minDistance = distance;
                    }
                }

                pointPos = checkRay.GetPoint(minDistance);
                
                Vector2 canvasPointPos = playerCamera.WorldToScreenPoint(pointPos);;

                const float canvasWidth = 1920f;
                const float canvasHeight = 1080f;
                
                var xAmount = Screen.width / canvasWidth;
                var yAmount = Screen.height / canvasHeight;
                
                canvasPointPos.x /= xAmount / (playerCamera.aspect / (canvasWidth / canvasHeight));
                canvasPointPos.y /= yAmount;

                ClampPointPos();
                void ClampPointPos()
                {
                    canvasPointPos.x = Mathf.Clamp(canvasPointPos.x,0,
                            canvasWidth * (playerCamera.aspect / (canvasWidth / canvasHeight)));
                    
                    canvasPointPos.y = Mathf.Clamp(canvasPointPos.y, 0, canvasHeight);
                }
                
                pointT.localPosition = canvasPointPos;
            }
        }
    }

    Transform[] GetPoints()
    {
        Transform[] pointTransforms;

        if(levelPassagePointsService.CurrentZoneIsNonLinear)
        {
            var isOneOfNonLinearZonesGoingNow = levelPassagePointsService.CurrentPassagePoint != null;
            if (isOneOfNonLinearZonesGoingNow)
            {
                pointTransforms = new Transform[]{levelPassagePointsService.CurrentPassagePoint.pointT};
                return pointTransforms;
            }
            
            var points = new List<Transform>();
            foreach (var nonLinearZone in levelPassagePointsService.CurrentPassageZone.NonLinearPassageZones)
            {
                if(!nonLinearZone.zoneIsBlocked)
                    points.Add(nonLinearZone.PassagePoints[0].pointT);
            }

            pointTransforms = points.ToArray();
        }
        else
            pointTransforms = new Transform[]{levelPassagePointsService.CurrentPassagePoint.pointT};
        

        return pointTransforms;
    }
    
}
