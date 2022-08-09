using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotShooterAttack : DefaultBotAttack
{
    [SerializeField] private float meleeAttackDamage = 5;
    [SerializeField] private MiniBotShooter miniBot;

    public override void StartAttack()
    {
        int random = Random.Range(0, 6);

        if (random < 4)
            StartCoroutine(StandartAttack());
        else
            StartCoroutine(DoubleAttack());

        IEnumerator StandartAttack()
        {
            isAttack = true;
            yield return new WaitForSeconds(0.5f);
            Shot(shotPoints[0]);
            botEffects.PlayAttackParticls();
            yield return new WaitForSeconds(1f);
            isAttack = false;
        }

        IEnumerator DoubleAttack()
        {
            isAttack = true;
            yield return new WaitForSeconds(0.25f);
            Shot(shotPoints[0]);
            botEffects.PlayAttackParticls();
            yield return new WaitForSeconds(0.25f);
            Shot(shotPoints[0]);
            botEffects.PlayAttackParticls();
            yield return new WaitForSeconds(1f);
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
