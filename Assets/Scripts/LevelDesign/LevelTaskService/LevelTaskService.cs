using System;
using UnityEngine;

public class LevelTaskService : MonoBehaviour
{

    [SerializeField] private string currentTask;
    
    [Space]
    
    [SerializeField] private int taskCounter;
    [SerializeField] private int taskCounterMax;
    
    [SerializeField] private TextAlignment counterAlignment;

    public event Action<string> OnLevelTaskUpdate;

    private void Start()
    {
        UpdateTask();
    }

    public void SetNewTask(string newTask)
    {
        switch (counterAlignment)
        {
            case TextAlignment.Left:
                currentTask = $"{currentTask}{taskCounter}/{taskCounterMax}";
                break;
            case TextAlignment.Right:
                currentTask = $"{currentTask}/{taskCounterMax}{taskCounter}";
                break;
            default:
                currentTask = newTask;
                break;
        }

        OnLevelTaskUpdate?.Invoke(currentTask);
    }

    private void UpdateTask()
    {
        switch (counterAlignment)
        {
            case TextAlignment.Left:
                currentTask = $"{currentTask}{taskCounter}/{taskCounterMax}";
                break;
            case TextAlignment.Right:
                currentTask = $"{currentTask}/{taskCounterMax}{taskCounter}";
                break;
        }

        OnLevelTaskUpdate?.Invoke(currentTask);
    }
    
    public void SetTaskCounterAlignment(TextAlignment alignment)
    {
        counterAlignment = alignment;
    }

    public void AddToTaskCounter()
    {
        if (taskCounter + 1 > taskCounterMax)
            throw new Exception("Task counter cannot be more than Task counter max!");

        taskCounter++;
        
        UpdateTask();
    }

    public void SetMaxTaskCounter(int taskCounterMaxValue)
    {
        taskCounterMax = taskCounterMaxValue;
        
        UpdateTask();
    }
    
}
