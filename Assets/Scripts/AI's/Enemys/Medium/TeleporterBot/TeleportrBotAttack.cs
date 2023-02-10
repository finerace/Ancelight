using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportrBotAttack : DefaultBotAttack
{
    [Space]
    [SerializeField] private float meleeAttackDamage = 25f;
    [SerializeField] private float postTeleportationWaitTime = 1f;

    public override void StartAttack()
    {
        StartCoroutine(AttackProcess()); ;
        
        IEnumerator AttackProcess()
        {
            if (attackPhase == 0)
            {
                isAttack = true;
                yield return WaitTime(postTeleportationWaitTime);

                attackPhase = 1;
            }
            
            int shotsCount = 3;
            float oneShotTime = 0.2f;

            for (int i = 1; i < shotsCount+1; i++)
            {
                if (isRecentlyLoad)
                    i = attackPhase;

                if (attackPhase % 2 == 1)
                {
                    if(!isRecentlyLoad)
                        Shot(shotPoints[0]);
                    
                    yield return WaitTime(oneShotTime * 0.5f);

                    attackPhase++;
                }

                if (attackPhase % 2 == 0)
                {
                    if(!isRecentlyLoad)
                        Shot(shotPoints[1]);
                    
                    yield return WaitTime(oneShotTime * 0.5f);

                    attackPhase++;
                }
                
                isRecentlyLoad = false;
            }

            attackPhase = 0;
            isAttack = false;
            
            ActivatePreShotEvent();
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target, meleeAttackDamage);
    }

}
