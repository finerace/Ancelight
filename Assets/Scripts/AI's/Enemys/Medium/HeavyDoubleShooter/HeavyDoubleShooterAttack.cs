using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDoubleShooterAttack : DefaultBotAttack
{
    [SerializeField] private float meleeAttackDamage = 25f;


    public override void StartAttack()
    {
        StartCoroutine(AttackAlgorithm());

        IEnumerator AttackAlgorithm()
        {
            isAttack = true;

            yield return new WaitForSeconds(1);

            Shot(shotPoints[0]);
            Shot(shotPoints[1]);
            bot.botEffects.PlayAttackParticls();

            yield return new WaitForSeconds(3);

            isAttack = false;
        }
    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(bot.target, meleeAttackDamage);
    }

}
