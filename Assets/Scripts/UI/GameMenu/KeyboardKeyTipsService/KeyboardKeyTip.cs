using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class KeyboardKeyTip : MonoBehaviour
{
    private IUsePlayerDevicesButtons devicesButtons;
    [SerializeField] private int targetDeviceButton;
    [SerializeField] private float keyBorders = 35f;
    [SerializeField] private TextAlignment keyAlignment;
    
    
    [Space] 
    
    [SerializeField] private Image keyImage;
    [SerializeField] private TMP_Text keyLabel;
    [SerializeField] private RectTransform keyT;
    private Vector3 defaultLocalPos;
    
    [Space] 
    
    [SerializeField] private Sprite defaultKeyIcon;
    [SerializeField] private Sprite mouseLeftClickIcon;
    [SerializeField] private Sprite mouseRightClickIcon;
    [SerializeField] private Sprite mouseCenterClickIcon;
    [SerializeField] private Sprite mouseWheelForwardIcon;
    [SerializeField] private Sprite mouseWheelBackIcon;

    private void Start()
    {
        devicesButtons = GetDevicesButton();
        defaultLocalPos = keyT.localPosition;
        
        UpdateKey();
    }

    private void OnEnable()
    {
        UpdateKey();   
    }

    private void UpdateKey()
    {
        if(devicesButtons == null)
            return;

        var useDeviceButton =
            devicesButtons.GetUsesDevicesButtons()[targetDeviceButton];
        
        var useButtonKeyCode =
            devicesButtons.GetUsesDevicesButtons()[targetDeviceButton].AssignedButtonKeyCode; 
        
        var useButtonName = 
            devicesButtons.GetUsesDevicesButtons()[targetDeviceButton].AssignedButtonKeyCode.ToString();

        switch (useButtonKeyCode)
        {
            case KeyCode.Mouse0:
                useButtonName = String.Empty;
                keyImage.sprite = mouseLeftClickIcon;
                break;
            
            case KeyCode.Mouse1:
                useButtonName = String.Empty;
                keyImage.sprite = mouseRightClickIcon;
                break;
            
            case KeyCode.Mouse2:
                useButtonName = String.Empty;
                keyImage.sprite = mouseCenterClickIcon;
                break;
            
            case KeyCode.None:
                useButtonName = String.Empty;
                
                if(useDeviceButton.AssignedButtonMouseWheelMove)
                    keyImage.sprite = mouseWheelForwardIcon;
                else
                    keyImage.sprite = mouseWheelBackIcon;

                break;

            default:
                keyImage.sprite = defaultKeyIcon;
                break;
        }
        
        keyLabel.text = useButtonName;

        var resultScale = keyLabel.preferredWidth + keyBorders;

        if (useButtonName.Length <= 1)
            resultScale = keyBorders;
        
        var axis = RectTransform.Axis.Horizontal;
        keyT.SetSizeWithCurrentAnchors(axis,resultScale);

        switch (keyAlignment)
        {
            case TextAlignment.Left:
                keyT.localPosition = defaultLocalPos - new Vector3(resultScale/2,0,0);
                break;
            case TextAlignment.Right:
                keyT.localPosition = defaultLocalPos + new Vector3(resultScale/2,0,0);
                break;
        }
        
    }

    protected abstract IUsePlayerDevicesButtons GetDevicesButton();

}
