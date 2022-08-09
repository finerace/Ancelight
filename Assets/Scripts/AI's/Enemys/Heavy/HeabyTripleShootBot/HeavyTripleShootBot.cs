using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTripleShootBot : DefaultBot
{
    [Header("Smart guns manage")]
    [SerializeField] private Transform gunLeft;
    [SerializeField] private Transform gunRight;
    [SerializeField] private Vector3 allowedRotationEuler;
    [SerializeField] private float gunsRotationSpeed = 25;
    [SerializeField] private float bulletSpeed = 35f;
    private Rigidbody targetRb;

    internal new void Start()
    {
        base.Start();

        targetRb = target.GetComponent<Rigidbody>();
    }

    internal new void Update()
    {
        base.Update();

        if (targetRb == null)
            return;

        Transform smartGun = null;
        Transform secondGun = null;

        if (IsLeftGunMain(targetRb.velocity))
        {
            smartGun = gunLeft;
            secondGun = gunRight;
        }
        else
        {
            smartGun = gunRight;
            secondGun = gunLeft;
        }

        Vector3 targetSmartPos = CalculateSmartTargetPos(bulletSpeed,targetRb);

        RotateToTargetClamp(smartGun,targetSmartPos,gunsRotationSpeed,allowedRotationEuler);
        RotateToTargetClamp(secondGun, target.position, gunsRotationSpeed, allowedRotationEuler);


    }

    private bool IsLeftGunMain(Vector3 targetRbVelocity)
    {
        float resultX = 0;

        float headForwardX = head.forward.x;
        float headForwardZ = head.forward.z;

        float targetRbVelX = targetRbVelocity.x;
        float targetRbVelZ = targetRbVelocity.z;

        if (headForwardX >= headForwardZ)
            resultX = targetRbVelZ;

        if (headForwardX < headForwardZ)
            resultX = targetRbVelX;

        return resultX > 0;

    }

}
