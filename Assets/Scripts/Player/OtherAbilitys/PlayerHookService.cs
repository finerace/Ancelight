using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


public class PlayerHookService : MonoBehaviour,IUsePlayerDevicesButtons
{
    [SerializeField] private Transform hookT;
    [SerializeField] private Transform hookMeshT;
    
    [SerializeField] private LineRenderer hookLineRenderer;
    [SerializeField] private PlayerMainService player;
    [SerializeField] private Transform hookStartPoint;
    [SerializeField] private SpringJoint playerJoint;
    [Space]
    
    [SerializeField] private float hookMaxStrength = 25f;
    [SerializeField] private float hookCurrentStrength = 0;
    
    public float HookMaxStrength => hookMaxStrength;
    public float HookCurrentStrength => hookCurrentStrength;
    
    [SerializeField] private float hookStrengthUsePerSecond = 5f;
    [SerializeField] private float hookStrengthRegenerationPerSecond = 3.5f;
    [SerializeField] private float hookStrengthRegenerationAfterUseTime = 5f;
    [SerializeField] private float hookAfterUseTimer = 0f;
    private bool isAfterUseTimerActive = true;
    
    public bool IsAfterUseTimerActive => isAfterUseTimerActive;
    public float HookStrengthRegenerationPerSecond => hookStrengthRegenerationPerSecond;

    [Space]
    [SerializeField] private float hookForce = 15f;
    [SerializeField] private float hookMaxActionRange = 30f;
    [SerializeField] private float hookDistanceDivider = 6f;
    [SerializeField] private float hookDamage = 15f;
    [SerializeField] private bool isHookOnlyPointMode = true;
    [SerializeField] private LayerMask hookUseSurfaceMask;
    [SerializeField] private LayerMask hookObstaclesSurfaceMask;
    
    
    [SerializeField] private float minStrengthAmountToUse = 0.1f;

    private Transform[] hookPoints;

    public Transform[] HookPoints => hookPoints;

    public Transform HookMeshT => hookMeshT;

    public float MinStrengthAmountToUse => minStrengthAmountToUse;

    public float HookMaxActionRange => hookMaxActionRange;

    public float HookStrengthUsePerSecond => hookStrengthUsePerSecond;

    public float HookStrengthRegenerationAfterUseTime => hookStrengthRegenerationAfterUseTime;

    public float HookAfterUseTimer => hookAfterUseTimer;

    public float HookForce => hookForce;

    public float HookDamage => hookDamage;

    public float StartDamper => startDamper;
    
    [Space] 
    
    [SerializeField] private bool isHookExist;
    public bool IsHookExist => isHookExist;
    private event Action onHookUnlock;
    public event Action OnHookUnlock
    {
        add => onHookUnlock += value;
        remove => onHookUnlock -= value;
    }
    
    
    private bool isHookUsed = false;

    public bool IsHookUsed => isHookUsed;

    private bool isHookStrengthRegenerationActive = false;

    public bool IsHookStrengthRegenerationActive => isHookStrengthRegenerationActive;

    private Rigidbody hookRb;

    private FixedJoint hookFixedJoint;

    //private Rigidbody playerRB;


    private float startDamper;

    private float currentDrawHookAmount = 0;

    private Vector3 lastHookRbPos;

    private Transform lastHookHitObjT;

    private bool hookManageIsBlocked = false;

    private event Action hookUseEvent;

    private event Action hookEndUseEvent;

    private event Action hookRegenerateEvent;

    private bool isHookRegenerateWorked;

    public event Action HookRegenerateEvent
    {
        add
        {
            if (value == null)
                throw new EventSourceException("Action is null!");
            
            hookRegenerateEvent += value;
        }

        remove
        {
            if (value == null)
                throw new EventSourceException("Action is null!");
            
            hookRegenerateEvent -= value;
        }
    }

    private DeviceButton useHookButton = new DeviceButton();

    private bool isLoaded = false;

