using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Health
{

    public override void Died()
    {
        Destroy(gameObject);
    }

}
