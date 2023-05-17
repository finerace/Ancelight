using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrientatorBot : DefaultBot
{
    private Rigidbody targetRB;

    [SerializeField] protected float aimTimeCof = 1;

    private Vector3 smartTargetPos
    {
        get
        {
            Vector3 resultPos = target.position;

            if(targetRB != null)
            {
                float smoothness = aimTimeCof;

                resultPos += targetRB.velocity * smoothness;
            }

            return resultPos;
        }
    }

    [SerializeField] private Transform mainGunT;
    [SerializeField] private float headAndGunRotationSpeed = 5;
    [SerializeField] private Vector3 headAllowedRotation;
     
    private new void Start()
    {
        base.Start();

        targetRB = target.GetComponent<Rigidbody>();
    }

    private new void Update()
    {
        base.Update();
        
        if(!botAttack.isShoot)
        {
            Vector3 smartTargetPosLocal = smartTargetPos;

            RotateToTargetClamp(mainGunT, smartTargetPosLocal, headAndGunRotationSpeed, headAllowedRotation);
            RotateToTargetClamp(head, smartTargetPosLocal, headAndGunRotationSpeed, headAllowedRotation);
        }
    }

    protected override void BotRotateToAgent()
    {
        if(!botAttack.isAttack)
            base.BotRotateToAgent();
    }

    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            float headTargetAngle = GetAngleY(head.position, target.position);
            bool isAngleAllowAttack = headTargetAngle < headAllowedRotation.x;

            if (isLookingTarget && !botAttack.isAttack && isAngleAllowAttack)
            {
                botAttack.StartAttack();
            }
        }
    }

    internal override void WalkBotToNavAgent()
    {
        if (!botAttack.isShoot)
        {
            base.WalkBotToNavAgent();
            
        }
    }

    private float GetAngleY(Vector3 pos1,Vector3 pos2)
    {
        float distance = Vector3.Distance(pos1,pos2);
        float pos2Hight = Mathf.Abs(pos2.y - pos1.y);

        float angle = (pos2Hight / distance) * 45f;

        return Mathf.Abs(angle);
    }

    protected override void StaticBotRotation()
    {
        if(!botAttack.isAttack)
            base.StaticBotRotation();
    }
    
}
