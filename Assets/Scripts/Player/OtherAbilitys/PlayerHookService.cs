using UnityEngine;


public class PlayerHookService : MonoBehaviour
{
    [SerializeField] private Transform hookT;
    [SerializeField] private Transform hookMeshT;
    
    [SerializeField] private LineRenderer hookLineRenderer;
    [SerializeField] private PlayerMainService player;
    [SerializeField] private Transform hookStartPoint;
    [SerializeField] private SpringJoint playerJoint;
    [Space]
    
    [SerializeField] private float hookMaxStrength = 25f;
    private float hookCurrentStrength = 0;
    
    public float HookMaxStrength => hookMaxStrength;
    public float HookCurrentStrength => hookCurrentStrength;
    
    [SerializeField] private float hookStrengthUsePerSecond = 5f;
    [SerializeField] private float hookStrengthRegenerationPerSecond = 3.5f;
    [SerializeField] private float hookStrengthRegenerationAfterUseTime = 5f;
    private float hookAfterUseTimer = 0f;
    private bool isAfterUseTimerActive = true;
    
    public bool IsAfterUseTimerActive => isAfterUseTimerActive;
    public float HookStrengthRegenerationPerSecond => hookStrengthRegenerationPerSecond;

    [Space]
    [SerializeField] private float hookForce = 15f;
    [SerializeField] private float hookMaxActionRange = 30f;
    [SerializeField] private float hookDistanceDivider = 6f;
    [SerializeField] private float hookDamage = 15f;
    [SerializeField] private LayerMask hookUseSurfaceMask;

    [SerializeField] private float minStrengthAmountToUse = 0.1f;


    public float MinStrengthAmountToUse => minStrengthAmountToUse;
    public float HookMaxActionRange => hookMaxActionRange;

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

    private void Awake()
    {
        //playerRB = GetComponent<Rigidbody>();
        hookRb = hookT.GetComponent<Rigidbody>();
        hookFixedJoint = hookT.GetComponent<FixedJoint>();
        
        hookRb.transform.parent = null;
        hookRb.gameObject.SetActive(false);

        startDamper = playerJoint.damper;
        playerJoint.damper = 0;
        hookCurrentStrength = hookMaxStrength;
        hookLineRenderer.transform.parent = null;

        lastHookRbPos = Vector3.forward;
    }

    private void Update()
    {
        if(hookManageIsBlocked)
            return;
        
        UpdateHookAlgorithm();

    }

    public void SetManageActive(bool state)
    {
        hookManageIsBlocked = !state;
    }
    
    private void UpdateHookAlgorithm()
    {
        float fire2 = Axis.Fire2;

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

            const float hookLegthAmount = 0f;

            DrawHookSmoothAmount(hookLegthAmount);

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


        bool GetHookRayRaycastHit(out RaycastHit hitObj)
        {
            Ray hookCheckSurfaceRay;

            Vector3 hookRayOrigin = player.playerLook.ShootingPoint.position;
            Vector3 hookRayDirection = player.playerLook.ShootingPoint.forward;

            hookCheckSurfaceRay = new Ray(hookRayOrigin, hookRayDirection);

            return
                Physics.Raycast(hookCheckSurfaceRay, out hitObj, hookMaxActionRange, hookUseSurfaceMask);
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

}
