using UnityEngine;
using System.Collections.Generic;
using System;

public class ChainManager : MonoBehaviour
{
   public static ChainManager Instance;

   [Header("Chain Settings")]
   public float timeWindow = 5.0f;
   private float currentTimer;
   private bool isTimeRunning = false;

   [Header("Nodes Configuraation")]
   public List<Node> activeNodeChain = new List<Node>();
   public List<Node> unvisitedNodes = new List<Node>();
   private Node currentTargetNode;

   [Header("Audio Settings")]
    
    [SerializeField] private AudioClip connectSound; 
    [SerializeField] private AudioClip chainBreakSound;
    //[SerializeField] private AudioClip chainCompleteSound;

    public static event Action<int, int> OnChainUpdated; 

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

   private void OnEnable()
    {
        Node.OnNodeConnected += ValidateConnection;
        GameManager.OnGameOver += HandleGameOver;

    }
    private void OnDisable()
    {
        Node.OnNodeConnected -= ValidateConnection;
        GameManager.OnGameOver -= HandleGameOver;

    }
    private void Start()
    {
        activeNodeChain = new List<Node>(FindObjectsByType<Node>());
        ResetChain();
    }
    private void Update()
    {
        
        if (isTimeRunning )
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0)
            {
                Debug.Log("Time Out! Chain Broken!");
                
                AudioManager.Instance.PlaySFX(chainBreakSound);
                ResetChain();
            }
        }
    }

    private void SetNextRandomTargetNode()
    {
        int totalNodes = activeNodeChain.Count;
        int connectedNodes = totalNodes - unvisitedNodes.Count ;

        OnChainUpdated?.Invoke(connectedNodes, totalNodes);

        if (unvisitedNodes.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, unvisitedNodes.Count);
            currentTargetNode = unvisitedNodes[randomIndex];
            unvisitedNodes.RemoveAt(randomIndex);
            currentTargetNode.SetState(currentTargetNode.targetedState);

        }
        else
        {
            //Added the debug statement in the LevelWin method
            Debug.Log("Chain Completed Successfully!");
      
            //AudioManager.Instance.PlaySFX(chainCompleteSound);
            GameManager.Instance.LevelWin();
            isTimeRunning = false;
        }
    }

    public void ValidateConnection(Node node)
    {
        if(node == currentTargetNode)
        {
          
            currentTimer = timeWindow;
            isTimeRunning = true;
            SetNextRandomTargetNode();
            if (AudioManager.Instance != null && connectSound != null)
            {
                AudioManager.Instance.PlaySFX(connectSound);
            }
        }
    }

    public void OnWrongNodeHit()
    {
        Debug.Log("Wrong Node Hit! Chain Broken!");
        AudioManager.Instance.PlaySFX(chainBreakSound); ;
        ResetChain();
    }

    public void DisconnectSingleNode(Node nodeToDisconnect)
    {
        if (nodeToDisconnect == null) return;

        Debug.Log($"Dinosaur disconnected Node: {nodeToDisconnect.nodeID}");

        AudioManager.Instance.PlaySFX(chainBreakSound);
        nodeToDisconnect.SetState(nodeToDisconnect.inactiveState);

   
        if (!unvisitedNodes.Contains(nodeToDisconnect))
        {
            unvisitedNodes.Add(nodeToDisconnect);
        }
}
    public void ResetChain()
    {
        isTimeRunning = false;
        currentTimer = timeWindow;
        foreach(Node node in activeNodeChain)
        {
            node.SetState(node.inactiveState);
        }

        unvisitedNodes = new List<Node>(activeNodeChain);
        SetNextRandomTargetNode();

        OnChainUpdated?.Invoke(0, activeNodeChain.Count);
    }

    private void HandleGameOver(string gameOverReason)
    {
        isTimeRunning = false;

        foreach (Node node in activeNodeChain)
        {
            node.SetState(node.inactiveState);

        }
    }

}
