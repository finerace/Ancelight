using System;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language Data",menuName = "LanguageData",order = 51)]
public class LanguageData : ScriptableObject
{
    [SerializeField] private TextAsset file;

    public string GetText(int textId)
    {
        var currentId = 1;
        var resultText = String.Empty;;

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
                
            if (pups != ';')
                resultText += pups;
            else
            {
                if (currentId == textId)
                    return resultText;
                
                resultText = String.Empty;
                currentId++;
            }
            
            
        }
        
        throw new Exception($"Entered id {textId} not exist!");
    }
    
}