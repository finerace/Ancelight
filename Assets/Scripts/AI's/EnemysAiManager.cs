using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysAiManager : MonoBehaviour 
{

    private static List<DefaultBot> Bots = new List<DefaultBot>();
    
    [SerializeField] private float linkDistance = 15f;
    public static Transform mainTarget;
    [SerializeField] private Transform localTarget;
    [SerializeField] private float crowdAannoyedTime = 20f;

    public LayerMask AILayerMask;

    [Space] 
    
    private static LevelPassageService levelPassageService;

    private void Awake()
    {
        mainTarget = localTarget;

        StartCoroutine(AI_LinksCheckUpdate());

        levelPassageService = FindObjectOfType<LevelPassageService>();
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

    public static void RemoveAI(DefaultBot AI_T,int killScore = 0)
    {
        if (AI_T != null) 
            Bots.Remove(AI_T);

        if (killScore > 0)
            levelPassageService.AddScore(killScore);
        
        if(AI_T.IsTrueEnemy)
            levelPassageService.AddDiedEnemy();
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
                
                if (firstBot == null)
                {
                    Bots.Remove(firstBot);
                    continue;
                }

                if (!firstBot.isAnnoyed)
                    continue;

                if (firstBot.TargetOldLookTimer > crowdAannoyedTime)
                    continue;

                firstBot.GetAannoyed();
                
                for (int j = 0; j < Bots.Count; j++)
                {
                    if (j >= Bots.Count || j == i)
                        continue;

                    var secondBot = Bots[j];

                    if (secondBot == null)
                    {
                        Bots.Remove(secondBot);
                        continue;
                    }
                    
                    float distance = Vector3.Distance(firstBot.body.position, secondBot.body.position);

                    if (distance > linkDistance)
                        continue;
                    
                    if (firstBot.TargetOldLookTimer < secondBot.TargetOldLookTimer)
                    {
                        // if (secondBot.isBotGoToWayPoints)
                        //     continue;

                        secondBot.isBotGoToWayPoints = false;
                        secondBot.SetDestination(firstBot.Destination);
                        secondBot.nowSetTargetPosHiddenTimer = 2.5f;

                        secondBot.GetAannoyed();
                    }

                }


            }
        }
    }

}
