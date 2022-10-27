using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollObjectService : MonoBehaviour
{
    [SerializeField] private Transform scrollPoint;
    
    [Space]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector3 scrollDirection;
    [SerializeField] private float scrollDistance = 0;
    private Vector3 endPoint;

    [Space] 
    [SerializeField] private Scrollbar scrollbar;

    private void Awake()
    {
        var startPointPos = startPoint.localPosition;
        scrollPoint.localPosition = startPointPos;

        var moveScrollPointAction = new UnityAction<float>(MoveScrollPoint);
        scrollbar.onValueChanged.AddListener(moveScrollPointAction);

        endPoint = startPointPos + (scrollDirection.normalized * scrollDistance);
        
        ReloadScrollSystem(scrollDistance);
    }
    
    private void MoveScrollPoint(float scrollbarValue)
    {
        scrollPoint.localPosition = Vector3.Lerp(startPoint.localPosition, endPoint,scrollbarValue);
    }
    
    public void ReloadScrollSystem(float newScrollDistance)
    {
        scrollDistance = newScrollDistance;

        scrollbar.gameObject.SetActive(scrollDistance > 0);
        scrollbar.value = 0;
    }
}
