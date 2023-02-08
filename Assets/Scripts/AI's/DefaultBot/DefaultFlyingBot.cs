using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultFlyingBot : DefaultBot
{
    [Space]
    [Header("Flying manage")]
    [SerializeField] protected Transform flyPoint;
    [SerializeField] protected DroneState currentDroneState;
    [Space]
    [SerializeField] protected float maxFlyHight = 10f;
    [SerializeField] protected float droneSpeed;
    [SerializeField] private LayerMask obstaclesMask;
    [SerializeField] protected float botHight = 2f;
    [SerializeField] protected float allowedHightDifference = 0.5f;

    private float BotY_AllowedHightDifference
    { get => body.position.y - currentAllowedHight; }

    protected float currentAllowedHight = 0;
    protected float currentDistanceXZ = 0;
    protected bool isAgentVisible = false;

    [SerializeField] protected float agentStopTimer = 0;
    protected bool isAgentStopped = true;
    protected const float maxAgentStopTimeToRespawn = 3f;

    
    protected Vector3 motionDistortion;
    [SerializeField] protected float motionDistortionPower = 1f;

    private new void Start()
    {
        base.Start();

        flyPoint.parent = null;

        const float commonUpdateColdown = 0.1f;

        StartCoroutine(AllowedHightUpdater(commonUpdateColdown));
        StartCoroutine(AgentUpdater(commonUpdateColdown));
        StartCoroutine(MotionDistortionUpdater());
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        FlyPointManage();

        StateMachineAlgorithm();
    }

    protected new void Update()
    {
        base.Update();

        IsAgentStopTimerAlgorithm();

        void IsAgentStopTimerAlgorithm()
        {
            if (isAgentStopped)
                agentStopTimer += Time.deltaTime;
        }
    }

    protected virtual void StateMachineAlgorithm()
    {
        if (!isLookingTarget && !isAnnoyed)
        {
            currentDroneState = DroneState.Idle;
        }
        else if (isAnnoyed && (!isTargetClosely || !isLookingTarget))
        {
            currentDroneState = DroneState.ToTarget;
        }
        else
        {
            currentDroneState = DroneState.Dodge;
        }
    }

    protected virtual void FlyPointManage()
    {
        Vector3 resultFlyPointPos = Vector3.zero;

        switch (currentDroneState)
        {
            case DroneState.Idle:

                resultFlyPointPos = StayAlgorithm();
                break;

            case DroneState.ToTarget:

                resultFlyPointPos = ToTargetFlyAlgorithm();
                break;

            case DroneState.Dodge:

                resultFlyPointPos = DodgeAlgorithm();
                break;

        }

        flyPoint.position = resultFlyPointPos;
    }

    protected virtual Vector3 StayAlgorithm()
    {
        return body.position;
    }

    protected virtual Vector3 ToTargetFlyAlgorithm()
    {
        Vector3 resultFlyPointPos;
        Vector3 flyPointTargetPos;


        if (isLookingTarget)
            flyPointTargetPos = target.position;
        else
            flyPointTargetPos = agentT.position;

        bool isHightNormal =
            body.position.y <= currentAllowedHight &&
            body.position.y >= currentAllowedHight - allowedHightDifference;

        if (isHightNormal)
        {
            resultFlyPointPos =
                new Vector3(flyPointTargetPos.x, body.position.y, flyPointTargetPos.z);
        }
        else
        {
            float forceHight = -1 * BotY_AllowedHightDifference + body.position.y;

            resultFlyPointPos =
                new Vector3(flyPointTargetPos.x, forceHight, flyPointTargetPos.z);
        }

        return resultFlyPointPos;
    }

    protected abstract Vector3 DodgeAlgorithm();

    protected virtual Vector3 DefaultDodgeAlgorithm(Vector3 dodgeDirection)
    {
        Vector3 resultFlyPointPos;

        Vector3 dodgeOriginalDirection = dodgeDirection;

        Vector3 dodgeCircledDirection = ConvertToCircledTrajectory(dodgeOriginalDirection);

        Vector3 obstacleSurfaceNormal;
        bool isThereObstacles =
            CheckInDirectionObstacles(dodgeCircledDirection, out obstacleSurfaceNormal);

        dodgeCircledDirection = CheckAndAdjustDirectionToCorrectHight(dodgeCircledDirection);

        if (!isThereObstacles)
            resultFlyPointPos = body.position + dodgeCircledDirection;
        else
            resultFlyPointPos = body.position +
                AdjustDirectionToObstacleNormal(dodgeCircledDirection, obstacleSurfaceNormal);

        return resultFlyPointPos;


        Vector3 ConvertToCircledTrajectory(Vector3 originalDodgeDirection)
        {
            Vector3 dodgeDirection = originalDodgeDirection;

            Vector3 bot_TargetDirecitonXZ = target.position - body.position;
            bot_TargetDirecitonXZ.y = 0;
            bot_TargetDirecitonXZ.Normalize();

            return (dodgeDirection + bot_TargetDirecitonXZ).normalized;
        }

        bool CheckInDirectionObstacles(Vector3 direction, out Vector3 obstacleNormal)
        {

            RaycastHit hit;
            Ray directionRay = new Ray(body.position, direction);

            bool rayIsHit = Physics.Raycast(directionRay, out hit, 2, obstaclesMask);

            if (rayIsHit)
                obstacleNormal = hit.normal;
            else
                obstacleNormal = Vector3.zero;

            return rayIsHit;
        }

        Vector3 AdjustDirectionToObstacleNormal(Vector3 direction, Vector3 obstacleSurfaceNormal)
        {
            return (direction + obstacleSurfaceNormal).normalized;
        }

        Vector3 CheckAndAdjustDirectionToCorrectHight(Vector3 direction)
        {
            bool isHightNormal =
            body.position.y <= currentAllowedHight &&
            body.position.y >= currentAllowedHight - allowedHightDifference;

            if (isHightNormal)
            {
                return direction;
            }
            else
            {
                float forceHight = -(BotY_AllowedHightDifference);

                return new Vector3(direction.x, forceHight, direction.z).normalized;
            }
        }
    }


    internal override void WalkBotToNavAgent()
    {
        Vector3 toFlyPointDirection = (flyPoint.position - bodyRB.position).normalized;
        Vector3 finalDirection = toFlyPointDirection * droneSpeed;

        GetStatesSpecialModifications();

        bodyRB.AddForce(finalDirection,ForceMode.Acceleration);


        Vector3 IncreaseY(Vector3 vector,float sum)
        {
            return vector += Vector3.up * sum;
        }

        Vector3 GetDistortion(Vector3 vector)
        {
            float softeningMultiplier = 0.01f;

            return vector + (motionDistortion * motionDistortionPower * softeningMultiplier);
        }

        void GetStatesSpecialModifications()
        {
            switch (currentDroneState)
            {
                case DroneState.Idle:
                    //nothing (:/)
                    break;
                case DroneState.ToTarget:

                    const float yForceMultiplyier = 2f;
                    float resultYIncrease = yForceMultiplyier * finalDirection.y;

                    finalDirection = IncreaseY(finalDirection, resultYIncrease);

                    finalDirection = GetDistortion(finalDirection);
                    break;

                case DroneState.Dodge:

                    finalDirection = GetDistortion(finalDirection);
                    break;
            }
        }
    }

    internal override void NavAgentDistanceControl()
    {

        if (isLookingTarget)
        {
            if(agent.enabled)
                agent.enabled = false;
            return;
        }
        else if (!agent.enabled)
        {
            RespawnAgent();

            agent.enabled = true;
        }

        if (agentStopTimer >= maxAgentStopTimeToRespawn)
            RespawnAgent();

        currentDistanceXZ =
            Mathf.Abs(body.position.x - agentT.position.x) +
            Mathf.Abs(body.position.z - agentT.position.z);

        float allowedBotAgentDistanceXZ = 1.5f;

        if (!isAgentVisible)
            StopAgent();
        else
            StartAgent();

        if (currentDistanceXZ > allowedBotAgentDistanceXZ)
            StopAgent();
        else
            StartAgent();

        void StopAgent()
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            isAgentStopped = true;
            return;
        }

        void StartAgent()
        {
            agent.speed = startAgentSpeed;
            isAgentStopped = false;
        }

        void RespawnAgent()
        {
            RaycastHit hit;

            Ray respawnDownRay = new Ray(body.position,Vector3.down);

            bool floorIsFound = 
                Physics.Raycast
                (respawnDownRay, out hit,maxFlyHight*2f, obstaclesMask);

            if(floorIsFound)
            {
                agentT.position = hit.point;
                agent.velocity = bodyRB.velocity;
            }
            else
            {
                agentT.position = body.position;
            }

            agentStopTimer = 0;
        }
        
    }


    protected IEnumerator AllowedHightUpdater(float coldown)
    {
        YieldInstruction waitColdown = new WaitForSeconds(coldown);

        while (true)
        {
            Vector3 hightCheckPos;

            if (isLookingTarget)
                hightCheckPos = target.position;
            else
            {
                hightCheckPos = agentT.position;
            }


            RaycastHit hit;
            Ray checkFloorRay = new Ray(hightCheckPos, Vector3.down);

            bool isHit = Physics.Raycast
                (checkFloorRay, out hit, maxFlyHight, obstaclesMask);

            if (isHit)
                hightCheckPos = hit.point + (Vector3.up * 0.1f);

            Ray checkRoofRay = new Ray(hightCheckPos, Vector3.up);

            isHit = Physics.Raycast
                (checkRoofRay, out hit, maxFlyHight, obstaclesMask);

            if (isHit)
                currentAllowedHight = hit.point.y - botHight;
            else
                currentAllowedHight = hightCheckPos.y + maxFlyHight;

            yield return waitColdown;
        }
    }

    protected IEnumerator AgentUpdater(float time)
    {
        while (true)
        {
            IsAgentInVisible();

            void IsAgentInVisible()
            {
                Vector3 toAgentDirection = agentT.position - body.position;
                float bot_AgentDistance = Vector3.Distance(agentT.position, body.position);

                Ray checkAgentVisibleRay = new Ray(body.position, toAgentDirection);

                isAgentVisible = !Physics.Raycast(checkAgentVisibleRay, bot_AgentDistance, obstaclesMask);
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator MotionDistortionUpdater()
    {
        while (true)
        {
            motionDistortion = GetRandomDirectionVector();

            float randomTimeUpdate = Random.Range(1f,10f);

            yield return new WaitForSeconds(randomTimeUpdate);

            Vector3 GetRandomDirectionVector()
            {

                float randomX = Random.Range(0f, 10f);
                float randomY = Random.Range(0f, 10f);
                float randomZ = Random.Range(0f, 10f);

                return 
                    new Vector3(randomX, randomY, randomZ).normalized;

            }
        }
    }

    public override void Died()
    {
        Destroy(flyPoint.gameObject);
        base.Died();
    }

    public enum DroneState
    {
        Idle,
        ToTarget,
        Dodge
    }

}
