using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunnerEffects : DefaultBotEffects
{
    
    [SerializeField] private TrailRenderer[] attackTrails = new TrailRenderer[0];


    internal void PlayAttackTrails(float time)
    {

        StartCoroutine(PlayAttackTrailsCor(time));

        IEnumerator PlayAttackTrailsCor(float time)
        {

            foreach (var item in attackTrails)
            {
                item.enabled = true;
            }

            yield return new WaitForSeconds(time);

            foreach (var item in attackTrails)
            {
                item.enabled = false;
            }


        }

    }


}
