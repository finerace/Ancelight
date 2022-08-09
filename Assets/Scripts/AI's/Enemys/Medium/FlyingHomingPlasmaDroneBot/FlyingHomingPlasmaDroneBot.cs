using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHomingPlasmaDroneBot : DefaultFlyingBot
{
    private bool isDodgeDirectionRight = false;

    protected new void Update()
    {
        const float headRotationSpeed = 10f;

        RotateToTarget(head, ScaleTargetPositionAnTargetHight(target), headRotationSpeed);

        base.Update();

        Vector3 ScaleTargetPositionAnTargetHight(Transform target)
        {
            float additionalHightMultiplier = 1.5f;
            return target.position + (Vector3.up * target.localScale.y
                * additionalHightMultiplier);
        }
    }

    protected override void StateMachineAlgorithm()
    {
        if (!isLookingTarget && !isAannoyed)
        {
            currentDroneState = DroneState.Idle;
        }
        else if (isAannoyed && (!isTargetClosely || !isLookingTarget))
        {
            currentDroneState = DroneState.ToTarget;
        }
        else
        {
            if (currentDroneState != DroneState.Dodge)
                isDodgeDirectionRight = RandomizeDodgeDirection();

            currentDroneState = DroneState.Dodge;

            bool RandomizeDodgeDirection()
            {
                int randomiser = Random.Range(0, 2);

                switch (randomiser)
                {
                    case 0:
                        return false;

                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }
    }

    protected override Vector3 DodgeAlgorithm()
    {
        Vector3 dodgeDirection;

        if (isDodgeDirectionRight)
            dodgeDirection = body.right;
        else
            dodgeDirection = -body.right;

        return DefaultDodgeAlgorithm(dodgeDirection);
    }

}
