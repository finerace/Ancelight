using System;
using TMPro;
using UnityEngine;

public class StaticTextLanguageAdaptable : MonoBehaviour
{
    [SerializeField] private StaticTextLanguageAdaptableForm[] staticTextAdaptableForms;
    
    private void Start()
    {
        SetStaticText();
        void SetStaticText()
        {
            foreach (var adaptableForm in staticTextAdaptableForms)
            {
                InitLanguageTextForm(adaptableForm);
            }
        }

        CurrentLanguageData.instance.OnLanguageChange += SetStaticText;
    }

    private void InitLanguageTextForm(StaticTextLanguageAdaptableForm languageAdaptableForm)
    {
        foreach (var targetText in languageAdaptableForm.TargetTexts)
        {
            targetText.text = CurrentLanguageData.GetText(languageAdaptableForm.LanguageDataTextId);
        }
    }
    
    [Serializable]
    public class StaticTextLanguageAdaptableForm
    {
        [SerializeField] private TMP_Text[] targetTexts;
        [SerializeField] private int languageDataTextId;

        public TMP_Text[] TargetTexts => targetTexts;

        public int LanguageDataTextId => languageDataTextId;
    }
}