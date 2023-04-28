using UnityEngine;

public class CutsceneModeMenuSwitchService : MonoBehaviour
{

    [SerializeField] private GameObject[] notActiveInCutsceneModeUiElements;
    [SerializeField] private GameObject[] activeInCutsceneModeUiElements;
    
    private bool isGameMenuInCutsceneMode;
    
    private void Awake()
    {
        SetCutsceneMode(false);
    }

    public void SetCutsceneMode(bool isActive)
    {
        foreach (var uiElement in notActiveInCutsceneModeUiElements)
        {
            uiElement.SetActive(!isActive);
        }
        
        foreach (var uiElement in activeInCutsceneModeUiElements)
        {
            uiElement.SetActive(isActive);
        }
    }

}
