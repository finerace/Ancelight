using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    [SerializeField] private Animator buttonAnimator;
    public ButtonState currentButtonAnimationState;

    
    private void Update()
    {

        switch (currentButtonAnimationState)
        {
            case ButtonState.Idle:
                SetButtonAnimatorStates(false, false);
                break;

            case ButtonState.MouseEnter:
                SetButtonAnimatorStates(true, false);
                break;

            case ButtonState.Clicked:
                SetButtonAnimatorStates(false, true);
                break;
        }

        void SetButtonAnimatorStates(bool isMouseOnButton,bool isMouseClickOnButton)
        {
            buttonAnimator.SetBool(nameof(isMouseOnButton), isMouseOnButton);
            buttonAnimator.SetBool(nameof(isMouseClickOnButton), isMouseClickOnButton);
        }

    }

    public enum ButtonState
    {
        Idle,
        MouseEnter,
        Clicked
    }
}


