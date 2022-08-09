using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlasmaBot : DefaultBot
{
    [Header("CirclePlasma Manage")]
    [SerializeField] private Transform circlePlasmaMainPivot;
    [SerializeField] private Transform circlePlasmaSimplePivot;
    [SerializeField] private Vector3 circlePlasmaMainAllowedRotationAngles;
    [SerializeField] private Vector3 circlePlasmaSimpleAroundRotationDirection;
    [SerializeField] private float circlePlasmaMainRotationSpeed = 8f;
    private const float headRotationSpeed = 10f;

    internal new void Update()
    {
        base.Update();

        RotateToTarget(head, target.position, headRotationSpeed);

        float circlePlasmaMainPivotRotationTimeStep = Time.deltaTime * circlePlasmaMainRotationSpeed;
        RotateToTargetClamp(
            circlePlasmaMainPivot,
            target.position,
            circlePlasmaMainPivotRotationTimeStep,
            circlePlasmaMainAllowedRotationAngles);

        ///
        
        Vector3 circlePlasmaAroundRotationEuler =
            circlePlasmaSimpleAroundRotationDirection * Time.deltaTime;

        circlePlasmaSimplePivot.Rotate(circlePlasmaAroundRotationEuler);
    }


}
