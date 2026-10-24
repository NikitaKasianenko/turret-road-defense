using Game.Configs;
using Game.Level;
using UnityEngine;

namespace Game.Weapons
{
    public class TurretHeat : IResettable
    {
        private readonly TurretConfig _config;

        public float Value { get; private set; }
        public bool IsOverheated { get; private set; }

        public TurretHeat(TurretConfig config) =>
            _config = config;

        public void AddShot()
        {
            Value = Mathf.Min(Value + _config.HeatPerShot, 1f);
            if (Value >= 1f)
                IsOverheated = true;
        }

        public void Cool(float deltaTime)
        {
            Value = Mathf.Max(Value - _config.CoolingRate * deltaTime, 0f);
            if (IsOverheated && Value <= _config.OverheatRecoverLevel)
                IsOverheated = false;
        }

        public void ResetState()
        {
            Value = 0f;
            IsOverheated = false;
        }
    }
}
