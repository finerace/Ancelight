using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBotAttack : DefaultBotAttack
{

    [SerializeField] internal float meleeAttackDamage = 20;

    public override void StartAttack()
    {

        StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            isAttack = true;
            attackPhase = 0;
            
            
            if (attackPhase == 0)
            {
                ActivatePreShotEvent();
                
                yield return WaitTime(1);

                if (!isMeleeAttack)
                {
                    botEffects.botParticlsAttack[0].Play();
                    Shot(shotPoints[0]);
                }

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                yield return WaitTime(0.5f);
             
                if (!isMeleeAttack)
                {
                    botEffects.botParticlsAttack[1].Play();
                    Shot(shotPoints[1]);
                }

                attackPhase = 2;
            }

            if (attackPhase == 2)
            {
                yield return new WaitForSeconds(2.5f);
            }
            
            isAttack = false;
            attackPhase = 0;
            
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

}
