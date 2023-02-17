using System.Collections.Generic;
using UnityEngine;

public class LevelTranslateScenarioCorrectPhysicService : MonoBehaviour
{

    [SerializeField] private LevelTransformAnimationSystem objectAnimationSystem;
    [SerializeField] private int neededScenarioId;
    private List<Rigidbody> connectedRigidbodies = new List<Rigidbody>();
    private Vector3 currentObjectVelocity;

    public Vector3 CurrentObjectVelocity => currentObjectVelocity;

    private void Start()
    {
        objectAnimationSystem.TransformScenarios[neededScenarioId]
            .ObjectTranslatePosEvent.AddListener(SetAllConnectedRigidbodies);
    }

    private void SetAllConnectedRigidbodies(Vector3 velocity)
    {
        currentObjectVelocity = velocity;

        for (var i = 0; i < connectedRigidbodies.Count; i++)
        {
            var rb = connectedRigidbodies[i];

            if (rb == null)
            {
                connectedRigidbodies.Remove(rb);
                
                continue;
            }

            rb.gameObject.transform.position += velocity;
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        Rigidbody rb;

        collider.gameObject.TryGetComponent(out rb);

        if (rb == null)
            return;

        foreach (var connectedRigidbody in connectedRigidbodies)
        {
            if(connectedRigidbody.Equals(rb))
                return;
        }
        
        connectedRigidbodies.Add(rb);
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Rigidbody rb))
        {
            if(!connectedRigidbodies.Contains(rb))
                return;
            
            connectedRigidbodies.Remove(rb);
            
            const float velocitySmoothness = 100;
            rb.velocity += currentObjectVelocity * velocitySmoothness;
        }
    }
    
}
