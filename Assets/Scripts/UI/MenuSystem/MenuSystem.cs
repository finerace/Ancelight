using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private ParentMenuData startMenuData;

    [Space]
    
    [SerializeField] private Animator menusChangeAnimation;
    [SerializeField] private float menusChangeAnimationTime = 0.5f;

    private string menusPath;
    private readonly List<MenuData> menusDataPath = new List<MenuData>();
    
    [Space]
    private MenuData currentMenuData;

    private bool currentMenuIsParent = true;

    private Camera mainCamera;
    private PlayerMainService player;
    private bool isPlayerExist;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        InitMenusData();
    }

    private void Start()
    {
        OpenStartMenu();
        
        FindPlayer();
        
        void FindPlayer()
        {
            try
            {
                player = GameObject.Find("Player").GetComponent<PlayerMainService>();
                isPlayerExist = true;
            }
            catch (Exception)
            {
                print("Menu System not found Player!");
                isPlayerExist = false;
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
        }
    }

    private void InitMenusData()
    {
        InitMenu(startMenuData);
        
        void InitMenu(MenuData menuData)
        {
            var spawnedMenu = Instantiate(menuData.menuPrefab);
            spawnedMenu.GetComponent<Canvas>().worldCamera = mainCamera;
            SetMenuSystemRef();
            
            menuData.menu = spawnedMenu;
            
            
            if(!IsMenuDataParent(menuData))
                return;

            var parentMenuData = (ParentMenuData)menuData;
            
            InitChildMenusData(parentMenuData);
            

            void InitChildMenusData(ParentMenuData parentMenuData)
            {
                foreach (var menuChildData in parentMenuData.childsMenusData)
                    InitMenu(menuChildData);

                foreach (var parentChildMenuData in parentMenuData.childsParentsMenusData)
                    InitMenu(parentChildMenuData);
            }

            void SetMenuSystemRef()
            {
                if (spawnedMenu.TryGetComponent(out MenuSystemReference menuSystemReference))
                    menuSystemReference.SetMenuSystemReference(this);
            }
        }

        bool IsMenuDataParent(MenuData menuData)
        {
            return menuData.GetType() == typeof(ParentMenuData);
        }
    }
    
    public void OpenLocalMenu(string menuID)
    {
        if (!currentMenuIsParent)
        {
            print($"Open menu has failed! \n" + $"Current menu {menuID} is not parent!");
            
            return;
        }

        var currentParentMenu = (ParentMenuData)currentMenuData;

        var foundParentMenuData =
            FindLocalParentMenu(currentParentMenu, menuID);

        var foundChildMenuData = 
            FindLocalChildMenu(currentParentMenu, menuID);

        if (foundChildMenuData != null && foundParentMenuData != null)
        {
            throw new InvalidDataException($"Open menu has failed!\n" + $"Menu ID {menuID} is repeated!");
        }
        
        if(foundChildMenuData == null && foundParentMenuData == null)
        {
            throw new InvalidDataException
                ($"Open menu has failed! \n" + $"The given ID {menuID} is not found!");
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
            OpenLocalMenu("PauseMenu");

            return;
        }

        var backMenu = menusDataPath[menusDataPath.Count - 2];

        menusDataPath.Remove(currentMenuData);
        
        CloseAllMenus();
        
        currentMenuData = backMenu;
        currentMenuData.menu.SetActive(true);
        currentMenuIsParent = true;

        UpdateMenuPath();
        SetMenuSpecialSettings(currentMenuData);
        
        PlayMenuChangeAnimation();
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

        menusPath += $"/{menuData.menuID}";
        menusDataPath.Add(menuData);

        SetMenuSpecialSettings(menuData);

        UpdateMenuPath();
        
        PlayMenuChangeAnimation();
    }

    private ParentMenuData FindLocalParentMenu(ParentMenuData parentMenu, string toFindMenuID)
    {
        return parentMenu.childsParentsMenusData.FirstOrDefault(item => item.menuID == toFindMenuID);
    }

    private MenuData FindLocalChildMenu(ParentMenuData parentMenu,string toFindMenuID)
    {
        return parentMenu.childsMenusData.FirstOrDefault(item => item.menuID == toFindMenuID);
    }

    private void UpdateMenuPath()
    {
        menusPath = "";

        foreach (var item in menusDataPath)
        {
            menusPath += $"{item.menuID}/";
        }
    }

    private void SetMenuSpecialSettings(MenuData menuData)
    {

        SetCursorActive(menuData.isCursorActive);

        SetTimeScaleActive(menuData.isTimeNotActive);

        if (isPlayerExist)
        {
            var playerManageActive =
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
            Time.timeScale = state ? 0 : 1;
        }
        
        void IsPlayerManageActive(bool state)
        {
            if(isPlayerExist)
                player.SetManageActive(state);
        }
    }

    private void CloseAllMenus()
    {
        startMenuData.Disable();
    }

    private void PlayMenuChangeAnimation()
    {
        menusChangeAnimation.Play("MenuChange");
    }

}

[System.Serializable]
public class MenuData
{
    public string menuID;
    public GameObject menuPrefab;
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
        foreach (var item in childsMenusData.Where(item => item.menu.activeSelf))
        {
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
