using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(MenuSystem))]
public class MenuSystemEditor : Editor
{
    private MenuSystem menuSystem;
    private bool isMenuPresetCreated = false;

    public void OnEnable()
    {
        menuSystem = (MenuSystem)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        isMenuPresetCreated = menuSystem.IsMenusPresetInitialize();

        if (!isMenuPresetCreated){

            if(GUILayout.Button("Initialize Menus Preset"))
            {
                menuSystem.InitializeMenusPreset();
            }
        }
    }

}
