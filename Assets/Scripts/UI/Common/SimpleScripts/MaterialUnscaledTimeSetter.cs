using UnityEngine;
using UnityEngine.UI;

public class MaterialUnscaledTimeSetter : MonoBehaviour
{
    
    [SerializeField] private Material[] materials = new Material[0];
    [SerializeField] private string unscaledTimeReferenceName = "_unscaledTime";
    
    private void Update()
    {
        if(materials.Length == 0)
            return;
        
        var currentUnscaledTime = materials[0].GetFloat(unscaledTimeReferenceName);

        foreach (var localMaterial in materials)
        {
            var unscaledTime = currentUnscaledTime + Time.unscaledDeltaTime;
            
            localMaterial.SetFloat(unscaledTimeReferenceName, unscaledTime);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        foreach (var localMaterial in materials)
        {
            localMaterial.SetFloat(unscaledTimeReferenceName, 0);
        }
    }
}
