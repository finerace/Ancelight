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

            if (attackPhase == 0)
            {
                yield return WaitTime(attackTime * 0.3f);
                isShoot = true;

                attackPhase = 1;
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            int shotCount = 1;
            float shotTime = 0.3f / shotCount;

            if (attackPhase >= 1 && attackPhase < (shotCount*2) + 1) ;
            {
                for (int i = 1; i < shotCount+1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;

                    attackPhase = i;
                    
                    BulletHomingExplousion bullet =
                        Shot(shotPoints[0]).GetComponent<BulletHomingExplousion>();
                    bullet.target = bot.target;

                    bot.botEffects.PlayAttackParticls();

                    yield return WaitTime(shotTime);

                    if (isRecentlyLoad)
                        isRecentlyLoad = false;
                }
            }

            if (attackPhase == (shotCount * 2) + 1)
            {
                isShoot = false;
                yield return WaitTime(attackTime * 0.4f);
            }

            attackPhase = 0;
            isAttack = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

    

}
