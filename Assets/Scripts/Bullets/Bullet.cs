using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Bullet : MonoBehaviour
{
    [SerializeField] internal BulletEffects effects;
    [SerializeField] internal Rigidbody rigidbody_;
    [SerializeField] internal Transform body_;
    [SerializeField] private GameObject colliders;

    [Space]
    private float destructionTime = 2;
    [SerializeField] private float lifeTime;

    internal bool isDestruction = false;
    [SerializeField] private bool isFly = true;

    [SerializeField] internal float damage;

    [Space]
    [SerializeField] private bool inCollisionAddForce = true;
    [SerializeField] private float collisionForce = 2f;
    protected float startRBPower = 1f;
    [Range(0, 1000)] [SerializeField] private float speed;

    protected void Start()
    {
        //Подготовка всего и вся
        body_ = transform;

        if(rigidbody_ != null) 
            rigidbody_.velocity += body_.forward * startRBPower * speed;

        StartCoroutine(AutoDestruction(lifeTime)) ;

    }

    //Полёт пули (если разрешён)
    protected void Update()
    {
      
        if (!isDestruction && isFly)
        {
            body_.position += body_.forward * Time.deltaTime * speed;
        }
    }

    //Сталкивание пули с коллайдером

    private void DestroyProcess(Transform colliderT)
    {
        if (isDestruction != true)
        {
            //Если есть здоровье, нанести ему урон
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

    //Функция уничтожения
    virtual internal void Destruction(Transform collisionObj)
    {
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

    //Корутин авто-разрушения
    private IEnumerator AutoDestruction(float time)
    {
        yield return new WaitForSeconds(time);
        Destruction(null);
    }

}
