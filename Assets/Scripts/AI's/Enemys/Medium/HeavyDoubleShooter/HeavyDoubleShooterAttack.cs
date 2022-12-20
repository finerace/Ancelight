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

            if (attackPhase == 0)
            {
                yield return WaitTime(1);

                attackPhase = 1;
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            if (attackPhase == 1)
            {

                if (!isRecentlyLoad)
                {
                    Shot(shotPoints[0]);
                    Shot(shotPoints[1]);
                    bot.botEffects.PlayAttackParticls();   
                }

                yield return WaitTime(3);

                if (isRecentlyLoad)
                    isRecentlyLoad = false;

                attackPhase = 0;
            }

            isAttack = false;
        }
    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(bot.target, meleeAttackDamage);
    }

}
