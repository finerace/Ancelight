using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectsScrollingService : MonoBehaviour
{

    [SerializeField] private List<Transform>scrollingObjects;
    [SerializeField] private Transform startScrollingObjectsPoint;
    
    [Space]
    
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private float notScrollAreaSize;
    [SerializeField] private float scrollingObjectSize;
    [SerializeField] private float scrollingObjectDistance;
    [SerializeField] private Vector3 scrollDirection;

    public IEnumerable<Transform> ScrollingObjects => scrollingObjects;

    private float ScrollbarValue => scrollbar.value;

    private void Awake()
    {
        scrollDirection.Normalize();
    }

    private void Update()
    {
        SetScrollingObjects(ScrollbarValue);
    }

    private void SetScrollingObjects(float scrollbarValue)
    {
        if(scrollingObjects.Count == 0)
            return;

        var startSetPoint = startScrollingObjectsPoint.localPosition;
        
        var useAreaSize = scrollingObjects.Count * (scrollingObjectSize + scrollingObjectDistance);

        var maxScrollingLength = useAreaSize - notScrollAreaSize;
                
        var currentScrollingLength = maxScrollingLength * scrollbarValue;

        var isScrollingNeed = useAreaSize > notScrollAreaSize;
        
        if(isScrollingNeed)
            startSetPoint -= scrollDirection * (currentScrollingLength);
        
        for (int i = 0; i < scrollingObjects.Count; i++)
        {
            var scrollingObject = scrollingObjects[i];
            
            var resultLocalPos = CalculateNewScrollingObjectPos();
            
            scrollingObject.localPosition = resultLocalPos;
                                    
            Vector3 CalculateNewScrollingObjectPos()
            {
                Vector3 calculatedLocalPoint;
                
                if (i == 0)
                {
                    calculatedLocalPoint = startSetPoint;
                    
                    calculatedLocalPoint += (scrollDirection * (scrollingObjectSize / 2f));    
                }
                else
                {
                    calculatedLocalPoint = scrollingObjects[i - 1].localPosition;

                    calculatedLocalPoint += scrollDirection * scrollingObjectSize;

                    calculatedLocalPoint += scrollDirection * scrollingObjectDistance;
                }

                return calculatedLocalPoint;
            }
        }

        scrollbar.gameObject.SetActive(isScrollingNeed);

    }

    public void AddScrollingObject(Transform objT)
    {
        scrollingObjects.Add(objT);
    }

    public void RemoveScrollingObject(Transform objT)
    {
        scrollingObjects.Remove(objT);
    }
    
}
