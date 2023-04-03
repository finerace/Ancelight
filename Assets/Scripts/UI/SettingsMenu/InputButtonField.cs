using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButtonField : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI assignedButtonLabel;

    [SerializeField] private KeyCode defaultAssignedButton;
    [SerializeField] private bool defaultAssignedMouseWheelMove;
    
    private DeviceButton assignedButton = new DeviceButton();

    private bool isButtonWaitAssigned = false;

    [Space]
    [SerializeField] private Image background;
    [SerializeField] private float waitNewButtonColorAlphaValue;

    private float defaultColorAlphaValue;

    [Space] 
    [SerializeField] private bool onClickMouseCursorDisabled = true;
    
    public KeyCode AssignedButtonKeyCode
    {
        get => assignedButton.AssignedButtonKeyCode;

        set
        {
            assignedButton.AssignedButtonKeyCode = value;
            assignedButtonLabel.text = value.ToString();
        }
    }

    public bool AssignedButtonMouseWheelMove
    {
        get => assignedButton.AssignedButtonMouseWheelMove;

        set
        {
            assignedButton.AssignedButtonMouseWheelMove = value;

            var resultText = "";

            resultText = value ? "MouseWheelUp" : "MouseWheelDown";

            assignedButtonLabel.text = resultText;
        }
    }

    private void Awake()
    {
        defaultColorAlphaValue = background.color.a;
        
        SetDefaultSettings();

        void SetDefaultSettings()
        {
            if (assignedButton == null)
                assignedButton = new DeviceButton();
            else
                return;
            
            assignedButton.AssignedButtonKeyCode = defaultAssignedButton;
            assignedButton.AssignedButtonMouseWheelMove = defaultAssignedMouseWheelMove;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isButtonWaitAssigned) return;
        
        StartButtonAssigned();

        if (onClickMouseCursorDisabled)
        {
            SetCursorState(false);
        }
    }

    private void Update()
    {
        var isButtonAssigned = isButtonWaitAssigned && (Input.anyKey || Axis.MouseWheel != 0);

        if (isButtonAssigned)
        {
            EndButtonAssigned();
        }

        void EndButtonAssigned()
        {
            var nowInputKeyCode = GetAnyPressedKeyCode();

            SetAssignedButtonName();
            assignedButton.AssignedButtonKeyCode = nowInputKeyCode;
            
            isButtonWaitAssigned = false;
            SetBackgroundColorAlphaValue(defaultColorAlphaValue);
            
            if (onClickMouseCursorDisabled)
                SetCursorState(true);
            
            void SetAssignedButtonName()
            {
                if (nowInputKeyCode != 0)
                {
                    assignedButtonLabel.text = nowInputKeyCode.ToString();
                }
                else
                {
                    var mouseWheelValue = Axis.MouseWheel;
                    var mouseStateName = "MouseWheel";

                    if (mouseWheelValue > 0)
                        mouseStateName += "Up";
                    else
                        mouseStateName += "Down";

                    assignedButtonLabel.text = mouseStateName;
                }
            }
        }
    }

    KeyCode GetAnyPressedKeyCode()
    {
        for (int keyCodeID = 1; keyCodeID < (int)KeyCode.Joystick8Button19 + 1; keyCodeID++)
        {
            var currentCheckKeyCode = (KeyCode)keyCodeID;
            var isCurrentKeyCodePressed = Input.GetKey(currentCheckKeyCode);

            if (isCurrentKeyCodePressed)
            {
                return currentCheckKeyCode;
            }
        }
        
        var mouseWheelValue = Axis.MouseWheel;

        assignedButton.AssignedButtonMouseWheelMove = mouseWheelValue > 0;
        
        return KeyCode.None;
    }

    private void StartButtonAssigned()
    {
        isButtonWaitAssigned = true;

        SetBackgroundColorAlphaValue(waitNewButtonColorAlphaValue);
    }
    
    private void SetCursorState(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SetBackgroundColorAlphaValue(float alphaValue)
    {
        var newColor = background.color;
        newColor.a = alphaValue;
        background.color = newColor;
    }
}

[Serializable]
public class DeviceButton
{
    public KeyCode AssignedButtonKeyCode;
    public bool AssignedButtonMouseWheelMove;
    
    public void AssignNewKeyCodeButton(KeyCode newButton)
    {
        AssignedButtonKeyCode = newButton;
    }
                
    public void AssignedNewMouseWheelMove(bool isForwardWheelMove)
    {
        AssignedButtonKeyCode = KeyCode.None;
        AssignedButtonMouseWheelMove = isForwardWheelMove;
    }

    public bool IsGetButton()
    {
        if (AssignedButtonKeyCode != KeyCode.None)
            return Input.GetKey(AssignedButtonKeyCode);
        else 
            return IsMouseWheelUse();
    }

    public bool IsGetButtonUp()
    {
        if (AssignedButtonKeyCode != KeyCode.None)
            return Input.GetKeyUp(AssignedButtonKeyCode);
        else 
            return IsMouseWheelUse();
    }

    public bool IsGetButtonDown()
    {
        if (AssignedButtonKeyCode != KeyCode.None)
            return Input.GetKeyDown(AssignedButtonKeyCode);
        else 
            return IsMouseWheelUse();
    }

    private bool IsMouseWheelUse()
    {
        if(AssignedButtonMouseWheelMove)
            return  (Axis.MouseWheel > 0);
        else
            return  (Axis.MouseWheel < 0);
    }

    public void AssignedNewDeviceButton(KeyCode newKeyCode, bool newMouseWheelMove)
    {
        AssignedButtonKeyCode = newKeyCode;
        AssignedButtonMouseWheelMove = newMouseWheelMove;
    }
    
}

public interface IUsePlayerDevicesButtons
{
    public DeviceButton[] GetUsesDevicesButtons();
}