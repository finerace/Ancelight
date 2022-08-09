using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBotAttack : DefaultBotAttack
{

    [SerializeField] internal float meleeAttackDamage = 15;
    [SerializeField] private HeavyBot heavyBot;

    public override void StartAttack()
    {

        StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            isAttack = true;

            yield return new WaitForSeconds(0.15f);

            for (int i = 0; i <= 3; i++)
            {
                yield return new WaitForSeconds(0.35f);
                if(!isMeleeAttack) Shot(shotPoints[0]);
                botEffects.PlayAttackParticls();
            }

            yield return new WaitForSeconds(1.5f);

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

            if (heavyBot.isTargetVeryClosely)
                health.GetDamage(meleeAttackDamage);

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }

    }

}
