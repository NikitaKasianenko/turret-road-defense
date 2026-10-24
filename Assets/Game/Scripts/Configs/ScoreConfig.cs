using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class ScoreConfig
    {
        [field: SerializeField, Min(0.1f), Tooltip("Seconds without a kill before the combo resets")]
        public float ComboWindow { get; private set; } = 2.5f;
        [field: SerializeField, Min(1)] public int KillsPerMultiplierStep { get; private set; } = 3;
        [field: SerializeField, Min(1)] public int MaxMultiplier { get; private set; } = 4;
    }
}
