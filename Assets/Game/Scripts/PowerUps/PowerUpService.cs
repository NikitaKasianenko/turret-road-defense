using System;
using Game.Configs;
using Game.Level;
using Game.Weapons;
using UnityEngine;
using Zenject;

namespace Game.PowerUps
{
    public class PowerUpService : ITickable, IResettable
    {
        private readonly TurretConfig _turretConfig;
        private readonly LevelSession _session;

        public event Action<PowerUp> Activated;
        public event Action Changed;

        public PowerUp Active { get; private set; }
        public float TimeLeft { get; private set; }

        public float NormalizedTimeLeft =>
            Active != null ? TimeLeft / Active.Duration : 0f;

        public ShotPattern ShotPattern =>
            Active != null ? Active.ShotPattern : _turretConfig.DefaultShotPattern;

        public float FireRateMultiplier =>
            Active != null ? Active.FireRateMultiplier : 1f;

        public PowerUpService(TurretConfig turretConfig, LevelSession session)
        {
            _turretConfig = turretConfig;
            _session = session;
        }

        public void Activate(PowerUp powerUp)
        {
            if (!_session.IsRunning)
                return;

            Active = powerUp;
            TimeLeft = powerUp.Duration;
            Activated?.Invoke(powerUp);
            Changed?.Invoke();
        }

        public void Tick()
        {
            if (Active == null)
                return;

            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0f)
                Deactivate();
        }

        public void ResetState()
        {
            if (Active != null)
                Deactivate();
        }

        private void Deactivate()
        {
            Active = null;
            TimeLeft = 0f;
            Changed?.Invoke();
        }
    }
}
