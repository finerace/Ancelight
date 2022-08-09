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

        StartCoroutine(AttackProcess());

        IEnumerator AttackProcess()
        {
            isAttack = true;
            yield return new WaitForSeconds(postTeleportationWaitTime);

            int shotsCount = 3;
            float oneShotTime = 0.2f;

            for (int i = 0; i < shotsCount; i++)
            {
                Shot(shotPoints[0]);
                yield return new WaitForSeconds(oneShotTime * 0.5f);
                Shot(shotPoints[1]);
                yield return new WaitForSeconds(oneShotTime * 0.5f);
            }

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target, meleeAttackDamage);
    }

}
