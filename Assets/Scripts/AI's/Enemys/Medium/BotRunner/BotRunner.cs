using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunner : DefaultBot
{

    [SerializeField] private ConfigurableJoint headJoint;
    [SerializeField] private BotRunnerEffects effects;

    [SerializeField] private BotRunnerAttack botRunnerAttack;

    [SerializeField] internal MoveState moveState = MoveState.Walk;
    [SerializeField] internal float attackDistance = 6f;


    private new void FixedUpdate()
    {
        RotateHeadToTarget();

        botAnimations.SetAnimations
            (isLookingTarget,botRunnerAttack.isMeleeAttack,botRunnerAttack.isJerk);
        
        base.FixedUpdate();
    }


    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            float distance = Vector3.Distance(target.position,body.position);

            bool distanceAllowAttack = distance <= attackDistance;

            bool notAttack = !(botRunnerAttack.isAttack || botRunnerAttack.isJerk);

            if (!isLookingTarget)
                continue;

            if (isTargetVeryClosely && notAttack && !botAttack.isMeleeAttack)
            {
                botAttack.StartMeleeAttack(target);
                continue;
            }

            if (notAttack && distanceAllowAttack && !botAttack.isMeleeAttack)
            {
                botAttack.StartAttack();
            }

        }
    }

    internal override void WalkBotToNavAgent()
    {
        if (moveState == MoveState.Idle || moveState == MoveState.Jerk)
            return;

        base.WalkBotToNavAgent();

    }

    internal override void NavAgentDistanceControl()
    {

        if (moveState == MoveState.Idle || moveState == MoveState.Jerk)
        {
            agent.enabled = false;
            agentT.position = body.position;

            return;
        }
        
        base.NavAgentDistanceControl();
    }


    private void RotateHeadToTarget()
    {
        if ((isLookingTarget || isAnnoyed) && moveState == MoveState.Walk)
            headJoint.targetRotation = Quaternion.Inverse(Quaternion.LookRotation(target.position - head.position)) *
                body.rotation;
        else
            headJoint.targetRotation = Quaternion.Inverse(Quaternion.identity);
    }


    public enum MoveState
    {
        Idle,
        Walk,
        Jerk
    }

}
