using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniKamikazeEffects : DefaultBotEffects
{
    public override void Destruct()
    {
        isDestruction = true;
        gameObject.transform.parent = null;

        foreach (var item in botParticls)
        {
            item.transform.parent = null;
            item.Stop();
            Destroy(item.gameObject, 5f);
        }

        foreach (var item in botParticlsDied)
        {
            item.transform.parent = null;
            item.Play();
            Destroy(item.gameObject, 5f);
        }

        foreach (var item in botMeshDeleteRenderers)
        {
            Destroy(item.gameObject);
        }

        Destroy(gameObject, 10f);
    }
}
