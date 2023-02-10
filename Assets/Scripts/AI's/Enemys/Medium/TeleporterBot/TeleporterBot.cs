using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleporterBot : DefaultBot
{
    [Space]
    [Range(5,150)] [SerializeField] private float maxTeleportDistance = 20f;
    [Range(5, 150)] [SerializeField] private float minTeleportDistance = 6f;
    [Range(0, 100)] [SerializeField] private float lookTargetTeleportColdown = 4f;
    [Range(0, 100)] [SerializeField] private float notLookTargetTeleportColdown = 1.5f;
    [Range(0, 100)] [SerializeField] private float teleportProcessTime = 1f;
    [Range(0, 100)] [SerializeField] private float agentLookingCheckColdown = 0.15f;
    [Range(5, 100)] [SerializeField] private float maxHighTeleported = 6f;
    [SerializeField] private bool isTeleportInProcess = false;
    [SerializeField] private LayerMask teleportObstaclesMask;

    [Space]
    [SerializeField] private TeleporterBotEffects specialBotEffects;
    [Space]
    [SerializeField] private Transform botGun1;
    [SerializeField] private Transform botGun2;
    private Vector3 gunsMaxRotationAngles = new Vector3(60,60,360);
    private const float gunsRotationSpeed = 15f;

    private Vector3 botMaxRotationAngles = new Vector3(0, 360, 0);
    private const float botRotationSpeed = 15f;

    private float headRotationSpeed = 25f;
    private bool isAgentLook = true;

    private event Action teleportationEndEvent;

    internal new void Start()
    {
        base.Start();

        StartCoroutine(LookingAgentChecker(agentLookingCheckColdown));
        StartCoroutine(TeleportationUpdater());
    }

    internal new void Update()
    {
        base.Update();
        
        RotateToTarget(head, target.position, headRotationSpeed);
        RotateToTargetClamp(body, target.position, botRotationSpeed, botMaxRotationAngles);

        RotateToTargetClamp(botGun1, target.position,gunsRotationSpeed, gunsMaxRotationAngles);
        RotateToTargetClamp(botGun2, target.position, gunsRotationSpeed, gunsMaxRotationAngles);
    }

    internal override void WalkBotToNavAgent()
    {
        //nothing :c
    }

    internal override void NavAgentDistanceControl()
    {
        float distance = Vector3.Distance(body.position, agentT.position);

        float allowedBotAgentDistance = maxTeleportDistance;

        if (distance >= allowedBotAgentDistance || !isAgentLook)
        {
            if (!agent.enabled)
                agent.enabled = true;

            agent.velocity = Vector3.zero;
            agent.speed = 0;
        }
        else
        {
            if (!agent.enabled)
                agent.enabled = true;

            agent.speed = startAgentSpeed;
        }
    }

    private IEnumerator LookingAgentChecker(float time)
    {
        while (true)
        {
            Vector3 bot_AgentDirection = (agentT.position - body.position).normalized;
            float bot_AgentDistance = Vector3.Distance(body.position,agentT.position);
            Ray checkObstacles = new Ray(body.position, bot_AgentDirection);

            isAgentLook = !Physics.Raycast(checkObstacles, bot_AgentDistance, teleportObstaclesMask);

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator TeleportationUpdater()
    {
        float coldown;

        while (true)
        {
            if (isLookingTarget)
                coldown = lookTargetTeleportColdown;
            else
                coldown = notLookTargetTeleportColdown;

            yield return new WaitForSeconds(coldown);

            if(!isTeleportInProcess && isAnnoyed)
                StartTeleportation();

        }
    }

    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        teleportationEndEvent += Attack;

        void Attack()
        {
            if (!isTeleportInProcess && !botAttack.isAttack && isLookingTarget)
                botAttack.StartAttack();
        }

        yield break;
    }

    private void StartTeleportation()
    {
        bool isTeleportPointFind = false;
        Vector3 teleportPoint = new Vector3();

        StartCoroutine(TeleportationProcess());

        IEnumerator TeleportationProcess()
        {
            isTeleportInProcess = true;

            int teleportAttemptMaxCount = 120;
            int teleportAttemptCount = 0;

            while (!isTeleportPointFind)
            {
                isTeleportPointFind = FindTeleportPointAttempt(ref teleportPoint);
                teleportAttemptCount++;

                if (teleportAttemptCount > teleportAttemptMaxCount)
                {

                    teleportPoint = agentT.position;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            specialBotEffects.PlayTeleportEffect(body.position);
            yield return new WaitForSeconds(teleportProcessTime * 0.1f);
            specialBotEffects.PlayTeleportEffect(teleportPoint);

            yield return new WaitForSeconds(teleportProcessTime * 0.25f);

            body.position = teleportPoint;

            yield return new WaitForSeconds(teleportProcessTime * 0.65f);

            isTeleportInProcess = false;

            if (teleportationEndEvent != null)
                teleportationEndEvent.Invoke();

        }

        bool FindTeleportPointAttempt(ref Vector3 resultTeleportPlace)
        {

            //-Randomize new teleport pos

            Vector3 center = agentT.position; 

            float xAdd = UnityEngine.Random.Range(-maxTeleportDistance,maxTeleportDistance);
            float zAdd = UnityEngine.Random.Range(-maxTeleportDistance, maxTeleportDistance);

            Vector3 preTeleportPoint = center + new Vector3(xAdd, 0, zAdd);

            //-Check and adjustment distance limits

            float bot_TeleportPointDistance =
                Vector3.Distance(body.position, preTeleportPoint);

            Vector3 bot_TeleportPointDirection 
                = (preTeleportPoint - body.position).normalized;

            if(bot_TeleportPointDistance > maxTeleportDistance)
            {
                float retreatMultiply = bot_TeleportPointDistance - maxTeleportDistance;

                Vector3 retreatVector =
                    -(bot_TeleportPointDirection * retreatMultiply);

                preTeleportPoint += retreatVector;
            }

            if (bot_TeleportPointDistance < minTeleportDistance)
            {
                float retreatMultiply = minTeleportDistance - maxTeleportDistance;

                Vector3 retreatVector =
                    (bot_TeleportPointDirection * retreatMultiply);

                preTeleportPoint += retreatVector;
            }

            //-Check walls in Bot-NewTeleportPoint way 

            bot_TeleportPointDirection
                = (preTeleportPoint - body.position).normalized;

            Ray checkObstaclesRay = new Ray(body.position, bot_TeleportPointDirection);

            RaycastHit hit;

            if (Physics.Raycast
                (checkObstaclesRay,out hit, maxTeleportDistance, teleportObstaclesMask))
            {
                return false;

            }

            //-Adjustment to teleport in high obstacles

            hit.point = Vector3.zero;

            float hightObstacle;

            checkObstaclesRay = new Ray(preTeleportPoint, Vector3.up);
            Physics.Raycast(checkObstaclesRay, out hit, maxHighTeleported, teleportObstaclesMask);

            if (hit.point != Vector3.zero)
                hightObstacle = hit.point.y - preTeleportPoint.y;
            else
                hightObstacle = 20;

            Vector3 highCheckPreTeleport = preTeleportPoint + (Vector3.up * hightObstacle);

            checkObstaclesRay = new Ray(highCheckPreTeleport, Vector3.down);

            float checkFloorRayDistance = maxHighTeleported + hightObstacle + 1f;

            if (Physics.Raycast
                (checkObstaclesRay, out hit, checkFloorRayDistance, teleportObstaclesMask))
            {
                const float botBodyHight = 1.5f;

                preTeleportPoint.y = hit.point.y + botBodyHight;
            }
            else return false;

            //-Final adjustements

            if (isLookingTarget)
            {
                Vector3 bot_TargetDirection = target.position - body.position;
                float bot_TargetDistance = Vector3.Distance(body.position, target.position);

                checkObstaclesRay = new Ray(body.position, bot_TargetDirection);
                if (Physics.Raycast(checkObstaclesRay, bot_TargetDistance, teleportObstaclesMask))
                    return false;
            }

            Vector3 preTeleportPoint_agentDirection = (agentT.position - preTeleportPoint).normalized;
            float preTeleportPoint_agentDistance = Vector3.Distance(agentT.position, preTeleportPoint);

            checkObstaclesRay = new Ray(preTeleportPoint, preTeleportPoint_agentDirection);

            if (Physics.Raycast(checkObstaclesRay, preTeleportPoint_agentDistance,teleportObstaclesMask))
                return false;

            resultTeleportPlace = preTeleportPoint;

            return true;

        }

    }
    


}
