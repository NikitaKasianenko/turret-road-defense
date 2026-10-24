using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class EnemyConfig
    {
        [field: Header("Idle")]
        [field: SerializeField, Range(0f, 1f), Tooltip("Chance to wander instead of standing after each pause")]
        public float WanderChance { get; private set; } = 0.6f;
        [field: SerializeField, Tooltip("Seconds, x = min, y = max")] public Vector2 IdleDurationRange { get; private set; } = new(1f, 3f);
        [field: SerializeField, Tooltip("Seconds, x = min, y = max")] public Vector2 WanderDurationRange { get; private set; } = new(1.5f, 3f);
        [field: SerializeField, Min(0f)] public float WalkSpeed { get; private set; } = 1.5f;
        [field: SerializeField, Min(0f), Tooltip("Wandering never takes an enemy further than this from its spawn point")]
        public float WanderRadius { get; private set; } = 3f;

        [field: Header("Chase")]
        [field: SerializeField, Min(0f)] public float AggroDistance { get; private set; } = 28f;
        [field: SerializeField, Min(0f)] public float TurnSpeed { get; private set; } = 720f;

        [field: Header("Attack")]
        [field: SerializeField, Min(0f), Tooltip("Distance to the car body at which the enemy strikes once and vanishes")]
        public float AttackRange { get; private set; } = 0.6f;
    }
}
