using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public struct EnemySpawnWeight
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField, Min(0f)] public float Weight { get; private set; }
    }
}
