using System.Collections;
using UnityEngine;

public class MagneticShock : ExtraAbility
{
    [SerializeField] private Transform shockPosition;
    [SerializeField] private MeshRenderer shockEffectRenderer;
    [SerializeField] private float shockEffectStartFresnelEffect;
    [SerializeField] private float shockEffectEndFresnelEffect;
    [SerializeField] private ParticleSystem shockWaveParticls;
    [SerializeField] private float shockEffectSpeed;
    [SerializeField] private float shockEffectTime;
    Material shockEffectMaterial;
    private float shockEffectFresnelEffectNow;

    [Space]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float minDot;
    [SerializeField] private float damage;
    [SerializeField] private bool dotScale = true;
    private bool isShockEffectOn;

    private new void Start()
    {
        base.Start();
        Transform playerT = GameObject.Find("Player").transform;
        shockPosition.parent = playerT;
        shockWaveParticls.transform.parent = playerT;
        Transform cameraT = GameObject.Find("PlayerCam").transform;
        transform.parent = cameraT;
        transform.rotation = cameraT.rotation;
        transform.localPosition = Vector3.zero;

        shockEffectMaterial = shockEffectRenderer.material;
        shockEffectMaterial.SetFloat("FresnelEffect", shockEffectEndFresnelEffect);
        shockEffectRenderer.enabled = false;
    }

    public override void LaunchAbility()
    {
        //Technical
        Explousions.DirectedExplosion(shockPosition.position, shockPosition.forward, 
            minDot, explosionForce, explosionRadius, damage, dotScale);

        //Visual effects
        shockEffectRenderer.enabled = true;
        shockWaveParticls.Play();
        shockEffectFresnelEffectNow = shockEffectStartFresnelEffect;
        base.LaunchAbility();
        StartCoroutine(ShockEffectTimer(shockEffectTime));

    }

    private void Update()
    {
        if(isShockEffectOn)
        {
            float resultSpeed = Time.deltaTime * shockEffectSpeed;
            shockEffectFresnelEffectNow = Mathf.Lerp
                (shockEffectFresnelEffectNow, shockEffectEndFresnelEffect,resultSpeed);

            shockEffectMaterial.SetFloat("FresnelEffect", shockEffectFresnelEffectNow);
        }
    }

    private IEnumerator ShockEffectTimer(float time)
    {
        isShockEffectOn = true;
        yield return new WaitForSeconds(time);
        shockEffectRenderer.enabled = false;
        isShockEffectOn = false;
    }

    private new void OnDestroy()
    {
        Destroy(shockWaveParticls.gameObject);
        Destroy(shockPosition.gameObject);
        base.OnDestroy();
    }

}
