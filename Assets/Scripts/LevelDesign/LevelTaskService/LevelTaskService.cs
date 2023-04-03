using System;
using UnityEngine;

public class LevelTaskService : MonoBehaviour
{
    private string currentTask;
    [SerializeField] private string realCurrentTask;
    
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
        realCurrentTask = newTask;
        
        UpdateTask();
    }

    private void UpdateTask()
    {
        switch (counterAlignment)
        {
            case TextAlignment.Left:
                currentTask = $"{taskCounter}/{taskCounterMax}{realCurrentTask}";
                break;
            case TextAlignment.Right:
                currentTask = $"{realCurrentTask}{taskCounter}/{taskCounterMax}";
                break;
            default:
                currentTask = realCurrentTask;
                break;
        }

        OnLevelTaskUpdate?.Invoke(currentTask);
    }
    
    public void SetTaskCounterAlignment(int state)
    {
        counterAlignment = (TextAlignment)state;
        
        UpdateTask();
    }

    public void AddToTaskCounter()
    {
        if (taskCounter + 1 > taskCounterMax)
            throw new Exception("Task counter cannot be more than Task counter max!");

        taskCounter++;
        
        UpdateTask();
    }

    public void IfIsCounterFullSetNewTask(string newTask)
    {
        if(taskCounter >= taskCounterMax)
            SetNewTask(newTask);
    }
    
    public void IfIsCounterFullSetNewCounterAlignment(int state)
    {
        if(taskCounter >= taskCounterMax)
            SetTaskCounterAlignment(state);
    }

    
    public void SetMaxTaskCounter(int taskCounterMaxValue)
    {
        taskCounterMax = taskCounterMaxValue;
        
        UpdateTask();
    }
    
}
