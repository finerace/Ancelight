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

            if (attackPhase == 0)
            {
                yield return WaitTime(attackTime * 0.25f);
             
                isShoot = true;
                attackPhase = 1;
            }

            int attackCount = 3;
            float thisAttacksTime = 0.4f;
            float oneSimpleAttackTime = thisAttacksTime / attackCount;

            if (attackPhase >= 1 && attackPhase < (attackCount*2)+1 )
            {
                for (int i = 1; i < attackCount+1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;

                    if (attackPhase % 2 == 1)
                    {
                        if (!isRecentlyLoad)
                        {
                            Shot(shotPoints[0]).GetComponent<BulletHoming>().target = bot.target;
                            botEffects.botParticlsAttack[0].Play();
                        }

                        yield return WaitTime(oneSimpleAttackTime * 0.5f);
                        attackPhase++;

                        if (isRecentlyLoad)
                            isRecentlyLoad = false;
                    }

                    if (attackPhase % 2 == 0)
                    {
                        if (!isRecentlyLoad)
                        {
                            Shot(shotPoints[1]).GetComponent<BulletHoming>().target = bot.target;
                            botEffects.botParticlsAttack[1].Play();
                        }

                        yield return WaitTime(oneSimpleAttackTime * 0.5f);
                        attackPhase++;

                        if (isRecentlyLoad)
                            isRecentlyLoad = false;
                    }

                }
            }

            isShoot = false;

            if (attackPhase == (attackCount * 2) + 1)
            {
                yield return WaitTime(attackTime * 0.35f);
                isAttack = false;
                
                if (isRecentlyLoad)
                    isRecentlyLoad = false;
            }

            attackPhase = 0;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target, meleeAttackDamage);
    }
}
