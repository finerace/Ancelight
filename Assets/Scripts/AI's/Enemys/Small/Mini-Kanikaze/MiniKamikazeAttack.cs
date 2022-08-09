using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniKamikazeAttack : DefaultBotAttack
{

    [SerializeField] private float explousionForce = 5f;
    [SerializeField] private float explousionRadius = 8f;
    [SerializeField] private float damage = 25;

    [Space]
    [SerializeField] private LayerMask wallsLayerMask;
    [SerializeField] private LayerMask damageLayerMask;
    [SerializeField] private LayerMask forceLayerMask;

    public override void StartAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void StartMeleeAttack(Transform target)
    {
        isAttack = true;

        Explousions.Explosion(bot.body.position, explousionForce, explousionRadius, damage,
            wallsLayerMask, damageLayerMask, forceLayerMask);

        bot.Died();

        botEffects.PlayAttackParticls();
    }

}
