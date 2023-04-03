using System;
using UnityEngine;

public class LevelPassagePoint : MonoBehaviour
{
    public event Action<int> OnPlayerInPoint;
    public Transform pointT;
    private int id;

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
