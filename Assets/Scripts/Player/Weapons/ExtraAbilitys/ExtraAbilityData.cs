using UnityEngine;

[CreateAssetMenu(fileName = "New ExtraAbilityData", menuName = "AbilityData", order = 51)]

public class ExtraAbilityData : ScriptableObject
{
    [SerializeField] private string abilityName = "Default Name";
    [SerializeField] private Sprite icon;
    [SerializeField] private float abilityDelayTime = 1f;
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private int abilityID = 0;
    [SerializeField] private GameObject abilityPrefab;

    public float DelayTime { get {return abilityDelayTime; }}
    public float AttackTime { get {return attackTime; }}
    public string Name {get {return abilityName;}}
    public Sprite Icon {get {return icon;}}
    public int Id {get {return abilityID;}}
    public GameObject AbilityPrefab {get {return abilityPrefab;}}

}
