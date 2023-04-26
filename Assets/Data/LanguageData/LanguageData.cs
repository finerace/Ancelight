using System;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language Data",menuName = "LanguageData",order = 51)]
public class LanguageData : ScriptableObject
{
    [SerializeField] private TextAsset file;

    [SerializeField] private string[] textlines = new String[500];

    public void InitLines()
    {
        var currentId = 0;

        var resultText = String.Empty;
        
        foreach (var pups in file.text)
        {
            var charIsTrueSymbol =
                Char.GetUnicodeCategory(pups) == UnicodeCategory.LowercaseLetter ||
                Char.GetUnicodeCategory(pups) == UnicodeCategory.UppercaseLetter ||
                pups == ' ' ||
                Char.IsNumber(pups) ||
                Char.IsPunctuation(pups) ||
                pups == ';' ||
                pups == '!' ||
                pups == '?';

            charIsTrueSymbol = charIsTrueSymbol && pups != '\n';
            
            if(!charIsTrueSymbol)
                continue;
            
            if (pups == ';')
            {
                textlines[currentId] = resultText;
                resultText = String.Empty;
                
                currentId++;
                
                continue;
            }
            
            resultText += pups;
        }
    }
    
    public string GetText(int textId)
    {
        return textlines[textId - 1];
    }
    
}