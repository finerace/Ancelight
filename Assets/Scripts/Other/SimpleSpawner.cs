using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{

    [SerializeField] private GameObject spawnObj;
    private Transform spawnPoint;
    [SerializeField] private bool timerSpawner;
    [SerializeField] private float spawnTime = 1f;
    private float spawnTimer = 1f;

    private void Start()
    {
        if(spawnPoint == null)
            spawnPoint = transform;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            Spawn();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timerSpawner = !timerSpawner;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if((spawnTime - 0.1f) > 0f)
                spawnTime -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnTime += 0.1f;
        }

        if (timerSpawner)
        {
            if (spawnTimer > 0)
                spawnTimer -= Time.deltaTime;
            else
            {
                Spawn();
                spawnTimer = spawnTime;
            }
        }
    }


    private void Spawn()
    {
        Vector3 pos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);

        pos.x += Random.Range(-0.1f, 0.1f);
        pos.y += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f, 0.1f);

        Instantiate(spawnObj, pos, spawnPoint.rotation);
    }

}
