using System.Collections;
using UnityEngine;
using System;

public class ParentElementAttack : DefaultBotAttack
{
    [SerializeField] private ParentElementEffects effects;

    private float meleeAttackDamage = 25f;
    private float startAttackColdown = 4f;

    internal event Action<Health> childSpawn;

    private void Start()
    {
        effects.startAttackColdown = startAttackColdown;
    }

    public override void StartAttack()
    {
        StartCoroutine( StartAttack() );

        IEnumerator StartAttack()
        {
            float localSpawnTimeLoop = 0.25f;
            int spawnLoopCount = 3;

            isAttack = true;

            YieldInstruction waitSpawnTime = new WaitForSeconds(localSpawnTimeLoop);

            yield return new WaitForSeconds(startAttackColdown);

            for (int i = 0; i < spawnLoopCount; i++)
            {
                SpawnChildBot(shotPoints[0]);
                botEffects.botParticlsAttack[0].Play();

                yield return waitSpawnTime;

                SpawnChildBot(shotPoints[1]);
                botEffects.botParticlsAttack[1].Play();
            }

            effects.ResetChargeTimer();
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

            if (bot.isTargetVeryClosely)
            {
                health.GetDamage(meleeAttackDamage);
            }

            botEffects.PlayMeleeAttackParticls();

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }
    }

    private void SpawnChildBot(Transform shotPoint)
    {
        Health botHealth = 
            Instantiate(bullet, shotPoint.position, shotPoint.rotation)
            .GetComponent<Health>();

        if(childSpawn != null)
         childSpawn.Invoke(botHealth);
    }

}
