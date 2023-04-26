using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashOneIndicator : MonoBehaviour
{
    [SerializeField] private Image[] dashIndicatorAllImages;
    [SerializeField] private Image dashDynamicElement;
    private Material dashDynamicElementMat;
    private float[] dashIndicatorAllImagesTransparency;

    private void Awake()
    {
        dashDynamicElementMat = new Material(dashDynamicElement.material);

        dashDynamicElement.material = dashDynamicElementMat;

        dashIndicatorAllImagesTransparency = new float[dashIndicatorAllImages.Length];

        for (int i = 0; i < dashIndicatorAllImages.Length; i++)
        {
            dashIndicatorAllImagesTransparency[i] = dashIndicatorAllImages[i].color.a;
        }
    }

    public void SetTransparency(float transparency)
    {

        for (int i = 0; i < dashIndicatorAllImages.Length; i++)
        {
            var item = dashIndicatorAllImages[i];

            Color newItemColor = item.color;

            newItemColor.a = transparency * dashIndicatorAllImagesTransparency[i];

            item.color = newItemColor;
        }

    }

    public void SetFillAmount(float newFillAmount)
    {
        dashDynamicElement.fillAmount = newFillAmount;
    }

    public void SetFillZoneEffectIntensity(float intensity)
    {
        //dashDynamicElementMat.SetFloat("_Intensity", intensity);
    }

    public float GetFillZoneEffectIntensity()
    {
        return dashDynamicElementMat.GetFloat("_Intensity");
    }

}
