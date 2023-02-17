using TMPro;
using UnityEngine;

public class SaveUnitData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveNameLabel;
    [SerializeField] private TextMeshProUGUI saveDateTimeLabel;
    [SerializeField] private string saveName;

    public TextMeshProUGUI SaveNameLabel => saveNameLabel;

    public TextMeshProUGUI SaveDateTimeLabel => saveDateTimeLabel;

    public string SaveName => saveName;

    public void CreateSave(string saveName, string saveDate)
    {
        this.saveName = saveName;

        saveNameLabel.text = saveName;
        saveDateTimeLabel.text = saveDate;
    }
    
}
