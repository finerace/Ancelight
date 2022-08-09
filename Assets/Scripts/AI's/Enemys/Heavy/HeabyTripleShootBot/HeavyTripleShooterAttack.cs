using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTripleShooterAttack : DefaultBotAttack
{
    [Space]
    [SerializeField] private float meleeAttackDamage = 20f;
    [SerializeField] private float attackTime = 6f;
    [SerializeField] private GameObject secondBullet;

    public override void StartAttack()
    {

        StartCoroutine(AttackProcess());

        IEnumerator AttackProcess()
        {
            isAttack = true;

            yield return new WaitForSeconds(attackTime * 0.2f);

            botEffects.botParticlsAttack[2].Play();
            botEffects.botParticlsAttack[3].Play();

            int secondAttackCount = 2;
            float secondAttackTimeMultiplier = (attackTime * 0.3f) / secondAttackCount;

            isShoot = true;

            for (int i = 0; i < secondAttackCount; i++)
            {
                Shot(shotPoints[0], secondBullet);
                botEffects.botParticlsAttack[0].Play();

                yield return new WaitForSeconds(secondAttackTimeMultiplier);

                Shot(shotPoints[1], secondBullet);
                botEffects.botParticlsAttack[1].Play();
            }

            yield return new WaitForSeconds(attackTime * 0.2f);

            Shot(shotPoints[2], bullet)
                .GetComponent<BulletHoming>().target = bot.target;

            isShoot = false;

            yield return new WaitForSeconds(attackTime * 0.3f);

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

    

}
