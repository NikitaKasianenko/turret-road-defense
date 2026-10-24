using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class EnemyConfig
    {
        [field: SerializeField, Min(1f)] public float MaxHealth { get; private set; } = 30f;

        [field: Header("Movement")]
        [field: SerializeField, Min(0f)] public float AggroDistance { get; private set; } = 28f;
        [field: SerializeField, Min(0f), Tooltip("Keep below car speed: enemies the car has passed fall behind instead of attacking from the turret's blind spot")]
        public float RunSpeed { get; private set; } = 5.5f;
        [field: SerializeField, Min(0f)] public float TurnSpeed { get; private set; } = 720f;

        [field: Header("Attack")]
        [field: SerializeField, Min(0f)] public float AttackRange { get; private set; } = 0.6f;
        [field: SerializeField, Min(0f)] public float AttackDamage { get; private set; } = 12f;
        [field: SerializeField, Min(0.1f)] public float AttackInterval { get; private set; } = 1f;
    }
}
