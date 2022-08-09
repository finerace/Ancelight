using System;
using UnityEngine;


public class WeaponEffects : MonoBehaviour
{

    [SerializeField] private ParticleSystem[] attackParticls = new ParticleSystem[1];
    [SerializeField] private ParticleSystem[] stayParticls = new ParticleSystem[1];
    [SerializeField] private Transform[] animationObjects = new Transform[3];
    [SerializeField] private Vector3[] animationStayPoss = new Vector3[3];
    [SerializeField] private Vector3[] animationAttackPoss = new Vector3[3];

    [SerializeField] private float animationsSpeed;
    [SerializeField] private float notAttackAnimateSpeed;

    [SerializeField] private MeshRenderer[] meshes = new MeshRenderer[2];
    [SerializeField] private Material[] materials = new Material[2];

    [Space]

    [SerializeField] private bool lazerEffect;
    [SerializeField] private Transform lazerStartPos;
    [SerializeField] private GameObject lazerObj;
    [SerializeField] private GameObject lazerEndObj;
    [SerializeField] private ParticleSystem lazerEndParticle;
    private Transform lazerEndParticleT;
    private Transform nowLazer;
    private Transform nowLazerEndObj;

    [Space]

    [SerializeField] private PlayerWeaponsManager weaponsManager;
    private Transform shootingPoint;

    [SerializeField] private float emissionMovementSpeed = 1;
    private float emissionMovement = 0;
    [SerializeField] private float brightness = 1;
    [SerializeField] private float noBulletsBrightness = -1f;
    private float nowBrightness;

    private void Start()
    {

        weaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
        shootingPoint = weaponsManager.shootingPoint;

        float particlsDivideCoof = 10f;

        //Подписка на ивент атаки игрока
        foreach (var item in attackParticls)
        {
            weaponsManager.SubscribeShotEvent(item.Play);

            item.transform.localScale = item.transform.localScale /particlsDivideCoof;
        }

        foreach (var item in stayParticls)
        {
            item.transform.localScale = item.transform.localScale / particlsDivideCoof;
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            materials[i] = meshes[i].material;;
            if (weaponsManager.isBulletsHere)
                nowBrightness = brightness;
            else
                nowBrightness = noBulletsBrightness;

            meshes[i].material.SetFloat("Brightness", nowBrightness);
        }

        if (lazerEndParticle != null)
        {
            lazerEndParticleT = lazerEndParticle.transform;
        }

    }

    private void Update()
    {
        bool isAttack = weaponsManager.IsAttacking;

        if (isAttack) //Анимация деталей
        {
            for (int i = 0; i < animationObjects.Length; i++)
            {
                float time = Time.deltaTime * animationsSpeed;

                animationObjects[i].localPosition =
                    Vector3.Lerp(animationObjects[i].localPosition, animationAttackPoss[i], time);
            }

            foreach (var item in materials)
            {
                emissionMovement += Time.deltaTime * emissionMovementSpeed;

                item.SetFloat("EmissionMovement",emissionMovement);
            }

            if(lazerEffect)
            {
                if(nowLazer == null)
                {
                    nowLazer = Instantiate(lazerObj, lazerStartPos).transform;
                    nowLazer.parent = null;
                }

                if (nowLazerEndObj == null)
                {
                    nowLazerEndObj = Instantiate(lazerEndObj, lazerStartPos).transform;
                    nowLazerEndObj.parent = null;
                }

                RaycastHit hit;
                Ray ray = new Ray
                    (lazerStartPos.position, weaponsManager.shootingPoint.forward);

                lazerStartPos.rotation = Quaternion.LookRotation(shootingPoint.forward * 300f);

                if(Physics.Raycast(ray,out hit,1000f, weaponsManager.LayerMaskRayCastMode))
                {
                    Vector3 newScale = 
                        new Vector3(nowLazer.localScale.x, nowLazer.localScale.y, hit.distance);

                    nowLazer.position = lazerStartPos.position;
                    nowLazer.localScale = newScale;
                    nowLazer.position += lazerStartPos.forward * nowLazer.localScale.z/2f;
                    nowLazer.rotation = lazerStartPos.rotation;
                    nowLazerEndObj.position = hit.point;
                    lazerEndParticleT.position = hit.point;

                    if (lazerEndParticle != null)
                    {
                        if (!lazerEndParticle.isPlaying)
                            lazerEndParticle.Play();
                    }
                }
                else
                {
                    if (lazerEndParticle != null)
                    {
                        if (lazerEndParticle.isPlaying)
                            lazerEndParticle.Stop();
                    }

                    float fakeDistance = 300f;

                    Vector3 newScale =
                        new Vector3(nowLazer.localScale.x, nowLazer.localScale.y, fakeDistance);

                    nowLazer.position = lazerStartPos.position;
                    nowLazer.localScale = newScale;
                    nowLazer.position += lazerStartPos.forward * nowLazer.localScale.z / 2f;
                    nowLazer.rotation = lazerStartPos.rotation;

                    Vector3 endLazerPos = lazerStartPos.position + (lazerStartPos.forward * fakeDistance);

                    nowLazerEndObj.position = endLazerPos;
                }
                
            }

        }
        else
        {
            for (int i = 0; i < animationObjects.Length; i++)
            {
                float time = notAttackAnimateSpeed * Time.deltaTime;

                animationObjects[i].localPosition =
                    Vector3.Lerp(animationObjects[i].localPosition, animationStayPoss[i], time);
            }

            if (lazerEffect)
            {
                if (nowLazer != null)
                {
                    Destroy(nowLazer.gameObject);
                }

                if (nowLazerEndObj != null)
                {
                    Destroy(nowLazerEndObj.gameObject);
                }

                if (lazerEndParticle != null)
                {
                    if (lazerEndParticle.isPlaying)
                        lazerEndParticle.Stop();
                }
            }
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            float lerpSpeed = Time.deltaTime * 1f; //1 - speed

            if (weaponsManager.isBulletsHere)
                nowBrightness = Mathf.Lerp(nowBrightness,brightness,lerpSpeed);
            else
                nowBrightness = Mathf.Lerp(nowBrightness, noBulletsBrightness, lerpSpeed);

            meshes[i].material.SetFloat("Brightness", nowBrightness);
        }

        foreach (var item in stayParticls)
        {
            if (weaponsManager.isBulletsHere && item.isStopped)
                item.Play();
            else if (!weaponsManager.isBulletsHere && item.isPlaying)
                item.Stop();
        }

    }

    private void OnDestroy()
    {
        foreach (var item in attackParticls)
        {
            if(weaponsManager != null && item != null)
                weaponsManager.UnsubShotEvent(item.Play);
        }
    }

}
