using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBot : DefaultBot
{
    [SerializeField] private float headRotationSpeed = 10f;

    private new void FixedUpdate()
    {
        RotateHeadToTarget();

        base.FixedUpdate();
    }

    private void RotateHeadToTarget()
    {
        float timeStep = Time.deltaTime * headRotationSpeed;

        if (isLookingTarget || isAannoyed)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - head.position);

            head.rotation = Quaternion.Lerp(head.rotation, lookRotation, timeStep);
        }
        else
            head.localRotation = Quaternion.Lerp(head.localRotation, Quaternion.identity, timeStep);
    }
}
