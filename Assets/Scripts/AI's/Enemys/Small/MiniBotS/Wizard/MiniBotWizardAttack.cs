using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotWizardAttack : DefaultBotAttack
{
    [SerializeField] private MiniBotWizardEffects effects;
    [SerializeField] private MinBotWizard botWizard;
    [SerializeField] private float meleeAttackDamage = 5f;

    public override void StartAttack()
    {
        StartCoroutine(Attack());

        IEnumerator Attack()
        {
            isAttack = true;
            attackPhase = 0;
            
            if (attackPhase == 0)
            {
                if(!isRecentlyLoad)
                    effects.PlayPreShootParticls();
                
                ActivatePreShotEvent();
                
                yield return WaitTime(1);
                
                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                if (!isRecentlyLoad)
                {
                    isShoot = true;

                    if (bot.isLookingTarget)
                        effects.PlayAttackParticls();

                    BulletHoming bulletHoming =
                        Shot(shotPoints[0]).GetComponent<BulletHoming>();

                    bulletHoming.target = bot.target;
                }
                
                yield return WaitTime(2);
            }

            isShoot = false;
            isAttack = false;
            attackPhase = 0;
            isRecentlyLoad = false;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
    }

}
