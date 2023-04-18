using System;
using TMPro;
using UnityEngine;

public class SuitInformationSetUI : MonoBehaviour
{
    [SerializeField] private SuitInformationDataBase dataBase;
    
    [Space]
    
    [SerializeField] private TMP_Text informationName;
    [SerializeField] private TMP_Text informationDesc;

    [SerializeField] private GameObject informationImageObject1;
    [SerializeField] private SpriteRenderer informationSprite1;
    
    [SerializeField] private GameObject informationImageObject2;
    [SerializeField] private SpriteRenderer informationSprite2;

    [SerializeField] private GameObject informationImageObject3;
    [SerializeField] private SpriteRenderer informationSprite3;

    private void Awake()
    {
        dataBase = FindObjectOfType<SuitInformationDataBase>();
    }

    public void OpenInformation(int informationId)
    {
        var informationData = dataBase.GetInformationData(informationId);

        informationName.text = informationData.InformationName;
        informationDesc.text = informationData.InformationDesc;

        if (informationData.InformationImage1 != null)
        {
            informationImageObject1.SetActive(true);
            informationSprite1.sprite = informationData.InformationImage1;
        }
        else
            informationImageObject1.SetActive(false);
        
        if (informationData.InformationImage2 != null)
        {
            informationImageObject2.SetActive(true);
            informationSprite2.sprite = informationData.InformationImage2;
        }
        else
            informationImageObject2.SetActive(false);

        if (informationData.InformationImage3 != null)
        {
            informationImageObject3.SetActive(true);
            informationSprite3.sprite = informationData.InformationImage3;
        }
        else
            informationImageObject3.SetActive(false);
    }
    
}
