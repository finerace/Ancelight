using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour 
{

    private static List<DefaultBot> Bots = new List<DefaultBot>();

    [SerializeField] private float linkDistance = 15f;
    public static Transform mainTarget;
    [SerializeField] private Transform localTarget;
    [SerializeField] private float crowdAannoyedTime = 20f;

    public LayerMask AILayerMask;

    private void Awake()
    {
        mainTarget = localTarget;

        StartCoroutine(AI_LinksCheckUpdate());
    }

    public static void AddAI(DefaultBot AI_T)
    {
        if(AI_T != null)
        {
            Bots.Add(AI_T);

            if (AI_T.target == null)
                AI_T.target = mainTarget;
        }


    }

    public static void RemoveAI(DefaultBot AI_T)
    {
        if (AI_T != null) Bots.Remove(AI_T);
    }

    private IEnumerator AI_LinksCheckUpdate()
    {
        YieldInstruction waiting = new WaitForSeconds(0.05f);

        yield return waiting;

        while (true)
        {
            yield return waiting;

            for (int i = 0; i < Bots.Count; i++)
            {
                yield return waiting;

                if (i >= Bots.Count)
                    continue;

                var firstBot = Bots[i];

                if (!firstBot.isAannoyed)
                    continue;

                if (firstBot.TargetOldLookTimer > crowdAannoyedTime)
                    continue;

                firstBot.GetAannoyed();

                for (int j = 0; j < Bots.Count; j++)
                {

                    if (j >= Bots.Count || j == i)
                        continue;

                    var secondBot = Bots[j];

                    float distance = Vector3.Distance(firstBot.body.position, secondBot.body.position);

                    if (distance > linkDistance)
                        continue;
                    
                    if (firstBot.TargetOldLookTimer < secondBot.TargetOldLookTimer)
                    {
                        if(secondBot.isBotGoToWayPoints)
                            continue;
                        
                        secondBot.SetDestination(firstBot.Destination);
                        secondBot.nowSetTargetPosHiddenTimer = 2.5f;

                        secondBot.GetAannoyed();
                    }

                }


            }
        }
    }

}
