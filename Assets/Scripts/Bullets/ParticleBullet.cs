using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBullet : MonoBehaviour
{
    private float damageReserve;
    [SerializeField] private float damage;
    [SerializeField] private PlayerWeaponsManager weaponsManager;
    [SerializeField] private Vector3 forceDirection;
    [SerializeField] private float forcePower = 1f;
    
    private void Start()
    {
        weaponsManager = FindObjectOfType<PlayerWeaponsManager>();

        ForceDirectionUpdate();
        weaponsManager.SubscribeShotEvent(ForceDirectionUpdate);
    }

    private void OnParticleCollision(GameObject other)
    {
        Health health;
        Rigidbody rb;

        if(other.TryGetComponent<Health>(out health))
        {
            if(damageReserve <= 0)
                return;

            damageReserve -= damage;
            
            health.GetDamage(damage);
        }

        if (other.TryGetComponent<Rigidbody>(out rb))
        {
            StartCoroutine(AddForceTimer(rb,forceDirection*forcePower));
        }
    }

    private IEnumerator AddForceTimer(Rigidbody targetRb, Vector3 force)
    {
        yield return null;
        yield return null;
        yield return null;

        targetRb.AddForce(force);
    }
    
    private void ForceDirectionUpdate()
    {
        damageReserve = weaponsManager.selectedWeaponData.Damage;
        
        forceDirection = weaponsManager.shootingPoint.forward;
    }

}
