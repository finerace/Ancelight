using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffects : MonoBehaviour
{

    [SerializeField] private ParticleSystem flyParticls;
    [SerializeField] private ParticleSystem[] destructionParticls = new ParticleSystem[1];
    [SerializeField] private Light pointLight;
    [SerializeField] private Transform bulletMesh;
    [SerializeField] internal bool isDestruction;
    [SerializeField] private float lightDestructionSpeed = 1f;
    private LayerMask correctDestructionEffectsLayMask;

    private void Start()
    {
        correctDestructionEffectsLayMask = 
            LayerMask.GetMask("Default","TransparentFX","EnemyHitBoxs");
    }

    private void Update()
    {
        if (isDestruction && pointLight != null)
        {
            if(pointLight.gameObject.activeSelf)
                pointLight.intensity -= lightDestructionSpeed*Time.deltaTime;
             
            if(pointLight.intensity <= 0f)
                Destroy(pointLight.gameObject);
        }
    }

    public void Destruction(float time,Transform parent)
    {
        isDestruction = true;
        
        if (pointLight != null)
        {
            RaycastHit hit;

            Ray newRay = new Ray(pointLight.transform.position - pointLight.transform.forward*2f
                ,pointLight.transform.forward);

            if(Physics.Raycast(newRay, out hit,2.5f, correctDestructionEffectsLayMask))
            {
                pointLight.transform.position = hit.point - (pointLight.transform.forward*0.1f);
                pointLight.transform.parent = parent;

                foreach (var item in destructionParticls)
                {
                    if (item != null)
                    {
                        item.transform.position = pointLight.transform.position;
                    }
                }

            }
        }

        if (flyParticls != null) 
            flyParticls.Stop();

        foreach (var item in destructionParticls)
        {
            if(item != null)
            {
                if(!item.isPlaying)
                    item.Play();
                else
                    item.Stop();
            }


        }

        bulletMesh.gameObject.SetActive(false);

        Destroy(gameObject, time);
    }

}
