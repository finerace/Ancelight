using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] internal BulletEffects effects;
    [SerializeField] internal Rigidbody rigidbody_;
    [SerializeField] internal Transform body_;
    [SerializeField] private GameObject colliders;
    
    [Space]
    [HideInInspector] [SerializeField] private float destructionTime = 2;
    [HideInInspector] [SerializeField] private float currentTime;
    
    public float CurrentTime => currentTime;
    
    [SerializeField] private float lifeTime;
    
    [HideInInspector] [SerializeField] internal bool isDestruction = false;
    [SerializeField] private bool isFly = true;
    [SerializeField] private float gravityScale;
    [SerializeField] internal float damage;

    [Space]
    [SerializeField] private bool inCollisionAddForce = true;
    [SerializeField] private float collisionForce = 2f; 
    protected float startRBPower = 1f;
    [Range(0, 1000)] [SerializeField] private float speed;
    [SerializeField] private int bulletId;
    private bool isLoaded = false;
    
    private event Action onDestroyEvent;
    
    public int BulletId => bulletId;

    public void Load(float currentTime)
    {
        this.currentTime = currentTime;
        isLoaded = true;
    }
    
    protected virtual void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
        
        body_ = transform;

        if(rigidbody_ != null && !isLoaded) 
            rigidbody_.velocity += body_.forward * startRBPower * speed;

        StartCoroutine(AutoDestruction(lifeTime)) ;

    }

    protected void Update()
    {
        currentTime += Time.deltaTime;
    }

    protected void FixedUpdate()
    {
        if (!isFly && gravityScale != 0)
        {
            var smooth = 100;

            rigidbody_.AddForce(Physics.gravity*gravityScale*Time.fixedDeltaTime * smooth);
        }
        
        if (!isDestruction && isFly)
        {
            body_.position += body_.forward * Time.fixedDeltaTime * speed;
        }
    }

    public void OnDrawGizmos()
    {
        
        Gizmos.DrawSphere(body_.position,0.5f);
    }

    private void DestroyProcess(Transform colliderT)
    {
        if (isDestruction != true)
        {
            if (colliderT.gameObject.TryGetComponent<Health>(out Health health))
            {
                health.GetDamage(damage,body_);
            }

            if (inCollisionAddForce)
            {
                if (colliderT.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    float smoothness = 1000f;
                    Vector3 direction = body_.forward * collisionForce * smoothness;

                    StartCoroutine(ForceAddTimer(rb,direction,body_.position));
                }
            }

            Destruction(colliderT.transform);
        }
    }

    private IEnumerator ForceAddTimer(Rigidbody targetRb,Vector3 direction,Vector3 forcePosition)
    {
        yield return null;
        yield return null;
        yield return null;

        targetRb.AddForceAtPosition(direction,forcePosition);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.isTrigger)
            return;

        DestroyProcess(other.transform);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        DestroyProcess(other.transform);
    }

    virtual internal void Destruction(Transform collisionObj)
    {
        LevelSaveData.mainLevelSaveData.RemoveFromSaveData(this);
        if (!isDestruction)
        {
            isDestruction = true;
            colliders.SetActive(false);

            if (rigidbody_ != null)
            {
                rigidbody_.velocity = Vector3.zero;
                rigidbody_.useGravity = false;
            }

            if (effects != null)
                effects.Destruction(destructionTime, collisionObj);
            
            if(onDestroyEvent != null)
                onDestroyEvent.Invoke();
            
            Destroy(gameObject, destructionTime);
        }
    }

    virtual public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    virtual public void SetStartImpulsePower(float power)
    {
        startRBPower = power/1.5f;
    }
    
    private IEnumerator AutoDestruction(float time)
    {

        while (true)
        {
            if(time <= currentTime)
                break;

            yield return null;
        }
        
        Destruction(null);
    }

    public void SubDestroyEvent(Action action)
    {
        if (action != null)
            onDestroyEvent += action;
    }
    
    public void UnSubDestroyEvent(Action action)
    {
        if (action != null)
            onDestroyEvent -= action;
    }

    
}
