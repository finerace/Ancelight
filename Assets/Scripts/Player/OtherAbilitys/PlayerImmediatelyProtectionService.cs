using System.Collections;
using UnityEngine;

public class PlayerImmediatelyProtectionService : MonoBehaviour,IUsePlayerDevicesButtons
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
    [HideInInspector] [SerializeField] private bool isShockEffectOn;
    [HideInInspector] [SerializeField] private bool isCooldownOut = true;
    private static readonly int FresnelEffectShaderID = Shader.PropertyToID("FresnelEffect");

    [HideInInspector] [SerializeField] private float cooldownTimer = 0;
    
    private bool isManageBlocked = false;

    public float ExplosionDamage => explosionDamage;
    public float ExplosionRadius => explosionRadius;
    public float CooldownTime => cooldownTime;
    public float CooldownTimer => cooldownTimer;

    public float ExplosionForce => explosionForce;

    public float MinDot => minDot;

    public bool IsShockEffectOn => isShockEffectOn;

    public bool IsCooldownOut => isCooldownOut;

    private DeviceButton useProtectionButton = new DeviceButton();

    private LayerMask shootingLayerMask;
    
    public void Load(float cooldownTime,float cooldownTimer,float explosionDamage, float explosionRadius,float minDot,float explosionForce, bool isCooldownOut)
    {
        this.cooldownTime = cooldownTime;
        this.cooldownTimer = cooldownTimer;

        this.explosionDamage = explosionDamage;
        this.explosionRadius = explosionRadius;

        this.minDot = minDot;

        this.explosionForce = explosionForce;

        this.isCooldownOut = isCooldownOut;

    }
    
    private void Start()
    {
        if (playerT == null)
            playerT = transform;
        
        shockPosition.parent = playerT;
        shockWaveParticls.transform.parent = playerT;
        
        shockEffectMaterial = shockEffectRenderer.material;
        shockEffectMaterial.SetFloat(FresnelEffectShaderID, shockEffectEndFresnelEffect);
        shockEffectRenderer.enabled = false;

        shootingLayerMask = FindObjectOfType<LayerMasks>().PlayerShootingLayerMask;
    }

    private void StartProtection()
    {
        //Technical
        StartCooldownTimer(); 
        
        Explousions.DirectedExplosion(shockPosition.position, shockPosition.forward, 
            minDot, explosionForce, explosionRadius,shootingLayerMask,explosionDamage, dotScale);

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
        
        if(useProtectionButton.IsGetButtonUp() && isCooldownOut)
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

    public DeviceButton[] GetUsesDevicesButtons()
    {
        var getButtons = new[]
            {useProtectionButton};

        return getButtons;
    }
}
