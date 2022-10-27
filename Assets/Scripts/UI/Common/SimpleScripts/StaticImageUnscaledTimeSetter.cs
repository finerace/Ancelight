using UnityEngine;
using UnityEngine.UI;

public class StaticImageUnscaledTimeSetter : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private string unscaledTimeReferenceName = "_unscaledTime";
    private bool oneUpdateBlock = false;
    
    private void Update()
    {
               
        foreach (var localImage in images)
        {
            // if (oneUpdateBlock)
            // {
            //     oneUpdateBlock = false;
            //     return;
            // }

            var newMat = new Material(localImage.material);
            
            newMat.SetFloat(unscaledTimeReferenceName,Time.unscaledTime);
            
            localImage.material = newMat;
            
            oneUpdateBlock = true;
        }
        
    }
    
    
}
