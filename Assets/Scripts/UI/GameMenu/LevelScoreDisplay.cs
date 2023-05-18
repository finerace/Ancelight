using System.Collections;
using TMPro;
using UnityEngine;

public class LevelScoreDisplay : MonoBehaviour
{

    private LevelPassageService levelPassageService;
    [SerializeField] private TMP_Text scoreLabel;
    
    private void Start()
    {
        levelPassageService = FindObjectOfType<LevelPassageService>();

        levelPassageService.OnScoreAdd += UpdateScoreLabel;
        
        StartCoroutine(SetStartScore());
        IEnumerator SetStartScore()
        {
            yield return null;
            UpdateScoreLabel(levelPassageService.Score);
        }
    }

    private void UpdateScoreLabel(int value)
    {
        scoreLabel.text = $"{value}";
    }
    
}