    public void Load(PlayerHookService savedHookService)
    {
        this.hookCurrentStrength = savedHookService.hookCurrentStrength;
        this.hookMaxStrength = savedHookService.hookMaxStrength;

        this.hookStrengthRegenerationPerSecond = savedHookService.hookStrengthRegenerationPerSecond;
        this.hookStrengthUsePerSecond = savedHookService.hookStrengthUsePerSecond;
        this.hookStrengthRegenerationAfterUseTime = savedHookService.hookStrengthRegenerationAfterUseTime;

        this.minStrengthAmountToUse = savedHookService.minStrengthAmountToUse;
        this.hookMaxActionRange = savedHookService.hookMaxActionRange;

        this.hookForce = savedHookService.hookForce;
        this.hookDamage = savedHookService.hookDamage;

        isHookExist = savedHookService.isHookExist;
        
        isLoaded = true;
    }

    private void Awake()
    {
        //playerRB = GetComponent<Rigidbody>();
        hookRb = hookT.GetComponent<Rigidbody>();
        hookFixedJoint = hookT.GetComponent<FixedJoint>();
        
        hookRb.transform.parent = null;
        hookRb.gameObject.SetActive(false);

        startDamper = playerJoint.damper;
        playerJoint.damper = 0;
        
        if(!isLoaded)
            hookCurrentStrength = hookMaxStrength;
        
        hookLineRenderer.transform.parent = null;

        lastHookRbPos = Vector3.forward;

        
        //StartCoroutine(HookPointsFinder());
        IEnumerator HookPointsFinder()
        {
            const float findCooldown = 0.5f;
            var waitSeconds = new WaitForSeconds(findCooldown);

            while (true)
            {
                yield return waitSeconds;
                
                FindHookPoints();
            }
            
            FindHookPoints();
        }
    }

    void FindHookPoints()
    {
        const int hookPointLayerMask = 1 << 15;
                
        var allHookPointInRange =
            Physics.OverlapSphere(player.weaponsManager.shootingPoint.position, hookMaxActionRange,hookPointLayerMask);
                
        hookPoints = GetAllTrueHookPoints(allHookPointInRange);

        Transform[] GetAllTrueHookPoints(Collider[] findsHookPointsColliders)
        {
            var resultHookPoints = new List<Transform>();

            foreach (var hookPoint in findsHookPointsColliders)
            {
                var origin = player.weaponsManager.shootingPoint;
                var hookPointT = hookPoint.transform;
                        
                var direction = -(origin.position - hookPointT.position).normalized;

                var checkRay = new Ray(origin.position, direction);

                var originHookPointDistance = 
                    Vector3.Distance(origin.position,hookPointT.position);
                        
                var isPlayerLookHookPoint = Vector3.Dot(origin.forward, direction) > 0.5f
                                            && !Physics.Raycast(checkRay, originHookPointDistance, (1 << 0) + (1 << 1));
                        
                if (isPlayerLookHookPoint)
                    resultHookPoints.Add(hookPoint.transform);
            }

            return resultHookPoints.ToArray();
        }
    }

    private void Update()
    {
        if(!isHookExist)
            return;
            
        if(hookManageIsBlocked)
            return;
        
        FindHookPoints();
        UpdateHookAlgorithm();
    }

    public void SetManageActive(bool state)
    {
        hookManageIsBlocked = !state;
    }
    
