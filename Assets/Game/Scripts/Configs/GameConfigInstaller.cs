using UnityEngine;
using Zenject;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Game/Game Config Installer")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField] private CarConfig _car;
        [SerializeField] private TurretConfig _turret;
        [SerializeField] private EnemyConfig _enemy;
        [SerializeField] private LevelConfig _level;
        [SerializeField] private ScoreConfig _score;
        [SerializeField] private PowerUpConfig _powerUps;
        [SerializeField] private FeelConfig _feel;

        public override void InstallBindings() =>
            Container.BindInstances(_car, _turret, _enemy, _level, _score, _powerUps, _feel);
    }
}
