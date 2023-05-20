using System;
using UnityEngine;

public class SimpleMenuSystem : MonoBehaviour
{

    [SerializeField] private MenuData[] menuDatas = new MenuData[2];
    private MenuData currentMenuData;

    private void Start()
    {
        AwakeMenuFix();

        void AwakeMenuFix()
        {
            currentMenuData = menuDatas[0];
        
            CloseAllMenus();
        
            currentMenuData.menu.SetActive(true);
        }
    }

    public void OpenMenu(string menuID)
    {
        foreach (var menuData in menuDatas)
        {
            if (menuData.menuID == menuID)
            {
                OpenNewMenu(menuData);
            }
            
            void OpenNewMenu(MenuData newMenuData)
            {
                currentMenuData.menu.SetActive(false);

                currentMenuData = newMenuData;
                currentMenuData.menu.SetActive(true);
            }
        }
    }

    private void CloseAllMenus()
    {
        foreach (var menuData in menuDatas)
        {
            menuData.menu.gameObject.SetActive(false);
        }
    }
}
