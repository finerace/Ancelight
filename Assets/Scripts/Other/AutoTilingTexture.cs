using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AutoTilingTexture : MonoBehaviour
{
    [SerializeField] Vector2 tiling;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private MeshRenderer mesh;

    private void Start()
    {
        UpdateTiling();
    }

    private void UpdateTiling()
    {
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();

        if (targetMaterial == null)
        {
            targetMaterial = new Material(mesh.material);
            mesh.material = targetMaterial;
        }

        float xTile = transform.localScale.x + transform.localScale.y;
        float zTile = transform.localScale.z + transform.localScale.y;

        targetMaterial.SetTextureScale("_BaseMap", new Vector2(xTile, zTile));
        mesh.material = targetMaterial;
        tiling = targetMaterial.GetTextureScale("_BaseMap");
    }

}
