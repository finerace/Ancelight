using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonMainService : MonoBehaviour
{

    [SerializeField] private ButtonAnimations buttonAnimations;
    [Space]
    [SerializeField] private UnityEvent onClickAction;
    private bool onMouseEnter = false;
    private bool isClicked = false;

    protected void OnMouseUp()
    {
        if (onMouseEnter)
        {
            onClickAction.Invoke();
        }

        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Idle;

        isClicked = false;
    }

    protected void OnMouseDown()
    {
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Clicked;

        isClicked = true;
    }

    protected void OnMouseEnter()
    {
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.MouseEnter;

        onMouseEnter = true;
    }

    protected void OnMouseExit()
    {

        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Idle;

        onMouseEnter = false;
    }

    protected void OnMouseOver()
    {
        if(onMouseEnter && !isClicked)
            buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.MouseEnter;
    }

}
