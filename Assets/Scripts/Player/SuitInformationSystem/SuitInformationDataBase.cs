using System.Collections.Generic;
using UnityEngine;

public class SuitInformationDataBase : MonoBehaviour
{
    [SerializeField] private SuitInformationData[] suitInformationDatas;
    [SerializeField] private List<int> unlockedInformationDatas = new List<int>();
    public IEnumerable<int> UnlockedInformationDatas => unlockedInformationDatas;

    public void UnlockInformationData(int id)
    {
        unlockedInformationDatas.Add(id);    
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
