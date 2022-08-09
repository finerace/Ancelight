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

            float randomWaitShotTime = Random.Range(0f,1f);
            float attackEndWaitTime = 1 - randomWaitShotTime;

            yield return new WaitForSeconds(attackTime * randomWaitShotTime);
            Shot(shotPoints[0]);
            botEffects.PlayAttackParticls();
            yield return new WaitForSeconds(attackTime * attackEndWaitTime);

            isAttack = false;

        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        //nothing :c
    }

}
