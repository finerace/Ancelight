using UnityEngine;
using UnityEngine.UI;

public class StaticImageUnscaledTimeSetter : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private string unscaledTimeReferenceName = "_unscaledTime";
    private bool oneUpdateBlock = false;
    
    /*private void Update()
    {
               
        foreach (var localImage in images)
        {
             if (oneUpdateBlock)
             {
                 oneUpdateBlock = false;
                return;
            }

            localImage.material = new Material(localImage.material);
            localImage.material.SetFloat(unscaledTimeReferenceName,Time.unscaledTime);
            
            oneUpdateBlock = true;
        }
        
    }*/
    
    
}
