using System;
using System.Collections;
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

    [SerializeField] internal float damage;

    [Space]
    [SerializeField] private bool inCollisionAddForce = true;
    [SerializeField] private float collisionForce = 2f; 
    protected float startRBPower = 1f;
    [Range(0, 1000)] [SerializeField] private float speed;
    [SerializeField] private int bulletId;
    
    private event Action onDestroyEvent;
    
    public int BulletId => bulletId;

    public void Load(float currentTime)
    {
        this.currentTime = currentTime;
    }
    
    protected virtual void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
        
        body_ = transform;

        if(rigidbody_ != null) 
            rigidbody_.velocity += body_.forward * startRBPower * speed;

        StartCoroutine(AutoDestruction(lifeTime)) ;

    }

    protected void Update()
    {
        currentTime += Time.deltaTime;
        
        if (!isDestruction && isFly)
        {
            body_.position += body_.forward * Time.deltaTime * speed;
        }
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

                    rb.AddForceAtPosition(direction, body_.position);
                }
            }

            Destruction(colliderT.transform);
        }
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

    virtual public void SetStartImpulsPower(float power)
    {
        startRBPower = power;
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
