using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBotAttack : DefaultBotAttack
{

    [SerializeField] internal float meleeAttackDamage = 15;
    [SerializeField] private HeavyBot heavyBot;

    public override void StartAttack()
    {
        StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            isAttack = true;
            attackPhase = 0;
            
            if (attackPhase == 0)
            {
                yield return WaitTime(0.15f);
                attackPhase = 1;
            }

            if (attackPhase >= 1 && attackPhase <= 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;
                    
                    yield return WaitTime(0.35f);

                    if (!isMeleeAttack)
                        Shot(shotPoints[0]);
                    
                    isRecentlyLoad = false;
                    botEffects.PlayAttackParticls();
                }

                attackPhase = 5;
            }

            if (attackPhase == 5)
            {
                yield return new WaitForSeconds(1.5f);
            }
            
            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

}
