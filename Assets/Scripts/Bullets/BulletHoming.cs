using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : Bullet
{
    [SerializeField] private float homingSpeed = 3.5f;
    [SerializeField] internal Transform target;
    [SerializeField] private bool isEnemyBullet = true;

    protected override void Start()
    {
        base.Start();

        if (isEnemyBullet && target == null)
            target = FindObjectOfType<PlayerMainService>().transform;
    }
    
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        
        Homing(target,homingSpeed);
    }

    protected virtual void Homing(Transform target,float homingSpeed)
    {
        float timeStep = Time.deltaTime * homingSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - body_.position);

        body_.rotation = Quaternion.Lerp(body_.rotation, targetRotation, timeStep);

    }


}
