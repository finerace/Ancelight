using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUnscaledTimeSetter : MonoBehaviour
{
    
    [SerializeField] private Material material;
    [SerializeField] private string unscaledTimeReferenceName = "_UnscaledTime";

    private void Update()
    {
        float currentUnscaledTime = material.GetFloat(unscaledTimeReferenceName);

        float unscaledTime = currentUnscaledTime + Time.unscaledDeltaTime;

        material.SetFloat(unscaledTimeReferenceName, unscaledTime);
    }

    private void OnApplicationPause(bool pause)
    {
        material.SetFloat(unscaledTimeReferenceName, 0);
    }

}
