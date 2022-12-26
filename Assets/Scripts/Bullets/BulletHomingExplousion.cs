using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHomingExplousion : ExplousionBullet
{

    [SerializeField] private float homingSpeed = 3.5f;
    [SerializeField] internal Transform target;
    [SerializeField] private bool isEnemyBullet = true;

    protected override void Start()
    {
        base.Start();

        if (isEnemyBullet && target == null)
            target = GameObject.Find("Player").transform;
    }
    
    
    private void FixedUpdate()
    {
        Homing(target,homingSpeed);
    }

    protected virtual void Homing(Transform target, float homingSpeed)
    {
        float timeStep = Time.deltaTime * homingSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - body_.position);

        body_.rotation = Quaternion.Lerp(body_.rotation, targetRotation, timeStep);

    }
}
