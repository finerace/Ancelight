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
    private float timer;
    
    private void Start()
    {
        smoothVanishUI.SetVanish(true);
        label.text = String.Empty;
    }

    public void SetInformation(int textId)
    {
        label.text = CurrentLanguageData.GetText(textId);
        
        smoothVanishUI.SetVanish(false);
        timer = existTime;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            smoothVanishUI.SetVanish(true);
    }
}
