using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButtonField : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI assignedButtonLabel;
    [SerializeField] private KeyCode assignedButtonKeyCode;

    private bool assignedButtonMouseWheelMove;

    private bool isButtonWaitAssigned = false;

    [Space]
    [SerializeField] private Image background;

    [SerializeField] private float waitNewButtonColorAlphaValue;

    private float defaultColorAlphaValue;

    [Space] 
    [SerializeField] private bool onClickMouseCursorDisabled = true;

    public KeyCode AssignedButtonKeyCode
    {
        get => assignedButtonKeyCode;

        set
        {
            assignedButtonKeyCode = value;
            assignedButtonLabel.text = value.ToString();
        }
    }

    public bool AssignedButtonMouseWheelMove
    {
        get => assignedButtonMouseWheelMove;

        set
        {
            assignedButtonMouseWheelMove = value;

            var resultText = "";

            resultText = value ? "MouseWheelUp" : "MouseWheelDown";

            assignedButtonLabel.text = resultText;
        }
    }

    private void Awake()
    {
        defaultColorAlphaValue = background.color.a;
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
            assignedButtonKeyCode = nowInputKeyCode;
            
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

        assignedButtonMouseWheelMove = mouseWheelValue > 0;
        
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
