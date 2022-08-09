using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinBotWizard : DefaultBot
{

    [SerializeField] internal MiniBotWizardEffects specialEffects;

    public new void Update()
    {
        if (isTargetVeryClosely && !botAttack.isMeleeAttack)
            botAttack.StartMeleeAttack(target);

        RotateHeadToTarget();

        base.Update();
    }

    private void RotateHeadToTarget()
    {
        float rotationTimeStep = Time.deltaTime * 7.5f;

        if (isLookingTarget || isAannoyed)
            head.rotation = Quaternion.Lerp(head.rotation, Quaternion.LookRotation(target.position - head.position), rotationTimeStep);
        else
            head.rotation = Quaternion.Lerp(head.rotation, Quaternion.identity * body.rotation, rotationTimeStep);
    }

}
