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

            yield return new WaitForSeconds(attackTime * 0.333f);
            botEffects.PlayAttackParticls();
            yield return new WaitForSeconds(attackTime * 0.666f);

            Shot(shotPoints[0])
                .GetComponent<BulletHoming>().target = bot.target;

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        //nothing :c
    }

}
