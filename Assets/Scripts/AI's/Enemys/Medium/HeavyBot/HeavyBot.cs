using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBot : DefaultBot
{

    [SerializeField] private ConfigurableJoint headJoint;
    [SerializeField] private Transform handWeapon;
    [SerializeField] private float handAimingSpeed = 10f;
    Quaternion startHandRotation;

    private new void Start()
    {
        startHandRotation = handWeapon.rotation;

        base.Start();
    }

    private new void FixedUpdate()
    {
        RotateHeadToTarget();
        RotateHandToTarget();

        SetBotAnimations();

        base.FixedUpdate();
    }

    private void SetBotAnimations()
    {
        bool isAttackingAnim = false;
        bool isPrepareAttack = false;
        bool isMeleeAttack = botAttack.isMeleeAttack;

        botAnimations.SetAnimations(isAttackingAnim, isMeleeAttack, isPrepareAttack);
    }
    
    private void RotateHeadToTarget()
    {
        if (isLookingTarget || isAannoyed)
            headJoint.targetRotation = Quaternion.Inverse(Quaternion.LookRotation(target.position - head.position)) *
                body.rotation;
        else 
            headJoint.targetRotation = Quaternion.Inverse(Quaternion.identity);
    }

    private void RotateHandToTarget()
    {

        float timeStep = Time.deltaTime * handAimingSpeed;
        Quaternion targetRotation = startHandRotation * Quaternion.Euler(0,0,1);

        if (isLookingTarget && botAttack.isAttack && !botAttack.isMeleeAttack)
        {
            Vector3 correctTargetPos = target.position + new Vector3(0, 0.5f, 0);

            targetRotation = Quaternion.LookRotation(correctTargetPos - handWeapon.position);
        }

        handWeapon.rotation = Quaternion.Lerp(handWeapon.rotation, targetRotation, timeStep);

    }

}
