using UnityEngine;
using IState = _01.Scripts.Entity.Common.Scripts.Interface.IState;

namespace _01.Scripts.Entity.Common.Scripts
{
    public abstract class StateMachine
    {
       public IState CurrentState { get; private set; }

       public void ChangeState(IState newState)
       {
           CurrentState?.Exit();
           CurrentState = newState;
           CurrentState?.Enter();
       }

       public void HandleInput()
       {
           CurrentState?.HandleInput();
       }

       public void PhysicsUpdate()
       {
           CurrentState?.PhysicsUpdate();
       }

       public void Update()
       {
           CurrentState?.Update();
       }

       public void LateUpdate()
       {
           CurrentState?.LateUpdate();
       }
    }
}