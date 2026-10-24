using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class FeelConfig
    {
        [field: Header("Hit Stop")]
        [field: SerializeField, Min(0f), Tooltip("Real-time seconds the game freezes on a kill")]
        public float HitStopDuration { get; private set; } = 0.045f;
        [field: SerializeField, Range(0f, 1f)] public float HitStopTimeScale { get; private set; } = 0.05f;

        [field: Header("Slow Motion")]
        [field: SerializeField, Range(0.05f, 1f)] public float SlowMotionTimeScale { get; private set; } = 0.25f;
        [field: SerializeField, Min(0f), Tooltip("Real-time seconds to ease back to normal speed")]
        public float SlowMotionDuration { get; private set; } = 1.2f;
    }
}
