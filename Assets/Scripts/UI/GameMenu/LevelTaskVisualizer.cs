using TMPro;
using UnityEngine;

public class LevelTaskVisualizer : MonoBehaviour
{

    [SerializeField] private TMP_Text levelTaskLabel;
    [SerializeField] private LevelTaskService levelTaskService;

    private void Awake()
    {
        levelTaskService = FindObjectOfType<LevelTaskService>();

        levelTaskService.OnLevelTaskUpdate += SetNewTask;
    }

    private void SetNewTask(string task)
    {
        levelTaskLabel.text = task;
    }
    
}
