using Unity.GraphToolkit.Editor;
using UnityEngine;

public class InactiveState : IState
{
    private Node node;
    public InactiveState(Node node)
    {
        this.node = node;
    }
   public void Enter()
    {
        node.spriteRenderer.color = Color.gray;
        node.isTargeted = false;
    }
     public void Execute()
    {  
        
    }
    public void Exit()
    {
        
    }
}
