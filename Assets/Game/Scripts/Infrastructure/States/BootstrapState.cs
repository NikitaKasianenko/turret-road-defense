using Game.Infrastructure.StateMachine;
using Game.Level;

namespace Game.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LevelBuilder _levelBuilder;

        public BootstrapState(GameStateMachine stateMachine, LevelBuilder levelBuilder)
        {
            _stateMachine = stateMachine;
            _levelBuilder = levelBuilder;
        }

        public void Enter()
        {
            _levelBuilder.Build();
            _stateMachine.Enter<LevelStartState>();
        }

        public void Exit()
        {
        }
    }
}
