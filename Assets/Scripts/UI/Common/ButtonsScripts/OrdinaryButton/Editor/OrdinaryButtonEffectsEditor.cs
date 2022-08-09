using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrdinaryButtonEffects))]
public class OrdinaryButtonEffectsEditor : Editor
{

    private OrdinaryButtonEffects buttonEffects;

    public void OnEnable()
    {
        buttonEffects = (OrdinaryButtonEffects)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Set Button Color"))
        {
            buttonEffects.SetButtonColor();
        }

    }

}
