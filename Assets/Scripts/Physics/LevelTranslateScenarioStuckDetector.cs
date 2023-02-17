using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTranslateScenarioStuckDetector : MonoBehaviour
{
    [SerializeField] private bool isStuck;
    [Range(0,30)] [SerializeField] private int stuckObserverSmoothness = 1;
    private List<GameObject> stuckObjects = new List<GameObject>();
    private List<GameObject> onTriggerObjects = new List<GameObject>();
    public bool IsStuck => isStuck;
    private int updateSleep;


    private void OnCollisionEnter(Collision collision)
    {
        stuckObjects.Add(collision.gameObject);
        isStuck = true;
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerObjects.Add(other.gameObject);
    }

    private void FixedUpdate()
    {
        StuckObjectsRemoveAndObserveProcess();
        
        void StuckObjectsRemoveAndObserveProcess()
        {
            StartCoroutine(StuckObjectProcessCoroutine());
            
            IEnumerator StuckObjectProcessCoroutine()
            {
                yield return new WaitForEndOfFrame();

                MainStuckObserveProcess();
                void MainStuckObserveProcess()
                {
                    updateSleep++;

                    if (updateSleep < 0)
                        return;
                    else
                        updateSleep = -stuckObserverSmoothness - 1;

                    for (int i = 0; i < stuckObjects.Count; i++)
                    {
                        var stuckObject = stuckObjects[i];

                        if (!onTriggerObjects.Contains(stuckObject))
                        {
                            stuckObjects.Remove(stuckObject);
                            i += 2;
                        }
                    }

                    isStuck = stuckObjects.Count > 0;
                }
                
                onTriggerObjects.Clear();
            }
        }
    }
}
