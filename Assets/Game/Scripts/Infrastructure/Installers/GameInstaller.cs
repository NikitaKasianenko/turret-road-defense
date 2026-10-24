using Game.CameraLogic;
using Game.Effects;
using Game.Enemies;
using Game.Infrastructure.StateMachine;
using Game.Infrastructure.States;
using Game.Input;
using Game.Level;
using Game.PowerUps;
using Game.Scoring;
using Game.UI;
using Game.Vehicle;
using Game.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private const int BulletPoolSize = 48;
        private const int EnemyPoolSize = 32;
        private const int FloatingTextPoolSize = 8;
        private const int PowerUpOrbPoolSize = 4;

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
        [SerializeField] private FloatingText _floatingTextPrefab;
        [SerializeField] private PowerUpOrb _powerUpOrbPrefab;

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
            Container.Bind<LevelSession>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PowerUpService>().AsSingle();
            Container.BindInterfacesTo<PowerUpDropper>().AsSingle();

            Container.BindInterfacesAndSelfTo<TurretHeat>().AsSingle();

            Container.BindInterfacesTo<TimeEffects>().AsSingle();
            Container.BindInterfacesTo<FloatingTextSpawner>().AsSingle();
        }

        private void BindSceneObjects()
        {
            Container.BindInterfacesAndSelfTo<Car>().FromInstance(_car);
            Container.BindInterfacesAndSelfTo<Turret>().FromInstance(_turret);
            Container.BindInstances(_camera, _levelBuilder, _finishLine, _cameraSwitcher, _hud, _impactEffect);
        }

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

            Container.BindMemoryPool<FloatingText, FloatingText.Pool>()
                .WithInitialSize(FloatingTextPoolSize)
                .FromComponentInNewPrefab(_floatingTextPrefab)
                .UnderTransformGroup("FloatingTexts");

            Container.BindMemoryPool<PowerUpOrb, PowerUpOrb.Pool>()
                .WithInitialSize(PowerUpOrbPoolSize)
                .FromComponentInNewPrefab(_powerUpOrbPrefab)
                .UnderTransformGroup("PowerUpOrbs");
        }
    }
}
