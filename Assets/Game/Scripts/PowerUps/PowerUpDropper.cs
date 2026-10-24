using System;
using Game.Configs;
using Game.Enemies;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.PowerUps
{
    public class PowerUpDropper : IInitializable, IDisposable
    {
        private const float DropHeight = 1.2f;

        private readonly EnemySpawner _enemySpawner;
        private readonly PowerUpConfig _config;
        private readonly PowerUpOrb.Pool _orbPool;

        public PowerUpDropper(EnemySpawner enemySpawner, PowerUpConfig config, PowerUpOrb.Pool orbPool)
        {
            _enemySpawner = enemySpawner;
            _config = config;
            _orbPool = orbPool;
        }

        public void Initialize() =>
            _enemySpawner.EnemyKilled += OnEnemyKilled;

        public void Dispose() =>
            _enemySpawner.EnemyKilled -= OnEnemyKilled;

        private void OnEnemyKilled(Enemy enemy)
        {
            if (_config.PowerUps.Length == 0 || Random.value > _config.DropChance)
                return;

            PowerUp powerUp = _config.PowerUps[Random.Range(0, _config.PowerUps.Length)];
            _orbPool.Spawn(enemy.transform.position + Vector3.up * DropHeight, powerUp);
        }
    }
}
