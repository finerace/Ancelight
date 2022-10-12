using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshAutoCombiner))]
public class MeshAutoCombinerEditor : Editor
{

    private MeshAutoCombiner meshsAutoCombiner;
    private string autoCombineButtonName;

    public void OnEnable()
    {
        meshsAutoCombiner = (MeshAutoCombiner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(25);
        
        AutoCombineNameSet();
        if (GUILayout.Button(autoCombineButtonName))
        {
            meshsAutoCombiner.StartMeshsAutoCombine();
        }

        void AutoCombineNameSet()
        {
            if (!meshsAutoCombiner.IsMeshsAutoCombineStarted)
                autoCombineButtonName = "Start Combine Meshs";
            else
                autoCombineButtonName = "Meshs combine started!";
        }
        
    }
    
}
