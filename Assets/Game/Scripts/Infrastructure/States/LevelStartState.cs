using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.CameraLogic;
using Game.Infrastructure.StateMachine;
using Game.Input;
using Game.Level;
using Game.UI;

namespace Game.Infrastructure.States
{
    public class LevelStartState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IReadOnlyList<IResettable> _resettables;
        private readonly CameraSwitcher _cameraSwitcher;
        private readonly GameHud _hud;
        private readonly IInputService _input;

        private CancellationTokenSource _cancellation;

        public LevelStartState(GameStateMachine stateMachine, List<IResettable> resettables, CameraSwitcher cameraSwitcher,
            GameHud hud, IInputService input)
        {
            _stateMachine = stateMachine;
            _resettables = resettables;
            _cameraSwitcher = cameraSwitcher;
            _hud = hud;
            _input = input;
        }

        public void Enter()
        {
            foreach (IResettable resettable in _resettables)
                resettable.ResetState();

            _cameraSwitcher.ShowIntro();
            _hud.ShowStartScreen();

            _cancellation = new CancellationTokenSource();
            WaitForStartAsync(_cancellation.Token).Forget();
        }

        public void Exit()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
            _hud.HideStartScreen();
        }

        private async UniTaskVoid WaitForStartAsync(CancellationToken cancellationToken)
        {
            await _input.WaitForTapAsync(cancellationToken);
            _stateMachine.Enter<GameplayState>();
        }
    }
}
