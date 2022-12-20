using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlasmaBotAttack : DefaultBotAttack
{
    [SerializeField] private float attackColdown = 2f;

    private const float meleeAttackDamage = 25;

    private readonly (float min_1, float max1) visibleRange = (0.999f,1);

    public override void StartAttack()
    {

        StartCoroutine( AttackProcess() );

        IEnumerator AttackProcess()
        {
            isAttack = true;
            isShoot = true;

            if (attackPhase == 0)
            {
                yield return WaitTime(attackColdown);
                attackPhase = 1;
            }

            if (attackPhase >= 1)
            {
                for (var i = 1; i < shotPoints.Length + 1; i++)
                {
                    if (isRecentlyLoad)
                        i = attackPhase;
                    
                    attackPhase = i;

                    var item = shotPoints[i-1];
                    yield return new WaitToVisiblePoint(item, bot.target, visibleRange);
                    Shot(item);

                    if (isRecentlyLoad)
                        isRecentlyLoad = false;
                }

                attackPhase = 0;
            }

            isShoot = false;
            isAttack = false;
        }


        

    }

    public override void StartMeleeAttack(Transform target)
    {
        SimpleMeleeAttack(target, meleeAttackDamage);
    }

}


public class WaitToVisiblePoint : CustomYieldInstruction
{

    private Transform eyePoint;
    private Transform waitToVisiblePoint;
    private (float min_1, float max1) visibleRange;

    public override bool keepWaiting
    {
        get
        {
            Vector3 toVisiblePointLocalPos =
                waitToVisiblePoint.position - eyePoint.position;

            Vector3 eyeLookDirection = eyePoint.forward;

            float visibleDOT = AuxiliaryFunc.PointDirection_TargetLocalPosDOT(
                toVisiblePointLocalPos,
                eyeLookDirection
                );

            bool isToVisiblePointIsVisible =
                visibleDOT >= visibleRange.min_1 && visibleDOT <= visibleRange.max1;

            return !isToVisiblePointIsVisible;

        }
    }

    public WaitToVisiblePoint(Transform eyePoint, Transform waitToVisiblePoint, 
        (float min_1,float max1) visibleRange )
    {

        if (visibleRange.min_1 > visibleRange.max1)
            throw new System.ArgumentException("The minimum value is greater than the maximum value!");

        this.eyePoint = eyePoint;
        this.waitToVisiblePoint = waitToVisiblePoint;
        this.visibleRange = visibleRange;
    }
}
