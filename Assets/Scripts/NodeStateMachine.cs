using UnityEngine;
using System;
[Serializable]

public class NodeStateMachine 
{
    public IState CurrentState { get; private set; }

    public void Initialize (IState  startingState)
    {
     CurrentState = startingState;
     CurrentState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
     if(CurrentState != null)
     {
        CurrentState.Exit();
     }
      CurrentState = nextState;
     CurrentState.Enter();

    }

    public void Execute()
    {
         if(CurrentState != null)
     {
        CurrentState.Execute();
     }
    }
}
