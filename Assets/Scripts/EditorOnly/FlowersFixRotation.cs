using UnityEditor;
using UnityEngine;

public class FlowersFixRotation : MonoBehaviour
{

    [SerializeField]
    private float randomRotationPower = 5;
        
    private void Awake()
    {
        Destroy(this);
    }

#if UNITY_EDITOR
    [ContextMenu("Fix Flower Rotation")]
    public void FixRotation()
    {
        Undo.RecordObject(transform,"Flower Fix Rotation");
        
        var flowerT = transform;

        var xRot = Random.Range(-randomRotationPower, randomRotationPower);
        var zRot = Random.Range(-randomRotationPower, randomRotationPower);
        
        flowerT.rotation = Quaternion.identity * Quaternion.Euler(xRot,flowerT.rotation.eulerAngles.y,zRot);
    }
#endif
    
}
