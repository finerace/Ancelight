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
    
    [SerializeField] private string informationName;
    [SerializeField] private string informationDescription;

    [SerializeField] private Color informedObjectMainColor = Color.white;
    
    public Transform InformedObjectT => informedObjectT;
    public float InformedObjectHight => informedObjectHight;
    
    public Health InformedObjectHealth => informedObjectHealth;

    public string InformationName => informationName;
    public string InformationDescription => informationDescription;

    public Color InformedObjectMainColor => informedObjectMainColor;

}
