using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarMouseScrolling : MonoBehaviour,IScrollHandler
{
    [SerializeField] private Scrollbar targetScrollbar;
    
    [Space] 
    [SerializeField] private float scrollPower = 1;
    [SerializeField] private bool invertScrollPower = true;
    [SerializeField] private bool scrollbarMayBeOff = false;
    
    public void OnScroll(PointerEventData eventData)
    {
        if (targetScrollbar.gameObject.activeSelf == false)
        {
            if(!scrollbarMayBeOff)
                throw new Exception("Scrollbar is off!");
            
            return;
        }
        
        var realScrollPower = eventData.scrollDelta.y;
        var scrollPowerSmoothing = 100 / scrollPower;

        if (invertScrollPower)
            realScrollPower = -realScrollPower;
            
        ScrollScrollbar(realScrollPower / scrollPowerSmoothing);
        
        void ScrollScrollbar(float scrollingPower)
        {
            var scrollbarValue = targetScrollbar.value;
            
            var isScrollingPowerUnused =
                (scrollbarValue <= 0 && scrollingPower < 0) ||
                (scrollbarValue >= 1 && scrollingPower > 0);
            
            if(isScrollingPowerUnused)
                return;

            targetScrollbar.value += scrollingPower;
        }
    }

}