    private void UpdateHookAlgorithm()
    {
        float fire2 = 0;

        if (useHookButton.IsGetButton())
            fire2 = 1;
        else
            fire2 = 0;
        
        if (!isHookUsed)
        {
            float hookCurrentStrengthAmount = hookCurrentStrength / hookMaxStrength;
            const float minDrawHookAmountToStartUse = 0.02f;

            bool isHookCanUse =
                fire2 > 0 &&
                hookCurrentStrengthAmount >= minStrengthAmountToUse &&
                currentDrawHookAmount <= minDrawHookAmountToStartUse;

            if (isHookCanUse)
                StartUseHook();

            HookAfterUseSetTimer();

            const float hookLengthAmount = 0f;

            DrawHookSmoothAmount(hookLengthAmount);

        }
        else
        {
            HookStrengthUse();

            bool isHookCanUse =
                fire2 > 0 &&
                hookCurrentStrength > 0 &&
                lastHookHitObjT != null;

            if(!isHookCanUse)
                EndHookUse();

            const float hookLegthAmount = 1f;

            if(hookRb.gameObject.activeSelf)
                lastHookRbPos = hookRb.position;

            DrawHookSmoothAmount(hookLegthAmount);

            hookAfterUseTimer = hookStrengthRegenerationAfterUseTime;
            isAfterUseTimerActive = true;

            isHookStrengthRegenerationActive = false;
        }

        if(isHookStrengthRegenerationActive && !player.playerMovement.isFlies)
            HookStrengthRegeneration();

        void HookStrengthRegeneration()
        {
            if (hookCurrentStrength >= hookMaxStrength)
            {
                isHookStrengthRegenerationActive = false;
                return;
            }

            hookCurrentStrength +=
                    hookStrengthRegenerationPerSecond * Time.deltaTime;

            if (hookCurrentStrength + 0.05f >= hookMaxStrength)
            {
                if (!isHookRegenerateWorked && hookRegenerateEvent != null)
                {
                    hookRegenerateEvent.Invoke();
                    isHookRegenerateWorked = true;
                }
            }
            else
                isHookRegenerateWorked = false;

        }

        void HookStrengthUse()
        {
            if (hookCurrentStrength < 0)
                return;

            hookCurrentStrength -=
                hookStrengthUsePerSecond * Time.deltaTime;
        }

        void HookAfterUseSetTimer()
        {
            if(hookAfterUseTimer > 0 && isAfterUseTimerActive)
                hookAfterUseTimer -= Time.deltaTime;
            else if(isAfterUseTimerActive)
            {
                isAfterUseTimerActive = false;
                isHookStrengthRegenerationActive = true;
            }

        }

        void DrawHookSmoothAmount(float targetAmount)
        {
            const float amountChangeSpeed = 5f;

            var timeStep = Time.deltaTime * amountChangeSpeed;
            
            currentDrawHookAmount = 
                Mathf.MoveTowards(currentDrawHookAmount, targetAmount, timeStep);

            DrawHookRenderer(currentDrawHookAmount);
        }

    }

    private void StartUseHook()
    {
        RaycastHit hookHitObj;
        
        bool isHitFindSurface = GetHookRayRaycastHit(out hookHitObj);

        if (!isHitFindSurface)
            return;
        
        float hookLength = Vector3.Distance(hookHitObj.point, hookStartPoint.position);

        if (hookLength > hookMaxActionRange)
            return;

        isHookUsed = true;

        if(isHookOnlyPointMode)
            hookHitObj.point = hookHitObj.transform.position;
        
        hookRb.gameObject.SetActive(true);
        hookRb.transform.position = hookHitObj.point;

        playerJoint.damper = startDamper;
        playerJoint.minDistance = hookLength / hookDistanceDivider;

        lastHookHitObjT = hookHitObj.collider.gameObject.transform;


        Rigidbody hitObjRb;
        bool isHitObjHasRb;

        isHitObjHasRb = 
            CheckHookHitObjectForRb(hookHitObj,out hitObjRb);

        if(isHitObjHasRb)
        {
            hookFixedJoint.connectedBody = hitObjRb;
            hookRb.isKinematic = false;

        }
        else
        {
            hookFixedJoint.connectedBody = null;
        }

        playerJoint.connectedBody = hookRb;
        playerJoint.spring = hookForce;
        
        Health hitObjHealh;
        bool isHitObjHasHealth;

        isHitObjHasHealth = 
            CheckHookHitObjectForHealth(hookHitObj,out hitObjHealh);

        if(isHitObjHasHealth)
        {
            hitObjHealh.GetDamage(hookDamage);
        }
        
        if(hookUseEvent != null)
            hookUseEvent.Invoke();
        
        bool GetHookRayRaycastHit(out RaycastHit hitObj)
        {
            Ray hookCheckSurfaceRay;

            Vector3 hookRayOrigin = player.playerLook.ShootingPoint.position;
            Vector3 hookRayDirection = player.playerLook.ShootingPoint.forward;

            hookCheckSurfaceRay = new Ray(hookRayOrigin, hookRayDirection);
            var rayLayerMask = hookUseSurfaceMask;

            if (isHookOnlyPointMode)
                rayLayerMask = 1 << 15;

            if(!Physics.Raycast(hookCheckSurfaceRay, out hitObj, hookMaxActionRange,rayLayerMask))
                return false;
            
            var originHookPointDistance =
                Vector3.Distance(hookRayOrigin, hitObj.collider.gameObject.transform.position) - 0.01f;
            if(Physics.Raycast(hookCheckSurfaceRay,originHookPointDistance,hookObstaclesSurfaceMask))
                return false;
            
            return true;
        }

        bool CheckHookHitObjectForRb(RaycastHit hitObj,out Rigidbody hitObjRb)
        {
            bool isHitObjHasRb = 
                hitObj.collider.gameObject.
                TryGetComponent<Rigidbody>(out hitObjRb);

            return isHitObjHasRb;
        }

        bool CheckHookHitObjectForHealth(RaycastHit hitObj,out Health hitObjHealth)
        {
            bool isHitObjHasHealth =
                hitObj.collider.gameObject.TryGetComponent<Health>(out hitObjHealth);

            return isHitObjHasHealth;
        }
    }
    
