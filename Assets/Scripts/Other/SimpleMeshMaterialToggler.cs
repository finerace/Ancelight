using System;
using UnityEngine;

public class SimpleMeshMaterialToggler : MonoBehaviour
{
    [SerializeField] private MeshRenderer targetMesh;

    [SerializeField] private Material offToggleMaterial;
    [SerializeField] private Material onToggleMaterial;
    [SerializeField] private bool currentState;

    private void Start()
    {
        SetMaterial();
    }

    public void SwitchToggle()
    {
        currentState = !currentState;
        
        SetMaterial();
    }
    
    private void SetMaterial()
    {
        if (!currentState)
            targetMesh.material = offToggleMaterial;
        else
            targetMesh.material = onToggleMaterial;
    }
    
}
