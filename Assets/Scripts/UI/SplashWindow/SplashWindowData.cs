using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SplashWindowData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI splashWindowMainLabel;
    [SerializeField] private ButtonMainService yesButtonService;
    [SerializeField] private ButtonMainService noButtonService;

    public void SetNewData(string newText, UnityAction yesButtonAction, UnityAction noButtonAction)
    {
        splashWindowMainLabel.text = newText;
        
        yesButtonService.onClickAction.RemoveAllListeners();
        yesButtonService.onClickAction.AddListener(yesButtonAction);
        yesButtonService.onClickAction.AddListener(DestroySplashWindow);
        
        noButtonService.onClickAction.RemoveAllListeners();
        
        if(noButtonAction != null)
            noButtonService.onClickAction.AddListener(noButtonAction);;

        noButtonService.onClickAction.AddListener(DestroySplashWindow);
    }

    private void DestroySplashWindow()
    {
        DestroyImmediate(gameObject);
    }
    
}
