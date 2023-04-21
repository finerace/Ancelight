using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language Data",menuName = "LanguageData",order = 51)]
public class LanguageData : ScriptableObject
{
    [SerializeField] private TextAsset file;

    public string GetText(int textId)
    {
        var currentId = 1;
        var resultText = "";

        foreach (var pups in file.text)
        {
            if(pups == '\n')
                continue; 
                
            if (pups != ';')
                resultText += pups;
            else
            {
                if (currentId == textId)
                    return resultText;
                
                resultText = new string("");
                currentId++;
            }
        }
        
        throw new Exception($"Entered id {textId} not exist!");
    }
    
}