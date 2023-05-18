using System;
using TMPro;
using UnityEngine;

public class LevelPassageTimeDisplay : MonoBehaviour
{
    private LevelPassageService levelPassageService;
    [SerializeField] private TMP_Text timeLabel; 

    private void Start()
    {
        levelPassageService = FindObjectOfType<LevelPassageService>();
    }

    private void Update()
    {
        UpdateTimeLabel();
    }

    private void UpdateTimeLabel()
    {
        var timeSpan = AuxiliaryFunc.ConvertSecondsToTimeSpan((int)levelPassageService.PassageTimeSec);        
        timeLabel.text = $"{timeSpan}";
    }
    
}
