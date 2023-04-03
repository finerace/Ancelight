using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelTaskVisualizer : MonoBehaviour
{
    [SerializeField] private LevelTaskService levelTaskService;

    [Space]
    
    [SerializeField] private float animationSpeed = 1;
    
    [SerializeField] private TMP_Text levelTaskLabel;
    [SerializeField] private float levelTaskLabelMaxAlpha = 0.6f;
    private bool taskLabelIsVisible = false;
    
    [SerializeField] private Transform taskIconT;
    [SerializeField] private float onNewTaskIconScaleFactor = 2;
    private Vector3 taskIconDefaultScale;
        
    private void Awake()
    {
        levelTaskService = FindObjectOfType<LevelTaskService>();

        levelTaskService.OnLevelTaskUpdate += SetNewTask;
        taskIconDefaultScale = taskIconT.localScale;
    }

    private void Update()
    {
        var timeStep = animationSpeed * Time.deltaTime;
        
        ScaleTaskIcon();
        void ScaleTaskIcon()
        {
            taskIconT.localScale = Vector3.Lerp(taskIconT.localScale, taskIconDefaultScale, timeStep);
        }

        SetTaskLabelAlpha();
        void SetTaskLabelAlpha()
        {
            var targetAlpha = levelTaskLabelMaxAlpha;

            if (!taskLabelIsVisible)
                targetAlpha = 0;
                
            var resultColor = levelTaskLabel.color;
            resultColor.a = Mathf.Lerp(resultColor.a,targetAlpha,timeStep);
            levelTaskLabel.color = resultColor;
        }
    }

    private void SetNewTask(string task)
    {
        StartCoroutine(StartTaskSetLabelAnimation(task));
        
        taskIconT.localScale = onNewTaskIconScaleFactor * taskIconDefaultScale;
    }

    private IEnumerator StartTaskSetLabelAnimation(string task)
    {
        var waitHalfSecond = new WaitForSeconds(0.5f);

        taskLabelIsVisible = false;
        
        yield return waitHalfSecond;
        
        levelTaskLabel.text = task;
        taskLabelIsVisible = true;
    }
    
}
