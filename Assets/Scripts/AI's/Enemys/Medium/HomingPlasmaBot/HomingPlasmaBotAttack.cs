using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPlasmaBotAttack : DefaultBotAttack
{
    [SerializeField] private float meleeAttackDamage = 15f;
    [SerializeField] private float attackTime = 8f;

    public override void StartAttack()
    {
        StartCoroutine(AttackProcess());

        IEnumerator AttackProcess()
        {
            isAttack = true;

            yield return new WaitForSeconds(attackTime * 0.25f);
            isShoot = true;

            int attackCount = 3;
            float thisAttacksTime = 0.4f;
            float oneSimpleAttackTime = thisAttacksTime / attackCount;

            for (int i = 0; i < attackCount; i++)
            {
                Shot(shotPoints[0]).GetComponent<BulletHoming>().target = bot.target;
                botEffects.botParticlsAttack[0].Play();
                yield return new WaitForSeconds(oneSimpleAttackTime * 0.5f);

                Shot(shotPoints[1]).GetComponent<BulletHoming>().target = bot.target;
                botEffects.botParticlsAttack[1].Play();
                yield return new WaitForSeconds(oneSimpleAttackTime * 0.5f);
            }

            isShoot = false;

            yield return new WaitForSeconds(attackTime * 0.35f);
            isAttack = false;

        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target, meleeAttackDamage);
    }
}
