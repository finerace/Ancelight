using System.Collections;
using UnityEngine;

public class PlayerImmediatelyProtectionService : MonoBehaviour
{
    [SerializeField] private Transform playerT;
    [SerializeField] private Transform shockPosition;
    [SerializeField] private MeshRenderer shockEffectRenderer;
    [SerializeField] private float shockEffectStartFresnelEffect;
    [SerializeField] private float shockEffectEndFresnelEffect;
    [SerializeField] private ParticleSystem shockWaveParticls;
    [SerializeField] private float shockEffectSpeed;
    [SerializeField] private float shockEffectTime;
    
    private Material shockEffectMaterial;
    private float shockEffectFresnelEffectNow;

    [Space]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float minDot;
    [SerializeField] private float explosionDamage;
    [SerializeField] private bool dotScale = true;
    private bool isShockEffectOn;
    private bool isCooldownOut = true;
    private static readonly int FresnelEffectShaderID = Shader.PropertyToID("FresnelEffect");

    private float cooldownTimer = 0;
    
    private bool isManageBlocked = false;

    public float ExplosionDamage => explosionDamage;
    public float ExplosionRadius => explosionRadius;
    public float CooldownTime => cooldownTime;
    public float CooldownTimer => cooldownTimer;
    
    private void Start()
    {
        if (playerT == null)
            playerT = transform;
        
        shockPosition.parent = playerT;
        shockWaveParticls.transform.parent = playerT;
        
        shockEffectMaterial = shockEffectRenderer.material;
        shockEffectMaterial.SetFloat(FresnelEffectShaderID, shockEffectEndFresnelEffect);
        shockEffectRenderer.enabled = false;
    }

    private void StartProtection()
    {
        //Technical
        StartCooldownTimer(); 
        
        Explousions.DirectedExplosion(shockPosition.position, shockPosition.forward, 
            minDot, explosionForce, explosionRadius, explosionDamage, dotScale);

        //Visual effects
        shockEffectRenderer.enabled = true;
        shockWaveParticls.Play();
        shockEffectFresnelEffectNow = shockEffectStartFresnelEffect;
        
        StartCoroutine(ShockEffectTimer(shockEffectTime));
        
        void StartCooldownTimer()
        {
            isCooldownOut = false;

            cooldownTimer = cooldownTime;
        }
        
    }

    private void Update()
    {
        if(!isCooldownOut)
            CooldownTimerSet();
            
        if(isManageBlocked)
            return;
        
        if(Input.GetKeyUp(KeyCode.V) && isCooldownOut)
            StartProtection();
        
        if (!isShockEffectOn) return;
        
        var resultSpeed = Time.deltaTime * shockEffectSpeed;
            
        shockEffectFresnelEffectNow = Mathf.Lerp
            (shockEffectFresnelEffectNow, shockEffectEndFresnelEffect,resultSpeed);
        
        shockEffectMaterial.SetFloat(FresnelEffectShaderID, shockEffectFresnelEffectNow);
    }

    public void SetManageActive(bool state)
    {
        isManageBlocked = !state;
    }

    private void CooldownTimerSet()
    {
        if (!isCooldownOut)
            cooldownTimer -= Time.deltaTime;

        if(cooldownTimer <= 0)
            EndCooldown();
        
        void EndCooldown()
        {
            isCooldownOut = true;
        }
    }
    
    private IEnumerator ShockEffectTimer(float time)
    {
        isShockEffectOn = true;
        yield return new WaitForSeconds(time);
        shockEffectRenderer.enabled = false;
        isShockEffectOn = false;
    }
}
