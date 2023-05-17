using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunnerAttack : DefaultBotAttack
{
    [SerializeField] internal bool isJerk = false;
    [SerializeField] internal BotRunner botRunner;
    [SerializeField] internal float jerkForce = 10f;
    [SerializeField] internal float getDamageDistance = 2.5f;
    [SerializeField] private float damage;
    [SerializeField] private float hammerDot = 0.4f;
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float targetImpulsePower = 1.25f;

    private bool isJerkingToTarget = false;

    public override void StartAttack()
    {
        StartCoroutine(Attack());

        IEnumerator Attack()
        {
            if (attackPhase == 0)
            {
                isJerk = true;
                isAttack = true;
                isJerkingToTarget = false;

                botRunner.moveState = BotRunner.MoveState.Jerk;

                float jerkTime = 1.5f;

                StartCoroutine(JerkTimer(jerkTime));

                Vector3 direction = (botRunner.target.position - botRunner.body.position).normalized;
                botRunner.bodyRB.velocity = direction * jerkForce;
            
            
                botEffects.botParticlsAttack[1].Play();
                ActivatePreShotEvent();
                
                while (!isJerkingToTarget)
                {
                
                    float distance = Vector3.Distance(botRunner.body.position, botRunner.target.position);

                    if (distance <= getDamageDistance)
                    {
                        bool dotCheck = Vector3.Dot(botRunner.body.forward,direction) >= hammerDot;
                    
                        if (dotCheck)
                        {
                            isJerkingToTarget = true;

                            botEffects.botParticlsAttack[0].Play();

                            if (botRunner.target.TryGetComponent<Rigidbody>(out Rigidbody targetRB))
                            {
                                targetRB.velocity += botRunner.bodyRB.velocity * targetImpulsePower;
                            }

                            if (botRunner.target.TryGetComponent<Health>(out Health targetHealth))
                            {
                                targetHealth.GetDamage(damage);
                            }
                            
                            ActivateShotEvent();
                        }
                    }

                    yield return null;
                }

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                isJerk = false;
                botRunner.moveState = BotRunner.MoveState.Idle;

                yield return WaitTime(idleTime);
            
                botRunner.moveState = BotRunner.MoveState.Walk;
                isAttack = false;

                attackPhase = 0;
            }
            
        }

        IEnumerator JerkTimer(float time)
        {
            yield return WaitTime(time);
            isJerkingToTarget = true;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {
        float meleeAttackTime = 1f;
        
        StartCoroutine(MeleeAttack(meleeAttackTime));

        IEnumerator MeleeAttack(float time)
        {
            if (attackPhase == 0)
            {
                isMeleeAttack = true;

                yield return WaitTime(time / 2);

                if (botRunner.isTargetVeryClosely)
                {
                    if (botRunner.target.TryGetComponent<Rigidbody>(out Rigidbody targetRB))
                    {
                        //float velocitySmoothness = 5f;
                        targetRB.velocity += botRunner.body.forward;
                    }

                    if (botRunner.target.TryGetComponent<Health>(out Health targetHealth))
                        targetHealth.GetDamage(damage);
                    
                    ActivateMeleeAttackEvent();
                }

                attackPhase = 1;
            }

            if (attackPhase == 1)
            {
                yield return WaitTime(time /2);

                isMeleeAttack = false;


                attackPhase = 0;
            }
        }

    }


}
