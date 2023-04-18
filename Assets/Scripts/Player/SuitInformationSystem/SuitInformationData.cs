using UnityEngine;

[CreateAssetMenu(fileName = "New Suit Information Data", menuName = "SuitInformationData", order = 51)]
public class SuitInformationData : ScriptableObject
{
    [SerializeField] private string informationName;
    [SerializeField] private string informationDesc;

    [SerializeField] private Sprite informationImage1;
    [SerializeField] private Sprite informationImage2;
    [SerializeField] private Sprite informationImage3;

    [SerializeField] private int informationId;
    
    public string InformationName => informationName;

    public string InformationDesc => informationDesc;

    public Sprite InformationImage1 => informationImage1;

    public Sprite InformationImage2 => informationImage2;

    public Sprite InformationImage3 => informationImage3;

    public int InformationId => informationId;
}
