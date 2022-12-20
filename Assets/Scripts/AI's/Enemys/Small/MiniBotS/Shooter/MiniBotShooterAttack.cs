using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotShooterAttack : DefaultBotAttack
{
    [SerializeField] private float meleeAttackDamage = 5;
    [SerializeField] private MiniBotShooter miniBot;

    public override void StartAttack()
    {
        StartCoroutine(StandartAttack());

        IEnumerator StandartAttack()
        {
            isAttack = true;

            if (attackPhase == 0)
            {
                yield return WaitTime(0.5f);
                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                if (!isRecentlyLoad)
                {
                    Shot(shotPoints[0]);
                    botEffects.PlayAttackParticls();
                }

                yield return WaitTime(1f);

                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            isAttack = false;
        }
    }

    public override void StartMeleeAttack(Transform target)
    {
        Health health;

        if (target.gameObject.TryGetComponent<Health>(out health))
            StartCoroutine(meleeAttack());

        IEnumerator meleeAttack()
        {
            isMeleeAttack = true;

            yield return new WaitForSeconds(0.5f);

            if (miniBot.isTargetVeryClosely)
            {
                health.GetDamage(meleeAttackDamage);
            }

            botEffects.PlayMeleeAttackParticls();

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }

    }

}
