using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniKamikazeBot : DefaultBot
{

    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        YieldInstruction waitFrame = new WaitForEndOfFrame();

        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.Equals(target))
            botAttack.StartMeleeAttack(target);
    }

    public override void Died()
    {
        if(!botAttack.isAttack)
            botAttack.StartMeleeAttack(target);

        base.Died();
    }

}
