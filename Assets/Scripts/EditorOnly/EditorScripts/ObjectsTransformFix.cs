using UnityEditor;
using UnityEngine;

public class ObjectsTransformFix : MonoBehaviour
{

    [SerializeField]
    private float randomRotationPower = 5;

    [SerializeField] private float posLimit = 250;
    
    // [SerializeField]
    // private ObjectsFixList currentFixObj; 

    private void Awake()
    {
        Destroy(this);
    }
    
#if UNITY_EDITOR
    [ContextMenu("Fix object Rotation")]
    public void FixRotation()
    {
        Undo.RecordObject(transform,"Fix object rotation");
        
        var targetObjectT = transform;

        var xRot = Random.Range(-randomRotationPower, randomRotationPower);
        var zRot = Random.Range(-randomRotationPower, randomRotationPower);
        var yRot = Random.Range(0, 360);
        
        targetObjectT.rotation = Quaternion.identity * Quaternion.Euler(xRot,yRot,zRot);
    }
    
    [ContextMenu("Fix object Position")]
    public void FixPosition()
    {
        Undo.RecordObject(transform,"Fix object position");
        
        var targetObjectT = transform;
        var targetPos = targetObjectT.position;
        
        bool IsCoordExceedsLimit(float coord, float limit)
        {
            return coord > limit || coord < -limit;
        }

        var isLimitExceeds = IsCoordExceedsLimit(targetPos.x, posLimit) || 
                             IsCoordExceedsLimit(targetPos.y, posLimit) ||
                             IsCoordExceedsLimit(targetPos.z, posLimit);

        if (isLimitExceeds)
        {
            Undo.RecordObject(transform,"Fix object position");
            
            targetObjectT.position = Vector3.zero;
        }

    }
    
    [ContextMenu("Delete object 50% chance")]
    public void MaybeDeleteObject()
    {
        var rand = Random.Range(0, 2);

        if (rand == 0)
        {
            Undo.RecordObject(gameObject,"Rename to destroy obj");

            gameObject.name = "ToDestroy";
        }

    }

    
#endif

    // private enum ObjectsFixList
    // {
    //     Flower,
    //     Tree
    // }
    
}
