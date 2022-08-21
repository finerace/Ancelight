using System.Collections;
using UnityEngine;

public class MainSystemFunctions : MonoBehaviour
{
    private void Awake()
    {
        StartRamCleaner();
    }

    public static void CloseGame()
    {
        Application.Quit();
    }

    private void StartRamCleaner()
    {
        const float cleanCooldown = 30f;

        var waitCooldown = new WaitForSecondsRealtime(cleanCooldown);

        StartCoroutine(SystemRamCleaner());        
        
        IEnumerator SystemRamCleaner()
        {
            while (true)
            {
                Resources.UnloadUnusedAssets();
                
                yield return waitCooldown;
            }
            
            // ReSharper disable once IteratorNeverReturns
        }
        
    }

}
