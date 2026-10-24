using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Infrastructure.StateMachine;
using Game.Input;
using Game.Level;
using Game.Scoring;
using Game.UI;

namespace Game.Infrastructure.States
{
    public class LevelEndState : IPayloadedState<LevelResult>
    {
        private static readonly TimeSpan RestartInputDelay = TimeSpan.FromSeconds(0.6f);

        private readonly GameStateMachine _stateMachine;
        private readonly ScoreService _score;
        private readonly GameHud _hud;
        private readonly IInputService _input;

        private CancellationTokenSource _cancellation;

        public LevelEndState(GameStateMachine stateMachine, ScoreService score, GameHud hud, IInputService input)
        {
            _stateMachine = stateMachine;
            _score = score;
            _hud = hud;
            _input = input;
        }

        public void Enter(LevelResult result)
        {
            _hud.ShowResult(result, _score.Kills, _score.Score);

            _cancellation = new CancellationTokenSource();
            WaitForRestartAsync(_cancellation.Token).Forget();
        }

        public void Exit()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
            _hud.HideResult();
        }

        private async UniTaskVoid WaitForRestartAsync(CancellationToken cancellationToken)
        {
            await UniTask.Delay(RestartInputDelay, ignoreTimeScale: true, cancellationToken: cancellationToken);
            await _input.WaitForTapAsync(cancellationToken);
            _stateMachine.Enter<LevelStartState>();
        }
    }
}
