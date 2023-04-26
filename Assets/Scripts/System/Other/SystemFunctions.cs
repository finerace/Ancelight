using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemFunctions : MonoBehaviour
{
    private void Awake()
    {
        StartRamCleaner();
    }

    public static void CloseGame()
    {
        Application.Quit();
    }

    public static void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
    
    
    private void StartRamCleaner()
    {
        const float cleanCooldown = 10f;

        var waitCooldown = new WaitForSecondsRealtime(cleanCooldown);

        StartCoroutine(SystemRamCleaner());        
        
        IEnumerator SystemRamCleaner()
        {
            while (true)
            {
                //Resources.UnloadUnusedAssets();
                
                yield return waitCooldown;
            }
            
            // ReSharper disable once IteratorNeverReturns
        }
        
    }

}
