using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotShooter : DefaultBot
{
    private const float headSpeed = 10f;

    public new void Update()
    {
        RotateToTarget(head,target.position, headSpeed);

        base.Update();
    }

    


}
