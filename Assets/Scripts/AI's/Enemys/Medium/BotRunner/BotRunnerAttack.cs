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


    private bool isJerkingToTarget = false;

    public override void StartAttack()
    {
        StartCoroutine(Attack());

        IEnumerator Attack()
        {
            
            isJerk = true;
            isAttack = true;
            isJerkingToTarget = false;

            botRunner.moveState = BotRunner.MoveState.Jerk;

            Vector3 direction = (botRunner.target.position - botRunner.body.position).normalized;
            botRunner.bodyRB.velocity = direction * jerkForce;

            YieldInstruction wait = new WaitForSeconds(0.01f);

            float jerkTime = 1.5f;

            StartCoroutine(JerkTimer(jerkTime));

            botEffects.botParticlsAttack[1].Play();

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
                            float velocitySmoothness = (targetRB.drag + 1f) / 2f;

                            targetRB.AddForce(Vector3.up * velocitySmoothness,ForceMode.Acceleration);
                            
                            targetRB.velocity += botRunner.bodyRB.velocity * velocitySmoothness;
                        }

                        if (botRunner.target.TryGetComponent<Health>(out Health targetHealth))
                        {
                            targetHealth.GetDamage(damage);
                        }

                    }

                }

                yield return wait;

            }

            isJerk = false;
            botRunner.moveState = BotRunner.MoveState.Idle;

            yield return new WaitForSeconds(idleTime);
            botRunner.moveState = BotRunner.MoveState.Walk;
            isAttack = false;
        }

        IEnumerator JerkTimer(float time)
        {
            yield return new WaitForSeconds(time);
            isJerkingToTarget = true;
        }

    }

    public override void StartMeleeAttack(Transform target)
    {

        float meleeAttackTime = 1f;

        

        StartCoroutine(MeleeAttack(meleeAttackTime));

        IEnumerator MeleeAttack(float time)
        {
            isMeleeAttack = true;

            YieldInstruction wait = new WaitForSeconds(time/2f); 

            yield return wait;

            if (botRunner.isTargetVeryClosely)
            {
                

                if (botRunner.target.TryGetComponent<Rigidbody>(out Rigidbody targetRB))
                {
                    //float velocitySmoothness = 5f;
                    targetRB.velocity += botRunner.body.forward;
                }

                if (botRunner.target.TryGetComponent<Health>(out Health targetHealth))
                    targetHealth.GetDamage(damage);
            }

            yield return wait;

            isMeleeAttack = false;
        }

    }


}
