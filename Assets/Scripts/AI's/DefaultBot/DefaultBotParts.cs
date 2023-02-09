using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBotParts : MonoBehaviour
{
    [SerializeField] private Rigidbody[] botPartsRb = new Rigidbody[0];
    [SerializeField] private Joint[] botPartsJoints = new Joint[0];
    [SerializeField] private Collider[] botPartsColliders = new Collider[0];
    [SerializeField] private HealthExporter[] botHealthExporters = new HealthExporter[0];
    [SerializeField] private float frozeTime = 5f;
    protected bool isDestruct;
    public bool IsDestruct => isDestruct;
    
    public virtual void DestructParts(Rigidbody parentRB)
    {
        transform.parent = null;
        isDestruct = true;
        
        foreach (var item in botPartsJoints)
        {
            Destroy(item);
        }

        foreach (var part in botPartsRb)
        {
            part.transform.parent = null;
            //part.gameObject.layer = 10;

            if (part.isKinematic)
            {
                part.isKinematic = false;
                part.velocity += parentRB.velocity;
            }

            if (!part.useGravity)
                part.useGravity = true;

            if (part.constraints != RigidbodyConstraints.None)
                part.constraints = RigidbodyConstraints.None;
        }

        foreach (var item in botHealthExporters)
        {
            Destroy(item);
        }

        foreach (var collider in botPartsColliders)
        {
            Destroy(collider.gameObject,SettingsSetSystem.enemyPartsDestroyTime);
        }

        StartCoroutine(RigidbodyRemover());

        IEnumerator RigidbodyRemover()
        {
            yield return new WaitForEndOfFrame();

            foreach (var part in botPartsRb)
            {
                if(part != null)
                    part.gameObject.layer = 10;
            }

            yield return new WaitForSeconds(frozeTime);

            while (true)
            {
                bool isFindActiveRBs = false;

                for (int i = 0; i < botPartsRb.Length; i++)
                {
                    if(botPartsRb[i] != null)
                    {
                        isFindActiveRBs = true;

                        if (botPartsRb[i].velocity.magnitude + botPartsRb[i].angularVelocity.magnitude <= 0.25f)
                        {
                            Destroy(botPartsRb[i]);
                            //Destroy(botPartsColliders[i]);
                        }
                    }

                }

                if (!isFindActiveRBs)
                {
                    Destroy(gameObject);
                    break;
                }

                yield return new WaitForSeconds(1f);
            }

        }

    }

}
