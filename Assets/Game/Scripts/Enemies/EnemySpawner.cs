using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemySpawner : IDisposable
    {
        private const float MinSlotOffset = 0.2f;
        private const float MaxSlotOffset = 0.8f;
        private const float MaxSpawnYawJitter = 30f;

        private readonly Enemy.Pool _pool;
        private readonly LevelConfig _config;
        private readonly List<Enemy> _enemies = new();

        private CancellationTokenSource _lifetimeCancellation = new();

        public event Action KilledCountChanged;

        public int KilledCount { get; private set; }

        public EnemySpawner(Enemy.Pool pool, LevelConfig config)
        {
            _pool = pool;
            _config = config;
        }

        public void Dispose()
        {
            _lifetimeCancellation.Cancel();
            _lifetimeCancellation.Dispose();
        }

        public void Respawn()
        {
            DespawnAll();
            SpawnAll();

            KilledCount = 0;
            KilledCountChanged?.Invoke();
        }

        private void SpawnAll()
        {
            float spawnZoneLength = _config.Length - _config.SpawnStartDistance - _config.SpawnEndOffset;
            float slotLength = spawnZoneLength / Mathf.Max(_config.EnemyCount, 1);

            for (int i = 0; i < _config.EnemyCount; i++)
            {
                float z = _config.SpawnStartDistance + slotLength * (i + Random.Range(MinSlotOffset, MaxSlotOffset));
                float x = Random.Range(-_config.SpawnHalfWidth, _config.SpawnHalfWidth);
                Quaternion rotation = Quaternion.Euler(0f, 180f + Random.Range(-MaxSpawnYawJitter, MaxSpawnYawJitter), 0f);

                Enemy enemy = _pool.Spawn(new Vector3(x, 0f, z), rotation);
                enemy.Died += OnEnemyDied;
                _enemies.Add(enemy);
            }
        }

        private void DespawnAll()
        {
            _lifetimeCancellation.Cancel();
            _lifetimeCancellation.Dispose();
            _lifetimeCancellation = new CancellationTokenSource();

            foreach (Enemy enemy in _enemies)
            {
                enemy.Died -= OnEnemyDied;
                _pool.Despawn(enemy);
            }

            _enemies.Clear();
        }

        private void OnEnemyDied(Enemy enemy)
        {
            KilledCount++;
            KilledCountChanged?.Invoke();

            DespawnAfterDeathAsync(enemy, _lifetimeCancellation.Token).Forget();
        }

        private async UniTaskVoid DespawnAfterDeathAsync(Enemy enemy, CancellationToken cancellationToken)
        {
            bool isCanceled = await enemy.PlayDeathAsync(cancellationToken).SuppressCancellationThrow();
            if (isCanceled)
                return;

            enemy.Died -= OnEnemyDied;
            _enemies.Remove(enemy);
            _pool.Despawn(enemy);
        }
    }
}
