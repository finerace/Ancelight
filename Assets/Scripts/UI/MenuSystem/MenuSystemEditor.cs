#if UNITY_EDITOR
using UnityEngine;
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
#endif