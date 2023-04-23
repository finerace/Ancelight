using UnityEngine;

[CreateAssetMenu(fileName = "New Suit Information Data", menuName = "SuitInformationData", order = 51)]
public class SuitInformationData : ScriptableObject
{
    [SerializeField] private int informationNameTextId;
    [SerializeField] private int informationDescTextId;

    [SerializeField] private Sprite informationImage1;
    [SerializeField] private Sprite informationImage2;
    [SerializeField] private Sprite informationImage3;

    [SerializeField] private int informationId;
    
    public int InformationNameTextId => informationNameTextId;

    public int InformationDescTextId => informationDescTextId;

    public Sprite InformationImage1 => informationImage1;

    public Sprite InformationImage2 => informationImage2;

    public Sprite InformationImage3 => informationImage3;

    public int InformationId => informationId;
}
