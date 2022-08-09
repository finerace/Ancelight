using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : Bullet
{
    [SerializeField] private float homingSpeed = 3.5f;
    [SerializeField] internal Transform target;

    private void FixedUpdate()
    {
        if(target != null)
            Homing(target,homingSpeed);
    }

    protected virtual void Homing(Transform target,float homingSpeed)
    {
        float timeStep = Time.deltaTime * homingSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - body_.position);

        body_.rotation = Quaternion.Lerp(body_.rotation, targetRotation, timeStep);

    }


}
