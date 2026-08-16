
using UnityEngine;

public class TargetedState : IState
{
   private Node node;
   public TargetedState(Node node)
    {
        this.node = node;

    }
     public void Enter()
    {
        node.spriteRenderer.color = Color.red;
        node.isTargeted = true;
    }
     public void Execute()
    {
        
    }
    public void Exit()
    {
        
    }
}
