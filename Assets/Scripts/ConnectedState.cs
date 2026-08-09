using UnityEngine;

public class ConnectedState : IState
{
    private Node node;

    
    public ConnectedState(Node node)
    {
        this.node = node;
    }

    public void Enter()
    {
        
        node.spriteRenderer.color = Color.green;
        node.isTargeted = false;
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {
       
    }
}
