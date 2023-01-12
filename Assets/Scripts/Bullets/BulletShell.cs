using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletShell : Bullet
{
    [SerializeField] private LayerMask forceLayerMask;
    [SerializeField] private float explousionForce;
    [SerializeField] private float exploisonRadius;

    protected new void Start()
    {
        body_.localScale = body_.localScale * (startRBPower);
        damage *= startRBPower;

        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.TryGetComponent<Health>(out Health health))
        {

            bool isPunched = damage > health.Health_;

            Explosions.Explosion(body_.position, explousionForce, exploisonRadius, 0, 0, 0, forceLayerMask);

            if (isPunched)
            {
                float healthPreDamage = health.Health_;
                
                health.GetDamage(damage+1f);

                damage -= healthPreDamage * 0.5f;

                return;
            }

            if (!isPunched)
            {
                health.GetDamage(damage);

                Destruction(other.transform);
            }

            return;
        }

        
        Destruction(other.transform);

    }

}
