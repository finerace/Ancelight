using System;
using System.Collections.Generic;
using UnityEngine;

public class HookPointsVisualizer : MonoBehaviour
{
    private PlayerMainService playerMainService;
    private PlayerHookService playerHookService;
    private List<Transform> hookPointsT = new List<Transform>();

    [SerializeField] private GameObject hookPointPrefab;

    private void Awake()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        playerHookService = playerMainService.hookService;
    }

    private void Update()
    {
        UpdateHookPointsCount();
        void UpdateHookPointsCount()
        {
            var hookPointTransforms = playerHookService.HookPoints;
            if(hookPointTransforms == null)
                return;
            
            var spawnedTheoryIndex = hookPointTransforms.Length - hookPointsT.Count;

            if (spawnedTheoryIndex > 0)
            {
                for (int i = 0; i < spawnedTheoryIndex; i++)
                {
                    var newHookPoint = Instantiate(hookPointPrefab, transform);
                    hookPointsT.Add(newHookPoint.transform);
                }
            }
            if (spawnedTheoryIndex < 0)
            {
                for (int i = 0; i < -spawnedTheoryIndex; i++)
                {
                    if(i >= hookPointsT.Count || i < 0)
                        return;
                    
                    var toDestroyHookPoint = hookPointsT[i].gameObject;
                    hookPointsT.Remove(hookPointsT[i]);
                    Destroy(toDestroyHookPoint);
                }
            }
        }
        
        UpdateHookPointsPositionOnScreen();
        void UpdateHookPointsPositionOnScreen()
        {
            var hookPointTransforms = playerHookService.HookPoints;
            if(hookPointTransforms == null)
                return;
            
            for (var i = 0; i < hookPointTransforms.Length; i++)
            {
                var hookPointT = hookPointsT[i];
                
                var playerCamera = playerMainService.playerLook.mainCamera;
                var hookObjectPos = hookPointTransforms[i].position;

                Vector2 hookPointPos = playerCamera.WorldToScreenPoint(hookObjectPos);

                var xAmount = playerCamera.pixelWidth / 1920f;
                var yAmount = playerCamera.pixelHeight / 1080f;

                hookPointPos.x /= xAmount;
                hookPointPos.y /= yAmount;

                hookPointT.localPosition = hookPointPos;
            }
        }
    }
    
}
