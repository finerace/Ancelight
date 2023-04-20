using System;
using UnityEngine;

public class NewDataNotification : MonoBehaviour
{

    [SerializeField] private SmoothVanishUI smoothVanishUI;

    [SerializeField] private float notificationTime = 5;
    
    private float vanishTimer;
    
    private void Awake()
    {
        FindObjectOfType<SuitInformationDataBase>().OnNewUnlockInformation += SetTimerValue;
        
        void SetTimerValue()
        {
            vanishTimer = notificationTime;
        }
    }
    
    private void Update()
    {
        if (vanishTimer > 0)
        {
            smoothVanishUI.SetVanish(false);
            vanishTimer -= Time.deltaTime;
        }
        else
            smoothVanishUI.SetVanish(true);
    }
}
