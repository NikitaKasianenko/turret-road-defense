using System;
using Game.Configs;
using Game.Enemies;
using Game.Level;
using UnityEngine;
using Zenject;

namespace Game.Scoring
{
    public class ScoreService : IInitializable, IDisposable, ITickable, IResettable
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly ScoreConfig _config;

        private float _comboTimeLeft;

        public event Action Changed;
        public event Action<int, Vector3> PointsGained;

        public int Kills { get; private set; }
        public int Score { get; private set; }
        public int Combo { get; private set; }

        public int Multiplier =>
            Mathf.Min(1 + Combo / _config.KillsPerMultiplierStep, _config.MaxMultiplier);

        public ScoreService(EnemySpawner enemySpawner, ScoreConfig config)
        {
            _enemySpawner = enemySpawner;
            _config = config;
        }

        public void Initialize()
        {
            _enemySpawner.EnemyKilled += OnEnemyKilled;
            _enemySpawner.EnemyStruck += OnEnemyStruck;
        }

        public void Dispose()
        {
            _enemySpawner.EnemyKilled -= OnEnemyKilled;
            _enemySpawner.EnemyStruck -= OnEnemyStruck;
        }

        public void Tick()
        {
            if (Combo == 0)
                return;

            _comboTimeLeft -= Time.deltaTime;
            if (_comboTimeLeft <= 0f)
                BreakCombo();
        }

        public void ResetState()
        {
            Kills = 0;
            Score = 0;
            Combo = 0;
            Changed?.Invoke();
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            Kills++;
            Combo++;
            _comboTimeLeft = _config.ComboWindow;

            int points = enemy.Type.ScoreReward * Multiplier;
            Score += points;

            PointsGained?.Invoke(points, enemy.transform.position);
            Changed?.Invoke();
        }

        private void OnEnemyStruck(Enemy enemy) =>
            BreakCombo();

        private void BreakCombo()
        {
            Combo = 0;
            Changed?.Invoke();
        }
    }
}
