using System;
using Game.PowerUps;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class PowerUpConfig
    {
        [field: SerializeField, Range(0f, 1f)] public float DropChance { get; private set; } = 0.15f;
        [field: SerializeField] public PowerUp[] PowerUps { get; private set; }
    }
}
