using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLookService : MonoBehaviour
{
    private Transform mainCameraT;
    private Transform shootT;

    public Transform MainCameraT => mainCameraT;

    public Transform ShootingPoint => shootT;

    public Camera mainCamera;
    private RaycastHit cameraHit;
    private RaycastHit shootingHit;
    private bool isCameraHit;
    private bool isShootingHit;

    public bool IsCameraHit { get => isCameraHit; }
    public bool IsShootingHit { get => isShootingHit; }

    [SerializeField] private LayerMask shootingLayerMask;

    private void Start()
    {

        shootingLayerMask =
            GameObject.Find("LayerMasks").GetComponent<LayerMasks>().PlayerShootingLayerMask;

        if (mainCamera == null) mainCamera = Camera.main;
        mainCameraT = mainCamera.transform;
        shootT = mainCamera.transform.parent.gameObject.GetComponent<PlayerWeaponsManager>().shootingPoint;
    }

    private void FixedUpdate()
    {
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        if (mainCameraT != null)
        {
            Ray newRay = new Ray(mainCameraT.position, mainCameraT.forward);

            if (!Physics.Raycast(newRay, out cameraHit, 2000))
                isCameraHit = false;
            else isCameraHit = true;
        }

        if (shootT != null)
        {
            Ray newRay = new Ray(shootT.position, shootT.forward);

            if(!Physics.Raycast(newRay, out shootingHit, 2000,shootingLayerMask))
                isShootingHit = false;
            else 
                isShootingHit = true;
        }
    }
    
    public RaycastHit GetCameraRayHit()
    {
        if(isCameraHit)
            return cameraHit;
        else return new RaycastHit();
    }

    public RaycastHit GetShootingRayHit()
    {
        if (isShootingHit)
            return shootingHit;
        else return new RaycastHit();
    }

}
