using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinosaurSpawner : MonoBehaviour
{
    [Header("References")]
   
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

            if (ChainManager.Instance != null && spawnPoints.Length > 0 )
            {
                if (IsAllNodesConnected())
                {
                    Debug.Log("All nodes connected! Stopping dinosaur spawns.");
                    yield break;
                }
                SpawnDinosaur();
            }
        }
    }
private bool IsAllNodesConnected()
    {
        if (ChainManager.Instance == null) return false;
        foreach (Node node in ChainManager.Instance.activeNodeChain)
        {
            if (node != null && node.stateMachine.CurrentState != node.connectedState)
            {
                return false;
            }
        }

        return true;
    }
    private void SpawnDinosaur()
    {

        List<Node> availableNodes = new List<Node>();
    foreach (Node node in ChainManager.Instance.activeNodeChain)
    {
        if (node != null && 
            node.stateMachine.CurrentState == node.connectedState && !node.isUnderAttack) 
        {
            availableNodes.Add(node);
        }
    }
    if (availableNodes.Count > 0)
    {
        Node selectedNode = availableNodes[Random.Range(0, availableNodes.Count)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject dinoObj = ObjectPool.Instance.GetFromPool(randomSpawnPoint.position, Quaternion.identity);

        Dinosaur dino = dinoObj.GetComponent<Dinosaur>();
        if (dino != null)
        {
            dino.Initialize(selectedNode);
        }
    }
    }
}
