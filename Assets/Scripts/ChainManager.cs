using UnityEngine;
using System.Collections.Generic;

public class ChainManager : MonoBehaviour
{
    public static ChainManager Instance { get; private set; }
   [Header("Chain Settings")]
   public float timeWindow = 5.0f;
   private float currentTimer;
   private bool isTimeRunning = false;

   [Header("Nodes Configuraation")]
   public List<Node> activeNodeChain = new List<Node>();
   public List<Node> unvisitedNodes = new List<Node>();
   private Node currentTargetNode;

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
    }
    private void OnDisable()
    {
        Node.OnNodeConnected -= ValidateConnection;
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
                ResetChain();
            }
        }
    }

    private void SetNextRandomTargetNode()
    {
        if(unvisitedNodes.Count > 0)
        {
            int randomIndex = Random.Range(0, unvisitedNodes.Count);
            currentTargetNode = unvisitedNodes[randomIndex];
            unvisitedNodes.RemoveAt(randomIndex);
            currentTargetNode.SetState(currentTargetNode.targetedState);

        }
        else
        {
            //Added the debug statement in the LevelWin method
            GameManager.Instance.LevelWin();
            isTimeRunning = false;
        }
    }

    public void ValidateConnection(Node node)
    {
        if(node == currentTargetNode)
        {
            Debug.Log($"Node {node.nodeID} Connected!");
            currentTimer = timeWindow;
            isTimeRunning = true;
            SetNextRandomTargetNode();
        }
    }

    public void OnWrongNodeHit()
    {
        Debug.Log("Wrong Node Hit! Chain Broken!");
        ResetChain();
    }

    public void DisconnectSingleNode(Node nodeToDisconnect)
{
    if (nodeToDisconnect == null) return;

    Debug.Log($"Dinosaur disconnected Node: {nodeToDisconnect.nodeID}");

 
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
    }
}
