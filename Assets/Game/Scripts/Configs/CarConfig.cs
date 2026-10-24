using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class CarConfig
    {
        [field: SerializeField, Min(1f)] public float MaxHealth { get; private set; } = 100f;
        [field: SerializeField, Min(0f)] public float MaxSpeed { get; private set; } = 8f;
        [field: SerializeField, Min(0.1f)] public float Acceleration { get; private set; } = 4f;
        [field: SerializeField, Min(0.1f)] public float BrakeDeceleration { get; private set; } = 6f;
    }
}
