using System;
using System.Collections.Generic;
using Game.Configs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;

namespace Game.Enemies
{
    public class EnemyCrowd : ITickable, IDisposable
    {
        private const int InitialCapacity = 64;

        private readonly EnemyConfig _config;
        private readonly IEnemyTarget _target;
        private readonly List<Enemy> _enemies = new();
        private readonly Dictionary<Enemy, int> _indices = new();

        private TransformAccessArray _transforms;
        private NativeArray<EnemyMotion> _motions;

        public EnemyCrowd(EnemyConfig config, IEnemyTarget target)
        {
            _config = config;
            _target = target;
            _transforms = new TransformAccessArray(InitialCapacity);
            _motions = new NativeArray<EnemyMotion>(InitialCapacity, Allocator.Persistent);
        }

        public void Dispose()
        {
            _transforms.Dispose();
            _motions.Dispose();
        }

        public void Add(Enemy enemy)
        {
            if (_indices.ContainsKey(enemy))
                return;

            EnsureCapacity(_enemies.Count + 1);

            _indices.Add(enemy, _enemies.Count);
            _motions[_enemies.Count] = new EnemyMotion { Direction = enemy.transform.forward };
            _enemies.Add(enemy);
            _transforms.Add(enemy.transform);
        }

        public void Remove(Enemy enemy)
        {
            if (!_indices.Remove(enemy, out int index))
                return;

            int lastIndex = _enemies.Count - 1;
            Enemy lastEnemy = _enemies[lastIndex];

            _enemies[index] = lastEnemy;
            _motions[index] = _motions[lastIndex];
            _enemies.RemoveAt(lastIndex);
            _transforms.RemoveAtSwapBack(index);

            if (index != lastIndex)
                _indices[lastEnemy] = index;
        }

        public void Tick()
        {
            if (_enemies.Count == 0)
                return;

            var job = new EnemyMotionJob
            {
                Motions = _motions,
                TargetPosition = _target.Position,
                DeltaTime = Time.deltaTime,
                Acceleration = _config.Acceleration,
                MaxTurnAngle = math.radians(_config.TurnSpeed) * Time.deltaTime
            };
            job.Schedule(_transforms).Complete();

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (i >= _enemies.Count)
                    continue;

                Enemy enemy = _enemies[i];
                EnemyMotion motion = _motions[i];
                enemy.Think(ref motion);

                if (_indices.TryGetValue(enemy, out int index))
                    _motions[index] = motion;
            }
        }

        private void EnsureCapacity(int count)
        {
            if (count <= _motions.Length)
                return;

            var resized = new NativeArray<EnemyMotion>(_motions.Length * 2, Allocator.Persistent);
            NativeArray<EnemyMotion>.Copy(_motions, resized, _enemies.Count);
            _motions.Dispose();
            _motions = resized;
        }
    }
}
