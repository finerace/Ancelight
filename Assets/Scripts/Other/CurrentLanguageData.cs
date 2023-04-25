using System;
using UnityEngine;

public class CurrentLanguageData : MonoBehaviour
{
    public static CurrentLanguageData instance;
    private static LanguageData languageData;
    [SerializeField] private LanguageData currentLanguageData;

    [Space] 
    
    [SerializeField] private LanguageData[] languageDatas;

    private event Action onLanguageChange;
    public event Action OnLanguageChange
    {
        add => onLanguageChange += value;
        remove => onLanguageChange -= value;
    }
    
    public void Awake()
    {
        instance = this;
        languageData = currentLanguageData;
        languageData.InitLines();
    }

    public void SetLanguageData(int languageDataId)
    {
        currentLanguageData = languageDatas[languageDataId];
        languageData = currentLanguageData;
        languageData.InitLines();
        
        onLanguageChange?.Invoke();
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
