using Game.CameraLogic;
using Game.Effects;
using Game.Enemies;
using Game.Infrastructure.StateMachine;
using Game.Infrastructure.States;
using Game.Input;
using Game.Level;
using Game.UI;
using Game.Vehicle;
using Game.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private const int BulletPoolSize = 32;
        private const int EnemyPoolSize = 32;

        [Header("Scene")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Car _car;
        [SerializeField] private Turret _turret;
        [SerializeField] private LevelBuilder _levelBuilder;
        [SerializeField] private FinishLine _finishLine;
        [SerializeField] private CameraSwitcher _cameraSwitcher;
        [SerializeField] private GameHud _hud;
        [SerializeField] private ImpactEffect _impactEffect;

        [Header("Prefabs")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Enemy _enemyPrefab;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindServices();
            BindSceneObjects();
            BindPools();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();

            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
            Container.Bind<LevelEndState>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IInputService>().To<PointerInputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
        }

        private void BindSceneObjects() =>
            Container.BindInstances(_camera, _car, _turret, _levelBuilder, _finishLine, _cameraSwitcher, _hud, _impactEffect);

        private void BindPools()
        {
            Container.BindMemoryPool<Bullet, Bullet.Pool>()
                .WithInitialSize(BulletPoolSize)
                .FromComponentInNewPrefab(_bulletPrefab)
                .UnderTransformGroup("Bullets");

            Container.BindMemoryPool<Enemy, Enemy.Pool>()
                .WithInitialSize(EnemyPoolSize)
                .FromComponentInNewPrefab(_enemyPrefab)
                .UnderTransformGroup("Enemies");
        }
    }
}
