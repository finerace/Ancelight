using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplousionBullet : Bullet
{
    [SerializeField] private float explosionForce = 1f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private CollisionDetectBoom detectBoom;
    [SerializeField] private LayerMask wallsLayerMask;
    [SerializeField] private LayerMask forceLayerMask;
    [SerializeField] private LayerMask damageLayerMask;
    //?????????? ????

    /*private new void Start()
    {
        base.Start();
    }*/

    internal override void Destruction(Transform collisionObj)
    {
        if (!isDestruction)
        {
            StartCoroutine(StartExplosion());
            
            IEnumerator StartExplosion()
            {
                Explosions.Explosion(body_.position, 0, explosionRadius, damage
                    , wallsLayerMask, damageLayerMask, forceLayerMask);

                yield return null;

                Explosions.Explosion(body_.position, explosionForce, explosionRadius, 0
                    , wallsLayerMask, damageLayerMask, forceLayerMask,1);
            }
        }

        

        base.Destruction(collisionObj);
    }

    //??????? ??????? ?????? ? ?????????
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(body_.position,explosionRadius);
    }

    //??? ?????? ??? ???????????? ? ???????????
    private void OnCollisionEnter(Collision collision)
    {
        switch(detectBoom)
        {
            //?????? ?????
            case CollisionDetectBoom.Always:
                Destruction(collision.transform);
                break;

            //????? ???? ???? ????????
            case CollisionDetectBoom.IfHealth:
                if(collision.gameObject.GetComponent<Health>())
                {
                    Destruction(collision.transform);
                }
                break;
        }
            
            
    }


    private enum CollisionDetectBoom
    {
        Always,
        IfHealth,
        Never
    }

}

public static class Explosions
{

    public static void Explosion(Vector3 explousionPos, float explosionForce, float explosionRadius, float damage
        ,LayerMask wallsLayerMask, LayerMask damageLayerMask, LayerMask forceLayerMask,float upModify = 0.25f)
    {
        float explosionForceSmoothness = 100f;
        float resultExplosionForce = explosionForce * explosionForceSmoothness;

        //??????????? ??????????? ? ???? ?????????
        Collider[] explousionColliders = Physics.OverlapSphere(explousionPos, explosionRadius);

        foreach (var collider in explousionColliders)
        {
            int colliderLayer = collider.gameObject.layer;

            bool forceAllow = forceLayerMask.IsLayerInMask(colliderLayer);
            bool damageAllow = damageLayerMask.IsLayerInMask(colliderLayer);

            if (!forceAllow && !damageAllow)
                continue;

            Rigidbody bodyRb;
            Health health;

            Vector3 trueBulletPos = explousionPos;

            RaycastHit hitInfo;
            Vector3 direction = collider.transform.position - trueBulletPos;
            Ray ray = new Ray(trueBulletPos, direction);

            float maxDistance = Vector3.Distance(explousionPos, collider.transform.position);

            Physics.Raycast(ray, out hitInfo, maxDistance, wallsLayerMask);
            bool raycastTest;

            if (hitInfo.transform != null)
            {
                int hitObjHash = hitInfo.transform.gameObject.GetHashCode();

                int colliderObjHash = collider.gameObject.GetHashCode();

                raycastTest = (hitObjHash == colliderObjHash);
            }
            else raycastTest = true;


            if (raycastTest)
            {
                if (damageAllow)
                    if (collider.gameObject.TryGetComponent<Health>(out health))
                    {
                        //?????? ?????
                        Vector3 healthPos = health.transform.position;

                        float distance = Vector3.Distance(explousionPos, healthPos);
                        float resultDamage = damage * (1 - (distance / explosionRadius));

                        health.GetDamage(resultDamage);
                    }
                
                if (forceAllow)
                    if (collider.gameObject.TryGetComponent(out bodyRb))
                    {
                        bodyRb.AddExplosionForce(resultExplosionForce, explousionPos, explosionRadius, upModify);
                    }
            }
        }
    }

    public static void DirectedExplosion(Vector3 explousionPos, Vector3 explousionDirection,
        float minDot, float explosionForce, float explosionRadius,LayerMask playerShootingLayerMask,float damage = 0, bool DotScale = false)
    {
        float explousionForceSmoothness = 100f;
        float resultExplousionForce = explosionForce * explousionForceSmoothness;
        float upModify = 0.25f;

        //??????????? ??????????? ? ???? ?????????
        Collider[] explousionColliders = Physics.OverlapSphere(explousionPos, explosionRadius);

        foreach (var collider in explousionColliders)
        {
            Rigidbody body;
            Health health;

            //?????? ???????? ?? ??????? ????
            Vector3 trueBulletPos = explousionPos;

            RaycastHit hitInfo;
            Vector3 direction = collider.transform.position - trueBulletPos;
            Ray ray = new Ray(trueBulletPos, direction);

            float maxDistance = Vector3.Distance(explousionPos, collider.transform.position);

            var layerMask =
                playerShootingLayerMask;
            
            Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
            bool raycastTest;

            if (hitInfo.transform != null)
            {
                int hitObjHash = hitInfo.transform.gameObject.GetHashCode();

                int colliderObjHash = collider.gameObject.GetHashCode();

                raycastTest = (hitObjHash == colliderObjHash);
            }
            else raycastTest = true;
            //????? ???????? ?? ?????

            //???? ???????? ???????? ????..
            if (raycastTest)
            {
                //?????? ???????? ?? ???????????? ??????????? ??????
                bool explousionDirectedTest = false;

                Vector3 localObjPos = (collider.transform.position - explousionPos).normalized;
                float dot = Vector3.Dot(explousionDirection.normalized, localObjPos);

                if (dot >= minDot)
                    explousionDirectedTest = true;

                //????? ???????? ?? ???????????? ??????????? ??????

                if (explousionDirectedTest)
                {

                    //??? ??????? ????? ???????? ????
                    if (collider.gameObject.TryGetComponent<Rigidbody>(out body))
                    {
                        if (DotScale)
                        {
                            if (dot > 0) resultExplousionForce *= dot;
                            else resultExplousionForce = 0;
                        }

                        body.AddExplosionForce(resultExplousionForce, explousionPos, explosionRadius, upModify);
                    }

                    //??? ??????? ???????? ??????? ????
                    if (collider.gameObject.TryGetComponent<Health>(out health))
                    {
                        //?????? ?????
                        Vector3 healthPos = health.transform.position;

                        float distance = Vector3.Distance(explousionPos, healthPos);
                        float resultDamage = damage * (1 - (distance / explosionRadius));

                        if (DotScale)
                        {
                            if (dot > 0) resultDamage *= dot;
                            else resultDamage = 0;
                        }

                        health.GetDamage(resultDamage);
                    }
                }
            }
        }
    }

}

