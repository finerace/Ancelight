using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHomingPlasmaDroneBotAttack : DefaultBotAttack
{
    [SerializeField] private float attackTime = 3f;

    public override void StartAttack()
    {

        StartCoroutine(AttackAlgorithm());

        IEnumerator AttackAlgorithm()
        {
            isAttack = true;

            if (attackPhase == 0)
            {
                ActivatePreShotEvent();
                
                yield return WaitTime(attackTime * 0.333f);
                botEffects.PlayAttackParticls();

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                yield return WaitTime(attackTime * 0.666f);

                Shot(shotPoints[0])
                    .GetComponent<BulletHoming>().target = bot.target;

                attackPhase = 0;
            }

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        //nothing :c
    }

}
