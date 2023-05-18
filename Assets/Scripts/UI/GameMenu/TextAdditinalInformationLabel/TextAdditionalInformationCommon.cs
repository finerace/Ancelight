using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAdditionalInformationCommon : MonoBehaviour
{
    [SerializeField] private SmoothVanishUI smoothVanishUI;
    [SerializeField] private float existTime;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        smoothVanishUI.SetVanish(true);
        label.text = String.Empty;
    }

    public void SetInformation(int textId)
    {
        label.text = CurrentLanguageData.GetText(textId);

        StartCoroutine(WaitExist());
    }

    private IEnumerator WaitExist()
    {
        smoothVanishUI.SetVanish(false);
        
        yield return new WaitForSeconds(existTime);
        
        smoothVanishUI.SetVanish(true);
    }
}
