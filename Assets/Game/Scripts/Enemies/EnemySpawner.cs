using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemySpawner : IResettable, IDisposable
    {
        private const float MinSlotOffset = 0.2f;
        private const float MaxSlotOffset = 0.8f;
        private const float MaxSpawnYawJitter = 30f;

        private readonly Enemy.Pool _pool;
        private readonly LevelConfig _config;
        private readonly List<Enemy> _enemies = new();

        private CancellationTokenSource _lifetimeCancellation = new();

        public event Action<Enemy> EnemyKilled;
        public event Action<Enemy> EnemyStruck;

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

        public void ResetState()
        {
            DespawnAll();
            SpawnAll();
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

                Enemy enemy = _pool.Spawn(new Vector3(x, 0f, z), rotation, PickType());
                enemy.Killed += OnEnemyKilled;
                enemy.Struck += OnEnemyStruck;
                _enemies.Add(enemy);
            }
        }

        private EnemyType PickType()
        {
            float totalWeight = 0f;
            foreach (EnemySpawnWeight entry in _config.EnemyTypes)
                totalWeight += entry.Weight;

            float roll = Random.value * totalWeight;
            foreach (EnemySpawnWeight entry in _config.EnemyTypes)
            {
                roll -= entry.Weight;
                if (roll <= 0f)
                    return entry.Type;
            }

            return _config.EnemyTypes[^1].Type;
        }

        private void DespawnAll()
        {
            _lifetimeCancellation.Cancel();
            _lifetimeCancellation.Dispose();
            _lifetimeCancellation = new CancellationTokenSource();

            foreach (Enemy enemy in _enemies)
                Release(enemy);

            _enemies.Clear();
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            EnemyKilled?.Invoke(enemy);
            DespawnAfterAsync(enemy, enemy.PlayDeathAsync(_lifetimeCancellation.Token)).Forget();
        }

        private void OnEnemyStruck(Enemy enemy)
        {
            EnemyStruck?.Invoke(enemy);
            DespawnAfterAsync(enemy, enemy.PlayVanishAsync(_lifetimeCancellation.Token)).Forget();
        }

        private async UniTaskVoid DespawnAfterAsync(Enemy enemy, UniTask disappearance)
        {
            bool isCanceled = await disappearance.SuppressCancellationThrow();
            if (isCanceled)
                return;

            _enemies.Remove(enemy);
            Release(enemy);
        }

        private void Release(Enemy enemy)
        {
            enemy.Killed -= OnEnemyKilled;
            enemy.Struck -= OnEnemyStruck;
            _pool.Despawn(enemy);
        }
    }
}
