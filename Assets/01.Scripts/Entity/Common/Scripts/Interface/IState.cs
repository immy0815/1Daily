namespace _01.Scripts.Entity.Common.Scripts.Interface
{
    public interface IState
    {
        void Enter();
        void HandleInput();
        void PhysicsUpdate();
        void Update();
        void Exit();
    }
}