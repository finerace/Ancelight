using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneBulletIndicator : MonoBehaviour
{

    [SerializeField] private Image mainImage;
    [SerializeField] private Image secondImage;

    public Image MainImage { get { return mainImage; } }

    public void SetNewSprite(Sprite newSprite)
    {
        mainImage.sprite = newSprite;
        secondImage.sprite = newSprite;
    }

}
