using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJumper : MonoBehaviour
{
    [SerializeField] private Vector3 forceDirection;
    [SerializeField] private float force;

    private void Start()
    {
        
        if(forceDirection == Vector3.zero)
            forceDirection = transform.forward;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.TryGetComponent<Rigidbody>(out Rigidbody targetRB))
        {
            float smoothness = 50f;

            targetRB.velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z);

            targetRB.AddForce(forceDirection.normalized * force * smoothness, ForceMode.Acceleration);
        }

    }

}
