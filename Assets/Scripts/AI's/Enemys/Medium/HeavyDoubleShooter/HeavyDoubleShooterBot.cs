using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDoubleShooterBot : DefaultBot
{
    
    [SerializeField] private float headRotationSpeed = 2f;
    [Space]
    [SerializeField] private float gunsRotationSpeed = 8f;
    [SerializeField] private Vector3 gunsAllowedRotationAngles;
    [SerializeField] private Transform gunLeft;
    [SerializeField] private Transform gunRight;
    private Rigidbody targetRB;

    [Header("For smart target position.")]
    [SerializeField] private float bulletSpeed = 50f;
    

    private new void Start()
    {
        base.Start();
        targetRB = target.GetComponent<Rigidbody>();
    }

    private new void Update()
    {
        base.Update();

        RotateToTarget(head,target.position,headRotationSpeed);

        Vector3 targetSmartPos = CalculateSmartTargetPos(bulletSpeed,targetRB);

        RotateToTargetClamp(gunLeft, targetSmartPos, gunsRotationSpeed,gunsAllowedRotationAngles);
        RotateToTargetClamp(gunRight, targetSmartPos, gunsRotationSpeed, gunsAllowedRotationAngles);
    }

}
