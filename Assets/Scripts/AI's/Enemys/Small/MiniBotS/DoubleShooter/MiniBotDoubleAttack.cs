using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotDoubleAttack : DefaultBotAttack
{

    [SerializeField] private float meleeAttackDamage = 5;
    [SerializeField] private DefaultBot miniBot;

    public override void StartAttack()
    {

        StartCoroutine(StandartAttack());

        IEnumerator StandartAttack()
        {
            isAttack = true;

            int doubleAttackCount = 3;

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < doubleAttackCount; i++)
            {
                Shot(shotPoints[0]);
                botEffects.botParticlsAttack[0].Play();
                yield return new WaitForSeconds(0.1f);

                Shot(shotPoints[1]);
                botEffects.botParticlsAttack[1].Play();
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
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
