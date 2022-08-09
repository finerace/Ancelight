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

            effects.PlayPreShootParticls();
            yield return new WaitForSeconds(1f);

            isShoot = true;

            if(bot.isLookingTarget)
                effects.PlayAttackParticls();

            BulletHoming bulletHoming = 
                Shot(shotPoints[0]).GetComponent<BulletHoming>();

            bulletHoming.target = bot.target;

            yield return new WaitForSeconds(2f);

            isShoot = false;
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

            if (botWizard.isTargetVeryClosely)
            {
                health.GetDamage(meleeAttackDamage);
            }

            botEffects.PlayMeleeAttackParticls();

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }

    }

}
