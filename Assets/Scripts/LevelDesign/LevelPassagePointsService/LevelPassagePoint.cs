using System;
using UnityEngine;

[Serializable]
public class LevelPassagePoint : MonoBehaviour
{
    public event Action<int> OnPlayerInPoint;
    public Transform pointT;
    [SerializeField] private int id;

    private void Awake()
    {
        pointT = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
            return;
        
        OnPlayerInPoint?.Invoke(id);
    }

    public void SetPointId(int newPointId)
    {
        id = newPointId;
    }
    
}
