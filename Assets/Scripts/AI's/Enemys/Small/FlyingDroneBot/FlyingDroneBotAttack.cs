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
            
            if (attackPhase == 0)
            {
                yield return WaitTime(attackTime/2);

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                if (!isRecentlyLoad)
                {
                    Shot(shotPoints[0]);
                    botEffects.PlayAttackParticls();
                }

                yield return WaitTime(attackTime/2);

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
