using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private ParentMenuData startMenuData;
    [SerializeField] private bool isMenusPresetCreate = false;

    private string menusPath;
    private List<MenuData> menusDataPath = new List<MenuData>();
    [Space]
    private MenuData currentMenuData;
    private bool currentMenuIsParent = true;
    private PlayerMainService player;


    private void Awake()
    {
        OpenStartMenu();

        player = GameObject.Find("Player").GetComponent<PlayerMainService>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            OpenLocalMenu("SuitManageMenu");


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
            return;
        }
    }

    public void OpenLocalMenu(string menuID)
    {
        if (!currentMenuIsParent)
        {
            throw new Exception
                ($"Open menu has failed! \n" +
                $"Current menu {menuID} is not parent!");
        }


        ParentMenuData currentParentMenu = (ParentMenuData)currentMenuData;

        ParentMenuData foundParentMenuData =
            FindLocalParentMenu(currentParentMenu, menuID);

        MenuData foundChildMenuData = 
            FindLocalChildMenu(currentParentMenu, menuID);


        if (foundChildMenuData != null && foundParentMenuData != null)
        {
            throw new System.Exception($"Open menu has failed!\n" +
                $"Menu ID {menuID} is repeated!");
        }
        if(foundChildMenuData == null && foundParentMenuData == null)
        {
            throw new Exception
                ($"Open menu has failed! \n" +
                $"The given ID {menuID} is not found!");
        }


        if(foundParentMenuData != null)
        {
            currentMenuData = foundParentMenuData;
            currentMenuIsParent = true;

        }
        else if(foundChildMenuData != null)
        {
            currentMenuData = foundChildMenuData;
            currentMenuIsParent = false;
        }

        ActivateMenu(currentMenuData);
    }

    public void Back()
    {
        if (currentMenuData == startMenuData)
        {
            OpenLocalMenu("InGameMenu");

            return;
        }

        MenuData backMenu = menusDataPath[menusDataPath.Count - 2];

        menusDataPath.Remove(currentMenuData);
        
        CloseAllMenus();
        
        currentMenuData = backMenu;
        currentMenuData.menu.SetActive(true);
        currentMenuIsParent = true;

        UpdateMenuPath();
        SetMenuSpecialSettings(currentMenuData);
    }

    private void OpenStartMenu()
    {
        ActivateMenu(startMenuData);
    }

    private void ActivateMenu(MenuData menuData)
    {
        CloseAllMenus();
        currentMenuData = menuData;
        menuData.menu.SetActive(true);

        menusPath += $"/{menuData.MenuID}";
        menusDataPath.Add(menuData);

        SetMenuSpecialSettings(menuData);

        UpdateMenuPath();
    }

    private ParentMenuData FindLocalParentMenu(ParentMenuData parentMenu, string toFindMenuID)
    {

        foreach (var item in parentMenu.childsParentsMenusData)
        {
            if (item.MenuID == toFindMenuID)
                return item;
        }

        return null;
    }

    private MenuData FindLocalChildMenu(ParentMenuData parentMenu,string toFindMenuID)
    {

        foreach (var item in parentMenu.childsMenusData)
        {
            if(item.MenuID == toFindMenuID)
                return item;
        }

        return null;
    }

    private void UpdateMenuPath()
    {
        menusPath = "";

        foreach (var item in menusDataPath)
        {
            menusPath += $"{item.MenuID}/";
        }
    }

    private void SetMenuSpecialSettings(MenuData menuData)
    {

        SetCursorActive(menuData.isCursorActive);

        SetTimeScaleActive(menuData.isTimeNotActive);

        if (player != null)
        {
            bool playerManageActive =
                !(menuData.isTimeNotActive || menuData.isCursorActive);

            IsPlayerManageActive(playerManageActive);
        }

        void SetCursorActive(bool state)
        {
            if (state)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void SetTimeScaleActive(bool state)
        {
            if (state)
            {
                Time.timeScale = 0;
            }
            else
            {

                Time.timeScale = 1;
            }
        }
        
        void IsPlayerManageActive(bool state)
        {
            player.SetManageActive(state);
        }
    }

    private void CloseAllMenus()
    {
        startMenuData.Disable();
    }


    public void InitializeMenusPreset()
    {
        isMenusPresetCreate = true;
    }

    public bool IsMenusPresetInitialize()
    { 
        return isMenusPresetCreate; 
    }
    
}

[System.Serializable]
public class MenuData
{
    public string MenuID;
    public GameObject menu;
    public bool isCursorActive = true;
    public bool isTimeNotActive = true;
}

[System.Serializable]
public class ParentMenuData : MenuData
{
    public List<MenuData> childsMenusData = new List<MenuData>();
    public List<ParentMenuData> childsParentsMenusData = new List<ParentMenuData>();

    public void Disable()
    {
        foreach (var item in childsMenusData)
        {
            if(item.menu.activeSelf)
                item.menu.SetActive(false);
        }

        foreach (var item in childsParentsMenusData)
        {
            item.Disable();

            if (item.menu.activeSelf)
                item.menu.SetActive(false);
        }

        if (menu.activeSelf)
            menu.SetActive(false);
    }

}
