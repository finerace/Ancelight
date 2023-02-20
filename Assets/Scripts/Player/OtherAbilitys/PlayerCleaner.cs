using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Unity.Burst;

public class PlayerCleaner : MonoBehaviour,IUsePlayerDevicesButtons
{

    [SerializeField] private float garbageAnimationSpeed = 1f;
    [SerializeField] private float garbageAnimationScaleSpeed = 2f;
    [Space]
    [SerializeField] private float garbageCollectDistance = 15f;
    [SerializeField] private float garbageCollectRadius = 5f;
    [SerializeField] private LayerMask garbageMask = 1 << 10;


    [SerializeField] private Transform cleanerPoint;
    [SerializeField] private PlayerLookService playerLook;

    [SerializeField] private float particlsToPointVelocitySpeed = 2f;
    [SerializeField] private ParticleSystem[] cleanerParticls = new ParticleSystem[0];

    private List<Transform> capturedGarbage = new List<Transform>();

    private bool isWork = false;
    private bool isGarbageCollectorActive = false;

    private bool cleanerIsWorking = false;
    private event Action onCleanerStart;
    public event Action OnCleanerStart
    {
        add 
        {
            onCleanerStart += value;
        }

        remove
        {
            if (value == null)
                throw new NullReferenceException();

            onCleanerStart -= value;
        }
    }
    
    private event Action onCleanerEnd;
    public event Action OnCleanerEnd
    {
        add 
        {
            onCleanerEnd += value;
        }

        remove
        {
            if (value == null)
                throw new NullReferenceException();

            onCleanerEnd -= value;
        }
    }
    
    private event Action onCleanerDestroyTrash;
    public event Action OnCleanerDestroyTrash
    {
        add 
        {
            onCleanerDestroyTrash += value;
        }

        remove
        {
            if (value == null)
                throw new NullReferenceException();

            onCleanerDestroyTrash -= value;
        }
    }
    
    [SerializeField] private Animator cleaningAnimator;

    private DeviceButton useCleanerButton = new DeviceButton();

    private void Update()
    {
        isWork = useCleanerButton.IsGetButton();

        if (isWork && !isGarbageCollectorActive)
        {
            CollectGarbage();
        }
        
        
        if (isWork && !cleanerIsWorking)
        {
            cleanerIsWorking = true;
                
            if(onCleanerStart != null)
                onCleanerStart.Invoke();
        }
        else if (!isWork && cleanerIsWorking)
        {
            cleanerIsWorking = false;
            
            if(onCleanerEnd != null)
                onCleanerEnd.Invoke();
        }
             

        CorrectParticleDirectionToTarget(cleanerPoint.position, particlsToPointVelocitySpeed);

        MoveAndDestroyGarbage();

        SetAnimations();
    }

    private void CollectGarbage()
    {

        float updateTime = 0.25f;

        StartCoroutine( CollectorUpdater(updateTime) );

        void CollectGarbageAlgorithm()
        {
            Vector3 playerLookDirection = playerLook.MainCameraT.forward;
            Vector3 startCapsulePosition = playerLook.MainCameraT.position;
            Vector3 endCapsulePosition = startCapsulePosition +
                (playerLookDirection * garbageCollectDistance);


            Collider[] collectedGarbageColliders = Physics.OverlapCapsule
                (startCapsulePosition, endCapsulePosition, garbageCollectRadius, garbageMask);

            foreach (var item in collectedGarbageColliders)
            {
                Transform itemT = item.transform;

                if (capturedGarbage.Contains(itemT))
                    continue;
                capturedGarbage.Add(itemT);

                if (item.TryGetComponent<Rigidbody>(out Rigidbody itemRB))
                    Destroy(itemRB);
            }
        }

        IEnumerator CollectorUpdater(float updateTime)
        {
            YieldInstruction waitTime = new WaitForSeconds(updateTime);

            isGarbageCollectorActive = true;
            LaunchCleanerParticls();

            do
            {
                CollectGarbageAlgorithm();

                yield return waitTime;
            } 
            while (isWork);

            isGarbageCollectorActive = false;
            StopCleanerParticls();

        }

    }

    private void MoveAndDestroyGarbage()
    {

        float timeStep = Time.deltaTime * garbageAnimationSpeed;
        float garbageDestroyDistance = 1f;

        int maxDeletedGarbagesInFrame = 2;
        List<Transform> deleteGarbages = new List<Transform>(maxDeletedGarbagesInFrame);

        for (int i = 0; i < capturedGarbage.Count; i++)
        {

            if (capturedGarbage[i] == null)
                continue;

            var item = capturedGarbage[i];

            float cleanerGarbageDistance = Vector3.Distance(item.position, cleanerPoint.position);

            //float timeStepDistanceScale = timeStep * (cleanerGarbageDistance / distanceSpeedSmoothness);


            item.position = Vector3.MoveTowards
                (item.position, cleanerPoint.position, timeStep);

            item.localRotation *= Quaternion.Euler(10 * timeStep, 0, 0);

            item.localScale = Vector3.Lerp(item.localScale, Vector3.zero,
                timeStep * garbageAnimationScaleSpeed);


            if (cleanerGarbageDistance < garbageDestroyDistance && 
            deleteGarbages.Count != maxDeletedGarbagesInFrame - 1)
            {
                deleteGarbages.Add(item);
            }
        }


        for (int i = 0; i < deleteGarbages.Count; i++)
        {
            if (deleteGarbages[i] == null)
                continue;

            capturedGarbage.Remove(deleteGarbages[i]);
            Destroy(deleteGarbages[i].gameObject);
            
            onCleanerDestroyTrash.Invoke();
        }

    }

    private void SetAnimations()
    {
        cleaningAnimator.SetBool("IsWork", isWork);
    }

    private void LaunchCleanerParticls()
    {
        foreach (var item in cleanerParticls)
        {
            if(item == null || item.isEmitting)
                continue;

            item.Play();
            
        }

    }

    private void StopCleanerParticls()
    {
        foreach (var item in cleanerParticls)
        {
            if (item == null || item.isStopped)
                continue;

            item.Stop();
        }
    }

    private void CorrectParticleDirectionToTarget(Vector3 targetPos, float speed = 250f)
    {
        float rotationSpeed = speed;

        ParticleRotateToTargetJob particleRotateToTargetJob
            = new ParticleRotateToTargetJob
            {
                deltaTime = Time.deltaTime,
                target = targetPos,
                rotationSpeed = rotationSpeed
            };

        foreach (var item in cleanerParticls)
        {
            if (item == null || !item.isPlaying)
                continue;
            
            particleRotateToTargetJob.Schedule(item,512);
        }

    }

    [BurstCompile]
    private struct ParticleRotateToTargetJob : IJobParticleSystemParallelFor
    {
        public Vector3 target;
        public float rotationSpeed;
        public float deltaTime;

        public void Execute(ParticleSystemJobData partiiclsData,int index)
        {
            var positions = partiiclsData.positions;
            var velocities = partiiclsData.velocities;

            var position = positions[index];
            var direction = target - position;

            var velocity = velocities[index];

            velocities[index] = 
                Vector3.RotateTowards(velocity, direction, rotationSpeed * deltaTime, 0);

        }

    }

    public DeviceButton[] GetUsesDevicesButtons()
    {
        var getButtons = new[]
            {useCleanerButton};

        return getButtons;
    }
}
