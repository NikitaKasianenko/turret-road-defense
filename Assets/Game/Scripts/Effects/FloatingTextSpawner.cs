using System;
using Game.PowerUps;
using Game.Scoring;
using Game.Vehicle;
using UnityEngine;
using Zenject;

namespace Game.Effects
{
    public class FloatingTextSpawner : IInitializable, IDisposable
    {
        private const float KillTextHeight = 2.4f;
        private const float PowerUpTextHeight = 3.2f;
        private static readonly Color PointsColor = new(1f, 0.85f, 0.25f);
        private static readonly Color ComboPointsColor = new(1f, 0.45f, 0.2f);

        private readonly FloatingText.Pool _pool;
        private readonly ScoreService _score;
        private readonly PowerUpService _powerUps;
        private readonly Car _car;

        public FloatingTextSpawner(FloatingText.Pool pool, ScoreService score, PowerUpService powerUps, Car car)
        {
            _pool = pool;
            _score = score;
            _powerUps = powerUps;
            _car = car;
        }

        public void Initialize()
        {
            _score.PointsGained += OnPointsGained;
            _powerUps.Activated += OnPowerUpActivated;
        }

        public void Dispose()
        {
            _score.PointsGained -= OnPointsGained;
            _powerUps.Activated -= OnPowerUpActivated;
        }

        private void OnPointsGained(int points, Vector3 position)
        {
            bool isCombo = _score.Multiplier > 1;
            string text = isCombo ? $"+{points} x{_score.Multiplier}" : $"+{points}";
            _pool.Spawn(position + Vector3.up * KillTextHeight, text, isCombo ? ComboPointsColor : PointsColor);
        }

        private void OnPowerUpActivated(PowerUp powerUp) =>
            _pool.Spawn(_car.Position + Vector3.up * PowerUpTextHeight, powerUp.DisplayName.ToUpperInvariant(), powerUp.Color);
    }
}
