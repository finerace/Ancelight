using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class DefaultBot : Health
{
    [SerializeField] internal NavMeshAgent agent;
    [SerializeField] internal Transform agentT;
    [SerializeField] internal Transform body;
    [SerializeField] internal Transform head;
    [SerializeField] internal Rigidbody bodyRB;
    [SerializeField] internal float botSpeed = 4f;

    [Space]
    [SerializeField] internal Transform target;
    [SerializeField] internal float lookingRadius = 40f;
    [SerializeField] internal float isPathLong = 40;
    [SerializeField] internal float targetCloselyRadius = 6f;
    [SerializeField] internal float targetVeryCloselyRadius = 3f;
    [SerializeField] protected float damageImpulse = 3f;

    [SerializeField] private Vector3 destination;

    internal Vector3 Destination
    {
        get 
        {
            if (!agent.enabled)
                return destination;

            if(!agent.isOnNavMesh)
                return destination;

            if(agent.destination == agentT.position)
                return destination;

            destination = agent.destination;

            return agent.destination;
        }
    }

    [SerializeField] internal bool isStaticBot = false;

    [SerializeField] private float targetOldLookTimer = 1024f;
    public float TargetOldLookTimer { get { return targetOldLookTimer; }}

    [SerializeField] [Range(-1f,1f)] internal float fovDot = 0.7f;
    [SerializeField] internal LayerMask lookingLayerMask;

    [Space]
    [SerializeField] internal bool isSetTargetPosOnHiddenTarget = true;
    [SerializeField] internal float setTargetPosTime = 10f;
    internal float nowSetTargetPosHiddenTimer;
    [SerializeField] internal bool nowSettesTargetPos = false;

    [Space]
    [SerializeField] internal bool isRbVelocityToNavAgnStrict;

    [Space]
    [SerializeField] internal bool isAnnoyed;
    [SerializeField] internal const float aannoyedTime = 5f;
    [SerializeField] internal float aannoyedTimer = 0f;

    [Space]
    [SerializeField] internal bool isLookingTarget;
    [SerializeField] internal bool isTargetClosely;
    [SerializeField] internal bool isTargetVeryClosely;
    
    [SerializeField] internal bool isBotGoToWayPoints = false;
    [SerializeField] internal Vector3 currentWayPoint;
    
    [SerializeField] internal bool isSmart;
    [Space]

    [SerializeField] internal DefaultBotParts botParts;
    [SerializeField] internal DefaultBotEffects botEffects;
    [SerializeField] internal DefaultBotAttack botAttack;
    [SerializeField] internal DefaultBotAnimations botAnimations;
    
    [SerializeField] internal float startAgentSpeed;

    [Space] 
    
    [SerializeField] private int killScoreValue = 10;
    [SerializeField] private bool isTrueEnemy = true;

    public bool IsTrueEnemy => isTrueEnemy;

    private float antiStuckPower = 8;
    [SerializeField] private bool isBotOnlyRun = false;
    
    protected event Action botPlayerLookEvent;
    
    public DefaultBotParts BotParts => botParts;

    public DefaultBotEffects BotEffects => botEffects;

    public DefaultBotAttack BotAttack => botAttack;

    public DefaultBotAnimations BotAnimations => botAnimations;

    public void Load(DefaultBot savedBot)
    {
         SetHealth(savedBot.Health_);
         SetMaxHealth(savedBot.MaxHealth_);

         botSpeed = savedBot.botSpeed;
         lookingRadius = savedBot.lookingRadius;
         isPathLong = savedBot.isPathLong;

         targetCloselyRadius = savedBot.targetCloselyRadius;
         targetVeryCloselyRadius = savedBot.targetVeryCloselyRadius;

         damageImpulse = savedBot.damageImpulse;

         destination = savedBot.destination;

         isStaticBot = savedBot.isStaticBot;

         targetOldLookTimer = savedBot.targetOldLookTimer;

         fovDot = savedBot.fovDot;
         lookingLayerMask = savedBot.lookingLayerMask;

         isSetTargetPosOnHiddenTarget = savedBot.isSetTargetPosOnHiddenTarget;
         setTargetPosTime = savedBot.setTargetPosTime;
         nowSetTargetPosHiddenTimer = savedBot.nowSetTargetPosHiddenTimer;
         nowSettesTargetPos = savedBot.nowSettesTargetPos;

         isRbVelocityToNavAgnStrict = savedBot.isRbVelocityToNavAgnStrict;

         isAnnoyed = savedBot.isAnnoyed;
         aannoyedTimer = savedBot.aannoyedTimer;

         isLookingTarget = savedBot.isLookingTarget;
         isTargetClosely = savedBot.isTargetClosely;
         isTargetVeryClosely = savedBot.isTargetVeryClosely;

         isBotGoToWayPoints = savedBot.isBotGoToWayPoints;
         currentWayPoint = savedBot.currentWayPoint;

         isSmart = savedBot.isSmart;

         startAgentSpeed = savedBot.startAgentSpeed;
         agent.transform.position = Vector3.zero;
    }
    
    internal void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
        
        float agentSpeedSmoothness = 2f;

        agent.speed = botSpeed * agentSpeedSmoothness;

        startAgentSpeed = agent.speed;

        if (body == null)
            body = transform;

        if (agentT == null)
            agentT = agent.transform;

        if (target == null)
            target = EnemysAiManager.mainTarget;

        if(bodyRB == null)
            bodyRB = GetComponent<Rigidbody>();

        if(isSmart) 
            EnemysAiManager.AddAI(this);

        if (isStaticBot)
        {
            agent.enabled = false;
            agent.updateRotation = false;
            bodyRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            bodyRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        agentT.parent = null;

        agentT.gameObject.name = $"NavAgent_{gameObject.name}";

        float lookingUpdateTime = 0.5f;
        float agentDestinationUpdateTime = 0.35f;
        float attackCheckerUpdateTime = 0.25f;

        StartCoroutine(LookingChecker(lookingUpdateTime));
        StartCoroutine(AgentDestinationUpdater(agentDestinationUpdateTime));
        StartCoroutine(IsAttacksAllowChecker(attackCheckerUpdateTime));
        StartCoroutine(StuckChecker(0.35f));
    }

    protected void Update()
    {
        SetTargetPosHiddenTimer();

        if (targetOldLookTimer < 1024f)
            targetOldLookTimer += Time.deltaTime;

        if (aannoyedTimer > 0)
        {
            aannoyedTimer -= Time.deltaTime;
            isAnnoyed = true;
        }
        else isAnnoyed = false;

        if (isStaticBot && (isLookingTarget || isAnnoyed))
            StaticBotRotation();
        
        AttackManageAlgorithm();
        void AttackManageAlgorithm()
        {
            if (isTargetVeryClosely && !botAttack.isMeleeAttack && !botAttack.isAttack)
                botAttack.StartMeleeAttack(target);
        }
    }

    protected void FixedUpdate()
    {
        if (!isStaticBot)
        {
            NavAgentDistanceControl();
            WalkBotToNavAgent();
        }
    }

    internal virtual void WalkBotToNavAgent()
    {
        BotRotateToAgent();
        float allowedBotAgentDistance = 0.3f;

        if (!agent.isOnNavMesh)
            return;

        if (Vector3.Distance(body.position, agentT.position) >= allowedBotAgentDistance)
        {
            Vector3 forceDirection = (agentT.position - body.position).normalized;

            float speedSmoothness = 3f;
            float strictVelocitySmoothness = 3f;
            float heightSpeedSmoothness = 1;

            float bot_AgentYpos = agentT.position.y - body.position.y;
            if (bot_AgentYpos > 0)
                heightSpeedSmoothness += bot_AgentYpos*2f;

            speedSmoothness *= heightSpeedSmoothness;

            if (!isRbVelocityToNavAgnStrict)
                bodyRB.AddForce(forceDirection * botSpeed * speedSmoothness, ForceMode.Acceleration);
            else
                bodyRB.velocity = forceDirection * botSpeed * strictVelocitySmoothness;
        }

        if(agent.isOnOffMeshLink)
            body.position = agentT.position;

    }

    protected virtual void BotRotateToAgent()
    {
        body.rotation = agentT.rotation;
    }
    
    internal virtual void NavAgentDistanceControl()
    {
        float distance = Vector3.Distance(body.position, agentT.position);

        float allowedBotAgentDistance = 0.5f;
        float agentTeleportedDistance = 0.9f;

        if (distance >= allowedBotAgentDistance && distance <= agentTeleportedDistance)
        {
            if (!agent.enabled)
                agent.enabled = true;

            agent.velocity = Vector3.zero;
        }
        else if (distance > agentTeleportedDistance && !agent.isOnOffMeshLink)
        {
            agent.enabled = false;
            agentT.position = body.position;
            //agent.velocity = bodyRB.velocity;
        }
        else
        {
            if (!agent.enabled)
                agent.enabled = true;

            agent.speed = startAgentSpeed;
        }
    }

    internal virtual void SetDestination(Vector3 pos)
    {
        if(agent.isOnNavMesh)
            agent.destination = pos;

        //targetOldLookTimer = 0;
    }

    internal virtual Vector3 GetDestination()
    {
        return agent.destination;
    }

    internal virtual void SetTargetPosHiddenTimer()
    {
        if (nowSetTargetPosHiddenTimer > 0 && isSetTargetPosOnHiddenTarget)
        {
            nowSetTargetPosHiddenTimer -= Time.deltaTime;
            nowSettesTargetPos = true;
        }
        else 
            nowSettesTargetPos = false;
    }

    internal virtual IEnumerator LookingChecker(float time)
    {
        while (true)
        {
            var oldLookingTarget = isLookingTarget;
            
            isLookingTarget = CheckIsLookingTarget();

            if (isLookingTarget && !oldLookingTarget)
            {
                if(botPlayerLookEvent != null)
                    botPlayerLookEvent.Invoke();
            }
                
            if(isLookingTarget)
            {
                targetOldLookTimer = 0;
                isBotGoToWayPoints = false;

                GetAannoyed();
            }

            if (isLookingTarget && isSetTargetPosOnHiddenTarget && !isBotGoToWayPoints)
                nowSetTargetPosHiddenTimer = setTargetPosTime;

            if (botEffects != null)
                botEffects.botIsActive = isLookingTarget;

            yield return new WaitForSeconds(time);
        }
    }

    internal virtual IEnumerator AgentDestinationUpdater(float time)
    {
        while (true)
        {
            if (isLookingTarget || nowSettesTargetPos)
            {
                SetDestination(target.position);
                targetOldLookTimer = 0;
            }

            if (isLookingTarget && PathLegth(agent.path.corners) > isPathLong)
                agent.speed = 0;
            else if(isBotGoToWayPoints)
                SetDestination(currentWayPoint);
            else
                agent.speed = startAgentSpeed;
            
            
            
            yield return new WaitForSeconds(time);
        }
    }

    internal virtual IEnumerator IsAttacksAllowChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            
            if (isTargetVeryClosely && !botAttack.isMeleeAttack && !botAttack.isAttack)
                botAttack.StartMeleeAttack(target);
            
            if (isLookingTarget && !botAttack.isAttack && !botAttack.isMeleeAttack)
            {
                botAttack.StartAttack();
            }

        }
    }
    
    internal virtual IEnumerator StuckChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            
            if(!agent.isOnNavMesh || isBotGoToWayPoints)
                continue;
                
            var isBotStuck = !agent.isStopped && bodyRB.velocity.magnitude < 0.01f && isAnnoyed;

            if (isBotStuck && !isBotOnlyRun)
                isBotStuck = !isLookingTarget;
                
            
            if (isBotStuck)
            {
                bodyRB.velocity += 
                    new Vector3(Random.Range(-1f,1f),Random.Range(0.15f,1f),Random.Range(-1f,1f)) * antiStuckPower;
            }

        }
    }


    internal virtual bool CheckIsLookingTarget()
    {

        //? ???? ?? ?????? ????
        Vector3 localObjPos = (target.position - head.position);
        float dot = Vector3.Dot(head.forward, localObjPos.normalized);
        float botTargetDistance = Vector3.Distance(target.position,body.position);
        bool wallsCheck = WallsCheck();

        if (wallsCheck && botTargetDistance <= targetVeryCloselyRadius)
        {
            isBotGoToWayPoints = false;
            isTargetVeryClosely = true;
            GetAannoyed();
        }
        else
            isTargetVeryClosely = false;

        if (dot >= fovDot && botTargetDistance <= lookingRadius)
        {

            //???????? ?? ??????? ???? ????? ?????????
            if(wallsCheck)
            {
                //?????? ?? ????
                if (botTargetDistance <= targetCloselyRadius)
                    isTargetClosely = true;
                else 
                    isTargetClosely = false;

                return true;
            }    


        }

        return false;

        bool WallsCheck()
        {
            RaycastHit hit;

            if (Physics.Raycast(head.position, localObjPos, out hit, lookingRadius, lookingLayerMask))
            {
                int hash1 = hit.collider.gameObject.transform.position.GetHashCode();
                int hash2 = target.position.GetHashCode();

                if (hash1 == hash2)
                    return true;

            }

            return false;

        }

    }

    protected virtual void StaticBotRotation()
    {
        Vector3 targetRotation = Quaternion.LookRotation(target.position - body.position).eulerAngles;

        targetRotation.x = 0;
        targetRotation.z = 0;

        float rotSpeed = 5f;

        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotSpeed);

    }

    internal virtual void GetAannoyed()
    {
        aannoyedTimer = aannoyedTime;
    }

    public void SetWayPoint(Vector3 wayPoint,float wayTime)
    {
        currentWayPoint = wayPoint;
        
        SetDestination(wayPoint);
        isBotGoToWayPoints = true;
        aannoyedTimer = wayTime;
    }

    ///////

    public override void GetDamage(float damage,Transform source)
    {
        base.GetDamage(damage);

        GetAannoyed();
        targetOldLookTimer = 0;

        isBotGoToWayPoints = false;
        
        if (agent.isOnNavMesh)
            SetDestination(target.position);

        if (source != null)
        {
            float sourcePosSmoothness = 0.65f;

            Vector3 trueSourcePos = source.position - (source.forward * sourcePosSmoothness);

            if (botEffects != null && source != null)
                botEffects.PlayHitParticle(trueSourcePos);

            float forceMultiply = 50f;

            Vector3 direction = source.forward * forceMultiply;
            direction.y = 0;

            bodyRB.AddForce(direction * damageImpulse, ForceMode.Acceleration);
        }
    }

    ///////

    public override void Died()
    {
        if(botAnimations != null)
            botAnimations.Destruct();

        StartCoroutine(Die());
        
        IEnumerator Die()
        {
            yield return null;
            
            LevelSaveData.mainLevelSaveData.RemoveFromSaveData(this);

            if (botParts != null)
                botParts.DestructParts(bodyRB);

            botEffects.Destruct();
            gameObject.SetActive(false);
            
            if (isSmart) 
                EnemysAiManager.RemoveAI(this,killScoreValue);

            if(agent != null) 
                Destroy(agent.gameObject);
            
            Destroy(gameObject, 5);
        }
    }

    public float PathLegth(Vector3[] corners)
    {
        float pathLegth = 0;

        if (corners.Length >= 2)
        {
            for (int i = 0; i < corners.Length - 1; i++)
            {
                pathLegth += Vector3.Distance(corners[i], corners[i + 1]);
            }
        }

        return pathLegth;

    }

    protected void RotateToTarget(Transform item, Vector3 targetPos, float speed)
    {
        Quaternion resultRotation = item.rotation;
        Quaternion targetRot = Quaternion.identity;
        float timeStep = Time.deltaTime * speed;

        if (!isBotGoToWayPoints && (isLookingTarget || isAnnoyed ))
        {
            targetRot = Quaternion.LookRotation(targetPos - item.position);

            resultRotation = Quaternion.Lerp(resultRotation, targetRot, timeStep);
        }
        else
        {
            targetRot *= body.rotation;

            resultRotation = Quaternion.Lerp(resultRotation, targetRot, timeStep);
        }

        item.rotation = resultRotation;
    }

    protected void RotateToTargetClamp(Transform itemChild, Vector3 targetPos, float speed,
        Vector3 clampAngles)
    {
        Quaternion resultRotation = itemChild.rotation;
        Quaternion targetRot = Quaternion.identity;
        float timeStep = Time.deltaTime * speed;

        if (!isBotGoToWayPoints && (isLookingTarget || isAnnoyed))
        {
            targetRot = Quaternion.LookRotation(targetPos - itemChild.position);

            resultRotation = Quaternion.Lerp(resultRotation, targetRot, timeStep);
        }
        else
        {
            targetRot *= body.rotation;

            resultRotation = Quaternion.Lerp(resultRotation, targetRot, timeStep);
        }

        itemChild.rotation = resultRotation;

        Quaternion itemLocalRotation = itemChild.localRotation;

        ClampRotation(ref itemLocalRotation, clampAngles);

        itemChild.localRotation = itemLocalRotation;

        void ClampRotation(ref Quaternion rotation, Vector3 clampAngles)
        {
            float xAngle = rotation.eulerAngles.x;
            float yAngle = rotation.eulerAngles.y;
            float zAngle = rotation.eulerAngles.z;

            float xClampAngle = clampAngles.x;
            float yClampAngle = clampAngles.y;
            float zClampAngle = clampAngles.z;

            ClampAngle(ref xAngle, xClampAngle);
            ClampAngle(ref yAngle, yClampAngle);
            ClampAngle(ref zAngle, zClampAngle);

            Vector3 clampedRotation = new Vector3(xAngle, yAngle, zAngle);

            rotation = Quaternion.Euler(clampedRotation);

            void ClampAngle(ref float angle, float clampAngle)
            {

                bool angle2range = angle <= 360 && angle > 180;

                if (!angle2range)
                {
                    if (angle > clampAngle)
                        angle = clampAngle;
                }
                else
                {
                    if (angle < 360 - clampAngle)
                        angle = 360 - clampAngle;
                }
            }
        }
    }

    protected Vector3 CalculateSmartTargetPos(float bulletSpeed,Rigidbody targetRb)
    {
        if (targetRb == null)
            return target.position;

        float botTargetDistance = Vector3.Distance(body.position, target.position);

        float targetRbVelocityAmount = botTargetDistance / bulletSpeed;

        Vector3 targetSmatrPos = target.position + (targetRb.velocity * targetRbVelocityAmount);

        return targetSmatrPos;
    }

    public void SubPlayerLookEvent(Action action)
    {
        if(action != null)
            botPlayerLookEvent += action;
    }
    
    public void UnSubPlayerLookEvent(Action action)
    {
        if(action != null)
            botPlayerLookEvent -= action;
    }
    
}