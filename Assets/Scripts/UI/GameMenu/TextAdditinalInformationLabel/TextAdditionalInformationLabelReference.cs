using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAdditionalInformationLabelReference : MonoBehaviour
{

    private TextAdditionalInformationCommon textAdditionalInformationCommon;

    private void Start()
    {
        textAdditionalInformationCommon = FindObjectOfType<TextAdditionalInformationCommon>();
    }

    public void SetInformation(int textId)
    {
        textAdditionalInformationCommon.SetInformation(textId);
    }
    
}
