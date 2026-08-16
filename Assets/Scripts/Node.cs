using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    [Header("Node Properties")]
    public string nodeID; 
    public bool isTargeted { get; set; } 
    [Header("Dinosaur Tracking")]
     public bool isUnderAttack { get; set; } = false;
    [Header("Components")]
    public SpriteRenderer spriteRenderer;


    public NodeStateMachine stateMachine { get; private set; }
    public InactiveState inactiveState { get; private set; }
    public TargetedState targetedState { get; private set; }
    public ConnectedState connectedState { get; private set; }

    
    public static event Action<Node> OnNodeConnected;

    private void Awake()
    {
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

      
        stateMachine = new NodeStateMachine();
        inactiveState = new InactiveState(this);
        targetedState = new TargetedState(this);
        connectedState = new ConnectedState(this);
    }

    private void Start()
    {
      
        stateMachine.Initialize(inactiveState);
    }

    private void Update()
    {
       
        stateMachine.Execute();
    }

   
    public void SetState(IState newState)
    {
        stateMachine.TransitionTo(newState);
    }

    
    public void ActivateNode()
    {
        if (isTargeted)
        {
            SetState(connectedState);
            OnNodeConnected?.Invoke(this); 
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isTargeted)
            {
                ActivateNode();
            }
            else if (stateMachine.CurrentState == inactiveState)
            {
                ChainManager chainMgr = FindAnyObjectByType<ChainManager>();
                if (chainMgr != null)
                {
                    chainMgr.OnWrongNodeHit();
                }
            }
        }
       
    }
}
