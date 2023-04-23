using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AdditionalInformationData : MonoBehaviour
{
    [SerializeField] private Transform informedObjectT;
    [SerializeField] private float informedObjectHight = 1f;
    
    [Space]
    
    [SerializeField] private Health informedObjectHealth;
    
    [SerializeField] private int informationNameTextId;
    [SerializeField] private int informationDescriptionTextId;

    [SerializeField] private Color informedObjectMainColor = Color.white;
    
    public Transform InformedObjectT => informedObjectT;
    public float InformedObjectHight => informedObjectHight;
    
    public Health InformedObjectHealth => informedObjectHealth;

    public int InformationNameTextId => informationNameTextId;
    public int InformationDescriptionTextId => informationDescriptionTextId;

    public Color InformedObjectMainColor => informedObjectMainColor;

}
