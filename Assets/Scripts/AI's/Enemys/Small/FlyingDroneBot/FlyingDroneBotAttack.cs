using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDroneBotAttack : DefaultBotAttack
{

    [SerializeField] private float attackTime = 5f;

    public override void StartAttack()
    {
        StartCoroutine(AttackAlgorithm());

        IEnumerator AttackAlgorithm()
        {
            isAttack = true;
            float randomWaitShotTime = Random.Range(0f, 1f);
            float attackEndWaitTime = 1 - randomWaitShotTime;
            
            if (attackPhase == 0)
            {
                yield return WaitTime(attackTime*randomWaitShotTime);

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                if (!isRecentlyLoad)
                {
                    Shot(shotPoints[0]);
                    botEffects.PlayAttackParticls();
                }

                yield return WaitTime(attackTime*randomWaitShotTime);

                isAttack = false;

                attackPhase = 0;

                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }
            
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        //nothing :c
    }

}
