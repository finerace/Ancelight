using System;
using UnityEngine;

public class CurrentLanguageData : MonoBehaviour
{
    public static CurrentLanguageData instance;
    private static LanguageData languageData;
    [SerializeField] private LanguageData currentLanguageData;
    
    public void Awake()
    {
        instance = this;
        languageData = currentLanguageData;
    }

    public static string GetText(int textId)
    {
        return languageData.GetText(textId);
    }

    public void SetNewLanguageData(LanguageData newLanguageData)
    {
        currentLanguageData = languageData;
        languageData = newLanguageData;
    }
    
}
