using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrientatorAttack : DefaultBotAttack
{
    [SerializeField] private float damagePerSecond = 200f;
    [SerializeField] private float startAttackColdownTime = 5;
    [SerializeField] private float attackTime = 2;
    [SerializeField] private LayerMask laserLayerMask;
    [SerializeField] private LaserOrientatorBotEffects effects2;

    private void Start()
    {
        effects2.laserHitEvent += TargetGetDamage;

        void TargetGetDamage(GameObject obj)
        {
            if (!IsTarget(obj))
                return;

            GetDamage(obj);
        }

        bool IsTarget(GameObject obj)
        {
            return obj.transform.GetHashCode() == bot.target.transform.GetHashCode();
        }

        void GetDamage(GameObject obj)
        {
            if (obj.TryGetComponent<Health>(out Health targetHealth))
            {
                float damage = damagePerSecond * Time.deltaTime;

                targetHealth.GetDamage(damage);
            }
        }
    }

    public override void StartAttack()
    {
        StartCoroutine(Attacking());

        IEnumerator Attacking()
        {
            isAttack = true;

            if (attackPhase == 0)
            {
                if(!isRecentlyLoad)
                    effects2.SetGunBrightState(true);

                yield return WaitTime(startAttackColdownTime * 0.9f);
                
                attackPhase = 1;

                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            if (attackPhase == 1)
            {
                if(!isRecentlyLoad)
                    effects2.SetLazerState(LaserOrientatorBotEffects.LaserState.Prepare);
             
                yield return WaitTime(startAttackColdownTime * 0.1f);

                if (isRecentlyLoad)
                    isRecentlyLoad = false;

                attackPhase = 2;
            }

            if (attackPhase == 2)
            {
                if (!isRecentlyLoad)
                {
                    effects2.SetLazerState(LaserOrientatorBotEffects.LaserState.On);

                    isShoot = true;
                }

                yield return WaitTime(attackTime);
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;
                
            }

            effects2.SetLazerState(LaserOrientatorBotEffects.LaserState.Off);
            isShoot = false;

            effects2.SetGunBrightState(false);

            attackPhase = 0;
            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        
    }
}
