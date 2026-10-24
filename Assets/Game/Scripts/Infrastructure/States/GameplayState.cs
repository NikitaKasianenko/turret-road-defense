using Game.CameraLogic;
using Game.Infrastructure.StateMachine;
using Game.Level;
using Game.Vehicle;
using Game.Weapons;

namespace Game.Infrastructure.States
{
    public class GameplayState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LevelSession _session;
        private readonly Car _car;
        private readonly Turret _turret;
        private readonly FinishLine _finishLine;
        private readonly CameraSwitcher _cameraSwitcher;

        public GameplayState(GameStateMachine stateMachine, LevelSession session, Car car, Turret turret,
            FinishLine finishLine, CameraSwitcher cameraSwitcher)
        {
            _stateMachine = stateMachine;
            _session = session;
            _car = car;
            _turret = turret;
            _finishLine = finishLine;
            _cameraSwitcher = cameraSwitcher;
        }

        public void Enter()
        {
            _car.Health.Died += OnCarDied;
            _finishLine.Reached += OnFinishReached;

            _cameraSwitcher.ShowGameplay();
            _car.StartDriving();
            _turret.Activate();
            _session.Start();
        }

        public void Exit()
        {
            _car.Health.Died -= OnCarDied;
            _finishLine.Reached -= OnFinishReached;

            _session.End();
            _turret.Deactivate();
            _car.Stop();
        }

        private void OnCarDied() =>
            _stateMachine.Enter<LevelEndState, LevelResult>(LevelResult.Lose);

        private void OnFinishReached() =>
            _stateMachine.Enter<LevelEndState, LevelResult>(LevelResult.Win);
    }
}
