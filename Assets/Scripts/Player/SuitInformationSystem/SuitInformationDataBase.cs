using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitInformationDataBase : MonoBehaviour
{
    [SerializeField] private SuitInformationData[] suitInformationDatas;
    [SerializeField] private List<int> unlockedInformationDatas = new List<int>();
    public IEnumerable<int> UnlockedInformationDatas => unlockedInformationDatas;

    private event Action onNewUnlockInformation;
    public event Action OnNewUnlockInformation
    {
        add => onNewUnlockInformation += value;

        remove => onNewUnlockInformation -= value;
    }
    
    public void Load(SuitInformationDataBase suitInformationDataBase)
    {
        unlockedInformationDatas = suitInformationDataBase.unlockedInformationDatas;
    }
    
    public void UnlockInformationData(int id)
    {
        foreach (var unlockedInformationId in unlockedInformationDatas)
        {
            if(unlockedInformationId == id)
                return;
        }
        
        unlockedInformationDatas.Add(id);    
        onNewUnlockInformation?.Invoke();
    }
    
    public SuitInformationData GetInformationData(int id)
    {
        foreach (var suitInformationData in suitInformationDatas)
        {
            if (suitInformationData.InformationId == id)
                return suitInformationData;
        }

        return null;
    }
    
}
