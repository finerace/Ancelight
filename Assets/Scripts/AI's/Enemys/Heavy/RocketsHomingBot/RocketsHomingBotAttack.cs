using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketsHomingBotAttack : DefaultBotAttack
{
    [SerializeField] private float meleeAttackDamage = 25f;
    [SerializeField] private float attackTime = 8f;

    public override void StartAttack()
    {
        StartCoroutine(AttackProcess());
        
        IEnumerator AttackProcess()
        {
            isAttack = true;

            yield return new WaitForSeconds(attackTime*0.3f);
            isShoot = true;

            int shotCount = 1;
            float shotTime = 0.3f / shotCount;

            for (int i = 0; i < shotCount; i++)
            {
                BulletHomingExplousion bullet = 
                    Shot(shotPoints[0]).GetComponent<BulletHomingExplousion>();
                bullet.target = bot.target;

                bot.botEffects.PlayAttackParticls();

                yield return new WaitForSeconds(shotTime);
            }

            isShoot = false;
            yield return new WaitForSeconds(attackTime * 0.4f);

            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

    

}