    private void EndHookUse()
    {
        isHookUsed = false;

        if(!hookRb.isKinematic)
            hookRb.isKinematic = true;

        hookFixedJoint.connectedBody = null;
        hookRb.gameObject.SetActive(false);

        playerJoint.connectedBody = null;
        playerJoint.spring = 0;
        playerJoint.damper = 0;
        
        if(hookEndUseEvent != null)
            hookEndUseEvent.Invoke();
    }

    private void DrawHookRenderer(float amount)
    {
        var hookRendererPositions = new Vector3[2];

        var hookStartPos = hookStartPoint.position;
        
        hookRendererPositions[0] = hookStartPos;

        var toHookPosDirection =
            (lastHookRbPos - hookStartPos).normalized;

        var hookStartHookDistance = 
            Vector3.Distance(hookStartPos, lastHookRbPos);

        hookRendererPositions[1] = hookStartPos +
            toHookPosDirection * hookStartHookDistance * amount;

        hookLineRenderer.SetPositions(hookRendererPositions);

        HookModelRendererAlgorithm();

        void HookModelRendererAlgorithm()
        {

            if (amount >= 0.99f)
            {
                hookMeshT.gameObject.SetActive(true);
            }
            else if(amount >= 0.05f)
            {

                hookMeshT.gameObject.SetActive(true);

                Vector3 toHookRbDirection = 
                    (lastHookRbPos - hookMeshT.position).normalized;

                if (toHookRbDirection != Vector3.zero)
                {
                    Quaternion modelToHookRotation =
                    Quaternion.LookRotation(toHookRbDirection);

                    hookMeshT.rotation = modelToHookRotation;
                }
            }
            else
            {
                hookMeshT.gameObject.SetActive(false);
            }

            hookMeshT.position = hookRendererPositions[1];
        }

    }

    public void UnlockHook()
    {
        isHookExist = true;
        
        onHookUnlock?.Invoke();
    }
    
    public void SubHookUseEvent(Action action)
    {
        if (action != null)
            hookUseEvent += action;
    }
    
    public void UnSubHookUseEvent(Action action)
    {
        if (action != null)
            hookUseEvent -= action;
    }
    
    public void SubHookEndUseEvent(Action action)
    {
        if (action != null)
            hookEndUseEvent += action;
    }
    
    public void UnSubHookEndUseEvent(Action action)
    {
        if (action != null)
            hookEndUseEvent -= action;
    }
    
    public DeviceButton[] GetUsesDevicesButtons()
    {
        return
            new[] {useHookButton};
    }
}
