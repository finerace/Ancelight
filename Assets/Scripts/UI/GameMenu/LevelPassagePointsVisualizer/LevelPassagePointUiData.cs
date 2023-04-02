using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassagePointUiData : MonoBehaviour
{

    [SerializeField] private Transform pointT;
    [SerializeField] private Image pointImage;

    [Space] 
    
    [SerializeField] private float visibleTransparency;
    [SerializeField] private float nonVisibleTransparency;
    
    public Transform PointT => pointT;
    public Image PointImage => pointImage;

    public void SetVisibility(bool isVisible)
    {
        var resultPointAlpha = visibleTransparency;

        if (!isVisible)
            resultPointAlpha = nonVisibleTransparency;

        var resultColor = pointImage.color;
        resultColor.a = resultPointAlpha;
        pointImage.color = resultColor;
    }
    
}
