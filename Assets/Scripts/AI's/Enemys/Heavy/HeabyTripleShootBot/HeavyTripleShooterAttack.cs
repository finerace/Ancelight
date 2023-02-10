using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTripleShooterAttack : DefaultBotAttack
{
    [Space]
    [SerializeField] private float meleeAttackDamage = 20f;
    [SerializeField] private float attackTime = 6f;
    [SerializeField] private GameObject secondBullet;

    public override void StartAttack()
    {

        StartCoroutine(AttackProcess());

        IEnumerator AttackProcess()
        {
            isAttack = true;
            
            int secondAttackCount = 2;
            float secondAttackTimeMultiplier = (attackTime * 0.3f) / secondAttackCount;
            
            if (attackPhase == 0)
            {
                yield return WaitTime(attackTime * 0.2f);
                attackPhase = 1;
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            if (attackPhase >= 1 && attackPhase < (secondAttackCount+1))
            {
                botEffects.botParticlsAttack[2].Play();
                botEffects.botParticlsAttack[3].Play();

                isShoot = true;

                for (int i = 1; i < secondAttackCount+1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;

                    attackPhase = i;

                    if (!isRecentlyLoad)
                    {
                        Shot(shotPoints[0], secondBullet);
                        botEffects.botParticlsAttack[0].Play();
                    }

                    yield return WaitTime(secondAttackTimeMultiplier);

                    Shot(shotPoints[1], secondBullet);
                    botEffects.botParticlsAttack[1].Play();

                    attackPhase++;
                }
            }

            if (attackPhase == (secondAttackCount + 1))
            {
                yield return WaitTime(attackTime * 0.2f);


                Shot(shotPoints[2], bullet)
                    .GetComponent<BulletHoming>().target = bot.target;
                
                ActivatePreShotEvent();

                isShoot = false;
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;

                attackPhase++;
            }

            if (attackPhase == (secondAttackCount + 2))
            {
                yield return WaitTime(attackTime * 0.3f);
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;

                attackPhase = 0;
            }

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

    

}
