using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPlasmaBot : DefaultBot
{
    [Space]
    [SerializeField] private float headRotationSpeed = 10f;

    private new void Update()
    {
        base.Update();

        float headRotationTimeStep = headRotationSpeed * Time.deltaTime;
        RotateToTarget(head,target.position, headRotationTimeStep);
    }

}
