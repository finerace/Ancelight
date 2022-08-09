using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketsHomingBot : DefaultBot
{

    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (isLookingTarget && !botAttack.isAttack)
            {
                botAttack.StartAttack();
            }

            if (isTargetVeryClosely && !botAttack.isMeleeAttack)
                botAttack.StartMeleeAttack(target);

        }
    }

}
