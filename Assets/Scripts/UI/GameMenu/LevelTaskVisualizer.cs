using TMPro;
using UnityEngine;

public class LevelTaskVisualizer : MonoBehaviour
{
    [SerializeField] private LevelTaskService levelTaskService;

    [Space]
    
    [SerializeField] private float animationSpeed = 1;
    [SerializeField] private float animationTime = 0.5f;
    
    [SerializeField] private TMP_Text levelTaskLabel;
    [SerializeField] private float levelTaskLabelMaxAlpha = 0.6f;
    private bool taskLabelIsVisible = false;
    
    [SerializeField] private Transform taskIconT;
    [SerializeField] private float onNewTaskIconScaleFactor = 2;
    private Vector3 taskIconDefaultScale;
    
    private float taskLabelTimer;
    private string bufferTaskText;
    
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

        AnimationTimerWork();
        void AnimationTimerWork()
        {
            if (taskLabelTimer > 0)
            {
                taskLabelIsVisible = false;
                taskLabelTimer -= Time.deltaTime;
            }
            else
            {
                levelTaskLabel.text = bufferTaskText;
                taskLabelIsVisible = true;
            }
        }

    }

    private void SetNewTask(string task)
    {
        StartTaskSetLabelAnimation(task);
        
        taskIconT.localScale = onNewTaskIconScaleFactor * taskIconDefaultScale;
    }

    private void StartTaskSetLabelAnimation(string task)
    {
        taskLabelTimer = animationTime;
        bufferTaskText = task;
    }
    
}
