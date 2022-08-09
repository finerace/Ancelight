using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBotAttack : DefaultBotAttack
{

    [SerializeField] internal float meleeAttackDamage = 20;

    public override void StartAttack()
    {

        StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            isAttack = true;

            yield return new WaitForSeconds(1f);

            if (!isMeleeAttack)
            {
                botEffects.botParticlsAttack[0].Play();
                Shot(shotPoints[0]);
            }

            yield return new WaitForSeconds(0.5f);

            if (!isMeleeAttack)
            {
                botEffects.botParticlsAttack[1].Play();
                Shot(shotPoints[1]);
            }

            yield return new WaitForSeconds(2.5f);

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

            yield return new WaitForSeconds(0.25f);

            if (bot.isTargetVeryClosely)
                health.GetDamage(meleeAttackDamage);

            botEffects.PlayMeleeAttackParticls();

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }

    }

}
