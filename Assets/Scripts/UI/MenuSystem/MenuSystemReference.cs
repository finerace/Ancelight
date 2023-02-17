using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuSystemReference : MonoBehaviour
{
    private MenuSystem menuSystem;
    public MenuSystem MenuSystem => menuSystem;

    public void SetMenuSystemReference(MenuSystem menuSystemRef)
    {
        menuSystem = menuSystemRef;
    }

    public void MenuSystemBack()
    {
        menuSystem.Back();
    }

    public void MenuSystemOpenMenu(string menuId)
    {
        menuSystem.OpenLocalMenu(menuId);
    }
    
    public void MenuSystemCallSplashWindow(string splashWindowText, UnityAction yesAction, UnityAction noAction = null)
    {
        menuSystem.CallSplashWindow(splashWindowText,yesAction,noAction);
    }
    
}
