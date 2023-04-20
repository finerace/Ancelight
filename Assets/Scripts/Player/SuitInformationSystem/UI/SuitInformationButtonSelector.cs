using System;
using TMPro;
using UnityEngine;

public class SuitInformationButtonSelector : MonoBehaviour
{
    [SerializeField] private SuitInformationSetUI uiSetter;
    [SerializeField] private ButtonMainService buttonMainService;
    [SerializeField] private TMP_Text informationNameLabel;
    
    public void SetInformation(int informationId, string informationName, SuitInformationSetUI suitInformationSetUI)
    {
        uiSetter = suitInformationSetUI;
        void OpenInformation()
        {
            uiSetter.OpenInformation(informationId);
        }
        
        buttonMainService.onClickAction.AddListener(OpenInformation);

        informationNameLabel.text = informationName;
    }
}
