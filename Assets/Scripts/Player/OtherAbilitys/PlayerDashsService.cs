using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashsService : MonoBehaviour,IUsePlayerDevicesButtons
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLookService playerLook;
    [SerializeField] private ParticleSystem dashEffect;
    [Space]
    [SerializeField] private int dashsCount = 3;
    [SerializeField] private float oneDashEnergySpend = 3f;
    [SerializeField] private float dashsRegenerationSpeed = 3f;

    private DeviceButton useDashButton  = new DeviceButton();
    
    public int DashsCount => dashsCount;

    public float OneDashEnergySpend => oneDashEnergySpend;
    public float DashsRegenerationSpeed => dashsRegenerationSpeed;
    
    [SerializeField] private float dashPower = 2f;
    [SerializeField] private float dashColdown = 0.5f;
    private float dashCurrentColdownTimer = 0;
    
    public float DashPower => dashPower;

    [SerializeField] private int dashStopDeltaTimeTicks = 30;
    [SerializeField] private float flyDashResidualForceAmount = 0.2f;
    
    private float dashCurrentEnergy;
    private float dashMaxEnergy;
    
    private bool isManageBlocked;

    public float DashCurrentEnergy => dashCurrentEnergy;
    public float DashMaxEnergy => dashMaxEnergy;


    private void Awake()
    {
        dashCurrentEnergy = dashsCount * oneDashEnergySpend;
        dashMaxEnergy = dashsCount * oneDashEnergySpend;
    }

    private void Update()
    {
        DashUpdateAlgorithm();
    }

    public void SetManageActive(bool state)
    {
        isManageBlocked = !state;
    }
    
    private void DashUpdateAlgorithm()
    {
        if(!isManageBlocked)
            DashsManageAlgorithm();

        DashsEnergyRegeneration();

        DashsColdownTimer();
    }

    private void DashsManageAlgorithm()
    {
        bool dashIsReady =
            useDashButton.IsGetButtonDown() &&
            dashCurrentColdownTimer <= 0 &&
            dashCurrentEnergy >= oneDashEnergySpend;

        if (dashIsReady)
            StartDash();
    }

    private void DashsEnergyRegeneration()
    {
        if (dashCurrentEnergy < dashMaxEnergy && !playerMovement.isFlies)
            dashCurrentEnergy += Time.deltaTime * dashsRegenerationSpeed;
    }

    private void DashsColdownTimer()
    {
        if(dashCurrentColdownTimer > 0)
            dashCurrentColdownTimer -= Time.deltaTime;
    }

    private void StartDash()
    {
        dashCurrentEnergy =
            (int)((dashCurrentEnergy - oneDashEnergySpend) / oneDashEnergySpend)
            * oneDashEnergySpend;

        dashCurrentColdownTimer += dashColdown;

        Vector3 currentPlayerDirection = CalculateCurrentPlayerDirection();
        StartCoroutine(DashProcess(currentPlayerDirection));


        RotateDashEffectToDashDirection(currentPlayerDirection);
        dashEffect.Play();


        Vector3 CalculateCurrentPlayerDirection()
        {
            Vector3 currentPlayerMovementDirection = playerMovement.CurrentMovementDirection;

            if (currentPlayerMovementDirection == Vector3.zero)
                currentPlayerMovementDirection = playerMovement.Body.forward;

            currentPlayerMovementDirection.y = 0;
            currentPlayerMovementDirection.Normalize();

            return currentPlayerMovementDirection;

        }

        void RotateDashEffectToDashDirection(Vector3 playerDirection)
        {
            Transform dashEffectT = dashEffect.transform;

            Quaternion dashDirection =
                Quaternion.LookRotation(-playerDirection);
            
            dashEffectT.rotation = dashDirection;
        }

    }

    private IEnumerator DashProcess(Vector3 currentPlayerXYDirection)
    {

        Vector3 resultDashForce = 
            currentPlayerXYDirection * dashPower;

        YieldInstruction waitFixedUpdate = new WaitForFixedUpdate();

        playerMovement.PlayerRb.velocity += resultDashForce;

        for (int i = 0; i < dashStopDeltaTimeTicks; i++)
        {
            FlyDeshForceAdjustment();

            yield return waitFixedUpdate;

        }

        void FlyDeshForceAdjustment()
        {
            if(playerMovement.isFlies)
            {
                Vector3 adjustmentForce =
                    (resultDashForce / dashStopDeltaTimeTicks) * (1f - flyDashResidualForceAmount);

                Vector3 playerVelocity = playerMovement.PlayerRb.velocity;

                bool isPlayerVelocityContainAdjustmentForceXY =
                    IsVectorContainVectorXZ(playerVelocity, currentPlayerXYDirection);

                if (isPlayerVelocityContainAdjustmentForceXY)
                {
                    playerMovement.PlayerRb.velocity -= adjustmentForce;
                }
            }


            bool IsVectorContainVectorXZ(Vector3 vector,Vector3 containsVector)
            {

                bool x;
                bool z;

                if (containsVector.x >= 0)
                    x = (vector.x - containsVector.x) >= 0;
                else
                    x = (vector.x - containsVector.x) <= 0;

                if (containsVector.z >= 0)
                    z = (vector.z - containsVector.z) >= 0;
                else
                    z = (vector.z - containsVector.z) <= 0;

                return x || z;

            }

        }

    }

    public DeviceButton[] GetUsesDevicesButtons()
    {
        var getButtons = new[]
            {useDashButton};

        return getButtons;
    }
}
