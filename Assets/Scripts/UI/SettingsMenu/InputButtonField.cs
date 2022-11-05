using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButtonField : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI selectedButtonLabel;
    [SerializeField] private Image background;

    private bool isButtonWaitAssigned = false;

    [Space] 
    [SerializeField] private bool onClickMouseCursorDisabled = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isButtonWaitAssigned)
        {
            StartButtonAssigned();

            if (onClickMouseCursorDisabled)
            {
                SetCursorState(false);
            }
        }

        void StartButtonAssigned()
        {
            isButtonWaitAssigned = true;
        }
    }

    private void Update()
    {
        var isButtonAssigned = isButtonWaitAssigned && (Input.anyKey || Axis.MouseWheel != 0);

        if (isButtonAssigned)
        {
            SetNewButtonAssigned();
        }

        void SetNewButtonAssigned()
        {
            var nowInputKeyCode = GetAnyPressedKeyCode();

            SetAssignedButtonName();
            
            isButtonWaitAssigned = false;
            
            if (onClickMouseCursorDisabled)
                SetCursorState(true);
            
            void SetAssignedButtonName()
            {
                if (nowInputKeyCode != 0)
                {
                    selectedButtonLabel.text = nowInputKeyCode.ToString();
                }
                else
                {
                    var mouseWheelValue = Axis.MouseWheel;
                    var mouseStateName = "MouseWheel";

                    if (mouseWheelValue > 0)
                        mouseStateName += "Up";
                    else
                        mouseStateName += "Down";

                    selectedButtonLabel.text = mouseStateName;
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

        return KeyCode.None;
    }

    void SetCursorState(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
