using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonMainService : MonoBehaviour, IPointerUpHandler,IPointerDownHandler, 
    IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private bool enabledAnimations = true;
    [SerializeField] private ButtonAnimations buttonAnimations;
    [Space]
    public UnityEvent onClickAction;
    private bool onMouseEnter = false;
    private bool isClicked = false;
    private AudioPoolService audioPoolService;
    [SerializeField] private AudioCastData onClickAudioData;
    [SerializeField] private AudioCastData onEnterExitAudioData;
    
    private void Start()
    {
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
    }
    
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

        if (onClickAudioData.Clips.Length != 0)
            audioPoolService.CastAudio(onClickAudioData);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(!enabledAnimations)
            return;
        
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.MouseEnter;

        onMouseEnter = true;

        if (onEnterExitAudioData.Clips.Length != 0)
            audioPoolService.CastAudio(onEnterExitAudioData);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(!enabledAnimations)
            return;
        
        buttonAnimations.currentButtonAnimationState =
            ButtonAnimations.ButtonState.Idle;

        onMouseEnter = false;
        
        if (onEnterExitAudioData.Clips.Length != 0)
            audioPoolService.CastAudio(onEnterExitAudioData);
    }

    // public void OnMouseOver()
    // {
    //     if(onMouseEnter && !isClicked)
    //         buttonAnimations.currentButtonAnimationState =
    //         ButtonAnimations.ButtonState.MouseEnter;
    // }

}
