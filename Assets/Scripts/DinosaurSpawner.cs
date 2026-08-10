using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinosaurSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> allRouters; 
    [SerializeField] private Transform[] spawnPoints;   

    [Header("Spawn Settings")]
    [SerializeField] private float timeBetweenSpawns = 4f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            if (allRouters.Count > 0 && spawnPoints.Length > 0)
            {
                SpawnDinosaur();
            }
        }
    }

    private void SpawnDinosaur()
    {
        
        Transform randomRouter = allRouters[Random.Range(0, allRouters.Count)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        
        GameObject dinoObj = ObjectPool.Instance.GetFromPool(randomSpawnPoint.position, Quaternion.identity);

        Dinosaur dino = dinoObj.GetComponent<Dinosaur>();
        if (dino != null)
        {
            dino.Initialize(randomRouter);
        }
    }
}
