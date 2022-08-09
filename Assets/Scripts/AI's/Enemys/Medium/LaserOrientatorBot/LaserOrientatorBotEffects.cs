using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserOrientatorBotEffects : DefaultBotEffects
{

    [SerializeField] private MeshRenderer[] gunMeshes = new MeshRenderer[0];
    private Material mainGunMat;

    internal float gunTargetBright = 1;
    [SerializeField] private float gunBrightSpeed = 1f;
    [SerializeField] private float gunChargeBright = 8;
    [SerializeField] private float gunWaitBright = 1;
    [Header("Lazer manage")]
    [SerializeField] private LayerMask laserLayerMask;
    [SerializeField] private GameObject lasetPreObj;
    [SerializeField] private GameObject laserObj;
    [SerializeField] private Transform laserPont;
    public event Action<GameObject> laserHitEvent;
    private Transform laserT;
    private LaserState laserState = 0;


    protected new void Start()
    {
        base.Start();

        UniteGunMaterials();

        void UniteGunMaterials()
        {
            mainGunMat = new Material(gunMeshes[0].material);

            foreach (var item in gunMeshes)
            {
                item.material = mainGunMat;
            }
        }
    }

    protected new void Update()
    {
        base.Update();

        if(laserState != LaserState.Off)
            LazerVisualizeProcess();
    }

    public void SetLazerState(LaserState state)
    {
        if (state == laserState)
            print($"Lazer state already {state}");

        switch(state)
        {
            case LaserState.Off:
                EndLazerVisualize();
                break;

            case LaserState.Prepare:
                StartPreLazerVisualize();
                break;

            case LaserState.On:
                StartLazerVisualize();
                break;
        }

    }

    private void StartPreLazerVisualize()
    {
        if (laserState == LaserState.Prepare)
            return;

        laserState = LaserState.Prepare;

        Transform shotPoint = laserPont;
        laserT = Instantiate(lasetPreObj, shotPoint.position, shotPoint.rotation).transform;

    }

    private void StartLazerVisualize()
    {
        if (laserState == LaserState.On)
            return;

        if (laserState == LaserState.Prepare)
            Destroy(laserT.gameObject);

        laserState = LaserState.On;

        Transform shotPoint = laserPont;
        laserT = Instantiate(laserObj, shotPoint.position, shotPoint.rotation).transform;

        botParticlsAttack[0].Play();

    }

    private void LazerVisualizeProcess()
    {
        Transform shotPoint = laserPont;

        Ray laserRay = new Ray(shotPoint.position, shotPoint.forward);
        RaycastHit hit;

        float simulateDistance = 1000f;

        if (Physics.Raycast(laserRay, out hit, simulateDistance, laserLayerMask))
        {
            
            SetLaser(hit.distance);

            if (laserState > LaserState.Prepare)
                laserHitEvent.Invoke(hit.collider.gameObject);

        }
        else
        {
            SetLaser(simulateDistance);
        }

        void SetLaser(float hitDistance)
        {
            laserT.localScale = new Vector3(laserT.localScale.x, laserT.localScale.y, hitDistance);
            laserT.position = shotPoint.position + shotPoint.forward * (hitDistance / 2f);
            laserT.rotation = shotPoint.rotation;
        }


    }

    private void EndLazerVisualize()
    {
        if (laserState == LaserState.Off)
            return;

        laserState = LaserState.Off;

        botParticlsAttack[0].Stop();
        Destroy(laserT.gameObject);
    }

    public override void MaterialManageService()
    {
        base.MaterialManageService();

        GunBrightScaler();

        void GunBrightScaler()
        {
            float timeStep = Time.deltaTime * gunBrightSpeed;

            float currentBright = mainGunMat.GetFloat("Brightness");
            currentBright = Mathf.Lerp(currentBright, gunTargetBright, timeStep);

            mainGunMat.SetFloat("Brightness", currentBright);

        }
    }

    public void SetGunBrightState(bool gunBrightState)
    {

        if (gunBrightState)
            gunTargetBright = gunChargeBright;
        else
            mainGunMat.SetFloat("Brightness", gunWaitBright);

    }

    public override void Destruct()
    {
        if(laserState > 0)
            SetLazerState(LaserState.Off);

        if (laserT != null)
            Destroy(laserT.gameObject);

        base.Destruct();

        gunBrightSpeed += 0.5f;

        gunTargetBright = -Mathf.Abs(gunTargetBright);
    }

    public enum LaserState
    {
        Off,
        Prepare,
        On
    }

}
