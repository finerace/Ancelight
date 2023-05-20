using UnityEngine;

public class SettingsSetReferences : MonoBehaviour
{
    [SerializeField] private Light mainLight;
    
    [Space]

    [SerializeField] private Material[] levelUsedGrassMaterials;
    [SerializeField] private MeshRenderer[] levelUsedGrassMeshes;

    public Material[] LevelUsedGrassMaterials => levelUsedGrassMaterials;
    public MeshRenderer[] LevelUsedGrassMeshes => levelUsedGrassMeshes;
    public Light MainLight => mainLight;
}
