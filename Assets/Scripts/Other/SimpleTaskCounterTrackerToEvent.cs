using System;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTaskCounterTrackerToEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent onTargetCountEvent;
    [SerializeField] private int targetCount;

    private void Awake()
    {
        var taskService = FindObjectOfType<LevelTaskService>();
        taskService.OnLevelTaskCounterUpdate += CheckToTargetCount;

        void CheckToTargetCount(int taskCounterValue)
        {
            if(taskCounterValue >= targetCount)
                onTargetCountEvent.Invoke();
        }
    }
}
