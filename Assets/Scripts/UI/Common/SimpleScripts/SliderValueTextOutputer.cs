using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueTextOutputer : MonoBehaviour
{
    [SerializeField] private Slider targetSlider;
    [SerializeField] private TextMeshProUGUI label;
    
    [Space]
    [SerializeField] private string additionalText;
    [SerializeField] private LeftOrRight additionalTextPosRegardingValue;

    private void Awake()
    {
        UpdateValue(targetSlider.value);        
        
        targetSlider.onValueChanged.AddListener(UpdateValue);

        void UpdateValue(float value)
        {
            var resultValue = value.ClampToTwoRemainingCharacters();

            if (additionalTextPosRegardingValue == LeftOrRight.Right)
                label.text = $"{resultValue}{additionalText}";
            else 
                label.text = $"{additionalText}{resultValue}";
        }
    }

    private enum LeftOrRight
    {
        Left,
        Right
    }
}