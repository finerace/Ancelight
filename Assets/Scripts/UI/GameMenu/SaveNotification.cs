using UnityEngine;

public class SaveNotification : MonoBehaviour
{
    [SerializeField] private SmoothVanishUI smoothVanishUI;
    [SerializeField] private float activeTime = 1.5f;
    private float timer;
    
    private void Awake()
    {
        smoothVanishUI.SetVanish(true);
        
        FindObjectOfType<LevelSaveLoadSystem>().OnSaveEvent += ActivateNotification;
    }

    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
            smoothVanishUI.SetVanish(true);
            
    }

    private void ActivateNotification()
    {
        smoothVanishUI.SetVanish(false);
        timer = activeTime;
    }

}
