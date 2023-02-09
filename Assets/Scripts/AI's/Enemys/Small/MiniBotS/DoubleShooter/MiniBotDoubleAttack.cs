using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotDoubleAttack : DefaultBotAttack
{

    [SerializeField] private float meleeAttackDamage = 5;
    [SerializeField] private DefaultBot miniBot;

    public override void StartAttack()
    {
        StartCoroutine(StandartAttack());

        IEnumerator StandartAttack()
        {
            int doubleAttackCount = 3;
            
            if (attackPhase == 0)
            {
                isAttack = true;
                
                ActivatePreShotEvent();
                yield return WaitTime(1f);
                
                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                for (int i = 1; i < doubleAttackCount+1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;

                    if (attackPhase % 2 == 1)
                    {
                        if (!isRecentlyLoad)
                        {
                            Shot(shotPoints[0]);
                            botEffects.botParticlsAttack[0].Play();
                        }
                        
                        yield return WaitTime(0.1f);
                        attackPhase++;
                    }

                    if (attackPhase % 2 == 0)
                    {
                        if(!isRecentlyLoad)
                            Shot(shotPoints[1]);
                        
                        botEffects.botParticlsAttack[1].Play();
                        yield return WaitTime(0.1f);;
                        attackPhase++;
                    }

                    isRecentlyLoad = false;
                }
            }

            if (attackPhase >= doubleAttackCount * 2)
            {
                yield return WaitTime(1.5f);
                isAttack = false;
            }
            
            attackPhase = 0;
            
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

}
