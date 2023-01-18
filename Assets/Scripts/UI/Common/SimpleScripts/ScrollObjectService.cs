using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollObjectService : MonoBehaviour
{
    [SerializeField] private Transform scrollObjectT;
    
    [Space]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector3 scrollDirection;
    [SerializeField] private float scrollDistance = 0;
    [SerializeField] private float scrollSmoothCof = 1f;
    private Vector3 currentScrollObjectPos = Vector3.zero;
    private Vector3 endPoint;

    [Space] 
    [SerializeField] private Scrollbar scrollbar;

    public Transform StartPoint => startPoint;
    public Transform ScrollObjectT => scrollObjectT;
    
    private void Awake()
    {
        var startPointPos = startPoint.localPosition;
        scrollObjectT.localPosition = startPointPos;

        var moveScrollPointAction = new UnityAction<float>(MoveScrollObject);
        scrollbar.onValueChanged.AddListener(moveScrollPointAction);

        endPoint = startPointPos + (scrollDirection.normalized * scrollDistance);
        
        ReloadScrollSystem(scrollDistance);
    }

    private void Update()
    {
        SmoothMoveScrollObject();
    }

    private void MoveScrollObject(float scrollbarValue)
    {
        currentScrollObjectPos = Vector3.Lerp(startPoint.localPosition, endPoint,scrollbarValue);
    }

    private void SmoothMoveScrollObject()
    {
        var scrollObjectMoveSpeed = Time.unscaledDeltaTime * scrollSmoothCof;
        
        scrollObjectT.localPosition = 
            Vector3.Lerp(scrollObjectT.localPosition, currentScrollObjectPos,scrollObjectMoveSpeed);
    }

    public void ReloadScrollSystem(float newScrollDistance)
    {
        scrollDistance = newScrollDistance;
        var startPointPos = startPoint.localPosition;
        
        endPoint = startPointPos + (scrollDirection.normalized * scrollDistance);
        
        scrollbar.gameObject.SetActive(scrollDistance > 0);
        scrollbar.value = 0;

        scrollObjectT.localPosition = startPointPos;
        currentScrollObjectPos = startPointPos;
    }
}
