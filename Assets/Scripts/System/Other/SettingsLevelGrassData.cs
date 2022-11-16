using UnityEngine;

public class SettingsLevelGrassData : MonoBehaviour
{
    [SerializeField] private Material[] levelUsedGrassMaterials;
    [SerializeField] private MeshRenderer[] levelUsedGrassMeshes;

    public Material[] LevelUsedGrassMaterials => levelUsedGrassMaterials;
    public MeshRenderer[] LevelUsedGrassMeshes => levelUsedGrassMeshes;
}
