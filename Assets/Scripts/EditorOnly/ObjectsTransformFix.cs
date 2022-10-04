using UnityEditor;
using UnityEngine;

public class ObjectsTransformFix : MonoBehaviour
{

    [SerializeField]
    private float randomRotationPower = 5;

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
    
#endif

    // private enum ObjectsFixList
    // {
    //     Flower,
    //     Tree
    // }
    
}
