using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmoothVanishUI : MonoBehaviour
{

    [SerializeField] private Image[] images;
    private List<float> imagesStatAlpha = new List<float>();

    [SerializeField] private TMP_Text[] texts;
    private List<float> textsStartAlpha = new List<float>();

    [SerializeField] private float changeSpeed;
    
    [SerializeField] private bool isVanish;

    public void SetVanish(bool isVanish)
    {
        this.isVanish = isVanish;
    }

    private void Awake()
    {
        SetNewStartAlphaValues();
        void SetNewStartAlphaValues()
        {
            foreach (var image in images)
            {
                var startAlpha = image.color.a;
                imagesStatAlpha.Add(startAlpha);
            }

            foreach (var text in texts)
            {
                var textAlpha = text.color.a;
                textsStartAlpha.Add(textAlpha);
            }
            
        }

        InstantVanish();
        void InstantVanish()
        {
            foreach (var image in images)
            {
                var newColor = image.color;
                newColor.a = 0;
                image.color = newColor;
            }

            foreach (var text in texts)
            {
                var newColor = text.color;
                newColor.a = 0;
                text.color = newColor;
            }
        }
    }

    private void Update()
    {
        var timeStep = Time.deltaTime * changeSpeed;
        
        if (isVanish)
        {
            for (int i = 0; i < images.Length; i++)
            {
                var newColor = images[i].color;
                newColor.a = Mathf.Lerp(newColor.a, 0, timeStep);
                images[i].color = newColor;
            }
            
            for (int i = 0; i < texts.Length; i++)
            {
                var newColor = texts[i].color;
                newColor.a = Mathf.Lerp(newColor.a, 0, timeStep);
                texts[i].color = newColor;
            }
        }
        else
        {
            for (int i = 0; i < images.Length; i++)
            {
                var newColor = images[i].color;
                newColor.a = Mathf.Lerp(newColor.a,imagesStatAlpha[i], timeStep);
                images[i].color = newColor;
            }
            
            for (int i = 0; i < texts.Length; i++)
            {
                var newColor = texts[i].color;
                newColor.a = Mathf.Lerp(newColor.a, textsStartAlpha[i], timeStep);
                texts[i].color = newColor;
            }
        }
        
    }
}
