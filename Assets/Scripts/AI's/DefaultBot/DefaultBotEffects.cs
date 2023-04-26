using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBotEffects : MonoBehaviour
{
    [SerializeField] protected ParticleSystem[] botParticls = new ParticleSystem[0];
    [SerializeField] protected ParticleSystem[] botParticlsDied = new ParticleSystem[0];
    [SerializeField] internal ParticleSystem[] botParticlsAttack = new ParticleSystem[0];
    [SerializeField] protected ParticleSystem[] botParticlsMeleeAttack = new ParticleSystem[0];
    [SerializeField] protected MeshRenderer[] botMeshRenderers = new MeshRenderer[0];
    [SerializeField] protected MeshRenderer[] botMeshLOD0;
    [SerializeField] protected MeshRenderer[] botMeshLOD1;
    [SerializeField] protected MeshRenderer[] botMeshLOD2;
    
    [Space]
    
    [SerializeField] private ParticleSystem hitParticle;

    protected Material mainMaterial;
    [Space]
    [SerializeField] protected float botBrightSpeed = 0.1f;
    [SerializeField] protected float botActiveBright = 1f;
    [SerializeField] protected float botNoActiveBright = 0.05f;
    protected float botCurrentBright = 1;
    protected float botStartBright;
    [SerializeField] public bool botIsActive = false;
    internal bool isDestruction = false;


    protected void Start()
    {
        UniteMeshMaterials();

        void UniteMeshMaterials()
        {
            botStartBright = botMeshRenderers[0].material.GetFloat("Brightness");

            mainMaterial = botMeshRenderers[0].material;

            if (botMeshRenderers.Length > 1)
            {
                for (int i = 1; i < botMeshRenderers.Length; i++)
                {
                    botMeshRenderers[i].material = mainMaterial;
                }
            }
        }
    }

    protected void Update()
    {
        MaterialManageService();
    }
    
    public virtual void MaterialManageService()
    {
        BrightScaler();
    }

    protected virtual void BrightScaler()
    {
        float nowBright = mainMaterial.GetFloat("Brightness");
        float stepBright = Time.deltaTime * botBrightSpeed;

        if (botIsActive && !isDestruction)
            botCurrentBright = botStartBright * botActiveBright;
        else if (!botIsActive && !isDestruction)
            botCurrentBright = botStartBright * botNoActiveBright;
        else
        {
            float botDestructBright = -0.5f;

            botCurrentBright = botDestructBright;
        }

        nowBright = Mathf.Lerp(nowBright, botCurrentBright, stepBright);
        mainMaterial.SetFloat("Brightness", nowBright);
    }

    public virtual void PlayHitParticle(Vector3 pos)
    {
        hitParticle.transform.position = pos;

        hitParticle.Play();
    }

    public virtual void PlayAttackParticls()
    {
        foreach (var item in botParticlsAttack)
        {
            item.Play();
        }
    }

    public virtual void PlayMeleeAttackParticls()
    {
        foreach (var item in botParticlsMeleeAttack)
        {
            item.Play();
        }
    }

    public virtual void Destruct()
    {
        isDestruction = true;
        gameObject.transform.parent = null;

        foreach (var item in botParticls)
        {
            item.transform.parent = null;
            item.Stop();
            Destroy(item.gameObject,5f);
        }

        foreach (var item in botParticlsDied)
        {
            item.transform.parent = null;
            item.Play();
            Destroy(item.gameObject, 5f);
        }

        foreach (var item in botMeshRenderers)
        {
            item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            item.receiveShadows = false;
        }

        
        var partsDestroyTime = SettingsSetSystem.enemyPartsDestroyTime;
        switch (SettingsSetSystem.enemyPartsQuality)
        {
            case 0:
            {
                foreach (var item in botMeshLOD0)
                {
                    Destroy(item.gameObject);
                }
                
                foreach (var item in botMeshLOD1)
                {
                    Destroy(item.gameObject);
                }
                
                foreach (var item in botMeshLOD2)
                {
                    Destroy(item.gameObject,partsDestroyTime);
                }
                break;
            }
            
            case 1:
            {
                foreach (var item in botMeshLOD0)
                {
                    Destroy(item.gameObject);
                }
                
                foreach (var item in botMeshLOD1)
                {
                    Destroy(item.gameObject,partsDestroyTime);
                }
                
                foreach (var item in botMeshLOD2)
                {
                    Destroy(item.gameObject);
                }
                break;
            }
            
            case 2:
            {
                foreach (var item in botMeshLOD0)
                {
                    Destroy(item.gameObject,partsDestroyTime);
                }
                
                foreach (var item in botMeshLOD1)
                {
                    Destroy(item.gameObject);
                }
                
                foreach (var item in botMeshLOD2)
                {
                    Destroy(item.gameObject);
                }
                break;
            }

        }

        Destroy(gameObject, 10f);
    }





}
