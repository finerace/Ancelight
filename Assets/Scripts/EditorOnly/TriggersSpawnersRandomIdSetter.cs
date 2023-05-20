using UnityEngine;

public class TriggersSpawnersRandomIdSetter : MonoBehaviour
{
#if UNITY_EDITOR

    [ContextMenu("Set spawners and triggers IDs")]
    public void FixIDs()
    {
        SetTriggers();
        void SetTriggers()
        {
            var allTriggers = FindObjectsOfType<LevelTrigger>(true);
            var randomIDs = GetRandomIDs(allTriggers.Length);
            
            for (int i = 0; i < allTriggers.Length; i++)
            {
                allTriggers[i].SetNewID(randomIDs[i]);
            }
        }
        
        SetSpawners();
        void SetSpawners()
        {
            var allSpawners = FindObjectsOfType<LevelSpawnScenario>(true);
            var randomIDs = GetRandomIDs(allSpawners.Length);
            
            for (int i = 0; i < allSpawners.Length; i++)
            {
                allSpawners[i].SetNewID(randomIDs[i]);
            }
        }
        
        SetTransformScenarios();
        void SetTransformScenarios()
        {
            var allScenarios = FindObjectsOfType<LevelTransformAnimationSystem>(true);
            var randomIDs = GetRandomIDs(allScenarios.Length);
            
            for (int i = 0; i < allScenarios.Length; i++)
            {
                allScenarios[i].SetNewID(randomIDs[i]);
            }
        }
        
        int[] GetRandomIDs(int arrayLength)
        {
            var resultArray = new int[arrayLength];

            for (var i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = GetRandomId();
            }

            return CheckForUniqueness(resultArray);
            
            int[] CheckForUniqueness(int[] randomIDs)
            {
                for (var i = 0; i < randomIDs.Length; i++)
                {
                    for (var j = 0; j < randomIDs.Length; j++)
                    {
                        if(i == j)
                            continue;

                        if (randomIDs[i] == randomIDs[j])
                        {
                            randomIDs[j] = GetRandomId();

                            return CheckForUniqueness(randomIDs);
                        }
                    }
                }

                return randomIDs;
            }
        }

        int GetRandomId()
        {
            return Random.Range(-128000, 128001);
        }
        
    }
    
#endif
}
