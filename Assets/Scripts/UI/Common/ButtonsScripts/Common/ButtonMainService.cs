using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMainService : MonoBehaviour, IPointerUpHandler,IPointerDownHandler, 
    IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private bool enabledAnimations = true;
    [SerializeField] private ButtonAnimations buttonAnimations;
    [Space]
    public UnityEvent onClickAction;
    private bool onMouseEnter = false;
    private bool isClicked = false;

    public void OnPointerUp(PointerEventData data)
    {
        if (onMouseEnter)
        {
            onClickAction.Invoke();
        }
        
        if(!enabledAnimations)
            return;
            
        buttonAnimations.currentButtonAnimationState = onMouseEnter ? ButtonAnimations.ButtonState.MouseEnter 
            : ButtonAnimations.ButtonState.Idle;
        
        isClicked = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if(!enabledAnimations)
            return;
        
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Clicked;

        isClicked = true;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(!enabledAnimations)
            return;
        
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.MouseEnter;

        onMouseEnter = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(!enabledAnimations)
            return;
        
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Idle;

        onMouseEnter = false;
    }

    // public void OnMouseOver()
    // {
    //     if(onMouseEnter && !isClicked)
    //         buttonAnimations.currentButtonAnimationState =
    //         ButtonAnimations.ButtonState.MouseEnter;
    // }

}
