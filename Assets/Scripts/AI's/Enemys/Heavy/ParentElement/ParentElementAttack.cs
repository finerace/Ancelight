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

            if (attackPhase == 0)
            {
                yield return WaitTime(startAttackColdown);

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                for (int i = 1; i < spawnLoopCount+1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;
                    
                    SpawnChildBot(shotPoints[0]);
                    botEffects.botParticlsAttack[0].Play();

                    SpawnChildBot(shotPoints[1]);
                    botEffects.botParticlsAttack[1].Play();
                    
                    yield return WaitTime(localSpawnTimeLoop);

                    attackPhase++;

                    if (isRecentlyLoad)
                        isRecentlyLoad = false;
                }

                attackPhase = 0;
            }

            effects.ResetChargeTimer();
            isAttack = false;
        }
    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target,meleeAttackDamage);
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
