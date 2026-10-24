using System.Threading;
using Cysharp.Threading.Tasks;
using Game.CameraLogic;
using Game.Enemies;
using Game.Infrastructure.StateMachine;
using Game.Input;
using Game.UI;
using Game.Vehicle;
using Game.Weapons;

namespace Game.Infrastructure.States
{
    public class LevelStartState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly EnemySpawner _enemySpawner;
        private readonly Car _car;
        private readonly Turret _turret;
        private readonly CameraSwitcher _cameraSwitcher;
        private readonly GameHud _hud;
        private readonly IInputService _input;

        private CancellationTokenSource _cancellation;

        public LevelStartState(GameStateMachine stateMachine, EnemySpawner enemySpawner, Car car, Turret turret,
            CameraSwitcher cameraSwitcher, GameHud hud, IInputService input)
        {
            _stateMachine = stateMachine;
            _enemySpawner = enemySpawner;
            _car = car;
            _turret = turret;
            _cameraSwitcher = cameraSwitcher;
            _hud = hud;
            _input = input;
        }

        public void Enter()
        {
            _car.ResetState();
            _turret.ResetAim();
            _enemySpawner.Respawn();
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
